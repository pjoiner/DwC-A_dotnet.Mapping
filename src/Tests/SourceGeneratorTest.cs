using DwC_A.Mapping;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Tests
{
    public class SourceGeneratorTest
    {
        [Fact]
        public async Task ShouldGenerateMapMethod()
        {
            var testSource = File.ReadAllText("./TestSourceFiles/Multimedia.cs");
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(testSource);

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName: "Tests",
                syntaxTrees: [syntaxTree]);

            var generator = new MappingSourceGenerator();

            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            driver = driver.RunGenerators(compilation);

            await Verify(driver);
        }
    }
}
