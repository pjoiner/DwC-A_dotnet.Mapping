using DwC_A;
using DwC_A.Mapping;

namespace Tests
{
    public class AttributeMapperTests
    {
        private readonly ArchiveReader archive = new ArchiveReader("resources/dwca-mvzobs_bird-v34.48");

        [Fact]
        public void ShouldCreateMapper()
        {
            var row = archive.CoreFile
                .DataRows
                .First();

            var mapper = MapperFactory.CreateMapper<Models.WithTerms.Occurrence>((o, row) => o.MapRow(row));

            var occurrence = row.Map(mapper);

            Assert.NotNull(occurrence.Id);
        }

        [Fact]
        public void ShouldAddMapRowMethod()
        {
            var row = archive.CoreFile
                .DataRows
                .First();

            var occurrence = new Models.WithTerms.Occurrence();
            occurrence.MapRow(row);

            Assert.NotNull(occurrence.ScientificName);
        }

        [Fact]
        public void ShouldMapByIndex()
        {
            var row = archive.CoreFile
                .DataRows
                .First();

            var mapper = MapperFactory.CreateMapper<Models.WithIndex.Occurrence>((o, row) => o.MapRow(row));

            var occurrence = row.Map(mapper);

            Assert.NotNull(occurrence.Id);
        }

        [Fact]
        public void ShouldMapAllOccurrences()
        {
            var mapper = MapperFactory.CreateMapper<Models.WithTerms.Occurrence>((o, row) => o.MapRow(row));

            foreach (var occurence in archive.CoreFile.Map<Models.WithTerms.Occurrence>(mapper))
            {
                Assert.NotNull(occurence.Id);
            }
        }

    }
}
