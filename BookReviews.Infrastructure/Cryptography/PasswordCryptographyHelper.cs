using BC = BCrypt.Net.BCrypt;

namespace BookReviews.Infrastructure.Cryptography;

public class BCryptPasswordHelper : IPasswordCryptographyHelper
{
    private const bool _enhancedEntropy = true;

    public string GenerateHash(string password)
    {
        return BC.EnhancedHashPassword(password);
    }

    public bool VerifyPassword(string password, string storedHash)
    {
        return BC.Verify(password, storedHash, enhancedEntropy: _enhancedEntropy);
    }
}
