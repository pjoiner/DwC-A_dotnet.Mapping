namespace Tests.Models.WithIndex
{
    using DwC_A.Attributes;

    public partial class Multimedia
    {
        [Term(0)]
        public string Id { get; set; }
        [Term(1)]
        public string Identifier { get; set; }
        [Term(2)]
        public string Type { get; set; }
        [Term(3)]
        public string Type1 { get; set; }
        [Term(4)]
        public string Title { get; set; }
        [Term(5)]
        public string Modified { get; set; }
        [Term(6)]
        public string WebStatement { get; set; }
        [Term(7)]
        public string Source { get; set; }
        [Term(8)]
        public string Source1 { get; set; }
        [Term(9)]
        public string Creator { get; set; }
        [Term(10)]
        public string Creator1 { get; set; }
        [Term(11)]
        public string Description { get; set; }
        [Term(12)]
        public string CreateDate { get; set; }
        [Term(13)]
        public string AccessURI { get; set; }
        [Term(14)]
        public string Format { get; set; }
        [Term(15)]
        public string Format1 { get; set; }
        [Term(16)]
        public string FurtherInformationURL { get; set; }
    }
}