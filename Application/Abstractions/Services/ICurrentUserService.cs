namespace Application.Abstractions.Services
{
    public interface ICurrentUserService
    {
        long? UserId { get; }
        string? Email { get; }
        string? UserName { get; }
        bool IsAuthenticated { get; }
    }
}