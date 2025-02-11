using DwC_A.Config;
using DwcaCodegen.Config;
using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Linq;

namespace DwcaCodegen.CommandLine;

internal class OptionBuilder(ArchiveGeneratorConfigFactory factory)
{
    private readonly IGeneratorConfiguration config = factory.BuildConfiguration();

    public Option<string> BuildNamespaceOption()
    {
        return new Option<string>(
                aliases: ["-n", "--namespace"],
                description: "Namespace of generated source",
                getDefaultValue: () => config.Namespace);
    }

    public Option<string> BuildOutputOption()
    {
        return new Option<string>(
                aliases: ["-o", "--output"],
                description: "Path to generated source",
                getDefaultValue: () => config.Output);
    }

    public Option<bool> BuildPascalCaseOption()
    {
        return new Option<bool>(
            aliases: ["-p", "--pascalCase"],
            description: "Use Pascal Case for property and classnames",
            getDefaultValue: () => config.PascalCase);
    }

    public static Option<TermAttributeType> BuildTermAttributeOption()
    {
        var opt = new Option<TermAttributeType>(
            aliases: ["-t", "--termAttribute"],
            parseArgument: (s) =>
            {
                var argument = s.Tokens.FirstOrDefault(t => t.Type == TokenType.Argument);
                if (!Enum.TryParse<TermAttributeType>(argument?.Value, out TermAttributeType termAttributeType))
                {
                    s.ErrorMessage = "termAttribute must be one of none, name or index";
                }
                return termAttributeType;
            },
            description: "Add Term attribute to properties");
        return opt;
    }

    public Option<bool> BuildIncludeMapMethodOption()
    {
        return new Option<bool>(
            aliases: ["-m", "--mapMethod"],
            description: "Include MapRow method",
            getDefaultValue: () => config.MapMethod);
    }

}
