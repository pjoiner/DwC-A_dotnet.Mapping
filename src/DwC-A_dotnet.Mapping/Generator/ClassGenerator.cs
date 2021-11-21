﻿extern alias Core;

using Core::DwC_A.Meta;
using DwC_A.Generator;
using DwcaCodegen.Config;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System;
using System.IO;

namespace DwcaCodegen.Generator
{
    public class ClassGenerator
    {
        public string GenerateFile(IFileMetaData fileMetaData, IArchiveGeneratorConfiguration config)
        {
            var classDeclaration = GeneratClassSyntax(fileMetaData, config);
            if(config.MapMethod)
            {
                //TODO: At the moment this only works if -t name or -t index
                //      If termAttribue is none then we need to call the new MapStaticInstanceMethod with fileMetaData
                var staticMethodSyntax = MapMethodGenerator.MapStaticInstanceMethodSyntax(classDeclaration);
                classDeclaration = classDeclaration.AddMembers(staticMethodSyntax);
            }
            if(string.IsNullOrEmpty(config.Namespace))
            {
                //TODO: Need to tack on some usings at the beginning of the returned source
                return FormatDeclarationSyntax(classDeclaration);
            }
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(config.Namespace));
            foreach (var usingNamespace in config.Usings)
            {
                @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(usingNamespace)));
            }
            @namespace = @namespace.AddMembers(classDeclaration);
            return FormatDeclarationSyntax(@namespace);
        }

        private static string FormatDeclarationSyntax(SyntaxNode @namespace)
        {
            var doc = Formatter.Format(@namespace, new AdhocWorkspace());
            var code = doc.ToFullString();
            return code;
        }

        public string GenerateClass(IFileMetaData fileMetaData, IArchiveGeneratorConfiguration config)
        {
            var classDeclaration = GeneratClassSyntax(fileMetaData, config);
            var doc = Formatter.Format(classDeclaration, new AdhocWorkspace());
            var code = doc.ToFullString();
            return code;
        }

        public ClassDeclarationSyntax GeneratClassSyntax(IFileMetaData fileMetaData, IArchiveGeneratorConfiguration config)
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

        private PropertyDeclarationSyntax GenerateProperty(FieldType metaData, 
            PropertyConfiguration propertyConfiguration, 
            RoslynGeneratorUtils roslynGeneratorUtils,
            IArchiveGeneratorConfiguration config)
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

        private AttributeListSyntax GenerateTermAttribute(FieldType metaData, IArchiveGeneratorConfiguration config)
        {
            LiteralExpressionSyntax literalExpression = null;
            switch (config.TermAttribute)
            {
                case TermAttributeType.name: literalExpression = SyntaxFactory.LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    SyntaxFactory.Literal(metaData.Term));
                    break;
                case TermAttributeType.index:
                    literalExpression = SyntaxFactory.LiteralExpression(
                    SyntaxKind.NumericLiteralExpression,
                    SyntaxFactory.Literal(metaData.Index));
                    break;
                default: throw new Exception("Invalid term attribute configuration");
            };

            return SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName("Term"))
                        .WithArgumentList(
                            SyntaxFactory.AttributeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                    SyntaxFactory.AttributeArgument(literalExpression))))));
        }
    }
}