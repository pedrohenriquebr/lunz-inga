namespace LuzInga.Domain.SharedKernel.Exceptions;

public class GlobalApplicationException : System.Exception
{
    public ApplicationExceptionType Type { get; private set; }
    public ApplicationErrorCode? Code { get; private set; } = null;
    public List<string> Errors { get; private set; } = new List<string>();


    public GlobalApplicationException AddError(string error)
    {
        Errors.Add(error);
        return this;
    }

    public GlobalApplicationException AddErrors(List<string> errors)
    {
        Errors.AddRange(errors);
        return this;
    }

    public GlobalApplicationException(ApplicationExceptionType type, string message) : base(message) {
        this.Type = type;
    }
    
    public GlobalApplicationException(ApplicationExceptionType type, string message, System.Exception inner) : base(message, inner) {
        this.Type = type;
    }

    public GlobalApplicationException(ApplicationExceptionType type, string message, ApplicationErrorCode code) : base(message) {
        this.Type = type;
        this.Code  = code;
    }
    public GlobalApplicationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
}


public enum ApplicationErrorCode 
{
    CONFIRMATION_CODE_EXPIRED
}


public enum ApplicationExceptionType
{
    Application,
    Validation,
    Business
}