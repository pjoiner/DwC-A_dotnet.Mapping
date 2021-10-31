using DwcaCodegen.Config;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace DwcaCodegen.CommandLine
{
    public class ConfigCommand : Command
    {
        private readonly IConfigApp configApp;

        public ConfigCommand(IConfigApp configApp) :
            base("configuration", "Manage property configuration")
        {
            this.configApp = configApp;
            AddAlias("config");
            AddCommand(BuildListCommand());
        }

        private Command BuildListCommand()
        {
            var list = new Command("list", "List configuration items");
            list.Handler = CommandHandler.Create(() =>
            {
                configApp.ConfigList();
            });
            return list;
        }
    }
}