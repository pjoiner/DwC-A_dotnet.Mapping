using DwC_A.Terms;
using Microsoft.CodeAnalysis.CSharp;

namespace DwcaCodegen.Generator
{
    public static class RoslynGeneratorUtils
    {
        public static string NormalizeIdentifiers(string name, bool pascalCase = false)
        {
            var propertyName = Terms.ShortName(name);
            if (pascalCase)
            {
                return char.ToUpper(propertyName[0]) + propertyName[1..];
            }
            if (!SyntaxFacts.IsValidIdentifier(propertyName))
            {
                return $"@{propertyName}";
            }
            return propertyName;
        }

    }
}
