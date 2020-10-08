using DwC_A;
using DwcaCodegen.Config;
using System.IO;
using System.Text;

namespace DwcaCodegen.Generator
{
    public class ArchiveSourceGenerator : IArchiveSourceGenerator
    {
        private readonly ArchiveGeneratorConfiguration config;
        public ArchiveSourceGenerator(ArchiveGeneratorConfiguration config)
        {
            if (config == null)
            {
                this.config = new ArchiveGeneratorConfiguration();
            }
            else
            {
                this.config = config;
            }
        }

        public void GenerateSource(string fileName)
        {
            using (var archive = new ArchiveReader(fileName))
            {
                var roslynClassGenerator = new ClassGenerator(config);
                var outputPath = config.Output;
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
                var sourceFileName = Path.Combine(outputPath,
                    Path.GetFileNameWithoutExtension(archive.CoreFile.FileName) + ".cs");
                var metaData = archive.CoreFile.FileMetaData;
                var coreSource = roslynClassGenerator.GenerateFile(metaData);
                File.WriteAllText(sourceFileName, coreSource, Encoding.UTF8);
                foreach (var extension in archive.Extensions.GetFileReaders())
                {
                    var extensionFileName = Path.Combine(outputPath,
                        Path.GetFileNameWithoutExtension(extension.FileName) + ".cs");
                    var meta = extension.FileMetaData;
                    var extensionSource = roslynClassGenerator.GenerateFile(meta);
                    File.WriteAllText(extensionFileName, extensionSource, Encoding.UTF8);
                }
            }
        }
    }
}
