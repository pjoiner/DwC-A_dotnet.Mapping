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
            generate.AddOption(OptionBuilder.BuildNamespaceOption());
            generate.AddOption(OptionBuilder.BuildPascalCaseOption());
            generate.AddOption(OptionBuilder.BuildOutputOption());
            generate.AddOption(OptionBuilder.BuildTermAttributeOption());

            generate.Handler = CommandHandler.Create<string, string, bool?, bool?, string, string>((archive, @namespace, pascalCase, termAttribute, output, configName) =>
            {
                generator.Generate(archive, @namespace, pascalCase, termAttribute, output, configName);  
            });
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

    }
}
