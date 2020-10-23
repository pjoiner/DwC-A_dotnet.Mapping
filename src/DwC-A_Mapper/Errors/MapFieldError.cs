using System;

namespace MapErrors
{
    public class MapFieldError : IMapFieldError
    {
        private readonly string term;
        private readonly string value;
        private readonly Type destinationType;

        public string Term => term;

        public string Value => value;

        public Type DestinationType => destinationType;

        public MapFieldError(string term, string value, Type destinationType)
        {
            this.term = term;
            this.value = value;
            this.destinationType = destinationType;
        }

        public override string ToString()
        {
            return $"Could not convert term {term} with value {value} to {destinationType.FullName}";
        }
    }
}
