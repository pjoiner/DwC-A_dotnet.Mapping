using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;

namespace DwcaCodegen.CommandLine
{
    public class Commands
    {
        private readonly RootCommand root;

        public RootCommand Root => root;

        public Commands(IGenerator generator, IConfigApp configApp)
        {
            root = new RootCommand();
            root.AddGlobalOption(BuildConfigOption());
            var generate = new GenerateCommand(generator);
            var config = new ConfigCommand(configApp);
            root.AddCommand(generate.Command);
            root.AddCommand(config.Command);
        }

        private Option<string> BuildConfigOption()
        {
            var configOption = new Option<string>(
                aliases: new[] { "-c", "--configFile" },
                description: "Configuration file",
                getDefaultValue: () => "termConfig.json");
            configOption.AddValidator(s =>
            {
                var configFile = s.GetValueOrDefault<string>();
                if (!File.Exists(configFile))
                {
                    s.ErrorMessage = $"Configuration file {configFile} not found";
                }
                return s.ErrorMessage;
            });
            return configOption;
        }
    }
}
