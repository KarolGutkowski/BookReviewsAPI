using BookReviews.Infrastructure.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookReviews.UnitTests.Cryptography;

public class BCryptPasswordHelperTest
{
    [Fact]
    public void PasswordHelper_ShouldCorrectlyVerifyPassword()
    {
        // Arrange
        var bCryptHelper = new BCryptPasswordHelper();
        var password = "password";

        // Act
        var hashedPassword = bCryptHelper.GenerateHash(password);
        var result = bCryptHelper.VerifyPassword(password, hashedPassword);

        // Assert

        Assert.True(result);
    }

    [Fact]
    public void PasswordHelper_ShouldRejectIncorrectPassword()
    {
        // Arrange
        var bCryptHelper = new BCryptPasswordHelper();
        var password = "password";
        var incorrectPassword = "Password";

        // Act
        var hashedPassword = bCryptHelper.GenerateHash(password);
        var result = bCryptHelper.VerifyPassword(incorrectPassword, hashedPassword);

        // Assert

        Assert.False(result);
    }
}
