// Services/PasswordService.cs
using BCrypt.Net;

public class PasswordService
{
    // Hashowanie hasła za pomocą bcrypt
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Weryfikacja hasła
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
