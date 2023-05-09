using System.Security.Cryptography;
using System.Text;
using LuzInga.Domain;
using LuzInga.Domain.Services;
using LuzInga.Domain.ValueObjects;

namespace LuzInga.Infra.Services;

public class DefaultSubscriptionConfirmationCodeFactory : ISubscriptionConfirmationCodeFactory
{
    public string Generate(SubscriptionId id)
    {
        string secretKey = Constants.CONFIRMATION_TOKEN_SECRET_KEY;
        var now  = DateTimeProvider.Now;
        string confirmationCode = "";

        using (var sha256 = SHA256.Create()) {
            var hashData = sha256.ComputeHash(Encoding.UTF8.GetBytes(id + secretKey + now));
            var stringBuilder = new StringBuilder();
            foreach (var b in hashData) {
                stringBuilder.Append(b.ToString("x2"));
            }
            confirmationCode = stringBuilder.ToString();
        }

        return confirmationCode;
    }
}
