using DwC_A;
using System.Linq;
using Tests.Models;
using Xunit;

namespace Tests
{
    public class AttributeMapperTests
    {
        private readonly ArchiveReader archive = new ArchiveReader("resources/dwca-mvzobs_bird-v34.48");

        [Fact]
        public void ShouldAddMapOccurrence()
        {
            var row = archive.Extensions
                .GetFileReaderByFileName("multimedia.txt")
                .DataRows
                .First();

            var multimedia = new Multimedia();
            multimedia.MapRow(row);

            Assert.NotNull(multimedia.Id);
        }

        [Fact]
        public void ShouldMapOccurrenceProxy()
        {
            var row = archive.CoreFile
                .DataRows
                .First();

            var occurrence = new OccurrenceProxy();
            occurrence.MapRow(row);

            Assert.NotNull(occurrence.ScientificName);
        }
    }
}
