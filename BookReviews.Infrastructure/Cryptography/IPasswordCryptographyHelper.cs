namespace BookReviews.Infrastructure.Cryptography;

public interface IPasswordCryptographyHelper
{
    public bool VerifyPassword(string password, string storedHash);
    public string GenerateHash(string password);
}
