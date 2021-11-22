using DwC_A;
using DwC_A.Generator;
using DwcaCodegen.Config;
using DwcaCodegen.Generator;
using Xunit;

namespace Tests
{
    public class GeneratorTests
    {
        private readonly ArchiveReader archive = new ArchiveReader("resources/dwca-mvzobs_bird-v34.48");

        IArchiveGeneratorConfiguration config = new GeneratorConfigFake();

        [Fact]
        public void ShouldGeneratorOccurrenceClass()
        {
            var classGenerator = new ClassGenerator();
            var classDeclaration = classGenerator.GeneratClassSyntax(archive.CoreFile.FileMetaData, config);
            var classSource = RoslynGeneratorUtils.FormatSyntax(classDeclaration);
            var methodSyntax = MapMethodGenerator.MapStaticInstanceMethodSyntax(classDeclaration);
            var methodSource = RoslynGeneratorUtils.FormatSyntax(methodSyntax);
            classDeclaration = classDeclaration.AddMembers(methodSyntax);
            var source = RoslynGeneratorUtils.FormatSyntax(classDeclaration);
            Assert.NotNull(classDeclaration);
        }
    }
}
