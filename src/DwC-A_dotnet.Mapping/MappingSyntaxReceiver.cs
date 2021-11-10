using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace SourceGeneratorLib
{
    public class MappingSyntaxReceiver : ISyntaxReceiver
    {
        private readonly HashSet<SyntaxNode> candidates = new HashSet<SyntaxNode>();

        public HashSet<SyntaxNode> Candidates => candidates;

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if(syntaxNode is AttributeSyntax attribute)
            {
                if(attribute.Name is IdentifierNameSyntax identifier)
                {
                    if(identifier.Identifier.Text == "Term")
                    {
                        var node = syntaxNode;
                        while(!node.IsKind(SyntaxKind.ClassDeclaration))
                        {
                            node = node.Parent;
                        }
                        if(node != null)
                        {
                            candidates.Add(node);
                        }
                    }
                }
            }
        }
    }
}
