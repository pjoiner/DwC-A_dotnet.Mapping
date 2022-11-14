using System.CommandLine;
using System.CommandLine.Invocation;

namespace DwcaCodegen.CommandLine;

public class ConfigCommand : Command
{
    private readonly IConfigApp configApp;

    public ConfigCommand(IConfigApp configApp) :
        base("configuration", "Manage property configuration")
    {
        this.configApp = configApp;
        AddAlias("config");
        AddCommand(BuildListCommand());
        AddCommand(BuildInitCommand());
    }

    private Command BuildListCommand()
    {
        var list = new Command("list", "List configuration items")
        {
            Handler = CommandHandler.Create(configApp.ConfigList)
        };
        return list;
    }

    private Command BuildInitCommand()
    {
        var init = new Command("init", "Create new configuration file")
        {
            Handler = CommandHandler.Create(configApp.ConfigInit)
        };
        return init;
    }
}