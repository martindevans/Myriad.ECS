using Myriad.ECS.Queries;

namespace Myriad.ECS.Tests.Queries;

[TestClass]
public class ReducerTests
{
    [TestMethod]
    public void BooleanReducers()
    {
        Check<Reduce.Boolean.And>((a, b) => a & b);
        Check<Reduce.Boolean.Or>((a, b)  => a | b);
        Check<Reduce.Boolean.Xor>((a, b) => a ^ b);

        void Check<TR>(Func<bool, bool, bool> func)
            where TR : struct, IQueryReduce<bool>
        {
            var r = new TR();
            Assert.AreEqual(func(false, false), r.Reduce(false, false));
            Assert.AreEqual(func(false, true),  r.Reduce(false, true));
            Assert.AreEqual(func(true,  false), r.Reduce(true,  false));
            Assert.AreEqual(func(true,  true),  r.Reduce(true,  true));
        }
    }

    [TestMethod]
    public void I32Reducers()
    {
        var rng = new Random(34345);

        Check<Reduce.I32.Add>((a, b) => a + b);
        Check<Reduce.I32.And>((a, b) => a & b);
        Check<Reduce.I32.Xor>((a, b) => a ^ b);
        Check<Reduce.I32.Or>( (a, b) => a | b);
        Check<Reduce.I32.Mul>((a, b) => a * b);
        Check<Reduce.I32.Min>(Math.Min);
        Check<Reduce.I32.Max>(Math.Max);

        void Check<TR>(Func<int, int, int> func)
            where TR : struct, IQueryReduce<int>
        {
            for (var i = 0; i < 128; i++)
            {
                var a = rng.Next();
                var b = rng.Next();
                var c = func(a, b);

                Assert.AreEqual(c, new TR().Reduce(a, b));
            }
        }
    }

    [TestMethod]
    public void U32Reducers()
    {
        var rng = new Random(34345);

        Check<Reduce.U32.Add>((a, b) => a + b);
        Check<Reduce.U32.And>((a, b) => a & b);
        Check<Reduce.U32.Xor>((a, b) => a ^ b);
        Check<Reduce.U32.Or>((a, b) => a | b);
        Check<Reduce.U32.Mul>((a, b) => a * b);
        Check<Reduce.U32.Min>(Math.Min);
        Check<Reduce.U32.Max>(Math.Max);

        void Check<TR>(Func<uint, uint, uint> func)
            where TR : struct, IQueryReduce<uint>
        {
            for (var i = 0; i < 128; i++)
            {
                var a = unchecked((uint)rng.Next());
                var b = unchecked((uint)rng.Next());
                var c = func(a, b);

                Assert.AreEqual(c, new TR().Reduce(a, b));
            }
        }
    }

    [TestMethod]
    public void I64Reducers()
    {
        var rng = new Random(567345);

        Check<Reduce.I64.Add>((a, b) => a + b);
        Check<Reduce.I64.And>((a, b) => a & b);
        Check<Reduce.I64.Xor>((a, b) => a ^ b);
        Check<Reduce.I64.Or>((a, b) => a | b);
        Check<Reduce.I64.Mul>((a, b) => a * b);
        Check<Reduce.I64.Min>(Math.Min);
        Check<Reduce.I64.Max>(Math.Max);

        void Check<TR>(Func<long, long, long> func)
            where TR : struct, IQueryReduce<long>
        {
            for (var i = 0; i < 128; i++)
            {
                var a = rng.NextInt64();
                var b = rng.NextInt64();
                var c = func(a, b);

                Assert.AreEqual(c, new TR().Reduce(a, b));
            }
        }
    }

    [TestMethod]
    public void U64Reducers()
    {
        var rng = new Random(5673549);

        Check<Reduce.U64.Add>((a, b) => a + b);
        Check<Reduce.U64.And>((a, b) => a & b);
        Check<Reduce.U64.Xor>((a, b) => a ^ b);
        Check<Reduce.U64.Or>((a, b) => a | b);
        Check<Reduce.U64.Mul>((a, b) => a * b);
        Check<Reduce.U64.Min>(Math.Min);
        Check<Reduce.U64.Max>(Math.Max);

        void Check<TR>(Func<ulong, ulong, ulong> func)
            where TR : struct, IQueryReduce<ulong>
        {
            for (var i = 0; i < 128; i++)
            {
                var a = unchecked((ulong)rng.NextInt64());
                var b = unchecked((ulong)rng.NextInt64());
                var c = func(a, b);

                Assert.AreEqual(c, new TR().Reduce(a, b));
            }
        }
    }

    [TestMethod]
    public void F32Reducers()
    {
        var rng = new Random(678452);

        Check<Reduce.F32.Add>((a, b) => a + b);
        Check<Reduce.F32.Mul>((a, b) => a * b);
        Check<Reduce.F32.Min>(Math.Min);
        Check<Reduce.F32.Max>(Math.Max);

        void Check<TR>(Func<float, float, float> func)
            where TR : struct, IQueryReduce<float>
        {
            for (var i = 0; i < 128; i++)
            {
                var a = rng.NextSingle();
                var b = rng.NextSingle();
                var c = func(a, b);

                Assert.AreEqual(c, new TR().Reduce(a, b));
            }
        }
    }

    [TestMethod]
    public void F64Reducers()
    {
        var rng = new Random(62345654);

        Check<Reduce.F64.Add>((a, b) => a + b);
        Check<Reduce.F64.Mul>((a, b) => a * b);
        Check<Reduce.F64.Min>(Math.Min);
        Check<Reduce.F64.Max>(Math.Max);

        void Check<TR>(Func<double, double, double> func)
            where TR : struct, IQueryReduce<double>
        {
            for (var i = 0; i < 128; i++)
            {
                var a = rng.NextSingle();
                var b = rng.NextSingle();
                var c = func(a, b);

                Assert.AreEqual(c, new TR().Reduce(a, b));
            }
        }
    }
}