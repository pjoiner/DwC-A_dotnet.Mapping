using System;

namespace DwC_A.Attributes
{
    /// <summary>
    /// Attribute to indicate which term to map to a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class TermAttribute : Attribute
    {
        private readonly string term;

        private readonly int index;

        public TermAttribute(string term)
        {
            this.term = term;
        }

        public TermAttribute(int index)
        {
            this.index = index;
        }

        public string Term => term;

        public int Index => index;
    }
}
