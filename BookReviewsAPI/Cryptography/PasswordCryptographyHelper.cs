﻿using BC = BCrypt.Net.BCrypt;

namespace BookReviewsAPI.Cryptography
{
    public class BCryptPasswordHelper: IPasswordCryptographyHelper
    {
        private const bool enhancedEntropy = true;

        public string GenerateHash(string password)
        {
            return BC.EnhancedHashPassword(password);
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            return BC.Verify(password, storedHash, enhancedEntropy: enhancedEntropy);
        }
    }
}