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

        /// <summary>
        /// Map this property using the name of the term
        /// </summary>
        /// <param name="term">Fully qualified term IRI</param>
        public TermAttribute(string term)
        {
            this.term = term;
        }

        /// <summary>
        /// Map this property using the index of the row
        /// </summary>
        /// <param name="index">Zero based index to the column in the data file</param>
        public TermAttribute(int index)
        {
            this.index = index;
        }

        public string Term => term;

        public int Index => index;
    }
}
