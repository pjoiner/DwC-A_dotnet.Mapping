﻿using DwcaCodegen.Config;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace DwcaCodegen.CommandLine
{
    public class ConfigCommand
    {
        private readonly Command command;
        private readonly IConfigApp configApp;
        public Command Command => command;

        public ConfigCommand(IConfigApp configApp)
        {
            this.configApp = configApp;
            command = new Command("configuration", "Manage property configuration");
            command.AddAlias("config");
            command.AddCommand(BuildListCommand());
            command.AddCommand(BuildAddCommand());
            command.AddCommand(BuildDeleteCommand());
            command.AddCommand(BuildNewCommand());
            command.AddCommand(BuildAddUsingCommand());
        }

        private Command BuildListCommand()
        {
            var list = new Command("list", "List configuration items");
            list.Handler = CommandHandler.Create<string>((configName) =>
            {
                configApp.ConfigList(configName);
            });
            return list;
        }

        private Command BuildAddCommand()
        {
            var add = new Command("add", "Add new Property configuration");
            add.AddArgument(BuildTermArgument());
            add.AddOption(BuildNameOption());
            add.AddOption(BuildIncludeOption());
            add.AddOption(BuildTypeOption());
            add.Handler = CommandHandler.Create<string, string, string, bool, string>((configFile, term, name, include, type) =>
            {
                configApp.ConfigAdd(configFile, term, name, include, type);   
            });
            return add;
        }

        private Command BuildDeleteCommand()
        {
            var delete = new Command("delete", "Delete Property Configuration");
            delete.AddArgument(BuildTermArgument());
            delete.Handler = CommandHandler.Create<string, string>((configName, term) =>
            {
                configApp.ConfigDelete(configName, term);
            });
            return delete;
        }

        private Command BuildNewCommand()
        {
            var command = new Command("new", "Create a new configuration file");
            command.AddArgument(BuildNewConfigArgument());
            command.AddOption(BuildEmptyOption());
            command.AddOption(OptionBuilder.BuildNamespaceOption());
            command.AddOption(OptionBuilder.BuildOutputOption());
            command.AddOption(OptionBuilder.BuildPascalCaseOption());
            command.AddOption(OptionBuilder.BuildTermAttributeOption());
            command.Handler = CommandHandler.Create<string, bool, string, string, bool?, TermAttributeType?>((configName, empty, @namespace, output, pascalCase, termAttribute) =>
            {
                configApp.ConfigNew(configName, empty, @namespace, output, pascalCase, termAttribute);
            });
            return command;
        }

        private Command BuildAddUsingCommand()
        {
            var command = new Command("addUsing", "Add a namespace to the configuration");
            command.AddArgument(BuildNamespaceNameArgument());
            command.Handler = CommandHandler.Create<string, string>((configName, namespaceName) =>
            {
                configApp.ConfigAddUsing(configName, namespaceName);
            });
            return command;
        }

        private Argument<string> BuildNewConfigArgument()
        {
            return new Argument<string>(
                name: "configName",
                description: "Name of the new configuration to create");
        }

        private Argument<string> BuildTermArgument()
        {
            return new Argument<string>(
                name: "term",
                description: "Fully qualified uri for term");
        }

        private Argument<string> BuildNamespaceNameArgument()
        {
            return new Argument<string>(
                name: "namespaceName",
                description: "Fully qualified namespace to add to generated class");
        }

        private Option<string> BuildNameOption()
        {
            return new Option<string>(
                aliases: new[] { "-n", "--name" },
                description: "Name override");
        }

        private Option<bool> BuildIncludeOption()
        {
            return new Option<bool>(
                aliases: new[] { "-i", "--include" },
                getDefaultValue: () => true,
                description: "Indicate whether this term should be included in the generated class");
        }

        private Option<string> BuildTypeOption()
        {
            return new Option<string>(
                aliases: new[] { "-t", "--type" },
                getDefaultValue: () => "string",
                description: "Type of the property");
        }

        private Option<bool> BuildEmptyOption()
        {
            return new Option<bool>(
                aliases: new[] {"-e", "--empty"},
                getDefaultValue: () => false,
                description: "Create a new empty configuration" );
        }
    }
}
