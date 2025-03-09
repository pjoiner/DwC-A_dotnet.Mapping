extern alias Core;

using Core::DwC_A.Meta;
using DwC_A.Config;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;

namespace DwC_A.Generator
{
    public static class ClassGenerator
    {
        public static string GenerateFile(IFileMetaData fileMetaData, IGeneratorConfiguration config)
        {
            var classDeclaration = GeneratClassSyntax(fileMetaData, config);
            if (config.MapMethod)
            {
                var staticMethodSyntax = config.TermAttribute == TermAttributeType.none ?
                      MapMethodGenerator.MapStaticInstanceMethodSyntax(fileMetaData, config)
                    : MapMethodGenerator.MapStaticInstanceMethodSyntax(classDeclaration);
                classDeclaration = classDeclaration.AddMembers(staticMethodSyntax);
            }
            if (string.IsNullOrEmpty(config.Namespace))
            {
                var usings = new SyntaxList<UsingDirectiveSyntax>();
                foreach (var usingNamespace in config.Usings)
                {
                    usings = usings.Add(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(usingNamespace)));
                }
                var node = SyntaxFactory.CompilationUnit()
                    .WithUsings(usings)
                    .WithMembers(SyntaxFactory.SingletonList<MemberDeclarationSyntax>(classDeclaration));
                return FormatDeclarationSyntax(node);
            }
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(config.Namespace));
            foreach (var usingNamespace in config.Usings)
            {
                @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(usingNamespace)));
            }
            @namespace = @namespace.AddMembers(classDeclaration);
            return FormatDeclarationSyntax(@namespace);
        }

        private static string FormatDeclarationSyntax(SyntaxNode node)
        {
            var code = node.NormalizeWhitespace(eol: Environment.NewLine).ToFullString();
            return code;
        }

        public static string GenerateClass(IFileMetaData fileMetaData, IGeneratorConfiguration config)
        {
            var classDeclaration = GeneratClassSyntax(fileMetaData, config);
            var code = classDeclaration.NormalizeWhitespace(eol: Environment.NewLine).ToFullString();
            return code;
        }

        public static ClassDeclarationSyntax GeneratClassSyntax(IFileMetaData fileMetaData, IGeneratorConfiguration config)
        {
            var roslynGeneratorUtils = new RoslynGeneratorUtils();
            var className = roslynGeneratorUtils.NormalizeIdentifiers(Path.GetFileNameWithoutExtension(fileMetaData.FileName),
                config.PascalCase);
            var classDeclaration = SyntaxFactory
                .ClassDeclaration(className)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

            foreach (var metaData in fileMetaData.Fields)
            {
                var propertyConfiguration = config.GetPropertyConfiguration(metaData.Term);

                if (propertyConfiguration.Include)
                {
                    var propertyDeclaration = GenerateProperty(metaData, propertyConfiguration, roslynGeneratorUtils, config);
                    classDeclaration = classDeclaration.AddMembers(propertyDeclaration);
                }
            }
            return classDeclaration;
        }

        private static PropertyDeclarationSyntax GenerateProperty(FieldType metaData,
            PropertyConfiguration propertyConfiguration,
            RoslynGeneratorUtils roslynGeneratorUtils,
            IGeneratorConfiguration config)
        {
            var propertyName = propertyConfiguration.PropertyName ?? roslynGeneratorUtils.NormalizeIdentifiers(metaData.Term, config.PascalCase);
            var propertyDeclaration = SyntaxFactory
                .PropertyDeclaration(SyntaxFactory.ParseTypeName(propertyConfiguration.TypeName), propertyName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

            if (config.TermAttribute != TermAttributeType.none)
            {
                AttributeListSyntax attributeList = GenerateTermAttribute(metaData, config);
                propertyDeclaration = propertyDeclaration.AddAttributeLists(attributeList);
            }

            return propertyDeclaration;
        }

        private static AttributeListSyntax GenerateTermAttribute(FieldType metaData, IGeneratorConfiguration config)
        {
            LiteralExpressionSyntax literalExpression = config.TermAttribute switch
            {
                TermAttributeType.name => SyntaxFactory.LiteralExpression(
                                    SyntaxKind.StringLiteralExpression,
                                    SyntaxFactory.Literal(metaData.Term)),
                TermAttributeType.index => SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NumericLiteralExpression,
                                    SyntaxFactory.Literal(metaData.Index)),
                _ => throw new Exception("Invalid term attribute configuration"),
            };

            return SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName("Term"))
                        .WithArgumentList(
                            SyntaxFactory.AttributeArgumentList(
                                SyntaxFactory.SingletonSeparatedList(
                                    SyntaxFactory.AttributeArgument(literalExpression))))));
        }
    }
}
