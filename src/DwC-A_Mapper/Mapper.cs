using DwC_A;
using DwC_A.Terms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DwC_A_Mapper
{
    public class Mapper
    {
        //https://gist.github.com/cmendible/9b8c7d7598f1ab0bc7ab5d24b2622622

        public string Map<T>(IRow row)
        {
            Type obj = typeof(T);

            // Create CompilationUnitSyntax
            var syntaxFactory = SyntaxFactory.CompilationUnit();

            // Create a namespace: (namespace CodeGenerationSample)
            var @namespace = SyntaxFactory
                .NamespaceDeclaration(SyntaxFactory.ParseName("DwC"))
                .NormalizeWhitespace();

            var classDeclaration = SyntaxFactory
                .ClassDeclaration("Mapper")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            var statements = new List<StatementSyntax>();
            var createObjectStatement = SyntaxFactory
                .ParseStatement($"var obj = new {obj.Name}();");
            statements.Add(createObjectStatement);
            foreach (var term in row.FieldMetaData)
            {
                var propertyName = Capitalize(Terms.ShortName(term.Term));
                var property = obj.GetProperties(BindingFlags.Public)
                    .Where(n => n.Name == propertyName);
                var statement = SyntaxFactory
                    .ParseStatement($"obj.{propertyName} = row[\"{term.Term}\"];")
                    .NormalizeWhitespace();
                statements.Add(statement);
            }
            var returnStatement = SyntaxFactory
                .ParseStatement("return obj;");
            statements.Add(returnStatement);

            var identifier = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(obj.Name));

            var parameter = SyntaxFactory.Parameter(
                    new SyntaxList<AttributeListSyntax>(),
                    new SyntaxTokenList(),
                    SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("IRow")),
                    SyntaxFactory.Identifier("row"), 
                    null);

            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(obj.Name), "Map")
                 .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                 .AddParameterListParameters(parameter)
                 .WithBody(SyntaxFactory.Block(statements));

            classDeclaration = classDeclaration.AddMembers(methodDeclaration);

            // Add the class to the namespace.
            @namespace = @namespace.AddMembers(classDeclaration);

            // Add the namespace to the compilation unit.
            syntaxFactory = syntaxFactory.AddMembers(@namespace);

            // Normalize and get code as string.
            var code = syntaxFactory
                .NormalizeWhitespace()
                .ToFullString();

            return code;
        }

        private string Capitalize(string value)
        {
            return char.ToUpper(value[0]) + value.Substring(1);
        }
    }
}
