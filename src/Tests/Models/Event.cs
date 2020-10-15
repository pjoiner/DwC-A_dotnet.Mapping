namespace Tests.Models
{
    using System;

    public partial class Event
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string License { get; set; }

        public string RightsHolder { get; set; }

        public string OwnerInstitutionCode { get; set; }

        public string EventID { get; set; }

        public string SamplingProtocol { get; set; }

        public double? SampleSizeValue { get; set; }

        public string SampleSizeUnit { get; set; }

        public string SamplingEffort { get; set; }

        public string EventDate { get; set; }

        public Int32? Year { get; set; }

        public Int32? Month { get; set; }

        public Int32? Day { get; set; }

        public string EventRemarks { get; set; }

        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string Locality { get; set; }

        public double? DecimalLatitude { get; set; }

        public double? DecimalLongitude { get; set; }

        public string GeodeticDatum { get; set; }
    }
}