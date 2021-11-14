using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace DwC_A.Mapping
{
    public class MappingSyntaxReceiver : ISyntaxReceiver
    {
        private readonly List<SyntaxNode> candidates = new List<SyntaxNode>();

        public List<SyntaxNode> Candidates => candidates;

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclaration)
            {
                var hasTermAttribute = classDeclaration.ChildNodes()
                    .OfType<PropertyDeclarationSyntax>()
                    .SelectMany(p => p.AttributeLists)
                    .SelectMany(a => a.Attributes)
                    .Select(a => a.Name)
                    .OfType<IdentifierNameSyntax>()
                    .Any(i => i.Identifier.Text == "Term");

                if (hasTermAttribute)
                {
                    candidates.Add(syntaxNode);
                }
            }
        }
    }
}
