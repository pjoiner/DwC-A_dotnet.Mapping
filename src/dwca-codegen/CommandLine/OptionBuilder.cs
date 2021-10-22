using System.CommandLine;

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

        public static Option<bool> BuildTermAttributeOption()
        {
            return new Option<bool>(
                aliases: new[] { "-t", "--termAttribute" },
                description: "Add Term attribute to properties");
        }

    }
}
