using DwC_A;
using DwC_A.Config;
using DwC_A.Generator;
using Moq;

namespace Tests
{
    public class GeneratorTests
    {
        private readonly ArchiveReader archive = new("resources/dwca-mvzobs_bird-v34.48");

        private readonly Mock<IGeneratorConfiguration> mockConfig = new();

        public GeneratorTests()
        {
            mockConfig.Setup(n => n.MapMethod).Returns(true);
            mockConfig.Setup(n => n.PascalCase).Returns(true);
            mockConfig.Setup(n => n.Output).Returns("");
            mockConfig.Setup(n => n.TermAttribute).Returns(TermAttributeType.none);
            mockConfig.Setup(n => n.Properties).Returns(new Dictionary<string, PropertyConfiguration>());
            mockConfig.Setup(n => n.GetPropertyConfiguration(It.IsAny<string>())).Returns(new PropertyConfiguration());
            mockConfig.Setup(n => n.Usings).Returns(["System", "DwC_A.Extensions"]);
        }

        [Fact]
        public void ShouldGeneratorOccurrenceClassAndNamespace()
        {
            mockConfig.Setup(n => n.Namespace).Returns("DwC_A");
            var classSource = ClassGenerator.GenerateFile(archive.CoreFile.FileMetaData, mockConfig.Object);
            Assert.Contains("namespace DwC_A", classSource);
            Assert.Contains("public partial class Occurrence", classSource);
            //var methodSyntax = MapMethodGenerator.MapStaticInstanceMethodSyntax(classDeclaration);
            //var methodSource = RoslynGeneratorUtils.FormatSyntax(methodSyntax);
            //classDeclaration = classDeclaration.AddMembers(methodSyntax);
            //var source = RoslynGeneratorUtils.FormatSyntax(classDeclaration);
        }

        [Fact]
        public void ShouldAddTopLevelUsingsWhenNamespaceIsEmpty()
        {

            var classSource = ClassGenerator.GenerateFile(archive.CoreFile.FileMetaData, mockConfig.Object);

            Assert.Contains("using System;", classSource);
            Assert.Contains("using DwC_A.Extensions", classSource);
        }
    }
}
