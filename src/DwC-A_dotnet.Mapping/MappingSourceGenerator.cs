using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace SourceGeneratorLib
{
    [Generator]
    public class MappingSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxReceiver = context.SyntaxReceiver as MappingSyntaxReceiver;
            foreach(ClassDeclarationSyntax classDeclaration in syntaxReceiver.Candidates)
            {
                SyntaxNode node = classDeclaration;
                while(!node.IsKind(SyntaxKind.NamespaceDeclaration))
                {
                    node = node.Parent;
                }
                NamespaceDeclarationSyntax encloseingNamespace = node as NamespaceDeclarationSyntax;
                var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("DwC_A"));
                @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("DwC_A")))
                    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")))
                    .AddUsings(SyntaxFactory.UsingDirective(encloseingNamespace.Name));

                var className = $"{classDeclaration.Identifier.Text}Extensions";

                var extensionClassSyntax = SyntaxFactory.ClassDeclaration(className)
                    .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.StaticKeyword)));

                var parameterList = new List<ParameterSyntax>()
                {
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("obj"))
                        .WithType(SyntaxFactory.ParseTypeName(classDeclaration.Identifier.Text))
                        .AddModifiers(SyntaxFactory.Token(SyntaxKind.ThisKeyword)),

                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("row"))
                        .WithType(SyntaxFactory.ParseTypeName("IRow"))
                };

                var statements = new List<StatementSyntax>();
                foreach(var propertySyntax in classDeclaration.Members.OfType<PropertyDeclarationSyntax>())
                {
                    foreach(var attributeList in propertySyntax.AttributeLists)
                    {
                        foreach(var attribute in attributeList.Attributes)
                        {
                            if(attribute.Name is IdentifierNameSyntax identifier)
                            {
                                if(identifier.Identifier.Text == "Term")
                                {
                                    //What goes here?
                                    var paramString = attribute.ArgumentList.Arguments.First().Expression.ToFullString();
                                    var statement = SyntaxFactory.ParseStatement($"obj.{propertySyntax.Identifier.Text} = row[{paramString}];");
                                    statements.Add(statement);
                                }
                            }
                        }
                    }
                }

                var methodSyntax = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "MapRow")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                    .AddParameterListParameters(parameterList.ToArray())
                    .WithBody(SyntaxFactory.Block(statements.ToArray()));

                extensionClassSyntax = extensionClassSyntax.AddMembers(methodSyntax);

                @namespace = @namespace.AddMembers(extensionClassSyntax);

                var sourceCode = @namespace
                    .NormalizeWhitespace()
                    .ToFullString();
                context.AddSource($"{className}.g.cs", sourceCode);
            }
        }
    

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new MappingSyntaxReceiver());
        }
    }
}
