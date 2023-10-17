using Xunit;
using Moq;
using BookReviews.Infrastructure.Cryptography;
using BookReviews.Domain.Models;
using Microsoft.EntityFrameworkCore;
using BookReviews.Domain.Models.DataModels;
using System.Collections.Generic;
using Moq.EntityFrameworkCore;
using BookReviews.Infrastructure.Registration;
using System.Linq;

namespace BookReviews.UnitTests
{
    public class RegistrationTests
    {
        [Fact]
        public void RegistrationHelper_ShouldAddNewUserWhenNoOtherExists()
        {
            // arrange
            var mockCryptographyHelper = new Mock<IPasswordCryptographyHelper>();
            mockCryptographyHelper
                .Setup(s => s.GenerateHash(It.IsAny<string>()))
                .Returns<string>(x => x);

            mockCryptographyHelper
                .Setup(s => s.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string x, string y) => x == y);

            var usersList = new List<User>();

            var mockUsersRepository = new Mock<IUsersRepository>();
            mockUsersRepository
                .Setup(s => s.Users)
                .ReturnsDbSet(usersList);

            mockUsersRepository
                .Setup(s => s.Users.Add(It.IsAny<User>()))
                .Callback<User>(s => usersList.Add(s));

            var registrationHelper = new RegistrationHelper(mockUsersRepository.Object, mockCryptographyHelper.Object);

            var username = "testUser";
            var password = "1234";

            // act
            var result = registrationHelper.TryToRegisterUser(username, password);
            var newUser = usersList.SingleOrDefault();
            // assert

            Assert.Equal(password, newUser.Password);
            Assert.True(result);
            Assert.Single(usersList);
            Assert.NotNull(newUser);
            Assert.Equal(username, newUser.UserName);
        }
    }
}
