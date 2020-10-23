using System;

namespace MapErrors
{
    public interface IMapFieldError
    {
        string Term { get; }
        string Value { get; }
        Type DestinationType { get; }
        string ToString();
    }
}