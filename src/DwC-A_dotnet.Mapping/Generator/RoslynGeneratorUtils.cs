extern alias Core;

using Core::DwC_A.Terms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;

namespace DwC_A.Generator
{
    internal class RoslynGeneratorUtils
    {
        private readonly HashSet<string> propertyNameList = [];

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1035:Do not use APIs banned for analyzers", Justification = "We are generating not analyzing")]
        public static string FormatSyntax(SyntaxNode node)
        {
            var code = node.NormalizeWhitespace(eol: Environment.NewLine).ToFullString();
            return code;
        }
    }
}
