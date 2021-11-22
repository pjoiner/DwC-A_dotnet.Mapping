using DwC_A.Terms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using System.Collections.Generic;

namespace DwcaCodegen.Generator
{
    public class RoslynGeneratorUtils
    {
        private readonly HashSet<string> propertyNameList = new HashSet<string>();

        public string NormalizeIdentifiers(string name, bool pascalCase = false)
        {
            var propertyName = Terms.ShortName(name);
            if (pascalCase)
            {
                propertyName = char.ToUpper(propertyName[0]) + propertyName.Substring(1);
            }
            if (!SyntaxFacts.IsValidIdentifier(propertyName))
            {
                propertyName = $"@{propertyName}";
            }
            int count = 1;
            while (propertyNameList.Contains(propertyName))
            {
                propertyName = $"{propertyName}{count++}";
            }
            propertyNameList.Add(propertyName);
            return propertyName;
        }

        public static string FormatSyntax(SyntaxNode node)
        {
            var doc = Formatter.Format(node, new AdhocWorkspace());
            var code = doc.ToFullString();
            return code;
        }
    }
}
