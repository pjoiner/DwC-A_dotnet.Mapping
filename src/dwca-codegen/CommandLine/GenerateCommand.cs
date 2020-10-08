using DwcaCodegen.Config;
using DwcaCodegen.Generator;
using DwcaCodegen.Utils;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;

namespace DwcaCodegen.CommandLine
{
    class GenerateCommand
    {
        private readonly RootCommand root;

        public RootCommand RootCommand => root;

        public GenerateCommand()
        {
            root = new RootCommand("A tool to generate class files from Darwin Core Archive meta data");
            root.AddArgument(BuildArchiveArgument());
            root.AddOption(BuildNamespaceOption());
            root.AddOption(BuildCapitalizeOption());
            root.AddOption(BuildOutputOption());
            root.AddOption(BuildConfigOption());

            root.Handler = CommandHandler.Create<string, string, bool, string, string>((archive, @namespace, capitalize, output, config) =>
            {
                Console.WriteLine($"Generating files for archive {archive} for namespace {@namespace} in {output}");
                var archiveGeneratorConfiguration = new ArchiveGeneratorConfiguration();
                archiveGeneratorConfiguration.ReadFromFile(config, new JsonSerializer());
                archiveGeneratorConfiguration.AddNamespace(@namespace)
                    .AddCapitalize(capitalize)
                    .AddOutput(output);
                var archiveSourceGenerator = new ArchiveSourceGenerator(archiveGeneratorConfiguration);
                archiveSourceGenerator.GenerateSource(archive);
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

        private Option<string> BuildConfigOption()
        {
            var configOption = new Option<string>(
                aliases: new[] { "-c", "--config" },
                description: "Configuration file",
                getDefaultValue: () => "termConfig.json");
            configOption.AddValidator(s =>
            {
                var configFile = s.GetValueOrDefault<string>();
                if(!File.Exists(configFile))
                {
                    s.ErrorMessage = $"Configuration file {configFile} not found";
                }
                return s.ErrorMessage;
            });
            return configOption;
        }
    }
}
