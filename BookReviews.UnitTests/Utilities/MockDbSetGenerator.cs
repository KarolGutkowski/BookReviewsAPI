using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.UnitTests.Utilities
{
    public static class MockDbSetGenerator
    {
        public static Mock<DbSet<TEntity>> 
            CreateMockDbSetWithDataFromEnumerable<TEntity>(IEnumerable<TEntity> enumerable) where TEntity : class
        {
            var dbSetMock = new Mock<DbSet<TEntity>>();
            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(enumerable.AsQueryable().Provider);
            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(enumerable.AsQueryable().Expression);
            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(enumerable.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(enumerable.AsQueryable().GetEnumerator());
            return dbSetMock;
        }
    }
}
