namespace Tests.Models
{
    using DwC_A.Attributes;
    using System;

    public partial class OccurrenceAnnotatedWithIndex
    {
        [Term(0)]
        public string Id { get; set; }
        [Term(1)]
        public string Type { get; set; }
        [Term(2)]
        public DateTime Modified { get; set; }
        [Term(3)]
        public string Language { get; set; }
        [Term(4)]
        public string License { get; set; }
        [Term(5)]
        public string AccessRights { get; set; }
        [Term(6)]
        public string References { get; set; }
        [Term(7)]
        public string InstitutionID { get; set; }
        [Term(8)]
        public string CollectionID { get; set; }
        [Term(9)]
        public string InstitutionCode { get; set; }
        [Term(10)]
        public string CollectionCode { get; set; }
        [Term(11)]
        public string BasisOfRecord { get; set; }
        [Term(12)]
        public string InformationWithheld { get; set; }
        [Term(13)]
        public string DynamicProperties { get; set; }
        [Term(14)]
        public string OccurrenceID { get; set; }
        [Term(15)]
        public string CatalogNumber { get; set; }
        [Term(16)]
        public string OccurrenceRemarks { get; set; }
        [Term(17)]
        public string RecordNumber { get; set; }
        [Term(18)]
        public string RecordedBy { get; set; }
        [Term(19)]
        public int? IndividualCount { get; set; }
        [Term(20)]
        public string Sex { get; set; }
        [Term(21)]
        public string LifeStage { get; set; }
        [Term(22)]
        public string EstablishmentMeans { get; set; }
        [Term(23)]
        public string Preparations { get; set; }
        [Term(24)]
        public string OtherCatalogNumbers { get; set; }
        [Term(25)]
        public string AssociatedMedia { get; set; }
        [Term(26)]
        public string AssociatedSequences { get; set; }
        [Term(27)]
        public string AssociatedTaxa { get; set; }
        [Term(28)]
        public string OrganismID { get; set; }
        [Term(29)]
        public string AssociatedOccurrences { get; set; }
        [Term(30)]
        public string PreviousIdentifications { get; set; }
        [Term(31)]
        public string SamplingProtocol { get; set; }
        [Term(32)]
        public string EventDate { get; set; }
        [Term(33)]
        public string EventTime { get; set; }
        [Term(34)]
        public string EndDayOfYear { get; set; }
        [Term(35)]
        public int Year { get; set; }
        [Term(36)]
        public int Month { get; set; }
        [Term(37)]
        public int Day { get; set; }
        [Term(38)]
        public string VerbatimEventDate { get; set; }
        [Term(39)]
        public string Habitat { get; set; }
        [Term(40)]
        public string FieldNumber { get; set; }
        [Term(41)]
        public string EventRemarks { get; set; }
        [Term(42)]
        public string HigherGeography { get; set; }
        [Term(43)]
        public string Continent { get; set; }
        [Term(44)]
        public string WaterBody { get; set; }
        [Term(45)]
        public string IslandGroup { get; set; }
        [Term(46)]
        public string Island { get; set; }
        [Term(47)]
        public string Country { get; set; }
        [Term(48)]
        public string StateProvince { get; set; }
        [Term(49)]
        public string County { get; set; }
        [Term(50)]
        public string Locality { get; set; }
        [Term(51)]
        public string VerbatimLocality { get; set; }
        [Term(52)]
        public string MinimumElevationInMeters { get; set; }
        [Term(53)]
        public string MaximumElevationInMeters { get; set; }
        [Term(54)]
        public string MinimumDepthInMeters { get; set; }
        [Term(55)]
        public string MaximumDepthInMeters { get; set; }
        [Term(56)]
        public string LocationAccordingTo { get; set; }
        [Term(57)]
        public string LocationRemarks { get; set; }
        [Term(58)]
        public string VerbatimCoordinates { get; set; }
        [Term(59)]
        public string VerbatimCoordinateSystem { get; set; }
        [Term(60)]
        public double DecimalLatitude { get; set; }
        [Term(61)]
        public double DecimalLongitude { get; set; }
        [Term(62)]
        public string GeodeticDatum { get; set; }
        [Term(63)]
        public string CoordinateUncertaintyInMeters { get; set; }
        [Term(64)]
        public string GeoreferencedBy { get; set; }
        [Term(65)]
        public string GeoreferencedDate { get; set; }
        [Term(66)]
        public string GeoreferenceProtocol { get; set; }
        [Term(67)]
        public string GeoreferenceSources { get; set; }
        [Term(68)]
        public string GeoreferenceVerificationStatus { get; set; }
        [Term(69)]
        public string EarliestEonOrLowestEonothem { get; set; }
        [Term(70)]
        public string EarliestEraOrLowestErathem { get; set; }
        [Term(71)]
        public string EarliestPeriodOrLowestSystem { get; set; }
        [Term(72)]
        public string EarliestEpochOrLowestSeries { get; set; }
        [Term(73)]
        public string EarliestAgeOrLowestStage { get; set; }
        [Term(74)]
        public string Group { get; set; }
        [Term(75)]
        public string Formation { get; set; }
        [Term(76)]
        public string Member { get; set; }
        [Term(77)]
        public string IdentifiedBy { get; set; }
        [Term(78)]
        public string DateIdentified { get; set; }
        [Term(79)]
        public string IdentificationReferences { get; set; }
        [Term(80)]
        public string IdentificationRemarks { get; set; }
        [Term(81)]
        public string IdentificationQualifier { get; set; }
        [Term(82)]
        public string IdentificationVerificationStatus { get; set; }
        [Term(83)]
        public string TypeStatus { get; set; }
        [Term(84)]
        public string ScientificName { get; set; }
        [Term(85)]
        public string HigherClassification { get; set; }
        [Term(86)]
        public string Kingdom { get; set; }
        [Term(87)]
        public string Phylum { get; set; }
        [Term(88)]
        public string Class { get; set; }
        [Term(89)]
        public string Order { get; set; }
        [Term(90)]
        public string Family { get; set; }
        [Term(91)]
        public string Genus { get; set; }
        [Term(92)]
        public string SpecificEpithet { get; set; }
        [Term(93)]
        public string InfraspecificEpithet { get; set; }
        [Term(94)]
        public string TaxonRank { get; set; }
        [Term(95)]
        public string NomenclaturalCode { get; set; }
    }
}