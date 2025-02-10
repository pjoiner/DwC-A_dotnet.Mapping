//HintName: OccurrenceExtensions.g.cs
namespace DwC_A.Mapping
{
    using DwC_A;
    using DwC_A.Extensions;
    using System;
    using Tests.Models.WithTerms;

    public static class OccurrenceExtensions
    {
        public static void MapRow(this Occurrence obj, IRow row)
        {
            obj.Id = row["id"];
            obj.Type = row["http://purl.org/dc/terms/type"];
            obj.Modified = row["http://purl.org/dc/terms/modified"];
            obj.Language = row["http://purl.org/dc/terms/language"];
            obj.License = row["http://purl.org/dc/terms/license"];
            obj.AccessRights = row["http://purl.org/dc/terms/accessRights"];
            obj.References = row["http://purl.org/dc/terms/references"];
            obj.InstitutionID = row["http://rs.tdwg.org/dwc/terms/institutionID"];
            obj.CollectionID = row["http://rs.tdwg.org/dwc/terms/collectionID"];
            obj.InstitutionCode = row["http://rs.tdwg.org/dwc/terms/institutionCode"];
            obj.CollectionCode = row["http://rs.tdwg.org/dwc/terms/collectionCode"];
            obj.BasisOfRecord = row["http://rs.tdwg.org/dwc/terms/basisOfRecord"];
            obj.InformationWithheld = row["http://rs.tdwg.org/dwc/terms/informationWithheld"];
            obj.DynamicProperties = row["http://rs.tdwg.org/dwc/terms/dynamicProperties"];
            obj.OccurrenceID = row["http://rs.tdwg.org/dwc/terms/occurrenceID"];
            obj.CatalogNumber = row["http://rs.tdwg.org/dwc/terms/catalogNumber"];
            obj.OccurrenceRemarks = row["http://rs.tdwg.org/dwc/terms/occurrenceRemarks"];
            obj.RecordNumber = row["http://rs.tdwg.org/dwc/terms/recordNumber"];
            obj.RecordedBy = row["http://rs.tdwg.org/dwc/terms/recordedBy"];
            obj.IndividualCount = row.ConvertNullable<int>("http://rs.tdwg.org/dwc/terms/individualCount");
            obj.Sex = row["http://rs.tdwg.org/dwc/terms/sex"];
            obj.LifeStage = row["http://rs.tdwg.org/dwc/terms/lifeStage"];
            obj.EstablishmentMeans = row["http://rs.tdwg.org/dwc/terms/establishmentMeans"];
            obj.Preparations = row["http://rs.tdwg.org/dwc/terms/preparations"];
            obj.OtherCatalogNumbers = row["http://rs.tdwg.org/dwc/terms/otherCatalogNumbers"];
            obj.AssociatedMedia = row["http://rs.tdwg.org/dwc/terms/associatedMedia"];
            obj.AssociatedSequences = row["http://rs.tdwg.org/dwc/terms/associatedSequences"];
            obj.AssociatedTaxa = row["http://rs.tdwg.org/dwc/terms/associatedTaxa"];
            obj.OrganismID = row["http://rs.tdwg.org/dwc/terms/organismID"];
            obj.AssociatedOccurrences = row["http://rs.tdwg.org/dwc/terms/associatedOccurrences"];
            obj.PreviousIdentifications = row["http://rs.tdwg.org/dwc/terms/previousIdentifications"];
            obj.SamplingProtocol = row["http://rs.tdwg.org/dwc/terms/samplingProtocol"];
            obj.EventDate = row["http://rs.tdwg.org/dwc/terms/eventDate"];
            obj.EventTime = row["http://rs.tdwg.org/dwc/terms/eventTime"];
            obj.EndDayOfYear = row.ConvertNullable<int>("http://rs.tdwg.org/dwc/terms/endDayOfYear");
            obj.Year = row.ConvertNullable<int>("http://rs.tdwg.org/dwc/terms/year");
            obj.Month = row.ConvertNullable<int>("http://rs.tdwg.org/dwc/terms/month");
            obj.Day = row.ConvertNullable<int>("http://rs.tdwg.org/dwc/terms/day");
            obj.VerbatimEventDate = row["http://rs.tdwg.org/dwc/terms/verbatimEventDate"];
            obj.Habitat = row["http://rs.tdwg.org/dwc/terms/habitat"];
            obj.FieldNumber = row["http://rs.tdwg.org/dwc/terms/fieldNumber"];
            obj.EventRemarks = row["http://rs.tdwg.org/dwc/terms/eventRemarks"];
            obj.HigherGeography = row["http://rs.tdwg.org/dwc/terms/higherGeography"];
            obj.Continent = row["http://rs.tdwg.org/dwc/terms/continent"];
            obj.WaterBody = row["http://rs.tdwg.org/dwc/terms/waterBody"];
            obj.IslandGroup = row["http://rs.tdwg.org/dwc/terms/islandGroup"];
            obj.Island = row["http://rs.tdwg.org/dwc/terms/island"];
            obj.Country = row["http://rs.tdwg.org/dwc/terms/country"];
            obj.StateProvince = row["http://rs.tdwg.org/dwc/terms/stateProvince"];
            obj.County = row["http://rs.tdwg.org/dwc/terms/county"];
            obj.Locality = row["http://rs.tdwg.org/dwc/terms/locality"];
            obj.VerbatimLocality = row["http://rs.tdwg.org/dwc/terms/verbatimLocality"];
            obj.MinimumElevationInMeters = row.ConvertNullable<double>("http://rs.tdwg.org/dwc/terms/minimumElevationInMeters");
            obj.MaximumElevationInMeters = row["http://rs.tdwg.org/dwc/terms/maximumElevationInMeters"];
            obj.MinimumDepthInMeters = row["http://rs.tdwg.org/dwc/terms/minimumDepthInMeters"];
            obj.MaximumDepthInMeters = row.ConvertNullable<double>("http://rs.tdwg.org/dwc/terms/maximumDepthInMeters");
            obj.LocationAccordingTo = row["http://rs.tdwg.org/dwc/terms/locationAccordingTo"];
            obj.LocationRemarks = row["http://rs.tdwg.org/dwc/terms/locationRemarks"];
            obj.VerbatimCoordinates = row["http://rs.tdwg.org/dwc/terms/verbatimCoordinates"];
            obj.VerbatimCoordinateSystem = row["http://rs.tdwg.org/dwc/terms/verbatimCoordinateSystem"];
            obj.DecimalLatitude = row.ConvertNullable<double>("http://rs.tdwg.org/dwc/terms/decimalLatitude");
            obj.DecimalLongitude = row.ConvertNullable<double>("http://rs.tdwg.org/dwc/terms/decimalLongitude");
            obj.GeodeticDatum = row["http://rs.tdwg.org/dwc/terms/geodeticDatum"];
            obj.CoordinateUncertaintyInMeters = row.ConvertNullable<double>("http://rs.tdwg.org/dwc/terms/coordinateUncertaintyInMeters");
            obj.GeoreferencedBy = row["http://rs.tdwg.org/dwc/terms/georeferencedBy"];
            obj.GeoreferencedDate = row["http://rs.tdwg.org/dwc/terms/georeferencedDate"];
            obj.GeoreferenceProtocol = row["http://rs.tdwg.org/dwc/terms/georeferenceProtocol"];
            obj.GeoreferenceSources = row["http://rs.tdwg.org/dwc/terms/georeferenceSources"];
            obj.GeoreferenceVerificationStatus = row["http://rs.tdwg.org/dwc/terms/georeferenceVerificationStatus"];
            obj.EarliestEonOrLowestEonothem = row["http://rs.tdwg.org/dwc/terms/earliestEonOrLowestEonothem"];
            obj.EarliestEraOrLowestErathem = row["http://rs.tdwg.org/dwc/terms/earliestEraOrLowestErathem"];
            obj.EarliestPeriodOrLowestSystem = row["http://rs.tdwg.org/dwc/terms/earliestPeriodOrLowestSystem"];
            obj.EarliestEpochOrLowestSeries = row["http://rs.tdwg.org/dwc/terms/earliestEpochOrLowestSeries"];
            obj.EarliestAgeOrLowestStage = row["http://rs.tdwg.org/dwc/terms/earliestAgeOrLowestStage"];
            obj.Group = row["http://rs.tdwg.org/dwc/terms/group"];
            obj.Formation = row["http://rs.tdwg.org/dwc/terms/formation"];
            obj.Member = row["http://rs.tdwg.org/dwc/terms/member"];
            obj.IdentifiedBy = row["http://rs.tdwg.org/dwc/terms/identifiedBy"];
            obj.DateIdentified = row.ConvertNullable<DateTime>("http://rs.tdwg.org/dwc/terms/dateIdentified");
            obj.IdentificationReferences = row["http://rs.tdwg.org/dwc/terms/identificationReferences"];
            obj.IdentificationRemarks = row["http://rs.tdwg.org/dwc/terms/identificationRemarks"];
            obj.IdentificationQualifier = row["http://rs.tdwg.org/dwc/terms/identificationQualifier"];
            obj.IdentificationVerificationStatus = row["http://rs.tdwg.org/dwc/terms/identificationVerificationStatus"];
            obj.TypeStatus = row["http://rs.tdwg.org/dwc/terms/typeStatus"];
            obj.ScientificName = row["http://rs.tdwg.org/dwc/terms/scientificName"];
            obj.HigherClassification = row["http://rs.tdwg.org/dwc/terms/higherClassification"];
            obj.Kingdom = row["http://rs.tdwg.org/dwc/terms/kingdom"];
            obj.Phylum = row["http://rs.tdwg.org/dwc/terms/phylum"];
            obj.Class = row["http://rs.tdwg.org/dwc/terms/class"];
            obj.Order = row["http://rs.tdwg.org/dwc/terms/order"];
            obj.Family = row["http://rs.tdwg.org/dwc/terms/family"];
            obj.Genus = row["http://rs.tdwg.org/dwc/terms/genus"];
            obj.SpecificEpithet = row["http://rs.tdwg.org/dwc/terms/specificEpithet"];
            obj.InfraspecificEpithet = row["http://rs.tdwg.org/dwc/terms/infraspecificEpithet"];
            obj.TaxonRank = row["http://rs.tdwg.org/dwc/terms/taxonRank"];
            obj.NomenclaturalCode = row["http://rs.tdwg.org/dwc/terms/nomenclaturalCode"];
        }
    }
}