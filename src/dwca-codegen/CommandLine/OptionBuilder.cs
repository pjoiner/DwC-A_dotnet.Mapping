﻿using DwcaCodegen.Config;
using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Linq;

namespace DwcaCodegen.CommandLine
{
    internal class OptionBuilder
    {
        private readonly IArchiveGeneratorConfiguration config;

        public OptionBuilder(ArchiveGeneratorConfigFactory factory)
        {
            this.config = factory.BuildConfiguration();
        }

        public Option<string> BuildNamespaceOption()
        {
            return new Option<string>(
                    aliases: new[] { "-n", "--namespace" },
                    description: "Namespace of generated source",
                    getDefaultValue: () => config.Namespace );
        }

        public Option<string> BuildOutputOption()
        {
            return new Option<string>(
                    aliases: new[] { "-o", "--output" },
                    description: "Path to generated source",
                    getDefaultValue: () => config.Output );
        }

        public Option<bool> BuildPascalCaseOption()
        {
            return new Option<bool>(
                aliases: new[] { "-p", "--pascalCase" },
                description: "Use Pascal Case for property and classnames",
                getDefaultValue: () => config.PascalCase );
        }

        public Option<string> BuildTermAttributeOption()
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
            opt.SetDefaultValue(config.TermAttribute);
            return opt;
        }

    }
}
