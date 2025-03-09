using Microsoft.Extensions.DependencyInjection;
using System.CommandLine.Parsing;

namespace Tests
{
    public class DwcaCodegenTests
    {
        private readonly Parser parser;

        public DwcaCodegenTests()
        {
            Verifier.DerivePathInfo((sourceFile, projectDirectory, type, method) => new(
                directory: Path.Combine(projectDirectory, "GeneratedCode"),
                typeName: type.Name,
                methodName: method.Name));
            var services = new ServiceCollection();
            DwcaCodegen.Program.ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();
            parser = DwcaCodegen.Program.BuildParser(serviceProvider);
        }

        [Fact]
        public async Task ShouldGenerateDefaultConfigFile()
        {
            await parser.InvokeAsync("config init".Split());

            await VerifyFile(".dwca-codegen");
        }

        [Theory]
        [InlineData("OccurrenceDefault", "gen ./resources/dwca-mvzobs_bird-v34.48")]
        [InlineData("OccurrenceWithTerms", "gen -t name ./resources/dwca-mvzobs_bird-v34.48")]
        public async Task ShouldBuildOccurrenceWithDefaults(string methodName, string commandLine)
        {
            await parser.InvokeAsync(commandLine.Split());
            Console.WriteLine(File.ReadAllText("Occurrence.cs"));
            using var stream = new FileStream("Occurrence.cs", FileMode.Open);

            var settings = new VerifySettings();
            settings.UseMethodName(methodName);

            await Verify(stream, settings: settings, extension: "code");
        }
    }
}
