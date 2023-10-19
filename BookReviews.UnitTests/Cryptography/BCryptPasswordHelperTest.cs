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

    [Theory]
    [InlineData("password", "Password")]
    [InlineData("password", "password ")]
    [InlineData("password", " password")]
    [InlineData("password", "")]
    public void PasswordHelper_ShouldRejectIncorrectPassword(string password, string incorrectPassword)
    {
        // Arrange
        var bCryptHelper = new BCryptPasswordHelper();

        // Act
        var hashedPassword = bCryptHelper.GenerateHash(password);
        var result = bCryptHelper.VerifyPassword(incorrectPassword, hashedPassword);

        // Assert

        Assert.False(result);
    }
}
