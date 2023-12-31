﻿using Xunit;
using Moq;
using BookReviews.Infrastructure.Cryptography;
using BookReviews.Domain.Models;
using Microsoft.EntityFrameworkCore;
using BookReviews.Domain.Models.DataModels;
using System.Collections.Generic;
using Moq.EntityFrameworkCore;
using BookReviews.Infrastructure.Registration;
using System.Linq;
using System.Linq.Expressions;
using System;
using BookReviews.UnitTests.Utilities;

namespace BookReviews.UnitTests.Registration
{
    public class RegistrationTests
    {
        Mock<IPasswordCryptographyHelper> mockCryptographyHelper;
        List<User> inMemoryUsersList;
        Mock<IUsersRepository> mockUsersRepository;
        public RegistrationTests()
        {
            mockCryptographyHelper = new Mock<IPasswordCryptographyHelper>();
            mockCryptographyHelper
                .Setup(s => s.GenerateHash(It.IsAny<string>()))
                .Returns<string>(x => x);
            mockCryptographyHelper
                .Setup(s => s.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string x, string y) => x == y);

            inMemoryUsersList = new List<User>();

            var dbSetMock = MockDbSetGenerator.CreateMockDbSetWithDataFromCollection(inMemoryUsersList);

            mockUsersRepository = new Mock<IUsersRepository>();
            mockUsersRepository
                .Setup(s => s.Users)
                .Returns(dbSetMock.Object);
        }

        [Fact]
        public void RegistrationHelper_ShouldAddNewUserWhenNoOtherExists()
        {
            // arrange
            var registrationHelper = new RegistrationHelper(mockUsersRepository.Object, mockCryptographyHelper.Object);

            var username = "testUser";
            var password = "1234";

            // act
            var result = registrationHelper.TryToRegisterUser(username, password);
            var newUser = inMemoryUsersList.SingleOrDefault();
            // assert

            Assert.NotNull(newUser);
            Assert.Equal(password, newUser?.Password);
            Assert.True(result);
            Assert.Single(inMemoryUsersList);
            Assert.NotNull(newUser);
            Assert.Equal(username, newUser?.UserName);
        }

        [Fact]
        public void RegistrationHelper_ShouldntRegisterDuplicateUsers()
        {
            // arrange
            var user = new User
            {
                UserName = "user",
                Password = "1234"
            };

            var registrationHelper = new RegistrationHelper(mockUsersRepository.Object, mockCryptographyHelper.Object);
            // act
            var firstAttemptResult  = registrationHelper.TryToRegisterUser(user.UserName, user.Password);
            var result = registrationHelper.TryToRegisterUser(user.UserName, user.Password);
            var usersCount = inMemoryUsersList.Count();
            // assert
            Assert.True(firstAttemptResult);
            Assert.False(result);
            Assert.Equal(1, usersCount);
        }
    }
}
