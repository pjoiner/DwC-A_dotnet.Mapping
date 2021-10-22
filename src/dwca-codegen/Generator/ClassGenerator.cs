using DwC_A.Meta;
using DwcaCodegen.Config;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
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
            var doc  = Formatter.Format(@namespace, new AdhocWorkspace());
            var code = doc.ToFullString();
            return code;
        }

        private ClassDeclarationSyntax GenerateClass(string className, IFieldMetaData fieldMetaData)
        {
            className = RoslynGeneratorUtils.ModifyKeywords(className, config.PascalCase);
            var classDeclaration = SyntaxFactory
                .ClassDeclaration(className)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

            foreach (var metaData in fieldMetaData)
            {
                var propertyConfiguration = config.GetPropertyConfiguration(metaData.Term);
                var propertyName = propertyConfiguration.PropertyName ?? RoslynGeneratorUtils.ModifyKeywords(metaData.Term, config.PascalCase);
                //TODO: Add additional logic here to weed out duplicate property names from different dwca namespaces
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
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
                    
                    if(config.TermAttribute)
                    {
                        AttributeListSyntax attributeList = AddTermAttibute(metaData);
                        propertyDeclaration = propertyDeclaration.AddAttributeLists(attributeList);
                    }

                    classDeclaration = classDeclaration.AddMembers(propertyDeclaration);
                }
            }
            return classDeclaration;
        }

        private static AttributeListSyntax AddTermAttibute(FieldType metaData)
        {
            return SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName("Term"))
                        .WithArgumentList(
                            SyntaxFactory.AttributeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                    SyntaxFactory.AttributeArgument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression,
                                            SyntaxFactory.Literal(metaData.Term))))))));
        }
    }
}
