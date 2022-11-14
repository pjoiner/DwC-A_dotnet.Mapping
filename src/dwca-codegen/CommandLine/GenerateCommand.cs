using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;

namespace DwcaCodegen.CommandLine;

class GenerateCommand : Command
{
    public GenerateCommand(IGenerator generator, OptionBuilder optionBuilder) 
        : base("generate", "Generate class files from Darwin Core Archive meta data")
    {
        AddAlias("gen");
        var archivePathArg = BuildAndAddArchiveArgument();
        var namespaceOption = BuildAndAddOption(optionBuilder.BuildNamespaceOption());
        var pascalCaseOption = BuildAndAddOption(optionBuilder.BuildPascalCaseOption());
        var termAttributeOption = BuildAndAddOption(OptionBuilder.BuildTermAttributeOption());
        var outputOption = BuildAndAddOption(optionBuilder.BuildOutputOption());
        var mapMethodOption = BuildAndAddOption(optionBuilder.BuildIncludeMapMethodOption());

        Handler = CommandHandler.Create(generator.Generate);
    }

    private Argument<string> BuildAndAddArchiveArgument()
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
        AddArgument(archiveArgument);
        return archiveArgument;
    }

    private Option<T> BuildAndAddOption<T>(Option<T> option)
    {
        AddOption(option);
        return option;
    }
}
