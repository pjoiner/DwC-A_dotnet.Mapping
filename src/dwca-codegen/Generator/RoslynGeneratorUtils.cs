using DwC_A.Terms;
using Microsoft.CodeAnalysis.CSharp;

namespace DwcaCodegen.Generator
{
    public static class RoslynGeneratorUtils
    {
        public static string ModifyKeywords(string name, bool capitalize = false)
        {
            var propertyName = Terms.ShortName(name);
            if (capitalize)
            {
                return char.ToUpper(propertyName[0]) + propertyName.Substring(1);
            }
            if (!SyntaxFacts.IsValidIdentifier(propertyName))
            {
                return $"@{propertyName}";
            }
            return propertyName;
        }

    }
}
