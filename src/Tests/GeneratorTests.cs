using DwC_A;
using DwC_A.Generator;
using DwcaCodegen.Config;
using DwcaCodegen.Generator;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class GeneratorTests
    {
        private readonly ArchiveReader archive = new ArchiveReader("resources/dwca-mvzobs_bird-v34.48");

        Mock<IArchiveGeneratorConfiguration> mockConfig = new Mock<IArchiveGeneratorConfiguration>();

        ClassGenerator classGenerator = new ClassGenerator();

        public GeneratorTests()
        {
            mockConfig.Setup(n => n.MapMethod).Returns(true);
            mockConfig.Setup(n => n.PascalCase).Returns(true);
            mockConfig.Setup(n => n.Output).Returns("");
            mockConfig.Setup(n => n.TermAttribute).Returns(TermAttributeType.none);
            mockConfig.Setup(n => n.Properties).Returns(new Dictionary<string, PropertyConfiguration>());
            mockConfig.Setup(n => n.GetPropertyConfiguration(It.IsAny<string>())).Returns(new PropertyConfiguration());
            mockConfig.Setup(n => n.Usings).Returns(new[] { "System", "DwC_A.Extensions" });
        }

        [Fact]
        public void ShouldGeneratorOccurrenceClassAndNamespace()
        {
            mockConfig.Setup(n => n.Namespace).Returns("DwC_A");
            var classSource = classGenerator.GenerateFile(archive.CoreFile.FileMetaData, mockConfig.Object);
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

            var classGenerator = new ClassGenerator();
            var classSource = classGenerator.GenerateFile(archive.CoreFile.FileMetaData, mockConfig.Object);

            Assert.Contains("using System;", classSource);
            Assert.Contains("using DwC_A.Extensions", classSource);
        }
    }
}
