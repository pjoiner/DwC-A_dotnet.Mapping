﻿using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;

namespace DwcaCodegen.CommandLine
{
    class GenerateCommand
    {
        private readonly Command generate;

        public Command Command => generate;

        public GenerateCommand(IGenerator generator)
        {
            generate = new Command("generate", "Generate class files from Darwin Core Archive meta data");
            generate.AddAlias("gen");
            generate.AddArgument(BuildArchiveArgument());
            generate.AddOption(BuildNamespaceOption());
            generate.AddOption(BuildCapitalizeOption());
            generate.AddOption(BuildOutputOption());
            generate.AddOption(BuildAddAttributesOption());

            generate.Handler = CommandHandler.Create<string, string, bool, string, string, bool>((archive, @namespace, capitalize, output, configName, attributes) =>
            {
                generator.Generate(archive, @namespace, capitalize, output, configName, attributes);  
            });
        }

        private Option<string> BuildNamespaceOption()
        {
            return new Option<string>(
                    aliases: new[] { "-n", "--namespace" },
                    getDefaultValue: () => "DwC",
                    description: "Namespace of generated source");
        }

        private Option<string> BuildOutputOption()
        {
            return new Option<string>(
                    aliases: new[] {"-o", "--output"},
                    getDefaultValue: () => ".",
                    description: "Path to generated source");
        }

        private Argument<string> BuildArchiveArgument()
        {
            var archiveArgument = new Argument<string>(
                    name: "archive",
                    description: "Archive zip file name or path to archive folder");
            archiveArgument.AddValidator(s =>
            {
                var archive = s.GetValueOrDefault<string>();
                if (string.IsNullOrEmpty(archive))
                {
                    s.ErrorMessage = "Path to archive file or folder missing";
                }
                else if (!Directory.Exists(archive) && !File.Exists(archive))
                {
                    s.ErrorMessage = $"Archive file or path {archive} not found";
                }
                return s.ErrorMessage;
            });
            return archiveArgument;
        }

        private Option<bool> BuildCapitalizeOption()
        {
            return new Option<bool>(
                aliases: new[] { "-u", "--capitalize" },
                description: "Captialize property and classnames",
                getDefaultValue: () => true);
        }

        private Option<bool> BuildAddAttributesOption()
        {
            return new Option<bool>(
                aliases: new[] { "-a", "--attributes" },
                description: "Add attributes",
                getDefaultValue: () => false);
        }
    }
}
