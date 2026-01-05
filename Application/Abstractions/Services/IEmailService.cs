namespace Application.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string toEmail, string verifyToken);
    }
}