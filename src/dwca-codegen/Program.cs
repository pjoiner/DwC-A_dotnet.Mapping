using DwcaCodegen;
using DwcaCodegen.CommandLine;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

var application = new Application();
var rootCommand = new Commands(application, application).Root;
var commandLine = new CommandLineBuilder(rootCommand)
                        .UseDefaults()
                        .Build();

await commandLine.InvokeAsync(args);
