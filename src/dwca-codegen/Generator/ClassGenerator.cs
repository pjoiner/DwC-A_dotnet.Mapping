using DwC_A.Meta;
using DwcaCodegen.Config;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;

namespace DwcaCodegen.Generator
{
    public class ClassGenerator
    {
        private readonly ArchiveGeneratorConfiguration config;

        public ClassGenerator(ArchiveGeneratorConfiguration config)
        {
            this.config = config;
        }

        public string GenerateFile(IFileMetaData fileMetaData)
        {
            var className = Path.GetFileNameWithoutExtension(fileMetaData.FileName);
            var classDeclaration = GenerateClass(className, fileMetaData.Fields);
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(config.Namespace));
            foreach (var usingNamespace in config.Usings)
            {
                @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(usingNamespace)));
            }

            if(config.AddAttributes)
            {
                @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("DwC_A.Attributes")));
            }

            @namespace = @namespace.AddMembers(classDeclaration);
            var code = @namespace
                .NormalizeWhitespace()
                .NormalizeWhitespacesSingleLineProperties()
                .ToFullString();
            return code;
        }

        private ClassDeclarationSyntax GenerateClass(string className, IFieldMetaData fieldMetaData)
        {
            className = RoslynGeneratorUtils.ModifyKeywords(className, config.Capitalize);
            var classDeclaration = SyntaxFactory
                .ClassDeclaration(className)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

            if(config.AddAttributes)
            {
                var attribute = SyntaxFactory.Attribute(SyntaxFactory.ParseName("Map"));
                var attributes = new SeparatedSyntaxList<AttributeSyntax>();
                attributes = attributes.Add(attribute);
                var attributeList = SyntaxFactory.AttributeList(attributes);
                classDeclaration = classDeclaration.AddAttributeLists(attributeList);
            }

            foreach (var metaData in fieldMetaData)
            {
                var propertyConfiguration = config.GetPropertyConfiguration(metaData.Term);
                var propertyName = propertyConfiguration.PropertyName ?? RoslynGeneratorUtils.ModifyKeywords(metaData.Term, config.Capitalize);
                if(propertyName.Equals(className))
                {
                    propertyName += "1";
                }
                if (propertyConfiguration.Include)
                {
                    var propertyDeclaration = SyntaxFactory
                        .PropertyDeclaration(SyntaxFactory.ParseTypeName(propertyConfiguration.TypeName), propertyName)
                        .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                        .AddAccessorListAccessors(
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)))
                        .NormalizeWhitespace(indentation: "", eol: " ");

                    if(config.AddAttributes)
                    {
                        var arguments = SyntaxFactory.ParseAttributeArgumentList($"(\"{metaData.Term}\")");
                        var attribute = SyntaxFactory.Attribute(SyntaxFactory.ParseName("Term"), arguments);
                        var attributes = new SeparatedSyntaxList<AttributeSyntax>();
                        attributes = attributes.Add(attribute);
                        var attributeList = SyntaxFactory.AttributeList(attributes)
                            .WithTrailingTrivia(SyntaxFactory.Whitespace("\n"));
                        propertyDeclaration = propertyDeclaration
                            .AddAttributeLists(attributeList);
                    }

                    classDeclaration = classDeclaration.AddMembers(propertyDeclaration);
                }
            }
            return classDeclaration;
        }

    }
}
