extern alias Core;

using Core::DwC_A.Meta;
using DwcaCodegen.Config;
using DwcaCodegen.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DwC_A.Generator
{
    public class MapMethodGenerator
    {
        public static MethodDeclarationSyntax MapStaticInstanceMethodSyntax(IFileMetaData fileMetaData, IArchiveGeneratorConfiguration config)
        {
            return MapMethodSyntax(fileMetaData, config, false);
        }

        public static MethodDeclarationSyntax MapStaticInstanceMethodSyntax(ClassDeclarationSyntax classDeclaration)
        {
            return MapMethodSyntax(classDeclaration, false);
        }

        public static MethodDeclarationSyntax MapExtensionMethodSyntax(ClassDeclarationSyntax classDeclaration)
        {
            return MapMethodSyntax(classDeclaration, true);
        }

        private static MethodDeclarationSyntax MapMethodSyntax(IFileMetaData fileMetaData, IArchiveGeneratorConfiguration config, bool useExtensionSyntax = false)
        {
            var roslynGeneratorUtils = new RoslynGeneratorUtils();
            var className = roslynGeneratorUtils.NormalizeIdentifiers(Path.GetFileNameWithoutExtension(fileMetaData.FileName),
                config.PascalCase);

            var objParam = SyntaxFactory.Parameter(SyntaxFactory.Identifier("obj"))
                         .WithType(SyntaxFactory.ParseTypeName(className));
            if (useExtensionSyntax)
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
            foreach(var field in fileMetaData.Fields)
            {
                var propertyConfig = config.GetPropertyConfiguration(field.Term);
                var propertyName = propertyConfig.PropertyName ?? roslynGeneratorUtils.NormalizeIdentifiers(field.Term, config.PascalCase);
                if (propertyConfig.Include)
                {
                    var statement = CreateStatement(propertyName, field.Term, propertyConfig)
                        .WithTrailingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed);
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

        private static StatementSyntax CreateStatement(string propertyName, string term, PropertyConfiguration propertyConfig )
        {
            if (propertyConfig.TypeName == "string")
            {
                return SyntaxFactory.ParseStatement($"obj.{propertyName} = row[\"{term}\"];");
            }
            else if (IsNullableType(propertyConfig.TypeName, out string underlyingTypeName)) 
            {
                return SyntaxFactory.ParseStatement($"obj.{propertyName} = row.ConvertNullable<{underlyingTypeName}>(\"{term}\");");
            }
            return SyntaxFactory.ParseStatement($"obj.{propertyName} = row.Convert<{propertyConfig.TypeName}>(\"{term}\");");
        }

        private static bool IsNullableType(string typeName, out string newTypeName)
        {
            newTypeName = typeName.Replace("?", "");
            return typeName.EndsWith("?");
        }

        private static MethodDeclarationSyntax MapMethodSyntax(ClassDeclarationSyntax classDeclaration, bool useExtensionSyntax = true)
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

        private static StatementSyntax AssignPropertyStatement(PropertyDeclarationSyntax propertySyntax)
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
