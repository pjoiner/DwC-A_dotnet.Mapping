using DwC_A.Mapping;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Tests
{
    public class SourceGeneratorTest
    {
        public SourceGeneratorTest()
        {
            Verifier.DerivePathInfo((sourceFile, projectDirectory, type, method) => new(
                directory: Path.Combine(projectDirectory, "GeneratedCode"),
                typeName: type.Name,
                methodName: method.Name));
        }

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
