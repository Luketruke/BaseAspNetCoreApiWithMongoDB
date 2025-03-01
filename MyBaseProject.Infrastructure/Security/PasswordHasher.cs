namespace MyBaseProject.Infrastructure.Security;

using BCrypt.Net;
using MyBaseProject.Application.Interfaces.Services;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string passwords)
    {
        return BCrypt.HashPassword(passwords);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Verify(password, hashedPassword);
    }
}