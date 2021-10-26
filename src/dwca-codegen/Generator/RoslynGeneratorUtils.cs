using DwC_A.Terms;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

namespace DwcaCodegen.Generator
{
    internal class RoslynGeneratorUtils
    {
        private readonly HashSet<string> propertyNameList = new HashSet<string>();

        public string NormalizeIdentifiers(string name, bool pascalCase = false)
        {
            var propertyName = Terms.ShortName(name);
            if (pascalCase)
            {
                propertyName = char.ToUpper(propertyName[0]) + propertyName[1..];
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

    }
}
