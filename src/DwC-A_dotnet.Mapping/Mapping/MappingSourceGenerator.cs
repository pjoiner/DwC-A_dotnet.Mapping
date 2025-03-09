using DwC_A.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DwC_A.Mapping
{
    [Generator]
    public class MappingSourceGenerator : IIncrementalGenerator
    {
        private static readonly List<string> classes = [];

        static void Execute(SourceProductionContext context, ClassDeclarationSyntax classDeclaration)
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
                .NormalizeWhitespace(eol: Environment.NewLine)
                .ToFullString();
            var sourceFileName = $"{className}.g.cs";
            context.AddSource(sourceFileName, sourceCode);
        }

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            IncrementalValuesProvider<ClassDeclarationSyntax> classesToGenerate = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                    transform: static (s, _) => GetSemanticTargetForGeneration(s))
                .Where(static m => m is not null);

            context.RegisterSourceOutput(classesToGenerate,
                static (spc, classDeclarationSyntax) => Execute(spc, classDeclarationSyntax));
        }

        static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        {
            if (node is ClassDeclarationSyntax classDeclaration)
            {
                return classDeclaration.ChildNodes()
                    .OfType<PropertyDeclarationSyntax>()
                    .SelectMany(p => p.AttributeLists)
                    .SelectMany(a => a.Attributes)
                    .Select(a => a.Name)
                    .OfType<IdentifierNameSyntax>()
                    .Any(i => i.Identifier.Text == "Term");
            }

            return false;
        }

        static ClassDeclarationSyntax GetSemanticTargetForGeneration(GeneratorSyntaxContext ctx)
        {
            return ctx.Node as ClassDeclarationSyntax ?? throw new InvalidOperationException("Class not found");
        }
    }
}
