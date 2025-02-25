using GameService.Application.Tests.Handlers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GameService.Application.Tests.Helpers;

public static class MockDbSetFactory
{
    public static Mock<DbSet<T>> Create<T>(List<T> data) where T : class
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IAsyncEnumerable<T>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

        mockSet.As<IQueryable<T>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));

        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);

        using var enumerator = queryable.GetEnumerator();
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator())
            .Returns(enumerator);

        return mockSet;
    }
}