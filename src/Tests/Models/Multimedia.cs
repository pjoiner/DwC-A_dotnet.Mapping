namespace Tests.Models
{
    using DwC_A.Attributes;
    using System;

    public partial class Multimedia
    {
        [Term("id")]
        public string Id { get; set; }
        [Term("http://purl.org/dc/terms/identifier")]
        public string Identifier { get; set; }
        [Term("http://purl.org/dc/elements/1.1/type")]
        public string Type { get; set; }
        [Term("http://purl.org/dc/terms/type")]
        public string Type1 { get; set; }
        [Term("http://purl.org/dc/terms/title")]
        public string Title { get; set; }
        [Term("http://purl.org/dc/terms/modified")]
        public DateTime Modified { get; set; }
        [Term("http://ns.adobe.com/xap/1.0/rights/WebStatement")]
        public string WebStatement { get; set; }
        [Term("http://purl.org/dc/elements/1.1/source")]
        public string Source { get; set; }
        [Term("http://purl.org/dc/terms/source")]
        public string Source1 { get; set; }
        [Term("http://purl.org/dc/elements/1.1/creator")]
        public string Creator { get; set; }
        [Term("http://purl.org/dc/terms/creator")]
        public string Creator1 { get; set; }
        [Term("http://purl.org/dc/terms/description")]
        public string Description { get; set; }
        [Term("http://ns.adobe.com/xap/1.0/CreateDate")]
        public DateTime CreateDate { get; set; }
        [Term("http://rs.tdwg.org/ac/terms/accessURI")]
        public string AccessURI { get; set; }
        [Term("http://purl.org/dc/elements/1.1/format")]
        public string Format { get; set; }
        [Term("http://purl.org/dc/terms/format")]
        public string Format1 { get; set; }
        [Term("http://rs.tdwg.org/ac/terms/furtherInformationURL")]
        public string FurtherInformationURL { get; set; }
    }
}