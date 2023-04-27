namespace LuzInga.Domain.SharedKernel.Exceptions;

public class ConfirmationCodeExpiredException : ApplicationException
{
    public ConfirmationCodeExpiredException() : base("Confirmation code is expired! generate a new code!")
    {
        
    }
}
