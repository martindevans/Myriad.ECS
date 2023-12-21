using Myriad.ECS.Queries;

namespace Myriad.ECS.Tests;

[TestClass]
public class QueryBuilderTests
{
    [TestMethod]
    public void CreateEmpty()
    {
        var q = new QueryBuilder();
    }

    [TestMethod]
    public void CreateInclude()
    {
        var q = new QueryBuilder()
           .Include<ComponentFloat>();

        Assert.IsTrue(q.IsIncluded<ComponentFloat>());
        Assert.IsFalse(q.IsIncluded<ComponentInt32>());

        Assert.IsFalse(q.IsExcluded<ComponentInt32>());
        Assert.IsFalse(q.IsExcluded<ComponentFloat>());
    }

    [TestMethod]
    public void IncludeDuplicateThrows()
    {
        var q = new QueryBuilder()
           .Include<ComponentFloat>()
           .Include<ComponentInt16>()
           .Include<ComponentInt32>()
           .Include<ComponentFloat>();

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.Exclude<ComponentFloat>();
        });

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.ExactlyOneOf<ComponentFloat>();
        });

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.AtLeastOneOf<ComponentFloat>();
        });
    }

    [TestMethod]
    public void CreateExclude()
    {
        var q = new QueryBuilder()
           .Exclude<ComponentFloat>();

        Assert.IsTrue(q.IsExcluded<ComponentFloat>());
        Assert.IsFalse(q.IsExcluded<ComponentInt32>());

        Assert.IsFalse(q.IsIncluded<ComponentInt32>());
        Assert.IsFalse(q.IsIncluded<ComponentFloat>());
    }

    [TestMethod]
    public void ExcludeDuplicateThrows()
    {
        var q = new QueryBuilder()
               .Exclude<ComponentFloat>()
               .Exclude<ComponentInt16>()
               .Exclude<ComponentInt32>()
               .Exclude<ComponentFloat>();

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.Include<ComponentFloat>();
        });

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.ExactlyOneOf<ComponentFloat>();
        });

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.AtLeastOneOf<ComponentFloat>();
        });
    }

    [TestMethod]
    public void CreateAtLeastOneOf()
    {
        var q = new QueryBuilder()
           .AtLeastOneOf<ComponentFloat>();

        Assert.IsTrue(q.IsAtLeastOneOf<ComponentFloat>());
        Assert.IsFalse(q.IsAtLeastOneOf<ComponentInt32>());

        Assert.IsFalse(q.IsIncluded<ComponentInt32>());
        Assert.IsFalse(q.IsIncluded<ComponentFloat>());
    }

    [TestMethod]
    public void AtLeastOneOfDuplicateThrows()
    {
        var q = new QueryBuilder()
               .AtLeastOneOf<ComponentFloat>()
               .AtLeastOneOf<ComponentInt16>()
               .AtLeastOneOf<ComponentInt32>()
               .AtLeastOneOf<ComponentFloat>();

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.Include<ComponentFloat>();
        });

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.ExactlyOneOf<ComponentFloat>();
        });

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.Exclude<ComponentFloat>();
        });
    }

    [TestMethod]
    public void CreateExactyOneOf()
    {
        var q = new QueryBuilder()
           .ExactlyOneOf<ComponentFloat>();

        Assert.IsTrue(q.IsExactlyOneOf<ComponentFloat>());
        Assert.IsFalse(q.IsExactlyOneOf<ComponentInt32>());

        Assert.IsFalse(q.IsIncluded<ComponentInt32>());
        Assert.IsFalse(q.IsIncluded<ComponentFloat>());
    }

    [TestMethod]
    public void ExactyOneOfDuplicateThrows()
    {
        var q = new QueryBuilder()
               .ExactlyOneOf<ComponentFloat>()
               .ExactlyOneOf<ComponentInt16>()
               .ExactlyOneOf<ComponentInt32>()
               .ExactlyOneOf<ComponentFloat>();

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.Include<ComponentFloat>();
        });

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.AtLeastOneOf<ComponentFloat>();
        });

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.Exclude<ComponentFloat>();
        });
    }

    [TestMethod]
    public void CreateWithFilter()
    {
        var q = new QueryBuilder()
            .Include<ComponentInt32>()
            .Filter<FilterPositiveFloat>();

        // Because the "FilterPositiveFloat" filter requires the "ComponentFloat" it should now be implicitly included
        Assert.IsTrue(q.IsIncluded<ComponentFloat>());

        Assert.IsTrue(q.IsFilter<FilterPositiveFloat>());
        Assert.IsTrue(q.IsFilter(typeof(FilterPositiveFloat)));
        Assert.IsTrue(!q.IsFilter(typeof(FilterNegativeFloat)));

        // Adding it again is fine
        q.Filter<FilterPositiveFloat>();
    }
}