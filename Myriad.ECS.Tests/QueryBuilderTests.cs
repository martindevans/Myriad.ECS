using Myriad.ECS.IDs;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

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
        Assert.IsTrue(q.IsIncluded(ComponentID<ComponentFloat>.ID));
        CollectionAssert.Contains(q.Included.ToArray(), ComponentID<ComponentFloat>.ID);

        Assert.IsFalse(q.IsIncluded<ComponentInt32>());
        Assert.IsFalse(q.IsIncluded(typeof(ComponentInt32)));
        Assert.IsFalse(q.IsIncluded(ComponentID<ComponentInt32>.ID));
        CollectionAssert.DoesNotContain(q.Included.ToArray(), ComponentID<ComponentInt32>.ID);

        Assert.IsFalse(q.IsExcluded<ComponentInt32>());
        Assert.IsFalse(q.IsIncluded(typeof(ComponentInt32)));
        Assert.IsFalse(q.IsIncluded(ComponentID<ComponentInt32>.ID));

        Assert.IsFalse(q.IsExcluded<ComponentFloat>());
        Assert.IsFalse(q.IsExcluded(typeof(ComponentFloat)));
        Assert.IsFalse(q.IsExcluded(ComponentID<ComponentFloat>.ID));
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
        CollectionAssert.Contains(q.Excluded.ToArray(), ComponentID<ComponentFloat>.ID);

        Assert.IsFalse(q.IsExcluded<ComponentInt32>());
        CollectionAssert.DoesNotContain(q.Excluded.ToArray(), ComponentID<ComponentInt32>.ID);

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
        Assert.IsTrue(q.IsAtLeastOneOf(ComponentID<ComponentFloat>.ID));
        CollectionAssert.Contains(q.AtLeastOnes.ToArray(), ComponentID<ComponentFloat>.ID);

        Assert.IsFalse(q.IsAtLeastOneOf<ComponentInt32>());
        Assert.IsFalse(q.IsAtLeastOneOf(typeof(ComponentInt32)));
        Assert.IsFalse(q.IsAtLeastOneOf(ComponentID<ComponentInt32>.ID));
        CollectionAssert.DoesNotContain(q.AtLeastOnes.ToArray(), ComponentID<ComponentInt32>.ID);

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
        Assert.IsTrue(q.IsExactlyOneOf(ComponentID<ComponentFloat>.ID));
        CollectionAssert.Contains(q.ExactlyOnes.ToArray(), ComponentID<ComponentFloat>.ID);

        Assert.IsFalse(q.IsExactlyOneOf<ComponentInt32>());
        Assert.IsFalse(q.IsExactlyOneOf(typeof(ComponentInt32)));
        Assert.IsFalse(q.IsExactlyOneOf(ComponentID<ComponentInt32>.ID));
        CollectionAssert.DoesNotContain(q.ExactlyOnes.ToArray(), ComponentID<ComponentInt32>.ID);

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

    [TestMethod]
    public void ConvertQueryToBuilder()
    {
        var world = new WorldBuilder().Build();

        var query = new QueryBuilder()
             .Include<ComponentInt16>()
             .Exclude<ComponentInt32>()
             .ExactlyOneOf<ComponentFloat>()
             .AtLeastOneOf<ComponentInt64>()
             .Build(world);

        var builder = query.ToBuilder();

        Assert.IsTrue(builder.IsIncluded<ComponentInt16>());
        Assert.IsFalse(builder.IsIncluded<ComponentInt32>());
        Assert.IsFalse(builder.IsIncluded<ComponentFloat>());
        Assert.IsFalse(builder.IsIncluded<ComponentInt64>());

        Assert.IsFalse(builder.IsExcluded<ComponentInt16>());
        Assert.IsTrue(builder.IsExcluded<ComponentInt32>());
        Assert.IsFalse(builder.IsExcluded<ComponentFloat>());
        Assert.IsFalse(builder.IsExcluded<ComponentInt64>());

        Assert.IsFalse(builder.IsExactlyOneOf<ComponentInt16>());
        Assert.IsFalse(builder.IsExactlyOneOf<ComponentInt32>());
        Assert.IsTrue(builder.IsExactlyOneOf<ComponentFloat>());
        Assert.IsFalse(builder.IsExactlyOneOf<ComponentInt64>());

        Assert.IsFalse(builder.IsAtLeastOneOf<ComponentInt16>());
        Assert.IsFalse(builder.IsAtLeastOneOf<ComponentInt32>());
        Assert.IsFalse(builder.IsAtLeastOneOf<ComponentFloat>());
        Assert.IsTrue(builder.IsAtLeastOneOf<ComponentInt64>());
    }

    [TestMethod]
    public void BuildTwiceSharesParts()
    {
        var world = new WorldBuilder().Build();

        var builder = new QueryBuilder()
                     .Include<ComponentInt16>()
                     .Exclude<ComponentInt32>()
                     .ExactlyOneOf<ComponentFloat>()
                     .AtLeastOneOf<ComponentInt64>();

        var q1 = builder.Build(world);
        var q2 = builder.Build(world);

        Assert.AreSame(q1.Include, q2.Include);
        Assert.AreSame(q1.Exclude, q2.Exclude);
        Assert.AreSame(q1.ExactlyOneOf, q2.ExactlyOneOf);
        Assert.AreSame(q1.AtLeastOneOf, q2.AtLeastOneOf);
    }
}