using DwcaCodegen.Config;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;

namespace DwcaCodegen.CommandLine
{
    class GenerateCommand : Command
    {
        public GenerateCommand(IGenerator generator) 
            : base("generate", "Generate class files from Darwin Core Archive meta data")
        {
            AddAlias("gen");
            AddArgument(BuildArchiveArgument());
            AddOption(OptionBuilder.BuildNamespaceOption());
            AddOption(OptionBuilder.BuildPascalCaseOption());
            AddOption(OptionBuilder.BuildOutputOption());
            AddOption(OptionBuilder.BuildTermAttributeOption());

            Handler = CommandHandler.Create<string, string, bool?, TermAttributeType?, string, string>((archive, @namespace, pascalCase, termAttribute, output, configName) =>
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
