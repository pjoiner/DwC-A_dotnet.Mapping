using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;

namespace DwcaCodegen.CommandLine
{
    public class Commands
    {
        private readonly RootCommand root;
        private readonly IConfigApp configApp;

        public RootCommand Root => root;

        public Commands(IGenerator generator, IConfigApp configApp)
        {
            this.configApp = configApp;
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
                aliases: new[] { "-c", "--configName" },
                description: "Configuration name");
            configOption.AddValidator(s =>
            {
                var configName = s.GetValueOrDefault<string>();
                if (string.IsNullOrEmpty(configName))
                {
                    s.ErrorMessage = $"Configuration name required";
                    return s.ErrorMessage;
                }
                var configFile = configApp.ConfigPath(configName);
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
