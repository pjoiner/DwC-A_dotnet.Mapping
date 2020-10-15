namespace Tests.Models
{
    using System;

    public partial class Occurrence
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string License { get; set; }

        public string RightsHolder { get; set; }

        public string InstitutionCode { get; set; }

        public string OwnerInstitutionCode { get; set; }

        public string BasisOfRecord { get; set; }

        public string OccurrenceID { get; set; }

        public string RecordedBy { get; set; }

        public Int32? IndividualCount { get; set; }

        public string OrganismQuantity { get; set; }

        public string OrganismQuantityType { get; set; }

        public string OccurrenceStatus { get; set; }

        public string PreviousIdentifications { get; set; }

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

        public string Group { get; set; }

        public string IdentifiedBy { get; set; }

        public string IdentificationRemarks { get; set; }

        public string TaxonID { get; set; }

        public string ScientificName { get; set; }

        public string Kingdom { get; set; }

        public string Phylum { get; set; }

        public string Class { get; set; }

        public string Order { get; set; }

        public string Family { get; set; }

        public string Genus { get; set; }

        public string TaxonRank { get; set; }

        public string ScientificNameAuthorship { get; set; }

        public string TaxonomicStatus { get; set; }
    }
}