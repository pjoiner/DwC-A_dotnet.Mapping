//HintName: MultimediaExtensions.g.cs
namespace DwC_A.Mapping
{
    using DwC_A;
    using DwC_A.Extensions;
    using System;
    using Tests.Models.WithTerms;

    public static class MultimediaExtensions
    {
        public static void MapRow(this Multimedia obj, IRow row)
        {
            obj.Id = row["id"];
            obj.Identifier = row["http://purl.org/dc/terms/identifier"];
            obj.Type = row["http://purl.org/dc/elements/1.1/type"];
            obj.Type1 = row["http://purl.org/dc/terms/type"];
            obj.Title = row["http://purl.org/dc/terms/title"];
            obj.Modified = row["http://purl.org/dc/terms/modified"];
            obj.WebStatement = row["http://ns.adobe.com/xap/1.0/rights/WebStatement"];
            obj.Source = row["http://purl.org/dc/elements/1.1/source"];
            obj.Source1 = row["http://purl.org/dc/terms/source"];
            obj.Creator = row["http://purl.org/dc/elements/1.1/creator"];
            obj.Creator1 = row["http://purl.org/dc/terms/creator"];
            obj.Description = row["http://purl.org/dc/terms/description"];
            obj.CreateDate = row["http://ns.adobe.com/xap/1.0/CreateDate"];
            obj.AccessURI = row["http://rs.tdwg.org/ac/terms/accessURI"];
            obj.Format = row["http://purl.org/dc/elements/1.1/format"];
            obj.Format1 = row["http://purl.org/dc/terms/format"];
            obj.FurtherInformationURL = row["http://rs.tdwg.org/ac/terms/furtherInformationURL"];
        }
    }
}