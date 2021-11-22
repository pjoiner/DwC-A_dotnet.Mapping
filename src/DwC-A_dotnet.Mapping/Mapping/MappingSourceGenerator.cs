using DwC_A.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace DwC_A.Mapping
{
    [Generator]
    public class MappingSourceGenerator : ISourceGenerator
    {
        private readonly IList<string> classes = new List<string>();

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is MappingSyntaxReceiver syntaxReceiver)
            {
                foreach (ClassDeclarationSyntax classDeclaration in syntaxReceiver.Candidates)
                {
                    SyntaxNode node = classDeclaration;
                    while (!node.IsKind(SyntaxKind.NamespaceDeclaration))
                    {
                        node = node.Parent;
                    }
                    NamespaceDeclarationSyntax encloseingNamespace = node as NamespaceDeclarationSyntax;
                    var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("DwC_A.Mapping"));
                    @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("DwC_A")))
                        .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("DwC_A.Extensions")))
                        .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")))
                        .AddUsings(SyntaxFactory.UsingDirective(encloseingNamespace.Name));

                    var className = $"{classDeclaration.Identifier.Text}Extensions";
                    int x = 1;
                    while (classes.Contains(className))
                    {
                        className = $"{className}{x++}";
                    }
                    classes.Add(className);

                    var extensionClassSyntax = SyntaxFactory.ClassDeclaration(className)
                        .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                            SyntaxFactory.Token(SyntaxKind.StaticKeyword)));

                    MethodDeclarationSyntax methodSyntax = MapMethodGenerator.MapExtensionMethodSyntax(classDeclaration);

                    extensionClassSyntax = extensionClassSyntax.AddMembers(methodSyntax);

                    @namespace = @namespace.AddMembers(extensionClassSyntax);

                    var sourceCode = @namespace
                        .NormalizeWhitespace()
                        .ToFullString();
                    var sourceFileName = $"{className}.g.cs";
                    context.AddSource(sourceFileName, sourceCode);
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new MappingSyntaxReceiver());
        }
    }
}
