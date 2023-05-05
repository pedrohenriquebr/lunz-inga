namespace LuzInga.Domain.SharedKernel.Exceptions;

public class ConfirmationCodeExpiredException : GlobalApplicationException
{
    public ConfirmationCodeExpiredException() : base(ApplicationExceptionType.Business,
                                                    "Confirmation code is expired! generate a new code!",
                                                    ApplicationErrorCode.CONFIRMATION_CODE_EXPIRED)
    {
        
    }
}
