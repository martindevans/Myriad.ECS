
//using Parallel = System.Threading.Tasks.Parallel;
//using Parallel = ParallelTasks.Parallel;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ConvertToPrimaryConstructor

// ReSharper disable once CheckNamespace
namespace Myriad.ECS.Queries
{
    /// <summary>Standard reducers for Map/Reduce queries</summary>
    public static partial class Reduce
    {
	    /// <summary>Container class for reducer of int</summary>
	    public static class I32
	    {
		    /// <summary>Get the minimum of 2 values</summary>
		    public struct Min
                : IQueryReduce<int>
            {
			    /// <inheritdoc />
                public int Reduce(int a, int b)
                {
                    return Math.Min(a, b);
                }
            }

		    /// <summary>Get the maximum of 2 values</summary>
		    public struct Max
                : IQueryReduce<int>
            {
			    /// <inheritdoc />
                public int Reduce(int a, int b)
                {
                    return Math.Max(a, b);
                }
            }

		    /// <summary>Add the 2 values</summary>
		    public struct Add
                : IQueryReduce<int>
            {
			    /// <inheritdoc />
                public int Reduce(int a, int b)
                {
                    return a + b;
                }
            }

		    /// <summary>Multiply the 2 values</summary>
		    public struct Mul
                : IQueryReduce<int>
            {
			    /// <inheritdoc />
                public int Reduce(int a, int b)
                {
                    return a * b;
                }
            }

		    /// <summary>XOR the 2 values</summary>
		    public struct Xor
                : IQueryReduce<int>
            {
			    /// <inheritdoc />
                public int Reduce(int a, int b)
                {
                    return a ^ b;
                }
            }

		    /// <summary>AND the 2 values</summary>
		    public struct And
                : IQueryReduce<int>
            {
			    /// <inheritdoc />
                public int Reduce(int a, int b)
                {
                    return a & b;
                }
            }

		    /// <summary>OR the 2 values</summary>
		    public struct Or
                : IQueryReduce<int>
            {
			    /// <inheritdoc />
                public int Reduce(int a, int b)
                {
                    return a | b;
                }
            }
	    }
    }
}

// ReSharper disable once CheckNamespace
namespace Myriad.ECS.Queries
{
    /// <summary>Standard reducers for Map/Reduce queries</summary>
    public static partial class Reduce
    {
	    /// <summary>Container class for reducer of uint</summary>
	    public static class U32
	    {
		    /// <summary>Get the minimum of 2 values</summary>
		    public struct Min
                : IQueryReduce<uint>
            {
			    /// <inheritdoc />
                public uint Reduce(uint a, uint b)
                {
                    return Math.Min(a, b);
                }
            }

		    /// <summary>Get the maximum of 2 values</summary>
		    public struct Max
                : IQueryReduce<uint>
            {
			    /// <inheritdoc />
                public uint Reduce(uint a, uint b)
                {
                    return Math.Max(a, b);
                }
            }

		    /// <summary>Add the 2 values</summary>
		    public struct Add
                : IQueryReduce<uint>
            {
			    /// <inheritdoc />
                public uint Reduce(uint a, uint b)
                {
                    return a + b;
                }
            }

		    /// <summary>Multiply the 2 values</summary>
		    public struct Mul
                : IQueryReduce<uint>
            {
			    /// <inheritdoc />
                public uint Reduce(uint a, uint b)
                {
                    return a * b;
                }
            }

		    /// <summary>XOR the 2 values</summary>
		    public struct Xor
                : IQueryReduce<uint>
            {
			    /// <inheritdoc />
                public uint Reduce(uint a, uint b)
                {
                    return a ^ b;
                }
            }

		    /// <summary>AND the 2 values</summary>
		    public struct And
                : IQueryReduce<uint>
            {
			    /// <inheritdoc />
                public uint Reduce(uint a, uint b)
                {
                    return a & b;
                }
            }

		    /// <summary>OR the 2 values</summary>
		    public struct Or
                : IQueryReduce<uint>
            {
			    /// <inheritdoc />
                public uint Reduce(uint a, uint b)
                {
                    return a | b;
                }
            }
	    }
    }
}

// ReSharper disable once CheckNamespace
namespace Myriad.ECS.Queries
{
    /// <summary>Standard reducers for Map/Reduce queries</summary>
    public static partial class Reduce
    {
	    /// <summary>Container class for reducer of long</summary>
	    public static class I64
	    {
		    /// <summary>Get the minimum of 2 values</summary>
		    public struct Min
                : IQueryReduce<long>
            {
			    /// <inheritdoc />
                public long Reduce(long a, long b)
                {
                    return Math.Min(a, b);
                }
            }

		    /// <summary>Get the maximum of 2 values</summary>
		    public struct Max
                : IQueryReduce<long>
            {
			    /// <inheritdoc />
                public long Reduce(long a, long b)
                {
                    return Math.Max(a, b);
                }
            }

		    /// <summary>Add the 2 values</summary>
		    public struct Add
                : IQueryReduce<long>
            {
			    /// <inheritdoc />
                public long Reduce(long a, long b)
                {
                    return a + b;
                }
            }

		    /// <summary>Multiply the 2 values</summary>
		    public struct Mul
                : IQueryReduce<long>
            {
			    /// <inheritdoc />
                public long Reduce(long a, long b)
                {
                    return a * b;
                }
            }

		    /// <summary>XOR the 2 values</summary>
		    public struct Xor
                : IQueryReduce<long>
            {
			    /// <inheritdoc />
                public long Reduce(long a, long b)
                {
                    return a ^ b;
                }
            }

		    /// <summary>AND the 2 values</summary>
		    public struct And
                : IQueryReduce<long>
            {
			    /// <inheritdoc />
                public long Reduce(long a, long b)
                {
                    return a & b;
                }
            }

		    /// <summary>OR the 2 values</summary>
		    public struct Or
                : IQueryReduce<long>
            {
			    /// <inheritdoc />
                public long Reduce(long a, long b)
                {
                    return a | b;
                }
            }
	    }
    }
}

// ReSharper disable once CheckNamespace
namespace Myriad.ECS.Queries
{
    /// <summary>Standard reducers for Map/Reduce queries</summary>
    public static partial class Reduce
    {
	    /// <summary>Container class for reducer of ulong</summary>
	    public static class U64
	    {
		    /// <summary>Get the minimum of 2 values</summary>
		    public struct Min
                : IQueryReduce<ulong>
            {
			    /// <inheritdoc />
                public ulong Reduce(ulong a, ulong b)
                {
                    return Math.Min(a, b);
                }
            }

		    /// <summary>Get the maximum of 2 values</summary>
		    public struct Max
                : IQueryReduce<ulong>
            {
			    /// <inheritdoc />
                public ulong Reduce(ulong a, ulong b)
                {
                    return Math.Max(a, b);
                }
            }

		    /// <summary>Add the 2 values</summary>
		    public struct Add
                : IQueryReduce<ulong>
            {
			    /// <inheritdoc />
                public ulong Reduce(ulong a, ulong b)
                {
                    return a + b;
                }
            }

		    /// <summary>Multiply the 2 values</summary>
		    public struct Mul
                : IQueryReduce<ulong>
            {
			    /// <inheritdoc />
                public ulong Reduce(ulong a, ulong b)
                {
                    return a * b;
                }
            }

		    /// <summary>XOR the 2 values</summary>
		    public struct Xor
                : IQueryReduce<ulong>
            {
			    /// <inheritdoc />
                public ulong Reduce(ulong a, ulong b)
                {
                    return a ^ b;
                }
            }

		    /// <summary>AND the 2 values</summary>
		    public struct And
                : IQueryReduce<ulong>
            {
			    /// <inheritdoc />
                public ulong Reduce(ulong a, ulong b)
                {
                    return a & b;
                }
            }

		    /// <summary>OR the 2 values</summary>
		    public struct Or
                : IQueryReduce<ulong>
            {
			    /// <inheritdoc />
                public ulong Reduce(ulong a, ulong b)
                {
                    return a | b;
                }
            }
	    }
    }
}

// ReSharper disable once CheckNamespace
namespace Myriad.ECS.Queries
{
    /// <summary>Standard reducers for Map/Reduce queries</summary>
    public static partial class Reduce
    {
	    /// <summary>Container class for reducer of float</summary>
	    public static class F32
	    {
		    /// <summary>Get the minimum of 2 values</summary>
		    public struct Min
                : IQueryReduce<float>
            {
			    /// <inheritdoc />
                public float Reduce(float a, float b)
                {
                    return Math.Min(a, b);
                }
            }

		    /// <summary>Get the maximum of 2 values</summary>
		    public struct Max
                : IQueryReduce<float>
            {
			    /// <inheritdoc />
                public float Reduce(float a, float b)
                {
                    return Math.Max(a, b);
                }
            }

		    /// <summary>Add the 2 values</summary>
		    public struct Add
                : IQueryReduce<float>
            {
			    /// <inheritdoc />
                public float Reduce(float a, float b)
                {
                    return a + b;
                }
            }

		    /// <summary>Multiply the 2 values</summary>
		    public struct Mul
                : IQueryReduce<float>
            {
			    /// <inheritdoc />
                public float Reduce(float a, float b)
                {
                    return a * b;
                }
            }

	    }
    }
}

// ReSharper disable once CheckNamespace
namespace Myriad.ECS.Queries
{
    /// <summary>Standard reducers for Map/Reduce queries</summary>
    public static partial class Reduce
    {
	    /// <summary>Container class for reducer of double</summary>
	    public static class F64
	    {
		    /// <summary>Get the minimum of 2 values</summary>
		    public struct Min
                : IQueryReduce<double>
            {
			    /// <inheritdoc />
                public double Reduce(double a, double b)
                {
                    return Math.Min(a, b);
                }
            }

		    /// <summary>Get the maximum of 2 values</summary>
		    public struct Max
                : IQueryReduce<double>
            {
			    /// <inheritdoc />
                public double Reduce(double a, double b)
                {
                    return Math.Max(a, b);
                }
            }

		    /// <summary>Add the 2 values</summary>
		    public struct Add
                : IQueryReduce<double>
            {
			    /// <inheritdoc />
                public double Reduce(double a, double b)
                {
                    return a + b;
                }
            }

		    /// <summary>Multiply the 2 values</summary>
		    public struct Mul
                : IQueryReduce<double>
            {
			    /// <inheritdoc />
                public double Reduce(double a, double b)
                {
                    return a * b;
                }
            }

	    }
    }
}

// ReSharper disable once CheckNamespace
namespace Myriad.ECS.Queries
{
    /// <summary>Standard reducers for Map/Reduce queries</summary>
    public static partial class Reduce
    {
	    /// <summary>Container class for reducer of bool</summary>
	    public static class Boolean
	    {

		    /// <summary>XOR the 2 values</summary>
		    public struct Xor
                : IQueryReduce<bool>
            {
			    /// <inheritdoc />
                public bool Reduce(bool a, bool b)
                {
                    return a ^ b;
                }
            }

		    /// <summary>AND the 2 values</summary>
		    public struct And
                : IQueryReduce<bool>
            {
			    /// <inheritdoc />
                public bool Reduce(bool a, bool b)
                {
                    return a & b;
                }
            }

		    /// <summary>OR the 2 values</summary>
		    public struct Or
                : IQueryReduce<bool>
            {
			    /// <inheritdoc />
                public bool Reduce(bool a, bool b)
                {
                    return a | b;
                }
            }
	    }
    }
}



