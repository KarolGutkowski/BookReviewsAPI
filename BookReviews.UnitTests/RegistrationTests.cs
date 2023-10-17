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

namespace BookReviews.UnitTests
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

            var dbSetMock = new Mock<DbSet<User>>();
            dbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(inMemoryUsersList.AsQueryable().Provider);
            dbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(inMemoryUsersList.AsQueryable().Expression);
            dbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(inMemoryUsersList.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(inMemoryUsersList.AsQueryable().GetEnumerator());

            mockUsersRepository = new Mock<IUsersRepository>();
            mockUsersRepository
                .Setup(s => s.Users)
                .ReturnsDbSet(dbSetMock.Object);

            mockUsersRepository
               .Setup(s => s.Users.Add(It.IsAny<User>()))
               .Callback<User>(s => inMemoryUsersList.Add(s));
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
        public void RegistrationHelper_ShouldntRegisterExistingUser()
        {
            // arrange
            var existingUser = new User
            {
                UserName = "user",
                Password = "1234"
            };
            inMemoryUsersList.Add(existingUser);

            var registrationHelper = new RegistrationHelper(mockUsersRepository.Object, mockCryptographyHelper.Object);
            // act
            var result = registrationHelper.TryToRegisterUser(existingUser.UserName, existingUser.Password);
            var usersCount = inMemoryUsersList.Count();
            // assert
            Assert.False(result);
            Assert.Equal(1, usersCount);
        }
    }
}
