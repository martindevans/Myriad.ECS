using Myriad.ECS.Queries;

namespace Myriad.ECS.Tests;

[TestClass]
public class QueryBuilderTests
{
    [TestMethod]
    public void CreateEmpty()
    {
        Assert.IsNotNull(new QueryBuilder());
    }

    [TestMethod]
    public void CreateInclude()
    {
        var q = new QueryBuilder()
           .Include<ComponentFloat>()
           .Include<ComponentFloat>()
           .Include(typeof(ComponentFloat));

        Assert.IsTrue(q.IsIncluded<ComponentFloat>());
        Assert.IsTrue(q.IsIncluded(typeof(ComponentFloat)));
        Assert.IsFalse(q.IsIncluded<ComponentInt32>());
        Assert.IsFalse(q.IsIncluded(typeof(ComponentInt32)));

        Assert.IsFalse(q.IsExcluded<ComponentInt32>());
        Assert.IsFalse(q.IsIncluded(typeof(ComponentInt32)));
        Assert.IsFalse(q.IsExcluded<ComponentFloat>());
        Assert.IsFalse(q.IsExcluded(typeof(ComponentFloat)));
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
            q.Exclude(typeof(ComponentFloat));
        });

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.ExactlyOneOf<ComponentFloat>();
        });

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.ExactlyOneOf(typeof(ComponentFloat));
        });

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.AtLeastOneOf<ComponentFloat>();
        });

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.AtLeastOneOf(typeof(ComponentFloat));
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
           .Exclude(typeof(ComponentFloat))
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
           .AtLeastOneOf<ComponentFloat>()
           .AtLeastOneOf(typeof(ComponentFloat));

        Assert.IsTrue(q.IsAtLeastOneOf<ComponentFloat>());
        Assert.IsTrue(q.IsAtLeastOneOf(typeof(ComponentFloat)));
        Assert.IsFalse(q.IsAtLeastOneOf<ComponentInt32>());
        Assert.IsFalse(q.IsAtLeastOneOf(typeof(ComponentInt32)));

        Assert.IsFalse(q.IsIncluded<ComponentInt32>());
        Assert.IsFalse(q.IsIncluded(typeof(ComponentInt32)));
        Assert.IsFalse(q.IsIncluded<ComponentFloat>());
        Assert.IsFalse(q.IsIncluded(typeof(ComponentFloat)));
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
           .ExactlyOneOf<ComponentFloat>()
           .ExactlyOneOf(typeof(ComponentFloat));

        Assert.IsTrue(q.IsExactlyOneOf<ComponentFloat>());
        Assert.IsTrue(q.IsExactlyOneOf(typeof(ComponentFloat)));
        Assert.IsFalse(q.IsExactlyOneOf<ComponentInt32>());
        Assert.IsFalse(q.IsExactlyOneOf(typeof(ComponentInt32)));

        Assert.IsFalse(q.IsIncluded<ComponentInt32>());
        Assert.IsFalse(q.IsIncluded(typeof(ComponentInt32)));
        Assert.IsFalse(q.IsIncluded<ComponentFloat>());
        Assert.IsFalse(q.IsIncluded(typeof(ComponentFloat)));
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
}