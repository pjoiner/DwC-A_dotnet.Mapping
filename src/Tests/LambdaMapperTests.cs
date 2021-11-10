using DwC_A;
using DwC_A.Mapping;
using DwC_A.Terms;
using System.Linq;
using Tests.Models;
using Xunit;

namespace Tests
{
    public class LambdaMapperTests
    {
        ArchiveReader archive = new ArchiveReader("resources/dwca-mvzobs_bird-v34.48");

        [Fact]
        public void ShoulMapOccurrenceRow()
        {
            IMapper<Occurrence> mapper = new LambdaMapper<Occurrence>((o, row) =>
            {
                o.Id = row["id"];
                o.DecimalLatitude = double.Parse(row[Terms.decimalLatitude]);
                o.DecimalLongitude = double.Parse(row[Terms.decimalLongitude]);
                o.ScientificName = row[Terms.scientificName];
            });

            var row = archive.CoreFile.DataRows.First();
            var occurrence = row.Map<Occurrence>(mapper);

            double expected = 42.2089;
            Assert.Equal(expected, occurrence.DecimalLatitude.Value, 4);
        }

        [Fact]
        public void ShouldReturnClassOnNullLambda()
        {
            IMapper<Occurrence> mapper = new LambdaMapper<Occurrence>(null);

            var row = archive.CoreFile.DataRows.First();
            var occurrence = row.Map<Occurrence>(mapper);

            Assert.NotNull(occurrence);
            Assert.Equal(default, occurrence.DecimalLongitude);
        }

    }
}
