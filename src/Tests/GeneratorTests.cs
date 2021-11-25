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

        IArchiveGeneratorConfiguration config = new GeneratorConfigFake();

        Mock<IArchiveGeneratorConfiguration> mockConfig = new Mock<IArchiveGeneratorConfiguration>();

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

        [Fact]
        public void ShouldAddUsingsWhenNamespaceIsEmpty()
        {
            mockConfig.Setup(n => n.MapMethod).Returns(true);
            mockConfig.Setup(n => n.Namespace).Returns("");
            mockConfig.Setup(n => n.PascalCase).Returns(true);
            mockConfig.Setup(n => n.Output).Returns("");
            mockConfig.Setup(n => n.TermAttribute).Returns(TermAttributeType.none);
            mockConfig.Setup(n => n.Properties).Returns(new Dictionary<string, PropertyConfiguration>());
            mockConfig.Setup(n => n.GetPropertyConfiguration(It.IsAny<string>())).Returns(new PropertyConfiguration());
            mockConfig.Setup(n => n.Usings).Returns(new[] { "System", "DwC_A.Extensions" });

            var classGenerator = new ClassGenerator();
            var classSource = classGenerator.GenerateFile(archive.CoreFile.FileMetaData, mockConfig.Object);

            Assert.Contains("using System;", classSource);



        }
    }
}
