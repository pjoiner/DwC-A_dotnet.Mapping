using DwC_A.Mapping;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Tests
{
    public class SourceGeneratorTest
    {
        [Theory]
        [MemberData(nameof(TestSourceFiles))]
        public async Task ShouldGenerateMapMethod((string testSource, string fileName) testData)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(testData.testSource);

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName: "Tests",
                syntaxTrees: [syntaxTree]);

            var generator = new MappingSourceGenerator();

            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            driver = driver.RunGenerators(compilation);

            await Verify(driver)
                .UseDirectory("./GeneratedCode")
                .UseFileName($"ShouldGenerateMapMethod_{testData.fileName}");
        }

        public static TheoryData<(string, string)> TestSourceFiles()
        {
            var files = Directory.GetFiles("./TestSourceFiles");
            return new TheoryData<(string, string)>(files.Select(f => (File.ReadAllText(f), Path.GetFileName(f))));
        }
    }
}
