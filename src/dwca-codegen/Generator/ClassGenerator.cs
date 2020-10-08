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
            @namespace = @namespace.AddMembers(classDeclaration);
            var code = @namespace
                .NormalizeWhitespace()
                .NormalizeWhitespacesSingleLineProperties()
                .ToFullString();
            return code;
        }

        private ClassDeclarationSyntax GenerateClass(string className, IFieldMetaData fieldMetaData)
        {
            var classDeclaration = SyntaxFactory
                .ClassDeclaration(RoslynGeneratorUtils.ModifyKeywords(className, config.Capitalize))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

            foreach (var metaData in fieldMetaData)
            {
                var propertyConfiguration = config.GetPropertyConfiguration(metaData.Term);
                var propertyName = propertyConfiguration.PropertyName ?? RoslynGeneratorUtils.ModifyKeywords(metaData.Term, config.Capitalize);
                if (propertyConfiguration.Include)
                {
                    var propertyDeclaration = SyntaxFactory
                        .PropertyDeclaration(SyntaxFactory.ParseTypeName(propertyConfiguration.TypeName), propertyName)
                        .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                        .AddAccessorListAccessors(
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)))
                        .NormalizeWhitespace(indentation: "", eol: " ");
                    classDeclaration = classDeclaration.AddMembers(propertyDeclaration);
                }
            }
            return classDeclaration;
        }

    }
}
