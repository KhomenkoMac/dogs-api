using System.Runtime.Serialization;

namespace codebridge.api.application.exceptions;

public class NoSuchSortingAttributeException : Exception
{
    public NoSuchSortingAttributeException() : base("No such sorting attribute for dog")
    {
    }
}