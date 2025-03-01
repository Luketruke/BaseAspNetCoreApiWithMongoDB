namespace MyBaseProject.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string role);
        string GetUserIdFromToken(string token);
    }
}
