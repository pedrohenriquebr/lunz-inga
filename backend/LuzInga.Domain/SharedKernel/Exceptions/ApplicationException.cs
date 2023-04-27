namespace LuzInga.Domain.SharedKernel.Exceptions;

public class ApplicationException : System.Exception
{
    public ApplicationException() {}
    public ApplicationException(string message) : base(message) {}
    public ApplicationException(string message, System.Exception inner) : base(message, inner) {}
    public ApplicationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
}
