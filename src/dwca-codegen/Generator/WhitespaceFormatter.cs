using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DwcaCodegen.Generator
{
    public static class WhitespaceFormatter
    {
        public static SyntaxNode NormalizeWhitespacesSingleLineProperties(this SyntaxNode node) =>
            node.NormalizeWhitespace().SingleLineProperties();

        public static SyntaxNode SingleLineProperties(this SyntaxNode node) => new SingleLinePropertyRewriter().Visit(node);

        class SingleLinePropertyRewriter : CSharpSyntaxRewriter
        {
            public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
            {
                return node.NormalizeWhitespace(indentation: "", eol: " ")
                    .WithLeadingTrivia(node.GetLeadingTrivia())
                    .WithTrailingTrivia(node.GetTrailingTrivia());
            }
        }
    }
}
