﻿using DwC_A;
using DwC_A.Mapping;
using DwC_A.Terms;

namespace Tests
{
    public class LambdaMapperTests
    {
        private readonly ArchiveReader archive = new ArchiveReader("resources/dwca-mvzobs_bird-v34.48");

        private readonly IMapper<Models.Occurrence> mapper = MapperFactory.CreateMapper<Models.Occurrence>((o, row) =>
        {
            o.Id = row["id"];
            o.DecimalLatitude = row[Terms.decimalLatitude] == null ? null : double.Parse(row[Terms.decimalLatitude]);
            o.DecimalLongitude = row[Terms.decimalLongitude] == null ? null : double.Parse(row[Terms.decimalLongitude]);
            o.ScientificName = row[Terms.scientificName];
        });

        private readonly double?[] expectedLatitudes = {
                42.2089000000,
                31.9150000000,
                37.8893164400,
                39.4049420000,
                35.2650000000,
                -35.2045100000,
                31.9150000000,
                42.5790000000,
                null,
                37.2290700000
            };


        [Fact]
        public void ShoulMapOccurrenceRow()
        {
            var row = archive.CoreFile.DataRows.First();
            var occurrence = row.Map<Models.Occurrence>(mapper);

            double expected = 42.2089;
            Assert.Equal(expected, occurrence.DecimalLatitude.Value, 4);
        }

        [Fact]
        public void ShouldMapFromMapper()
        {
            var row = archive.CoreFile.DataRows.First();
            var occurrence = mapper.MapRow(row);

            double expected = 42.2089;
            Assert.Equal(expected, occurrence.DecimalLatitude.Value, 4);
        }

        [Fact]
        public void ShouldReturnClassOnNullLambda()
        {
            IMapper<Models.Occurrence> mapper1 = MapperFactory.CreateMapper<Models.Occurrence>(null);

            var row = archive.CoreFile.DataRows.First();
            var occurrence = row.Map<Models.Occurrence>(mapper1);

            Assert.NotNull(occurrence);
            Assert.Equal(default, occurrence.DecimalLongitude);
        }

        [Fact]
        public void ShouldMapFileReader()
        {
            var actual = archive.CoreFile
                .DataRows
                .Take(10)
                .Map<Models.Occurrence>(mapper)
                .Select(o => o.DecimalLatitude);

            Assert.Equal(expectedLatitudes, actual);
        }

        [Fact]
        public void ShouldMapIEnumeration()
        {
            var actual = archive.CoreFile
                .DataRows
                .Take(10)
                .Map<Models.Occurrence>(mapper)
                .Select(o => o.DecimalLatitude);

            Assert.Equal(expectedLatitudes, actual);
        }
    }
}
