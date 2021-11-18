using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace DwC_A.Generator
{
    public class MapMethodGenerator
    {
        public static MethodDeclarationSyntax MapStaticInstanceMethodSyntax(ClassDeclarationSyntax classDeclaration)
        {
            return MapMethodSyntax(classDeclaration, false);
        }

        public static MethodDeclarationSyntax MapExtensionMethodSyntax(ClassDeclarationSyntax classDeclaration)
        {
            return MapMethodSyntax(classDeclaration, true);
        }

        public static MethodDeclarationSyntax MapMethodSyntax(ClassDeclarationSyntax classDeclaration, bool useExtensionSyntax = true)
        {
            var objParam = SyntaxFactory.Parameter(SyntaxFactory.Identifier("obj"))
                        .WithType(SyntaxFactory.ParseTypeName(classDeclaration.Identifier.Text));

            if(useExtensionSyntax)
            {
                objParam = objParam.AddModifiers(SyntaxFactory.Token(SyntaxKind.ThisKeyword));
            }

            var parameterList = new List<ParameterSyntax>()
                {
                    objParam,
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("row"))
                        .WithType(SyntaxFactory.ParseTypeName("IRow"))
                };

            var statements = new List<StatementSyntax>();
            foreach (var propertySyntax in classDeclaration.Members.OfType<PropertyDeclarationSyntax>())
            {
                var statement = AssignPropertyStatement(propertySyntax)
                    .WithTrailingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed);
                if (statement != null)
                {
                    statements.Add(statement);
                }
            }

            var methodSyntax = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "MapRow")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                .AddParameterListParameters(parameterList.ToArray())
                .WithBody(SyntaxFactory.Block(statements.ToArray()));
            return methodSyntax;
        }

        public static StatementSyntax AssignPropertyStatement(PropertyDeclarationSyntax propertySyntax)
        {
            foreach (var attributeList in propertySyntax.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (attribute.Name is IdentifierNameSyntax identifier)
                    {
                        if (identifier.Identifier.Text == "Term")
                        {
                            var paramString = attribute.ArgumentList.Arguments.First().Expression.ToFullString();
                            if (propertySyntax.Type is PredefinedTypeSyntax pts &&
                                pts.Keyword.IsKind(SyntaxKind.StringKeyword))
                            {
                                return SyntaxFactory.ParseStatement($"obj.{propertySyntax.Identifier.Text} = row[{paramString}];");
                            }
                            else if (propertySyntax.Type.IsKind(SyntaxKind.NullableType))
                            {
                                var type = propertySyntax
                                    .Type
                                    .WithoutTrivia()
                                    .ToFullString();
                                type = type.Replace('?', ' ').Trim();
                                return SyntaxFactory.ParseStatement($"obj.{propertySyntax.Identifier.Text} = row.ConvertNullable<{type}>({paramString});");
                            }
                            else
                            {
                                var type = propertySyntax.Type.ToFullString();
                                return SyntaxFactory.ParseStatement($"obj.{propertySyntax.Identifier.Text} = row.Convert<{type}>({paramString});");
                            }
                        }
                    }
                }
            }
            return null;
        }

    }
}
