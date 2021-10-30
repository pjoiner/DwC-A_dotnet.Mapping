using DwcaCodegen.Config;
using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Linq;

namespace DwcaCodegen.CommandLine
{
    internal static class OptionBuilder
    {
        public static Option<string> BuildNamespaceOption()
        {
            return new Option<string>(
                    aliases: new[] { "-n", "--namespace" },
                    description: "Namespace of generated source");
        }

        public static Option<string> BuildOutputOption()
        {
            return new Option<string>(
                    aliases: new[] { "-o", "--output" },
                    description: "Path to generated source");
        }

        public static Option<bool> BuildPascalCaseOption()
        {
            return new Option<bool>(
                aliases: new[] { "-p", "--pascalCase" },
                description: "Use Pascal Case for property and classnames");
        }

        public static Option<string> BuildTermAttributeOption()
        {
            var opt = new Option<string>(
                aliases: new[] { "-t", "--termAttribute" },
                parseArgument: (s) => 
                {
                    var argument = s.Tokens.FirstOrDefault(t => t.Type == TokenType.Argument);
                    if (!Enum.TryParse<TermAttributeType>(argument?.Value, out TermAttributeType termAttributeType))
                    {
                        s.ErrorMessage = "termAttribute must be one of none, name or index";
                    }
                    return argument.Value;
                },
                description: "Add Term attribute to properties");
            return opt;
        }

    }
}
