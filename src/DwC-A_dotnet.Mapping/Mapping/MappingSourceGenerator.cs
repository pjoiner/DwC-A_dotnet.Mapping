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

                    MethodDeclarationSyntax methodSyntax = MapMethodSyntax(classDeclaration);

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

        private static MethodDeclarationSyntax MapMethodSyntax(ClassDeclarationSyntax classDeclaration)
        {
            var parameterList = new List<ParameterSyntax>()
                {
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("obj"))
                        .WithType(SyntaxFactory.ParseTypeName(classDeclaration.Identifier.Text))
                        .AddModifiers(SyntaxFactory.Token(SyntaxKind.ThisKeyword)),

                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("row"))
                        .WithType(SyntaxFactory.ParseTypeName("IRow"))
                };

            var statements = new List<StatementSyntax>();
            foreach (var propertySyntax in classDeclaration.Members.OfType<PropertyDeclarationSyntax>())
            {
                AssignPropertyStatement(statements, propertySyntax);
            }

            var methodSyntax = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "MapRow")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                .AddParameterListParameters(parameterList.ToArray())
                .WithBody(SyntaxFactory.Block(statements.ToArray()));
            return methodSyntax;
        }

        private static void AssignPropertyStatement(List<StatementSyntax> statements, PropertyDeclarationSyntax propertySyntax)
        {
            foreach (var attributeList in propertySyntax.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (attribute.Name is IdentifierNameSyntax identifier)
                    {
                        if (identifier.Identifier.Text == "Term")
                        {
                            StatementSyntax statement;
                            var paramString = attribute.ArgumentList.Arguments.First().Expression.ToFullString();
                            if (propertySyntax.Type is PredefinedTypeSyntax pts &&
                                pts.Keyword.IsKind(SyntaxKind.StringKeyword))
                            {
                                statement = SyntaxFactory.ParseStatement($"obj.{propertySyntax.Identifier.Text} = row[{paramString}];");
                            }
                            else if (propertySyntax.Type.IsKind(SyntaxKind.NullableType))
                            {
                                var type = propertySyntax
                                    .Type
                                    .WithoutTrivia()
                                    .ToFullString();
                                type = type.Replace('?', ' ').Trim();
                                statement = SyntaxFactory.ParseStatement($"obj.{propertySyntax.Identifier.Text} = row.ConvertNullable<{type}>({paramString});");
                            }
                            else
                            {
                                var type = propertySyntax.Type.ToFullString();
                                statement = SyntaxFactory.ParseStatement($"obj.{propertySyntax.Identifier.Text} = row.Convert<{type}>({paramString});");
                            }
                            statements.Add(statement);
                        }
                    }
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new MappingSyntaxReceiver());
        }
    }
}
