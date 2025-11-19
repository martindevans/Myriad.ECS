using System.Numerics;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using Myriad.ECS.Queries;
using Myriad.ECS.IDs;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0>
		where TV0 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		public void Execute(Span<Vector<TV0>> t0, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		
		public int ExecuteVectorChunk<TQ, T0, TV0>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where TV0 : unmanaged
			where TQ : IVectorChunkQuery<TV0>
		{
			query ??= GetCachedQuery<T0>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					// Execute the vectorised part
					q.Execute(tv0, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);

					// Execute the leftover at the end
					q.Execute(ls0, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
					}
				}
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0, TV1>
		where TV0 : unmanaged
        where TV1 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		/// <param name="t1">Span of vectors of values, reinterpreted from component 1</param>
		public void Execute(Span<Vector<TV0>> t0, Span<Vector<TV1>> t1, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T1">The type of the component 1</typeparam>
		/// <typeparam name="TV1">The type component 1 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		[ExcludeFromCodeCoverage]
		public int ExecuteVectorChunk<TQ, T0, TV0, T1, TV1>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where T1 : struct, IComponent
            where TV0 : unmanaged
            where TV1 : unmanaged
			where TQ : IVectorChunkQuery<TV0, TV1>
		{
			query ??= GetCachedQuery<T0, T1>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;
		    var c1 = ComponentID<T1>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];
			Span<TV1> lvs1 = stackalloc TV1[Vector<TV1>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					var tc1 = chunk.GetSpan<T1>(c1);
					var ts1 = MemoryMarshal.Cast<T1, TV1>(tc1);
					var tv1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(ts1);
					if (tv1.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					// Execute the vectorised part
					q.Execute(tv0, tv1, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);
					lvs1.Clear();
					ts1[^leftover..].CopyTo(lvs1);
					var ls1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(lvs1);

					// Execute the leftover at the end
					q.Execute(ls0, ls1, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
                        ts1[ts1.Length - leftover + i] = lvs1[i];
					}
				}
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0, TV1, TV2>
		where TV0 : unmanaged
        where TV1 : unmanaged
        where TV2 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		/// <param name="t1">Span of vectors of values, reinterpreted from component 1</param>
		/// <param name="t2">Span of vectors of values, reinterpreted from component 2</param>
		public void Execute(Span<Vector<TV0>> t0, Span<Vector<TV1>> t1, Span<Vector<TV2>> t2, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T1">The type of the component 1</typeparam>
		/// <typeparam name="TV1">The type component 1 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T2">The type of the component 2</typeparam>
		/// <typeparam name="TV2">The type component 2 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		[ExcludeFromCodeCoverage]
		public int ExecuteVectorChunk<TQ, T0, TV0, T1, TV1, T2, TV2>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where TV0 : unmanaged
            where TV1 : unmanaged
            where TV2 : unmanaged
			where TQ : IVectorChunkQuery<TV0, TV1, TV2>
		{
			query ??= GetCachedQuery<T0, T1, T2>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;
		    var c1 = ComponentID<T1>.ID;
		    var c2 = ComponentID<T2>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];
			Span<TV1> lvs1 = stackalloc TV1[Vector<TV1>.Count];
			Span<TV2> lvs2 = stackalloc TV2[Vector<TV2>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					var tc1 = chunk.GetSpan<T1>(c1);
					var ts1 = MemoryMarshal.Cast<T1, TV1>(tc1);
					var tv1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(ts1);
					if (tv1.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc2 = chunk.GetSpan<T2>(c2);
					var ts2 = MemoryMarshal.Cast<T2, TV2>(tc2);
					var tv2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(ts2);
					if (tv2.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					// Execute the vectorised part
					q.Execute(tv0, tv1, tv2, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);
					lvs1.Clear();
					ts1[^leftover..].CopyTo(lvs1);
					var ls1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(lvs1);
					lvs2.Clear();
					ts2[^leftover..].CopyTo(lvs2);
					var ls2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(lvs2);

					// Execute the leftover at the end
					q.Execute(ls0, ls1, ls2, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
                        ts1[ts1.Length - leftover + i] = lvs1[i];
                        ts2[ts2.Length - leftover + i] = lvs2[i];
					}
				}
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0, TV1, TV2, TV3>
		where TV0 : unmanaged
        where TV1 : unmanaged
        where TV2 : unmanaged
        where TV3 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		/// <param name="t1">Span of vectors of values, reinterpreted from component 1</param>
		/// <param name="t2">Span of vectors of values, reinterpreted from component 2</param>
		/// <param name="t3">Span of vectors of values, reinterpreted from component 3</param>
		public void Execute(Span<Vector<TV0>> t0, Span<Vector<TV1>> t1, Span<Vector<TV2>> t2, Span<Vector<TV3>> t3, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T1">The type of the component 1</typeparam>
		/// <typeparam name="TV1">The type component 1 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T2">The type of the component 2</typeparam>
		/// <typeparam name="TV2">The type component 2 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T3">The type of the component 3</typeparam>
		/// <typeparam name="TV3">The type component 3 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		[ExcludeFromCodeCoverage]
		public int ExecuteVectorChunk<TQ, T0, TV0, T1, TV1, T2, TV2, T3, TV3>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where T3 : struct, IComponent
            where TV0 : unmanaged
            where TV1 : unmanaged
            where TV2 : unmanaged
            where TV3 : unmanaged
			where TQ : IVectorChunkQuery<TV0, TV1, TV2, TV3>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;
		    var c1 = ComponentID<T1>.ID;
		    var c2 = ComponentID<T2>.ID;
		    var c3 = ComponentID<T3>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];
			Span<TV1> lvs1 = stackalloc TV1[Vector<TV1>.Count];
			Span<TV2> lvs2 = stackalloc TV2[Vector<TV2>.Count];
			Span<TV3> lvs3 = stackalloc TV3[Vector<TV3>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					var tc1 = chunk.GetSpan<T1>(c1);
					var ts1 = MemoryMarshal.Cast<T1, TV1>(tc1);
					var tv1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(ts1);
					if (tv1.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc2 = chunk.GetSpan<T2>(c2);
					var ts2 = MemoryMarshal.Cast<T2, TV2>(tc2);
					var tv2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(ts2);
					if (tv2.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc3 = chunk.GetSpan<T3>(c3);
					var ts3 = MemoryMarshal.Cast<T3, TV3>(tc3);
					var tv3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(ts3);
					if (tv3.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					// Execute the vectorised part
					q.Execute(tv0, tv1, tv2, tv3, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);
					lvs1.Clear();
					ts1[^leftover..].CopyTo(lvs1);
					var ls1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(lvs1);
					lvs2.Clear();
					ts2[^leftover..].CopyTo(lvs2);
					var ls2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(lvs2);
					lvs3.Clear();
					ts3[^leftover..].CopyTo(lvs3);
					var ls3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(lvs3);

					// Execute the leftover at the end
					q.Execute(ls0, ls1, ls2, ls3, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
                        ts1[ts1.Length - leftover + i] = lvs1[i];
                        ts2[ts2.Length - leftover + i] = lvs2[i];
                        ts3[ts3.Length - leftover + i] = lvs3[i];
					}
				}
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4>
		where TV0 : unmanaged
        where TV1 : unmanaged
        where TV2 : unmanaged
        where TV3 : unmanaged
        where TV4 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		/// <param name="t1">Span of vectors of values, reinterpreted from component 1</param>
		/// <param name="t2">Span of vectors of values, reinterpreted from component 2</param>
		/// <param name="t3">Span of vectors of values, reinterpreted from component 3</param>
		/// <param name="t4">Span of vectors of values, reinterpreted from component 4</param>
		public void Execute(Span<Vector<TV0>> t0, Span<Vector<TV1>> t1, Span<Vector<TV2>> t2, Span<Vector<TV3>> t3, Span<Vector<TV4>> t4, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T1">The type of the component 1</typeparam>
		/// <typeparam name="TV1">The type component 1 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T2">The type of the component 2</typeparam>
		/// <typeparam name="TV2">The type component 2 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T3">The type of the component 3</typeparam>
		/// <typeparam name="TV3">The type component 3 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T4">The type of the component 4</typeparam>
		/// <typeparam name="TV4">The type component 4 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		[ExcludeFromCodeCoverage]
		public int ExecuteVectorChunk<TQ, T0, TV0, T1, TV1, T2, TV2, T3, TV3, T4, TV4>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where T3 : struct, IComponent
            where T4 : struct, IComponent
            where TV0 : unmanaged
            where TV1 : unmanaged
            where TV2 : unmanaged
            where TV3 : unmanaged
            where TV4 : unmanaged
			where TQ : IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;
		    var c1 = ComponentID<T1>.ID;
		    var c2 = ComponentID<T2>.ID;
		    var c3 = ComponentID<T3>.ID;
		    var c4 = ComponentID<T4>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];
			Span<TV1> lvs1 = stackalloc TV1[Vector<TV1>.Count];
			Span<TV2> lvs2 = stackalloc TV2[Vector<TV2>.Count];
			Span<TV3> lvs3 = stackalloc TV3[Vector<TV3>.Count];
			Span<TV4> lvs4 = stackalloc TV4[Vector<TV4>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					var tc1 = chunk.GetSpan<T1>(c1);
					var ts1 = MemoryMarshal.Cast<T1, TV1>(tc1);
					var tv1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(ts1);
					if (tv1.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc2 = chunk.GetSpan<T2>(c2);
					var ts2 = MemoryMarshal.Cast<T2, TV2>(tc2);
					var tv2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(ts2);
					if (tv2.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc3 = chunk.GetSpan<T3>(c3);
					var ts3 = MemoryMarshal.Cast<T3, TV3>(tc3);
					var tv3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(ts3);
					if (tv3.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc4 = chunk.GetSpan<T4>(c4);
					var ts4 = MemoryMarshal.Cast<T4, TV4>(tc4);
					var tv4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(ts4);
					if (tv4.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					// Execute the vectorised part
					q.Execute(tv0, tv1, tv2, tv3, tv4, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);
					lvs1.Clear();
					ts1[^leftover..].CopyTo(lvs1);
					var ls1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(lvs1);
					lvs2.Clear();
					ts2[^leftover..].CopyTo(lvs2);
					var ls2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(lvs2);
					lvs3.Clear();
					ts3[^leftover..].CopyTo(lvs3);
					var ls3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(lvs3);
					lvs4.Clear();
					ts4[^leftover..].CopyTo(lvs4);
					var ls4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(lvs4);

					// Execute the leftover at the end
					q.Execute(ls0, ls1, ls2, ls3, ls4, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
                        ts1[ts1.Length - leftover + i] = lvs1[i];
                        ts2[ts2.Length - leftover + i] = lvs2[i];
                        ts3[ts3.Length - leftover + i] = lvs3[i];
                        ts4[ts4.Length - leftover + i] = lvs4[i];
					}
				}
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5>
		where TV0 : unmanaged
        where TV1 : unmanaged
        where TV2 : unmanaged
        where TV3 : unmanaged
        where TV4 : unmanaged
        where TV5 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		/// <param name="t1">Span of vectors of values, reinterpreted from component 1</param>
		/// <param name="t2">Span of vectors of values, reinterpreted from component 2</param>
		/// <param name="t3">Span of vectors of values, reinterpreted from component 3</param>
		/// <param name="t4">Span of vectors of values, reinterpreted from component 4</param>
		/// <param name="t5">Span of vectors of values, reinterpreted from component 5</param>
		public void Execute(Span<Vector<TV0>> t0, Span<Vector<TV1>> t1, Span<Vector<TV2>> t2, Span<Vector<TV3>> t3, Span<Vector<TV4>> t4, Span<Vector<TV5>> t5, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T1">The type of the component 1</typeparam>
		/// <typeparam name="TV1">The type component 1 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T2">The type of the component 2</typeparam>
		/// <typeparam name="TV2">The type component 2 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T3">The type of the component 3</typeparam>
		/// <typeparam name="TV3">The type component 3 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T4">The type of the component 4</typeparam>
		/// <typeparam name="TV4">The type component 4 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T5">The type of the component 5</typeparam>
		/// <typeparam name="TV5">The type component 5 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		[ExcludeFromCodeCoverage]
		public int ExecuteVectorChunk<TQ, T0, TV0, T1, TV1, T2, TV2, T3, TV3, T4, TV4, T5, TV5>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where T3 : struct, IComponent
            where T4 : struct, IComponent
            where T5 : struct, IComponent
            where TV0 : unmanaged
            where TV1 : unmanaged
            where TV2 : unmanaged
            where TV3 : unmanaged
            where TV4 : unmanaged
            where TV5 : unmanaged
			where TQ : IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;
		    var c1 = ComponentID<T1>.ID;
		    var c2 = ComponentID<T2>.ID;
		    var c3 = ComponentID<T3>.ID;
		    var c4 = ComponentID<T4>.ID;
		    var c5 = ComponentID<T5>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];
			Span<TV1> lvs1 = stackalloc TV1[Vector<TV1>.Count];
			Span<TV2> lvs2 = stackalloc TV2[Vector<TV2>.Count];
			Span<TV3> lvs3 = stackalloc TV3[Vector<TV3>.Count];
			Span<TV4> lvs4 = stackalloc TV4[Vector<TV4>.Count];
			Span<TV5> lvs5 = stackalloc TV5[Vector<TV5>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					var tc1 = chunk.GetSpan<T1>(c1);
					var ts1 = MemoryMarshal.Cast<T1, TV1>(tc1);
					var tv1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(ts1);
					if (tv1.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc2 = chunk.GetSpan<T2>(c2);
					var ts2 = MemoryMarshal.Cast<T2, TV2>(tc2);
					var tv2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(ts2);
					if (tv2.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc3 = chunk.GetSpan<T3>(c3);
					var ts3 = MemoryMarshal.Cast<T3, TV3>(tc3);
					var tv3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(ts3);
					if (tv3.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc4 = chunk.GetSpan<T4>(c4);
					var ts4 = MemoryMarshal.Cast<T4, TV4>(tc4);
					var tv4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(ts4);
					if (tv4.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc5 = chunk.GetSpan<T5>(c5);
					var ts5 = MemoryMarshal.Cast<T5, TV5>(tc5);
					var tv5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(ts5);
					if (tv5.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					// Execute the vectorised part
					q.Execute(tv0, tv1, tv2, tv3, tv4, tv5, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);
					lvs1.Clear();
					ts1[^leftover..].CopyTo(lvs1);
					var ls1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(lvs1);
					lvs2.Clear();
					ts2[^leftover..].CopyTo(lvs2);
					var ls2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(lvs2);
					lvs3.Clear();
					ts3[^leftover..].CopyTo(lvs3);
					var ls3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(lvs3);
					lvs4.Clear();
					ts4[^leftover..].CopyTo(lvs4);
					var ls4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(lvs4);
					lvs5.Clear();
					ts5[^leftover..].CopyTo(lvs5);
					var ls5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(lvs5);

					// Execute the leftover at the end
					q.Execute(ls0, ls1, ls2, ls3, ls4, ls5, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
                        ts1[ts1.Length - leftover + i] = lvs1[i];
                        ts2[ts2.Length - leftover + i] = lvs2[i];
                        ts3[ts3.Length - leftover + i] = lvs3[i];
                        ts4[ts4.Length - leftover + i] = lvs4[i];
                        ts5[ts5.Length - leftover + i] = lvs5[i];
					}
				}
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6>
		where TV0 : unmanaged
        where TV1 : unmanaged
        where TV2 : unmanaged
        where TV3 : unmanaged
        where TV4 : unmanaged
        where TV5 : unmanaged
        where TV6 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		/// <param name="t1">Span of vectors of values, reinterpreted from component 1</param>
		/// <param name="t2">Span of vectors of values, reinterpreted from component 2</param>
		/// <param name="t3">Span of vectors of values, reinterpreted from component 3</param>
		/// <param name="t4">Span of vectors of values, reinterpreted from component 4</param>
		/// <param name="t5">Span of vectors of values, reinterpreted from component 5</param>
		/// <param name="t6">Span of vectors of values, reinterpreted from component 6</param>
		public void Execute(Span<Vector<TV0>> t0, Span<Vector<TV1>> t1, Span<Vector<TV2>> t2, Span<Vector<TV3>> t3, Span<Vector<TV4>> t4, Span<Vector<TV5>> t5, Span<Vector<TV6>> t6, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T1">The type of the component 1</typeparam>
		/// <typeparam name="TV1">The type component 1 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T2">The type of the component 2</typeparam>
		/// <typeparam name="TV2">The type component 2 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T3">The type of the component 3</typeparam>
		/// <typeparam name="TV3">The type component 3 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T4">The type of the component 4</typeparam>
		/// <typeparam name="TV4">The type component 4 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T5">The type of the component 5</typeparam>
		/// <typeparam name="TV5">The type component 5 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T6">The type of the component 6</typeparam>
		/// <typeparam name="TV6">The type component 6 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		[ExcludeFromCodeCoverage]
		public int ExecuteVectorChunk<TQ, T0, TV0, T1, TV1, T2, TV2, T3, TV3, T4, TV4, T5, TV5, T6, TV6>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where T3 : struct, IComponent
            where T4 : struct, IComponent
            where T5 : struct, IComponent
            where T6 : struct, IComponent
            where TV0 : unmanaged
            where TV1 : unmanaged
            where TV2 : unmanaged
            where TV3 : unmanaged
            where TV4 : unmanaged
            where TV5 : unmanaged
            where TV6 : unmanaged
			where TQ : IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;
		    var c1 = ComponentID<T1>.ID;
		    var c2 = ComponentID<T2>.ID;
		    var c3 = ComponentID<T3>.ID;
		    var c4 = ComponentID<T4>.ID;
		    var c5 = ComponentID<T5>.ID;
		    var c6 = ComponentID<T6>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];
			Span<TV1> lvs1 = stackalloc TV1[Vector<TV1>.Count];
			Span<TV2> lvs2 = stackalloc TV2[Vector<TV2>.Count];
			Span<TV3> lvs3 = stackalloc TV3[Vector<TV3>.Count];
			Span<TV4> lvs4 = stackalloc TV4[Vector<TV4>.Count];
			Span<TV5> lvs5 = stackalloc TV5[Vector<TV5>.Count];
			Span<TV6> lvs6 = stackalloc TV6[Vector<TV6>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					var tc1 = chunk.GetSpan<T1>(c1);
					var ts1 = MemoryMarshal.Cast<T1, TV1>(tc1);
					var tv1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(ts1);
					if (tv1.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc2 = chunk.GetSpan<T2>(c2);
					var ts2 = MemoryMarshal.Cast<T2, TV2>(tc2);
					var tv2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(ts2);
					if (tv2.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc3 = chunk.GetSpan<T3>(c3);
					var ts3 = MemoryMarshal.Cast<T3, TV3>(tc3);
					var tv3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(ts3);
					if (tv3.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc4 = chunk.GetSpan<T4>(c4);
					var ts4 = MemoryMarshal.Cast<T4, TV4>(tc4);
					var tv4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(ts4);
					if (tv4.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc5 = chunk.GetSpan<T5>(c5);
					var ts5 = MemoryMarshal.Cast<T5, TV5>(tc5);
					var tv5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(ts5);
					if (tv5.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc6 = chunk.GetSpan<T6>(c6);
					var ts6 = MemoryMarshal.Cast<T6, TV6>(tc6);
					var tv6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(ts6);
					if (tv6.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					// Execute the vectorised part
					q.Execute(tv0, tv1, tv2, tv3, tv4, tv5, tv6, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);
					lvs1.Clear();
					ts1[^leftover..].CopyTo(lvs1);
					var ls1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(lvs1);
					lvs2.Clear();
					ts2[^leftover..].CopyTo(lvs2);
					var ls2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(lvs2);
					lvs3.Clear();
					ts3[^leftover..].CopyTo(lvs3);
					var ls3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(lvs3);
					lvs4.Clear();
					ts4[^leftover..].CopyTo(lvs4);
					var ls4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(lvs4);
					lvs5.Clear();
					ts5[^leftover..].CopyTo(lvs5);
					var ls5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(lvs5);
					lvs6.Clear();
					ts6[^leftover..].CopyTo(lvs6);
					var ls6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(lvs6);

					// Execute the leftover at the end
					q.Execute(ls0, ls1, ls2, ls3, ls4, ls5, ls6, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
                        ts1[ts1.Length - leftover + i] = lvs1[i];
                        ts2[ts2.Length - leftover + i] = lvs2[i];
                        ts3[ts3.Length - leftover + i] = lvs3[i];
                        ts4[ts4.Length - leftover + i] = lvs4[i];
                        ts5[ts5.Length - leftover + i] = lvs5[i];
                        ts6[ts6.Length - leftover + i] = lvs6[i];
					}
				}
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7>
		where TV0 : unmanaged
        where TV1 : unmanaged
        where TV2 : unmanaged
        where TV3 : unmanaged
        where TV4 : unmanaged
        where TV5 : unmanaged
        where TV6 : unmanaged
        where TV7 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		/// <param name="t1">Span of vectors of values, reinterpreted from component 1</param>
		/// <param name="t2">Span of vectors of values, reinterpreted from component 2</param>
		/// <param name="t3">Span of vectors of values, reinterpreted from component 3</param>
		/// <param name="t4">Span of vectors of values, reinterpreted from component 4</param>
		/// <param name="t5">Span of vectors of values, reinterpreted from component 5</param>
		/// <param name="t6">Span of vectors of values, reinterpreted from component 6</param>
		/// <param name="t7">Span of vectors of values, reinterpreted from component 7</param>
		public void Execute(Span<Vector<TV0>> t0, Span<Vector<TV1>> t1, Span<Vector<TV2>> t2, Span<Vector<TV3>> t3, Span<Vector<TV4>> t4, Span<Vector<TV5>> t5, Span<Vector<TV6>> t6, Span<Vector<TV7>> t7, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T1">The type of the component 1</typeparam>
		/// <typeparam name="TV1">The type component 1 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T2">The type of the component 2</typeparam>
		/// <typeparam name="TV2">The type component 2 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T3">The type of the component 3</typeparam>
		/// <typeparam name="TV3">The type component 3 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T4">The type of the component 4</typeparam>
		/// <typeparam name="TV4">The type component 4 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T5">The type of the component 5</typeparam>
		/// <typeparam name="TV5">The type component 5 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T6">The type of the component 6</typeparam>
		/// <typeparam name="TV6">The type component 6 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T7">The type of the component 7</typeparam>
		/// <typeparam name="TV7">The type component 7 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		[ExcludeFromCodeCoverage]
		public int ExecuteVectorChunk<TQ, T0, TV0, T1, TV1, T2, TV2, T3, TV3, T4, TV4, T5, TV5, T6, TV6, T7, TV7>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where T3 : struct, IComponent
            where T4 : struct, IComponent
            where T5 : struct, IComponent
            where T6 : struct, IComponent
            where T7 : struct, IComponent
            where TV0 : unmanaged
            where TV1 : unmanaged
            where TV2 : unmanaged
            where TV3 : unmanaged
            where TV4 : unmanaged
            where TV5 : unmanaged
            where TV6 : unmanaged
            where TV7 : unmanaged
			where TQ : IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;
		    var c1 = ComponentID<T1>.ID;
		    var c2 = ComponentID<T2>.ID;
		    var c3 = ComponentID<T3>.ID;
		    var c4 = ComponentID<T4>.ID;
		    var c5 = ComponentID<T5>.ID;
		    var c6 = ComponentID<T6>.ID;
		    var c7 = ComponentID<T7>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];
			Span<TV1> lvs1 = stackalloc TV1[Vector<TV1>.Count];
			Span<TV2> lvs2 = stackalloc TV2[Vector<TV2>.Count];
			Span<TV3> lvs3 = stackalloc TV3[Vector<TV3>.Count];
			Span<TV4> lvs4 = stackalloc TV4[Vector<TV4>.Count];
			Span<TV5> lvs5 = stackalloc TV5[Vector<TV5>.Count];
			Span<TV6> lvs6 = stackalloc TV6[Vector<TV6>.Count];
			Span<TV7> lvs7 = stackalloc TV7[Vector<TV7>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					var tc1 = chunk.GetSpan<T1>(c1);
					var ts1 = MemoryMarshal.Cast<T1, TV1>(tc1);
					var tv1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(ts1);
					if (tv1.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc2 = chunk.GetSpan<T2>(c2);
					var ts2 = MemoryMarshal.Cast<T2, TV2>(tc2);
					var tv2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(ts2);
					if (tv2.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc3 = chunk.GetSpan<T3>(c3);
					var ts3 = MemoryMarshal.Cast<T3, TV3>(tc3);
					var tv3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(ts3);
					if (tv3.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc4 = chunk.GetSpan<T4>(c4);
					var ts4 = MemoryMarshal.Cast<T4, TV4>(tc4);
					var tv4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(ts4);
					if (tv4.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc5 = chunk.GetSpan<T5>(c5);
					var ts5 = MemoryMarshal.Cast<T5, TV5>(tc5);
					var tv5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(ts5);
					if (tv5.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc6 = chunk.GetSpan<T6>(c6);
					var ts6 = MemoryMarshal.Cast<T6, TV6>(tc6);
					var tv6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(ts6);
					if (tv6.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc7 = chunk.GetSpan<T7>(c7);
					var ts7 = MemoryMarshal.Cast<T7, TV7>(tc7);
					var tv7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(ts7);
					if (tv7.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					// Execute the vectorised part
					q.Execute(tv0, tv1, tv2, tv3, tv4, tv5, tv6, tv7, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);
					lvs1.Clear();
					ts1[^leftover..].CopyTo(lvs1);
					var ls1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(lvs1);
					lvs2.Clear();
					ts2[^leftover..].CopyTo(lvs2);
					var ls2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(lvs2);
					lvs3.Clear();
					ts3[^leftover..].CopyTo(lvs3);
					var ls3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(lvs3);
					lvs4.Clear();
					ts4[^leftover..].CopyTo(lvs4);
					var ls4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(lvs4);
					lvs5.Clear();
					ts5[^leftover..].CopyTo(lvs5);
					var ls5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(lvs5);
					lvs6.Clear();
					ts6[^leftover..].CopyTo(lvs6);
					var ls6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(lvs6);
					lvs7.Clear();
					ts7[^leftover..].CopyTo(lvs7);
					var ls7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(lvs7);

					// Execute the leftover at the end
					q.Execute(ls0, ls1, ls2, ls3, ls4, ls5, ls6, ls7, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
                        ts1[ts1.Length - leftover + i] = lvs1[i];
                        ts2[ts2.Length - leftover + i] = lvs2[i];
                        ts3[ts3.Length - leftover + i] = lvs3[i];
                        ts4[ts4.Length - leftover + i] = lvs4[i];
                        ts5[ts5.Length - leftover + i] = lvs5[i];
                        ts6[ts6.Length - leftover + i] = lvs6[i];
                        ts7[ts7.Length - leftover + i] = lvs7[i];
					}
				}
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8>
		where TV0 : unmanaged
        where TV1 : unmanaged
        where TV2 : unmanaged
        where TV3 : unmanaged
        where TV4 : unmanaged
        where TV5 : unmanaged
        where TV6 : unmanaged
        where TV7 : unmanaged
        where TV8 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		/// <param name="t1">Span of vectors of values, reinterpreted from component 1</param>
		/// <param name="t2">Span of vectors of values, reinterpreted from component 2</param>
		/// <param name="t3">Span of vectors of values, reinterpreted from component 3</param>
		/// <param name="t4">Span of vectors of values, reinterpreted from component 4</param>
		/// <param name="t5">Span of vectors of values, reinterpreted from component 5</param>
		/// <param name="t6">Span of vectors of values, reinterpreted from component 6</param>
		/// <param name="t7">Span of vectors of values, reinterpreted from component 7</param>
		/// <param name="t8">Span of vectors of values, reinterpreted from component 8</param>
		public void Execute(Span<Vector<TV0>> t0, Span<Vector<TV1>> t1, Span<Vector<TV2>> t2, Span<Vector<TV3>> t3, Span<Vector<TV4>> t4, Span<Vector<TV5>> t5, Span<Vector<TV6>> t6, Span<Vector<TV7>> t7, Span<Vector<TV8>> t8, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T1">The type of the component 1</typeparam>
		/// <typeparam name="TV1">The type component 1 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T2">The type of the component 2</typeparam>
		/// <typeparam name="TV2">The type component 2 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T3">The type of the component 3</typeparam>
		/// <typeparam name="TV3">The type component 3 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T4">The type of the component 4</typeparam>
		/// <typeparam name="TV4">The type component 4 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T5">The type of the component 5</typeparam>
		/// <typeparam name="TV5">The type component 5 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T6">The type of the component 6</typeparam>
		/// <typeparam name="TV6">The type component 6 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T7">The type of the component 7</typeparam>
		/// <typeparam name="TV7">The type component 7 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T8">The type of the component 8</typeparam>
		/// <typeparam name="TV8">The type component 8 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		[ExcludeFromCodeCoverage]
		public int ExecuteVectorChunk<TQ, T0, TV0, T1, TV1, T2, TV2, T3, TV3, T4, TV4, T5, TV5, T6, TV6, T7, TV7, T8, TV8>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where T3 : struct, IComponent
            where T4 : struct, IComponent
            where T5 : struct, IComponent
            where T6 : struct, IComponent
            where T7 : struct, IComponent
            where T8 : struct, IComponent
            where TV0 : unmanaged
            where TV1 : unmanaged
            where TV2 : unmanaged
            where TV3 : unmanaged
            where TV4 : unmanaged
            where TV5 : unmanaged
            where TV6 : unmanaged
            where TV7 : unmanaged
            where TV8 : unmanaged
			where TQ : IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;
		    var c1 = ComponentID<T1>.ID;
		    var c2 = ComponentID<T2>.ID;
		    var c3 = ComponentID<T3>.ID;
		    var c4 = ComponentID<T4>.ID;
		    var c5 = ComponentID<T5>.ID;
		    var c6 = ComponentID<T6>.ID;
		    var c7 = ComponentID<T7>.ID;
		    var c8 = ComponentID<T8>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];
			Span<TV1> lvs1 = stackalloc TV1[Vector<TV1>.Count];
			Span<TV2> lvs2 = stackalloc TV2[Vector<TV2>.Count];
			Span<TV3> lvs3 = stackalloc TV3[Vector<TV3>.Count];
			Span<TV4> lvs4 = stackalloc TV4[Vector<TV4>.Count];
			Span<TV5> lvs5 = stackalloc TV5[Vector<TV5>.Count];
			Span<TV6> lvs6 = stackalloc TV6[Vector<TV6>.Count];
			Span<TV7> lvs7 = stackalloc TV7[Vector<TV7>.Count];
			Span<TV8> lvs8 = stackalloc TV8[Vector<TV8>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					var tc1 = chunk.GetSpan<T1>(c1);
					var ts1 = MemoryMarshal.Cast<T1, TV1>(tc1);
					var tv1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(ts1);
					if (tv1.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc2 = chunk.GetSpan<T2>(c2);
					var ts2 = MemoryMarshal.Cast<T2, TV2>(tc2);
					var tv2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(ts2);
					if (tv2.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc3 = chunk.GetSpan<T3>(c3);
					var ts3 = MemoryMarshal.Cast<T3, TV3>(tc3);
					var tv3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(ts3);
					if (tv3.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc4 = chunk.GetSpan<T4>(c4);
					var ts4 = MemoryMarshal.Cast<T4, TV4>(tc4);
					var tv4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(ts4);
					if (tv4.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc5 = chunk.GetSpan<T5>(c5);
					var ts5 = MemoryMarshal.Cast<T5, TV5>(tc5);
					var tv5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(ts5);
					if (tv5.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc6 = chunk.GetSpan<T6>(c6);
					var ts6 = MemoryMarshal.Cast<T6, TV6>(tc6);
					var tv6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(ts6);
					if (tv6.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc7 = chunk.GetSpan<T7>(c7);
					var ts7 = MemoryMarshal.Cast<T7, TV7>(tc7);
					var tv7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(ts7);
					if (tv7.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc8 = chunk.GetSpan<T8>(c8);
					var ts8 = MemoryMarshal.Cast<T8, TV8>(tc8);
					var tv8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(ts8);
					if (tv8.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					// Execute the vectorised part
					q.Execute(tv0, tv1, tv2, tv3, tv4, tv5, tv6, tv7, tv8, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);
					lvs1.Clear();
					ts1[^leftover..].CopyTo(lvs1);
					var ls1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(lvs1);
					lvs2.Clear();
					ts2[^leftover..].CopyTo(lvs2);
					var ls2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(lvs2);
					lvs3.Clear();
					ts3[^leftover..].CopyTo(lvs3);
					var ls3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(lvs3);
					lvs4.Clear();
					ts4[^leftover..].CopyTo(lvs4);
					var ls4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(lvs4);
					lvs5.Clear();
					ts5[^leftover..].CopyTo(lvs5);
					var ls5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(lvs5);
					lvs6.Clear();
					ts6[^leftover..].CopyTo(lvs6);
					var ls6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(lvs6);
					lvs7.Clear();
					ts7[^leftover..].CopyTo(lvs7);
					var ls7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(lvs7);
					lvs8.Clear();
					ts8[^leftover..].CopyTo(lvs8);
					var ls8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(lvs8);

					// Execute the leftover at the end
					q.Execute(ls0, ls1, ls2, ls3, ls4, ls5, ls6, ls7, ls8, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
                        ts1[ts1.Length - leftover + i] = lvs1[i];
                        ts2[ts2.Length - leftover + i] = lvs2[i];
                        ts3[ts3.Length - leftover + i] = lvs3[i];
                        ts4[ts4.Length - leftover + i] = lvs4[i];
                        ts5[ts5.Length - leftover + i] = lvs5[i];
                        ts6[ts6.Length - leftover + i] = lvs6[i];
                        ts7[ts7.Length - leftover + i] = lvs7[i];
                        ts8[ts8.Length - leftover + i] = lvs8[i];
					}
				}
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8, TV9>
		where TV0 : unmanaged
        where TV1 : unmanaged
        where TV2 : unmanaged
        where TV3 : unmanaged
        where TV4 : unmanaged
        where TV5 : unmanaged
        where TV6 : unmanaged
        where TV7 : unmanaged
        where TV8 : unmanaged
        where TV9 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		/// <param name="t1">Span of vectors of values, reinterpreted from component 1</param>
		/// <param name="t2">Span of vectors of values, reinterpreted from component 2</param>
		/// <param name="t3">Span of vectors of values, reinterpreted from component 3</param>
		/// <param name="t4">Span of vectors of values, reinterpreted from component 4</param>
		/// <param name="t5">Span of vectors of values, reinterpreted from component 5</param>
		/// <param name="t6">Span of vectors of values, reinterpreted from component 6</param>
		/// <param name="t7">Span of vectors of values, reinterpreted from component 7</param>
		/// <param name="t8">Span of vectors of values, reinterpreted from component 8</param>
		/// <param name="t9">Span of vectors of values, reinterpreted from component 9</param>
		public void Execute(Span<Vector<TV0>> t0, Span<Vector<TV1>> t1, Span<Vector<TV2>> t2, Span<Vector<TV3>> t3, Span<Vector<TV4>> t4, Span<Vector<TV5>> t5, Span<Vector<TV6>> t6, Span<Vector<TV7>> t7, Span<Vector<TV8>> t8, Span<Vector<TV9>> t9, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T1">The type of the component 1</typeparam>
		/// <typeparam name="TV1">The type component 1 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T2">The type of the component 2</typeparam>
		/// <typeparam name="TV2">The type component 2 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T3">The type of the component 3</typeparam>
		/// <typeparam name="TV3">The type component 3 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T4">The type of the component 4</typeparam>
		/// <typeparam name="TV4">The type component 4 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T5">The type of the component 5</typeparam>
		/// <typeparam name="TV5">The type component 5 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T6">The type of the component 6</typeparam>
		/// <typeparam name="TV6">The type component 6 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T7">The type of the component 7</typeparam>
		/// <typeparam name="TV7">The type component 7 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T8">The type of the component 8</typeparam>
		/// <typeparam name="TV8">The type component 8 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T9">The type of the component 9</typeparam>
		/// <typeparam name="TV9">The type component 9 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		[ExcludeFromCodeCoverage]
		public int ExecuteVectorChunk<TQ, T0, TV0, T1, TV1, T2, TV2, T3, TV3, T4, TV4, T5, TV5, T6, TV6, T7, TV7, T8, TV8, T9, TV9>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where T3 : struct, IComponent
            where T4 : struct, IComponent
            where T5 : struct, IComponent
            where T6 : struct, IComponent
            where T7 : struct, IComponent
            where T8 : struct, IComponent
            where T9 : struct, IComponent
            where TV0 : unmanaged
            where TV1 : unmanaged
            where TV2 : unmanaged
            where TV3 : unmanaged
            where TV4 : unmanaged
            where TV5 : unmanaged
            where TV6 : unmanaged
            where TV7 : unmanaged
            where TV8 : unmanaged
            where TV9 : unmanaged
			where TQ : IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8, TV9>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;
		    var c1 = ComponentID<T1>.ID;
		    var c2 = ComponentID<T2>.ID;
		    var c3 = ComponentID<T3>.ID;
		    var c4 = ComponentID<T4>.ID;
		    var c5 = ComponentID<T5>.ID;
		    var c6 = ComponentID<T6>.ID;
		    var c7 = ComponentID<T7>.ID;
		    var c8 = ComponentID<T8>.ID;
		    var c9 = ComponentID<T9>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];
			Span<TV1> lvs1 = stackalloc TV1[Vector<TV1>.Count];
			Span<TV2> lvs2 = stackalloc TV2[Vector<TV2>.Count];
			Span<TV3> lvs3 = stackalloc TV3[Vector<TV3>.Count];
			Span<TV4> lvs4 = stackalloc TV4[Vector<TV4>.Count];
			Span<TV5> lvs5 = stackalloc TV5[Vector<TV5>.Count];
			Span<TV6> lvs6 = stackalloc TV6[Vector<TV6>.Count];
			Span<TV7> lvs7 = stackalloc TV7[Vector<TV7>.Count];
			Span<TV8> lvs8 = stackalloc TV8[Vector<TV8>.Count];
			Span<TV9> lvs9 = stackalloc TV9[Vector<TV9>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					var tc1 = chunk.GetSpan<T1>(c1);
					var ts1 = MemoryMarshal.Cast<T1, TV1>(tc1);
					var tv1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(ts1);
					if (tv1.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc2 = chunk.GetSpan<T2>(c2);
					var ts2 = MemoryMarshal.Cast<T2, TV2>(tc2);
					var tv2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(ts2);
					if (tv2.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc3 = chunk.GetSpan<T3>(c3);
					var ts3 = MemoryMarshal.Cast<T3, TV3>(tc3);
					var tv3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(ts3);
					if (tv3.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc4 = chunk.GetSpan<T4>(c4);
					var ts4 = MemoryMarshal.Cast<T4, TV4>(tc4);
					var tv4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(ts4);
					if (tv4.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc5 = chunk.GetSpan<T5>(c5);
					var ts5 = MemoryMarshal.Cast<T5, TV5>(tc5);
					var tv5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(ts5);
					if (tv5.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc6 = chunk.GetSpan<T6>(c6);
					var ts6 = MemoryMarshal.Cast<T6, TV6>(tc6);
					var tv6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(ts6);
					if (tv6.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc7 = chunk.GetSpan<T7>(c7);
					var ts7 = MemoryMarshal.Cast<T7, TV7>(tc7);
					var tv7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(ts7);
					if (tv7.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc8 = chunk.GetSpan<T8>(c8);
					var ts8 = MemoryMarshal.Cast<T8, TV8>(tc8);
					var tv8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(ts8);
					if (tv8.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc9 = chunk.GetSpan<T9>(c9);
					var ts9 = MemoryMarshal.Cast<T9, TV9>(tc9);
					var tv9 = MemoryMarshal.Cast<TV9, Vector<TV9>>(ts9);
					if (tv9.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					// Execute the vectorised part
					q.Execute(tv0, tv1, tv2, tv3, tv4, tv5, tv6, tv7, tv8, tv9, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);
					lvs1.Clear();
					ts1[^leftover..].CopyTo(lvs1);
					var ls1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(lvs1);
					lvs2.Clear();
					ts2[^leftover..].CopyTo(lvs2);
					var ls2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(lvs2);
					lvs3.Clear();
					ts3[^leftover..].CopyTo(lvs3);
					var ls3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(lvs3);
					lvs4.Clear();
					ts4[^leftover..].CopyTo(lvs4);
					var ls4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(lvs4);
					lvs5.Clear();
					ts5[^leftover..].CopyTo(lvs5);
					var ls5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(lvs5);
					lvs6.Clear();
					ts6[^leftover..].CopyTo(lvs6);
					var ls6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(lvs6);
					lvs7.Clear();
					ts7[^leftover..].CopyTo(lvs7);
					var ls7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(lvs7);
					lvs8.Clear();
					ts8[^leftover..].CopyTo(lvs8);
					var ls8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(lvs8);
					lvs9.Clear();
					ts9[^leftover..].CopyTo(lvs9);
					var ls9 = MemoryMarshal.Cast<TV9, Vector<TV9>>(lvs9);

					// Execute the leftover at the end
					q.Execute(ls0, ls1, ls2, ls3, ls4, ls5, ls6, ls7, ls8, ls9, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
                        ts1[ts1.Length - leftover + i] = lvs1[i];
                        ts2[ts2.Length - leftover + i] = lvs2[i];
                        ts3[ts3.Length - leftover + i] = lvs3[i];
                        ts4[ts4.Length - leftover + i] = lvs4[i];
                        ts5[ts5.Length - leftover + i] = lvs5[i];
                        ts6[ts6.Length - leftover + i] = lvs6[i];
                        ts7[ts7.Length - leftover + i] = lvs7[i];
                        ts8[ts8.Length - leftover + i] = lvs8[i];
                        ts9[ts9.Length - leftover + i] = lvs9[i];
					}
				}
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8, TV9, TV10>
		where TV0 : unmanaged
        where TV1 : unmanaged
        where TV2 : unmanaged
        where TV3 : unmanaged
        where TV4 : unmanaged
        where TV5 : unmanaged
        where TV6 : unmanaged
        where TV7 : unmanaged
        where TV8 : unmanaged
        where TV9 : unmanaged
        where TV10 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		/// <param name="t1">Span of vectors of values, reinterpreted from component 1</param>
		/// <param name="t2">Span of vectors of values, reinterpreted from component 2</param>
		/// <param name="t3">Span of vectors of values, reinterpreted from component 3</param>
		/// <param name="t4">Span of vectors of values, reinterpreted from component 4</param>
		/// <param name="t5">Span of vectors of values, reinterpreted from component 5</param>
		/// <param name="t6">Span of vectors of values, reinterpreted from component 6</param>
		/// <param name="t7">Span of vectors of values, reinterpreted from component 7</param>
		/// <param name="t8">Span of vectors of values, reinterpreted from component 8</param>
		/// <param name="t9">Span of vectors of values, reinterpreted from component 9</param>
		/// <param name="t10">Span of vectors of values, reinterpreted from component 10</param>
		public void Execute(Span<Vector<TV0>> t0, Span<Vector<TV1>> t1, Span<Vector<TV2>> t2, Span<Vector<TV3>> t3, Span<Vector<TV4>> t4, Span<Vector<TV5>> t5, Span<Vector<TV6>> t6, Span<Vector<TV7>> t7, Span<Vector<TV8>> t8, Span<Vector<TV9>> t9, Span<Vector<TV10>> t10, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T1">The type of the component 1</typeparam>
		/// <typeparam name="TV1">The type component 1 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T2">The type of the component 2</typeparam>
		/// <typeparam name="TV2">The type component 2 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T3">The type of the component 3</typeparam>
		/// <typeparam name="TV3">The type component 3 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T4">The type of the component 4</typeparam>
		/// <typeparam name="TV4">The type component 4 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T5">The type of the component 5</typeparam>
		/// <typeparam name="TV5">The type component 5 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T6">The type of the component 6</typeparam>
		/// <typeparam name="TV6">The type component 6 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T7">The type of the component 7</typeparam>
		/// <typeparam name="TV7">The type component 7 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T8">The type of the component 8</typeparam>
		/// <typeparam name="TV8">The type component 8 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T9">The type of the component 9</typeparam>
		/// <typeparam name="TV9">The type component 9 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T10">The type of the component 10</typeparam>
		/// <typeparam name="TV10">The type component 10 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		[ExcludeFromCodeCoverage]
		public int ExecuteVectorChunk<TQ, T0, TV0, T1, TV1, T2, TV2, T3, TV3, T4, TV4, T5, TV5, T6, TV6, T7, TV7, T8, TV8, T9, TV9, T10, TV10>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where T3 : struct, IComponent
            where T4 : struct, IComponent
            where T5 : struct, IComponent
            where T6 : struct, IComponent
            where T7 : struct, IComponent
            where T8 : struct, IComponent
            where T9 : struct, IComponent
            where T10 : struct, IComponent
            where TV0 : unmanaged
            where TV1 : unmanaged
            where TV2 : unmanaged
            where TV3 : unmanaged
            where TV4 : unmanaged
            where TV5 : unmanaged
            where TV6 : unmanaged
            where TV7 : unmanaged
            where TV8 : unmanaged
            where TV9 : unmanaged
            where TV10 : unmanaged
			where TQ : IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8, TV9, TV10>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;
		    var c1 = ComponentID<T1>.ID;
		    var c2 = ComponentID<T2>.ID;
		    var c3 = ComponentID<T3>.ID;
		    var c4 = ComponentID<T4>.ID;
		    var c5 = ComponentID<T5>.ID;
		    var c6 = ComponentID<T6>.ID;
		    var c7 = ComponentID<T7>.ID;
		    var c8 = ComponentID<T8>.ID;
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];
			Span<TV1> lvs1 = stackalloc TV1[Vector<TV1>.Count];
			Span<TV2> lvs2 = stackalloc TV2[Vector<TV2>.Count];
			Span<TV3> lvs3 = stackalloc TV3[Vector<TV3>.Count];
			Span<TV4> lvs4 = stackalloc TV4[Vector<TV4>.Count];
			Span<TV5> lvs5 = stackalloc TV5[Vector<TV5>.Count];
			Span<TV6> lvs6 = stackalloc TV6[Vector<TV6>.Count];
			Span<TV7> lvs7 = stackalloc TV7[Vector<TV7>.Count];
			Span<TV8> lvs8 = stackalloc TV8[Vector<TV8>.Count];
			Span<TV9> lvs9 = stackalloc TV9[Vector<TV9>.Count];
			Span<TV10> lvs10 = stackalloc TV10[Vector<TV10>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					var tc1 = chunk.GetSpan<T1>(c1);
					var ts1 = MemoryMarshal.Cast<T1, TV1>(tc1);
					var tv1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(ts1);
					if (tv1.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc2 = chunk.GetSpan<T2>(c2);
					var ts2 = MemoryMarshal.Cast<T2, TV2>(tc2);
					var tv2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(ts2);
					if (tv2.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc3 = chunk.GetSpan<T3>(c3);
					var ts3 = MemoryMarshal.Cast<T3, TV3>(tc3);
					var tv3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(ts3);
					if (tv3.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc4 = chunk.GetSpan<T4>(c4);
					var ts4 = MemoryMarshal.Cast<T4, TV4>(tc4);
					var tv4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(ts4);
					if (tv4.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc5 = chunk.GetSpan<T5>(c5);
					var ts5 = MemoryMarshal.Cast<T5, TV5>(tc5);
					var tv5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(ts5);
					if (tv5.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc6 = chunk.GetSpan<T6>(c6);
					var ts6 = MemoryMarshal.Cast<T6, TV6>(tc6);
					var tv6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(ts6);
					if (tv6.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc7 = chunk.GetSpan<T7>(c7);
					var ts7 = MemoryMarshal.Cast<T7, TV7>(tc7);
					var tv7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(ts7);
					if (tv7.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc8 = chunk.GetSpan<T8>(c8);
					var ts8 = MemoryMarshal.Cast<T8, TV8>(tc8);
					var tv8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(ts8);
					if (tv8.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc9 = chunk.GetSpan<T9>(c9);
					var ts9 = MemoryMarshal.Cast<T9, TV9>(tc9);
					var tv9 = MemoryMarshal.Cast<TV9, Vector<TV9>>(ts9);
					if (tv9.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc10 = chunk.GetSpan<T10>(c10);
					var ts10 = MemoryMarshal.Cast<T10, TV10>(tc10);
					var tv10 = MemoryMarshal.Cast<TV10, Vector<TV10>>(ts10);
					if (tv10.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					// Execute the vectorised part
					q.Execute(tv0, tv1, tv2, tv3, tv4, tv5, tv6, tv7, tv8, tv9, tv10, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);
					lvs1.Clear();
					ts1[^leftover..].CopyTo(lvs1);
					var ls1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(lvs1);
					lvs2.Clear();
					ts2[^leftover..].CopyTo(lvs2);
					var ls2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(lvs2);
					lvs3.Clear();
					ts3[^leftover..].CopyTo(lvs3);
					var ls3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(lvs3);
					lvs4.Clear();
					ts4[^leftover..].CopyTo(lvs4);
					var ls4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(lvs4);
					lvs5.Clear();
					ts5[^leftover..].CopyTo(lvs5);
					var ls5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(lvs5);
					lvs6.Clear();
					ts6[^leftover..].CopyTo(lvs6);
					var ls6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(lvs6);
					lvs7.Clear();
					ts7[^leftover..].CopyTo(lvs7);
					var ls7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(lvs7);
					lvs8.Clear();
					ts8[^leftover..].CopyTo(lvs8);
					var ls8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(lvs8);
					lvs9.Clear();
					ts9[^leftover..].CopyTo(lvs9);
					var ls9 = MemoryMarshal.Cast<TV9, Vector<TV9>>(lvs9);
					lvs10.Clear();
					ts10[^leftover..].CopyTo(lvs10);
					var ls10 = MemoryMarshal.Cast<TV10, Vector<TV10>>(lvs10);

					// Execute the leftover at the end
					q.Execute(ls0, ls1, ls2, ls3, ls4, ls5, ls6, ls7, ls8, ls9, ls10, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
                        ts1[ts1.Length - leftover + i] = lvs1[i];
                        ts2[ts2.Length - leftover + i] = lvs2[i];
                        ts3[ts3.Length - leftover + i] = lvs3[i];
                        ts4[ts4.Length - leftover + i] = lvs4[i];
                        ts5[ts5.Length - leftover + i] = lvs5[i];
                        ts6[ts6.Length - leftover + i] = lvs6[i];
                        ts7[ts7.Length - leftover + i] = lvs7[i];
                        ts8[ts8.Length - leftover + i] = lvs8[i];
                        ts9[ts9.Length - leftover + i] = lvs9[i];
                        ts10[ts10.Length - leftover + i] = lvs10[i];
					}
				}
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8, TV9, TV10, TV11>
		where TV0 : unmanaged
        where TV1 : unmanaged
        where TV2 : unmanaged
        where TV3 : unmanaged
        where TV4 : unmanaged
        where TV5 : unmanaged
        where TV6 : unmanaged
        where TV7 : unmanaged
        where TV8 : unmanaged
        where TV9 : unmanaged
        where TV10 : unmanaged
        where TV11 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		/// <param name="t1">Span of vectors of values, reinterpreted from component 1</param>
		/// <param name="t2">Span of vectors of values, reinterpreted from component 2</param>
		/// <param name="t3">Span of vectors of values, reinterpreted from component 3</param>
		/// <param name="t4">Span of vectors of values, reinterpreted from component 4</param>
		/// <param name="t5">Span of vectors of values, reinterpreted from component 5</param>
		/// <param name="t6">Span of vectors of values, reinterpreted from component 6</param>
		/// <param name="t7">Span of vectors of values, reinterpreted from component 7</param>
		/// <param name="t8">Span of vectors of values, reinterpreted from component 8</param>
		/// <param name="t9">Span of vectors of values, reinterpreted from component 9</param>
		/// <param name="t10">Span of vectors of values, reinterpreted from component 10</param>
		/// <param name="t11">Span of vectors of values, reinterpreted from component 11</param>
		public void Execute(Span<Vector<TV0>> t0, Span<Vector<TV1>> t1, Span<Vector<TV2>> t2, Span<Vector<TV3>> t3, Span<Vector<TV4>> t4, Span<Vector<TV5>> t5, Span<Vector<TV6>> t6, Span<Vector<TV7>> t7, Span<Vector<TV8>> t8, Span<Vector<TV9>> t9, Span<Vector<TV10>> t10, Span<Vector<TV11>> t11, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T1">The type of the component 1</typeparam>
		/// <typeparam name="TV1">The type component 1 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T2">The type of the component 2</typeparam>
		/// <typeparam name="TV2">The type component 2 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T3">The type of the component 3</typeparam>
		/// <typeparam name="TV3">The type component 3 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T4">The type of the component 4</typeparam>
		/// <typeparam name="TV4">The type component 4 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T5">The type of the component 5</typeparam>
		/// <typeparam name="TV5">The type component 5 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T6">The type of the component 6</typeparam>
		/// <typeparam name="TV6">The type component 6 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T7">The type of the component 7</typeparam>
		/// <typeparam name="TV7">The type component 7 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T8">The type of the component 8</typeparam>
		/// <typeparam name="TV8">The type component 8 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T9">The type of the component 9</typeparam>
		/// <typeparam name="TV9">The type component 9 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T10">The type of the component 10</typeparam>
		/// <typeparam name="TV10">The type component 10 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T11">The type of the component 11</typeparam>
		/// <typeparam name="TV11">The type component 11 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		[ExcludeFromCodeCoverage]
		public int ExecuteVectorChunk<TQ, T0, TV0, T1, TV1, T2, TV2, T3, TV3, T4, TV4, T5, TV5, T6, TV6, T7, TV7, T8, TV8, T9, TV9, T10, TV10, T11, TV11>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where T3 : struct, IComponent
            where T4 : struct, IComponent
            where T5 : struct, IComponent
            where T6 : struct, IComponent
            where T7 : struct, IComponent
            where T8 : struct, IComponent
            where T9 : struct, IComponent
            where T10 : struct, IComponent
            where T11 : struct, IComponent
            where TV0 : unmanaged
            where TV1 : unmanaged
            where TV2 : unmanaged
            where TV3 : unmanaged
            where TV4 : unmanaged
            where TV5 : unmanaged
            where TV6 : unmanaged
            where TV7 : unmanaged
            where TV8 : unmanaged
            where TV9 : unmanaged
            where TV10 : unmanaged
            where TV11 : unmanaged
			where TQ : IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8, TV9, TV10, TV11>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;
		    var c1 = ComponentID<T1>.ID;
		    var c2 = ComponentID<T2>.ID;
		    var c3 = ComponentID<T3>.ID;
		    var c4 = ComponentID<T4>.ID;
		    var c5 = ComponentID<T5>.ID;
		    var c6 = ComponentID<T6>.ID;
		    var c7 = ComponentID<T7>.ID;
		    var c8 = ComponentID<T8>.ID;
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;
		    var c11 = ComponentID<T11>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];
			Span<TV1> lvs1 = stackalloc TV1[Vector<TV1>.Count];
			Span<TV2> lvs2 = stackalloc TV2[Vector<TV2>.Count];
			Span<TV3> lvs3 = stackalloc TV3[Vector<TV3>.Count];
			Span<TV4> lvs4 = stackalloc TV4[Vector<TV4>.Count];
			Span<TV5> lvs5 = stackalloc TV5[Vector<TV5>.Count];
			Span<TV6> lvs6 = stackalloc TV6[Vector<TV6>.Count];
			Span<TV7> lvs7 = stackalloc TV7[Vector<TV7>.Count];
			Span<TV8> lvs8 = stackalloc TV8[Vector<TV8>.Count];
			Span<TV9> lvs9 = stackalloc TV9[Vector<TV9>.Count];
			Span<TV10> lvs10 = stackalloc TV10[Vector<TV10>.Count];
			Span<TV11> lvs11 = stackalloc TV11[Vector<TV11>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					var tc1 = chunk.GetSpan<T1>(c1);
					var ts1 = MemoryMarshal.Cast<T1, TV1>(tc1);
					var tv1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(ts1);
					if (tv1.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc2 = chunk.GetSpan<T2>(c2);
					var ts2 = MemoryMarshal.Cast<T2, TV2>(tc2);
					var tv2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(ts2);
					if (tv2.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc3 = chunk.GetSpan<T3>(c3);
					var ts3 = MemoryMarshal.Cast<T3, TV3>(tc3);
					var tv3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(ts3);
					if (tv3.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc4 = chunk.GetSpan<T4>(c4);
					var ts4 = MemoryMarshal.Cast<T4, TV4>(tc4);
					var tv4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(ts4);
					if (tv4.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc5 = chunk.GetSpan<T5>(c5);
					var ts5 = MemoryMarshal.Cast<T5, TV5>(tc5);
					var tv5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(ts5);
					if (tv5.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc6 = chunk.GetSpan<T6>(c6);
					var ts6 = MemoryMarshal.Cast<T6, TV6>(tc6);
					var tv6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(ts6);
					if (tv6.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc7 = chunk.GetSpan<T7>(c7);
					var ts7 = MemoryMarshal.Cast<T7, TV7>(tc7);
					var tv7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(ts7);
					if (tv7.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc8 = chunk.GetSpan<T8>(c8);
					var ts8 = MemoryMarshal.Cast<T8, TV8>(tc8);
					var tv8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(ts8);
					if (tv8.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc9 = chunk.GetSpan<T9>(c9);
					var ts9 = MemoryMarshal.Cast<T9, TV9>(tc9);
					var tv9 = MemoryMarshal.Cast<TV9, Vector<TV9>>(ts9);
					if (tv9.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc10 = chunk.GetSpan<T10>(c10);
					var ts10 = MemoryMarshal.Cast<T10, TV10>(tc10);
					var tv10 = MemoryMarshal.Cast<TV10, Vector<TV10>>(ts10);
					if (tv10.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc11 = chunk.GetSpan<T11>(c11);
					var ts11 = MemoryMarshal.Cast<T11, TV11>(tc11);
					var tv11 = MemoryMarshal.Cast<TV11, Vector<TV11>>(ts11);
					if (tv11.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					// Execute the vectorised part
					q.Execute(tv0, tv1, tv2, tv3, tv4, tv5, tv6, tv7, tv8, tv9, tv10, tv11, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);
					lvs1.Clear();
					ts1[^leftover..].CopyTo(lvs1);
					var ls1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(lvs1);
					lvs2.Clear();
					ts2[^leftover..].CopyTo(lvs2);
					var ls2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(lvs2);
					lvs3.Clear();
					ts3[^leftover..].CopyTo(lvs3);
					var ls3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(lvs3);
					lvs4.Clear();
					ts4[^leftover..].CopyTo(lvs4);
					var ls4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(lvs4);
					lvs5.Clear();
					ts5[^leftover..].CopyTo(lvs5);
					var ls5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(lvs5);
					lvs6.Clear();
					ts6[^leftover..].CopyTo(lvs6);
					var ls6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(lvs6);
					lvs7.Clear();
					ts7[^leftover..].CopyTo(lvs7);
					var ls7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(lvs7);
					lvs8.Clear();
					ts8[^leftover..].CopyTo(lvs8);
					var ls8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(lvs8);
					lvs9.Clear();
					ts9[^leftover..].CopyTo(lvs9);
					var ls9 = MemoryMarshal.Cast<TV9, Vector<TV9>>(lvs9);
					lvs10.Clear();
					ts10[^leftover..].CopyTo(lvs10);
					var ls10 = MemoryMarshal.Cast<TV10, Vector<TV10>>(lvs10);
					lvs11.Clear();
					ts11[^leftover..].CopyTo(lvs11);
					var ls11 = MemoryMarshal.Cast<TV11, Vector<TV11>>(lvs11);

					// Execute the leftover at the end
					q.Execute(ls0, ls1, ls2, ls3, ls4, ls5, ls6, ls7, ls8, ls9, ls10, ls11, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
                        ts1[ts1.Length - leftover + i] = lvs1[i];
                        ts2[ts2.Length - leftover + i] = lvs2[i];
                        ts3[ts3.Length - leftover + i] = lvs3[i];
                        ts4[ts4.Length - leftover + i] = lvs4[i];
                        ts5[ts5.Length - leftover + i] = lvs5[i];
                        ts6[ts6.Length - leftover + i] = lvs6[i];
                        ts7[ts7.Length - leftover + i] = lvs7[i];
                        ts8[ts8.Length - leftover + i] = lvs8[i];
                        ts9[ts9.Length - leftover + i] = lvs9[i];
                        ts10[ts10.Length - leftover + i] = lvs10[i];
                        ts11[ts11.Length - leftover + i] = lvs11[i];
					}
				}
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8, TV9, TV10, TV11, TV12>
		where TV0 : unmanaged
        where TV1 : unmanaged
        where TV2 : unmanaged
        where TV3 : unmanaged
        where TV4 : unmanaged
        where TV5 : unmanaged
        where TV6 : unmanaged
        where TV7 : unmanaged
        where TV8 : unmanaged
        where TV9 : unmanaged
        where TV10 : unmanaged
        where TV11 : unmanaged
        where TV12 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		/// <param name="t1">Span of vectors of values, reinterpreted from component 1</param>
		/// <param name="t2">Span of vectors of values, reinterpreted from component 2</param>
		/// <param name="t3">Span of vectors of values, reinterpreted from component 3</param>
		/// <param name="t4">Span of vectors of values, reinterpreted from component 4</param>
		/// <param name="t5">Span of vectors of values, reinterpreted from component 5</param>
		/// <param name="t6">Span of vectors of values, reinterpreted from component 6</param>
		/// <param name="t7">Span of vectors of values, reinterpreted from component 7</param>
		/// <param name="t8">Span of vectors of values, reinterpreted from component 8</param>
		/// <param name="t9">Span of vectors of values, reinterpreted from component 9</param>
		/// <param name="t10">Span of vectors of values, reinterpreted from component 10</param>
		/// <param name="t11">Span of vectors of values, reinterpreted from component 11</param>
		/// <param name="t12">Span of vectors of values, reinterpreted from component 12</param>
		public void Execute(Span<Vector<TV0>> t0, Span<Vector<TV1>> t1, Span<Vector<TV2>> t2, Span<Vector<TV3>> t3, Span<Vector<TV4>> t4, Span<Vector<TV5>> t5, Span<Vector<TV6>> t6, Span<Vector<TV7>> t7, Span<Vector<TV8>> t8, Span<Vector<TV9>> t9, Span<Vector<TV10>> t10, Span<Vector<TV11>> t11, Span<Vector<TV12>> t12, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T1">The type of the component 1</typeparam>
		/// <typeparam name="TV1">The type component 1 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T2">The type of the component 2</typeparam>
		/// <typeparam name="TV2">The type component 2 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T3">The type of the component 3</typeparam>
		/// <typeparam name="TV3">The type component 3 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T4">The type of the component 4</typeparam>
		/// <typeparam name="TV4">The type component 4 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T5">The type of the component 5</typeparam>
		/// <typeparam name="TV5">The type component 5 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T6">The type of the component 6</typeparam>
		/// <typeparam name="TV6">The type component 6 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T7">The type of the component 7</typeparam>
		/// <typeparam name="TV7">The type component 7 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T8">The type of the component 8</typeparam>
		/// <typeparam name="TV8">The type component 8 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T9">The type of the component 9</typeparam>
		/// <typeparam name="TV9">The type component 9 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T10">The type of the component 10</typeparam>
		/// <typeparam name="TV10">The type component 10 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T11">The type of the component 11</typeparam>
		/// <typeparam name="TV11">The type component 11 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T12">The type of the component 12</typeparam>
		/// <typeparam name="TV12">The type component 12 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		[ExcludeFromCodeCoverage]
		public int ExecuteVectorChunk<TQ, T0, TV0, T1, TV1, T2, TV2, T3, TV3, T4, TV4, T5, TV5, T6, TV6, T7, TV7, T8, TV8, T9, TV9, T10, TV10, T11, TV11, T12, TV12>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where T3 : struct, IComponent
            where T4 : struct, IComponent
            where T5 : struct, IComponent
            where T6 : struct, IComponent
            where T7 : struct, IComponent
            where T8 : struct, IComponent
            where T9 : struct, IComponent
            where T10 : struct, IComponent
            where T11 : struct, IComponent
            where T12 : struct, IComponent
            where TV0 : unmanaged
            where TV1 : unmanaged
            where TV2 : unmanaged
            where TV3 : unmanaged
            where TV4 : unmanaged
            where TV5 : unmanaged
            where TV6 : unmanaged
            where TV7 : unmanaged
            where TV8 : unmanaged
            where TV9 : unmanaged
            where TV10 : unmanaged
            where TV11 : unmanaged
            where TV12 : unmanaged
			where TQ : IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8, TV9, TV10, TV11, TV12>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;
		    var c1 = ComponentID<T1>.ID;
		    var c2 = ComponentID<T2>.ID;
		    var c3 = ComponentID<T3>.ID;
		    var c4 = ComponentID<T4>.ID;
		    var c5 = ComponentID<T5>.ID;
		    var c6 = ComponentID<T6>.ID;
		    var c7 = ComponentID<T7>.ID;
		    var c8 = ComponentID<T8>.ID;
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;
		    var c11 = ComponentID<T11>.ID;
		    var c12 = ComponentID<T12>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];
			Span<TV1> lvs1 = stackalloc TV1[Vector<TV1>.Count];
			Span<TV2> lvs2 = stackalloc TV2[Vector<TV2>.Count];
			Span<TV3> lvs3 = stackalloc TV3[Vector<TV3>.Count];
			Span<TV4> lvs4 = stackalloc TV4[Vector<TV4>.Count];
			Span<TV5> lvs5 = stackalloc TV5[Vector<TV5>.Count];
			Span<TV6> lvs6 = stackalloc TV6[Vector<TV6>.Count];
			Span<TV7> lvs7 = stackalloc TV7[Vector<TV7>.Count];
			Span<TV8> lvs8 = stackalloc TV8[Vector<TV8>.Count];
			Span<TV9> lvs9 = stackalloc TV9[Vector<TV9>.Count];
			Span<TV10> lvs10 = stackalloc TV10[Vector<TV10>.Count];
			Span<TV11> lvs11 = stackalloc TV11[Vector<TV11>.Count];
			Span<TV12> lvs12 = stackalloc TV12[Vector<TV12>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					var tc1 = chunk.GetSpan<T1>(c1);
					var ts1 = MemoryMarshal.Cast<T1, TV1>(tc1);
					var tv1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(ts1);
					if (tv1.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc2 = chunk.GetSpan<T2>(c2);
					var ts2 = MemoryMarshal.Cast<T2, TV2>(tc2);
					var tv2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(ts2);
					if (tv2.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc3 = chunk.GetSpan<T3>(c3);
					var ts3 = MemoryMarshal.Cast<T3, TV3>(tc3);
					var tv3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(ts3);
					if (tv3.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc4 = chunk.GetSpan<T4>(c4);
					var ts4 = MemoryMarshal.Cast<T4, TV4>(tc4);
					var tv4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(ts4);
					if (tv4.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc5 = chunk.GetSpan<T5>(c5);
					var ts5 = MemoryMarshal.Cast<T5, TV5>(tc5);
					var tv5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(ts5);
					if (tv5.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc6 = chunk.GetSpan<T6>(c6);
					var ts6 = MemoryMarshal.Cast<T6, TV6>(tc6);
					var tv6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(ts6);
					if (tv6.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc7 = chunk.GetSpan<T7>(c7);
					var ts7 = MemoryMarshal.Cast<T7, TV7>(tc7);
					var tv7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(ts7);
					if (tv7.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc8 = chunk.GetSpan<T8>(c8);
					var ts8 = MemoryMarshal.Cast<T8, TV8>(tc8);
					var tv8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(ts8);
					if (tv8.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc9 = chunk.GetSpan<T9>(c9);
					var ts9 = MemoryMarshal.Cast<T9, TV9>(tc9);
					var tv9 = MemoryMarshal.Cast<TV9, Vector<TV9>>(ts9);
					if (tv9.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc10 = chunk.GetSpan<T10>(c10);
					var ts10 = MemoryMarshal.Cast<T10, TV10>(tc10);
					var tv10 = MemoryMarshal.Cast<TV10, Vector<TV10>>(ts10);
					if (tv10.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc11 = chunk.GetSpan<T11>(c11);
					var ts11 = MemoryMarshal.Cast<T11, TV11>(tc11);
					var tv11 = MemoryMarshal.Cast<TV11, Vector<TV11>>(ts11);
					if (tv11.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc12 = chunk.GetSpan<T12>(c12);
					var ts12 = MemoryMarshal.Cast<T12, TV12>(tc12);
					var tv12 = MemoryMarshal.Cast<TV12, Vector<TV12>>(ts12);
					if (tv12.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					// Execute the vectorised part
					q.Execute(tv0, tv1, tv2, tv3, tv4, tv5, tv6, tv7, tv8, tv9, tv10, tv11, tv12, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);
					lvs1.Clear();
					ts1[^leftover..].CopyTo(lvs1);
					var ls1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(lvs1);
					lvs2.Clear();
					ts2[^leftover..].CopyTo(lvs2);
					var ls2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(lvs2);
					lvs3.Clear();
					ts3[^leftover..].CopyTo(lvs3);
					var ls3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(lvs3);
					lvs4.Clear();
					ts4[^leftover..].CopyTo(lvs4);
					var ls4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(lvs4);
					lvs5.Clear();
					ts5[^leftover..].CopyTo(lvs5);
					var ls5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(lvs5);
					lvs6.Clear();
					ts6[^leftover..].CopyTo(lvs6);
					var ls6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(lvs6);
					lvs7.Clear();
					ts7[^leftover..].CopyTo(lvs7);
					var ls7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(lvs7);
					lvs8.Clear();
					ts8[^leftover..].CopyTo(lvs8);
					var ls8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(lvs8);
					lvs9.Clear();
					ts9[^leftover..].CopyTo(lvs9);
					var ls9 = MemoryMarshal.Cast<TV9, Vector<TV9>>(lvs9);
					lvs10.Clear();
					ts10[^leftover..].CopyTo(lvs10);
					var ls10 = MemoryMarshal.Cast<TV10, Vector<TV10>>(lvs10);
					lvs11.Clear();
					ts11[^leftover..].CopyTo(lvs11);
					var ls11 = MemoryMarshal.Cast<TV11, Vector<TV11>>(lvs11);
					lvs12.Clear();
					ts12[^leftover..].CopyTo(lvs12);
					var ls12 = MemoryMarshal.Cast<TV12, Vector<TV12>>(lvs12);

					// Execute the leftover at the end
					q.Execute(ls0, ls1, ls2, ls3, ls4, ls5, ls6, ls7, ls8, ls9, ls10, ls11, ls12, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
                        ts1[ts1.Length - leftover + i] = lvs1[i];
                        ts2[ts2.Length - leftover + i] = lvs2[i];
                        ts3[ts3.Length - leftover + i] = lvs3[i];
                        ts4[ts4.Length - leftover + i] = lvs4[i];
                        ts5[ts5.Length - leftover + i] = lvs5[i];
                        ts6[ts6.Length - leftover + i] = lvs6[i];
                        ts7[ts7.Length - leftover + i] = lvs7[i];
                        ts8[ts8.Length - leftover + i] = lvs8[i];
                        ts9[ts9.Length - leftover + i] = lvs9[i];
                        ts10[ts10.Length - leftover + i] = lvs10[i];
                        ts11[ts11.Length - leftover + i] = lvs11[i];
                        ts12[ts12.Length - leftover + i] = lvs12[i];
					}
				}
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8, TV9, TV10, TV11, TV12, TV13>
		where TV0 : unmanaged
        where TV1 : unmanaged
        where TV2 : unmanaged
        where TV3 : unmanaged
        where TV4 : unmanaged
        where TV5 : unmanaged
        where TV6 : unmanaged
        where TV7 : unmanaged
        where TV8 : unmanaged
        where TV9 : unmanaged
        where TV10 : unmanaged
        where TV11 : unmanaged
        where TV12 : unmanaged
        where TV13 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		/// <param name="t1">Span of vectors of values, reinterpreted from component 1</param>
		/// <param name="t2">Span of vectors of values, reinterpreted from component 2</param>
		/// <param name="t3">Span of vectors of values, reinterpreted from component 3</param>
		/// <param name="t4">Span of vectors of values, reinterpreted from component 4</param>
		/// <param name="t5">Span of vectors of values, reinterpreted from component 5</param>
		/// <param name="t6">Span of vectors of values, reinterpreted from component 6</param>
		/// <param name="t7">Span of vectors of values, reinterpreted from component 7</param>
		/// <param name="t8">Span of vectors of values, reinterpreted from component 8</param>
		/// <param name="t9">Span of vectors of values, reinterpreted from component 9</param>
		/// <param name="t10">Span of vectors of values, reinterpreted from component 10</param>
		/// <param name="t11">Span of vectors of values, reinterpreted from component 11</param>
		/// <param name="t12">Span of vectors of values, reinterpreted from component 12</param>
		/// <param name="t13">Span of vectors of values, reinterpreted from component 13</param>
		public void Execute(Span<Vector<TV0>> t0, Span<Vector<TV1>> t1, Span<Vector<TV2>> t2, Span<Vector<TV3>> t3, Span<Vector<TV4>> t4, Span<Vector<TV5>> t5, Span<Vector<TV6>> t6, Span<Vector<TV7>> t7, Span<Vector<TV8>> t8, Span<Vector<TV9>> t9, Span<Vector<TV10>> t10, Span<Vector<TV11>> t11, Span<Vector<TV12>> t12, Span<Vector<TV13>> t13, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T1">The type of the component 1</typeparam>
		/// <typeparam name="TV1">The type component 1 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T2">The type of the component 2</typeparam>
		/// <typeparam name="TV2">The type component 2 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T3">The type of the component 3</typeparam>
		/// <typeparam name="TV3">The type component 3 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T4">The type of the component 4</typeparam>
		/// <typeparam name="TV4">The type component 4 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T5">The type of the component 5</typeparam>
		/// <typeparam name="TV5">The type component 5 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T6">The type of the component 6</typeparam>
		/// <typeparam name="TV6">The type component 6 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T7">The type of the component 7</typeparam>
		/// <typeparam name="TV7">The type component 7 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T8">The type of the component 8</typeparam>
		/// <typeparam name="TV8">The type component 8 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T9">The type of the component 9</typeparam>
		/// <typeparam name="TV9">The type component 9 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T10">The type of the component 10</typeparam>
		/// <typeparam name="TV10">The type component 10 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T11">The type of the component 11</typeparam>
		/// <typeparam name="TV11">The type component 11 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T12">The type of the component 12</typeparam>
		/// <typeparam name="TV12">The type component 12 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T13">The type of the component 13</typeparam>
		/// <typeparam name="TV13">The type component 13 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		[ExcludeFromCodeCoverage]
		public int ExecuteVectorChunk<TQ, T0, TV0, T1, TV1, T2, TV2, T3, TV3, T4, TV4, T5, TV5, T6, TV6, T7, TV7, T8, TV8, T9, TV9, T10, TV10, T11, TV11, T12, TV12, T13, TV13>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where T3 : struct, IComponent
            where T4 : struct, IComponent
            where T5 : struct, IComponent
            where T6 : struct, IComponent
            where T7 : struct, IComponent
            where T8 : struct, IComponent
            where T9 : struct, IComponent
            where T10 : struct, IComponent
            where T11 : struct, IComponent
            where T12 : struct, IComponent
            where T13 : struct, IComponent
            where TV0 : unmanaged
            where TV1 : unmanaged
            where TV2 : unmanaged
            where TV3 : unmanaged
            where TV4 : unmanaged
            where TV5 : unmanaged
            where TV6 : unmanaged
            where TV7 : unmanaged
            where TV8 : unmanaged
            where TV9 : unmanaged
            where TV10 : unmanaged
            where TV11 : unmanaged
            where TV12 : unmanaged
            where TV13 : unmanaged
			where TQ : IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8, TV9, TV10, TV11, TV12, TV13>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;
		    var c1 = ComponentID<T1>.ID;
		    var c2 = ComponentID<T2>.ID;
		    var c3 = ComponentID<T3>.ID;
		    var c4 = ComponentID<T4>.ID;
		    var c5 = ComponentID<T5>.ID;
		    var c6 = ComponentID<T6>.ID;
		    var c7 = ComponentID<T7>.ID;
		    var c8 = ComponentID<T8>.ID;
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;
		    var c11 = ComponentID<T11>.ID;
		    var c12 = ComponentID<T12>.ID;
		    var c13 = ComponentID<T13>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];
			Span<TV1> lvs1 = stackalloc TV1[Vector<TV1>.Count];
			Span<TV2> lvs2 = stackalloc TV2[Vector<TV2>.Count];
			Span<TV3> lvs3 = stackalloc TV3[Vector<TV3>.Count];
			Span<TV4> lvs4 = stackalloc TV4[Vector<TV4>.Count];
			Span<TV5> lvs5 = stackalloc TV5[Vector<TV5>.Count];
			Span<TV6> lvs6 = stackalloc TV6[Vector<TV6>.Count];
			Span<TV7> lvs7 = stackalloc TV7[Vector<TV7>.Count];
			Span<TV8> lvs8 = stackalloc TV8[Vector<TV8>.Count];
			Span<TV9> lvs9 = stackalloc TV9[Vector<TV9>.Count];
			Span<TV10> lvs10 = stackalloc TV10[Vector<TV10>.Count];
			Span<TV11> lvs11 = stackalloc TV11[Vector<TV11>.Count];
			Span<TV12> lvs12 = stackalloc TV12[Vector<TV12>.Count];
			Span<TV13> lvs13 = stackalloc TV13[Vector<TV13>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					var tc1 = chunk.GetSpan<T1>(c1);
					var ts1 = MemoryMarshal.Cast<T1, TV1>(tc1);
					var tv1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(ts1);
					if (tv1.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc2 = chunk.GetSpan<T2>(c2);
					var ts2 = MemoryMarshal.Cast<T2, TV2>(tc2);
					var tv2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(ts2);
					if (tv2.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc3 = chunk.GetSpan<T3>(c3);
					var ts3 = MemoryMarshal.Cast<T3, TV3>(tc3);
					var tv3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(ts3);
					if (tv3.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc4 = chunk.GetSpan<T4>(c4);
					var ts4 = MemoryMarshal.Cast<T4, TV4>(tc4);
					var tv4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(ts4);
					if (tv4.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc5 = chunk.GetSpan<T5>(c5);
					var ts5 = MemoryMarshal.Cast<T5, TV5>(tc5);
					var tv5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(ts5);
					if (tv5.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc6 = chunk.GetSpan<T6>(c6);
					var ts6 = MemoryMarshal.Cast<T6, TV6>(tc6);
					var tv6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(ts6);
					if (tv6.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc7 = chunk.GetSpan<T7>(c7);
					var ts7 = MemoryMarshal.Cast<T7, TV7>(tc7);
					var tv7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(ts7);
					if (tv7.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc8 = chunk.GetSpan<T8>(c8);
					var ts8 = MemoryMarshal.Cast<T8, TV8>(tc8);
					var tv8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(ts8);
					if (tv8.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc9 = chunk.GetSpan<T9>(c9);
					var ts9 = MemoryMarshal.Cast<T9, TV9>(tc9);
					var tv9 = MemoryMarshal.Cast<TV9, Vector<TV9>>(ts9);
					if (tv9.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc10 = chunk.GetSpan<T10>(c10);
					var ts10 = MemoryMarshal.Cast<T10, TV10>(tc10);
					var tv10 = MemoryMarshal.Cast<TV10, Vector<TV10>>(ts10);
					if (tv10.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc11 = chunk.GetSpan<T11>(c11);
					var ts11 = MemoryMarshal.Cast<T11, TV11>(tc11);
					var tv11 = MemoryMarshal.Cast<TV11, Vector<TV11>>(ts11);
					if (tv11.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc12 = chunk.GetSpan<T12>(c12);
					var ts12 = MemoryMarshal.Cast<T12, TV12>(tc12);
					var tv12 = MemoryMarshal.Cast<TV12, Vector<TV12>>(ts12);
					if (tv12.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc13 = chunk.GetSpan<T13>(c13);
					var ts13 = MemoryMarshal.Cast<T13, TV13>(tc13);
					var tv13 = MemoryMarshal.Cast<TV13, Vector<TV13>>(ts13);
					if (tv13.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					// Execute the vectorised part
					q.Execute(tv0, tv1, tv2, tv3, tv4, tv5, tv6, tv7, tv8, tv9, tv10, tv11, tv12, tv13, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);
					lvs1.Clear();
					ts1[^leftover..].CopyTo(lvs1);
					var ls1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(lvs1);
					lvs2.Clear();
					ts2[^leftover..].CopyTo(lvs2);
					var ls2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(lvs2);
					lvs3.Clear();
					ts3[^leftover..].CopyTo(lvs3);
					var ls3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(lvs3);
					lvs4.Clear();
					ts4[^leftover..].CopyTo(lvs4);
					var ls4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(lvs4);
					lvs5.Clear();
					ts5[^leftover..].CopyTo(lvs5);
					var ls5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(lvs5);
					lvs6.Clear();
					ts6[^leftover..].CopyTo(lvs6);
					var ls6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(lvs6);
					lvs7.Clear();
					ts7[^leftover..].CopyTo(lvs7);
					var ls7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(lvs7);
					lvs8.Clear();
					ts8[^leftover..].CopyTo(lvs8);
					var ls8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(lvs8);
					lvs9.Clear();
					ts9[^leftover..].CopyTo(lvs9);
					var ls9 = MemoryMarshal.Cast<TV9, Vector<TV9>>(lvs9);
					lvs10.Clear();
					ts10[^leftover..].CopyTo(lvs10);
					var ls10 = MemoryMarshal.Cast<TV10, Vector<TV10>>(lvs10);
					lvs11.Clear();
					ts11[^leftover..].CopyTo(lvs11);
					var ls11 = MemoryMarshal.Cast<TV11, Vector<TV11>>(lvs11);
					lvs12.Clear();
					ts12[^leftover..].CopyTo(lvs12);
					var ls12 = MemoryMarshal.Cast<TV12, Vector<TV12>>(lvs12);
					lvs13.Clear();
					ts13[^leftover..].CopyTo(lvs13);
					var ls13 = MemoryMarshal.Cast<TV13, Vector<TV13>>(lvs13);

					// Execute the leftover at the end
					q.Execute(ls0, ls1, ls2, ls3, ls4, ls5, ls6, ls7, ls8, ls9, ls10, ls11, ls12, ls13, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
                        ts1[ts1.Length - leftover + i] = lvs1[i];
                        ts2[ts2.Length - leftover + i] = lvs2[i];
                        ts3[ts3.Length - leftover + i] = lvs3[i];
                        ts4[ts4.Length - leftover + i] = lvs4[i];
                        ts5[ts5.Length - leftover + i] = lvs5[i];
                        ts6[ts6.Length - leftover + i] = lvs6[i];
                        ts7[ts7.Length - leftover + i] = lvs7[i];
                        ts8[ts8.Length - leftover + i] = lvs8[i];
                        ts9[ts9.Length - leftover + i] = lvs9[i];
                        ts10[ts10.Length - leftover + i] = lvs10[i];
                        ts11[ts11.Length - leftover + i] = lvs11[i];
                        ts12[ts12.Length - leftover + i] = lvs12[i];
                        ts13[ts13.Length - leftover + i] = lvs13[i];
					}
				}
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8, TV9, TV10, TV11, TV12, TV13, TV14>
		where TV0 : unmanaged
        where TV1 : unmanaged
        where TV2 : unmanaged
        where TV3 : unmanaged
        where TV4 : unmanaged
        where TV5 : unmanaged
        where TV6 : unmanaged
        where TV7 : unmanaged
        where TV8 : unmanaged
        where TV9 : unmanaged
        where TV10 : unmanaged
        where TV11 : unmanaged
        where TV12 : unmanaged
        where TV13 : unmanaged
        where TV14 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		/// <param name="t1">Span of vectors of values, reinterpreted from component 1</param>
		/// <param name="t2">Span of vectors of values, reinterpreted from component 2</param>
		/// <param name="t3">Span of vectors of values, reinterpreted from component 3</param>
		/// <param name="t4">Span of vectors of values, reinterpreted from component 4</param>
		/// <param name="t5">Span of vectors of values, reinterpreted from component 5</param>
		/// <param name="t6">Span of vectors of values, reinterpreted from component 6</param>
		/// <param name="t7">Span of vectors of values, reinterpreted from component 7</param>
		/// <param name="t8">Span of vectors of values, reinterpreted from component 8</param>
		/// <param name="t9">Span of vectors of values, reinterpreted from component 9</param>
		/// <param name="t10">Span of vectors of values, reinterpreted from component 10</param>
		/// <param name="t11">Span of vectors of values, reinterpreted from component 11</param>
		/// <param name="t12">Span of vectors of values, reinterpreted from component 12</param>
		/// <param name="t13">Span of vectors of values, reinterpreted from component 13</param>
		/// <param name="t14">Span of vectors of values, reinterpreted from component 14</param>
		public void Execute(Span<Vector<TV0>> t0, Span<Vector<TV1>> t1, Span<Vector<TV2>> t2, Span<Vector<TV3>> t3, Span<Vector<TV4>> t4, Span<Vector<TV5>> t5, Span<Vector<TV6>> t6, Span<Vector<TV7>> t7, Span<Vector<TV8>> t8, Span<Vector<TV9>> t9, Span<Vector<TV10>> t10, Span<Vector<TV11>> t11, Span<Vector<TV12>> t12, Span<Vector<TV13>> t13, Span<Vector<TV14>> t14, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T1">The type of the component 1</typeparam>
		/// <typeparam name="TV1">The type component 1 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T2">The type of the component 2</typeparam>
		/// <typeparam name="TV2">The type component 2 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T3">The type of the component 3</typeparam>
		/// <typeparam name="TV3">The type component 3 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T4">The type of the component 4</typeparam>
		/// <typeparam name="TV4">The type component 4 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T5">The type of the component 5</typeparam>
		/// <typeparam name="TV5">The type component 5 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T6">The type of the component 6</typeparam>
		/// <typeparam name="TV6">The type component 6 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T7">The type of the component 7</typeparam>
		/// <typeparam name="TV7">The type component 7 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T8">The type of the component 8</typeparam>
		/// <typeparam name="TV8">The type component 8 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T9">The type of the component 9</typeparam>
		/// <typeparam name="TV9">The type component 9 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T10">The type of the component 10</typeparam>
		/// <typeparam name="TV10">The type component 10 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T11">The type of the component 11</typeparam>
		/// <typeparam name="TV11">The type component 11 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T12">The type of the component 12</typeparam>
		/// <typeparam name="TV12">The type component 12 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T13">The type of the component 13</typeparam>
		/// <typeparam name="TV13">The type component 13 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T14">The type of the component 14</typeparam>
		/// <typeparam name="TV14">The type component 14 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		[ExcludeFromCodeCoverage]
		public int ExecuteVectorChunk<TQ, T0, TV0, T1, TV1, T2, TV2, T3, TV3, T4, TV4, T5, TV5, T6, TV6, T7, TV7, T8, TV8, T9, TV9, T10, TV10, T11, TV11, T12, TV12, T13, TV13, T14, TV14>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where T3 : struct, IComponent
            where T4 : struct, IComponent
            where T5 : struct, IComponent
            where T6 : struct, IComponent
            where T7 : struct, IComponent
            where T8 : struct, IComponent
            where T9 : struct, IComponent
            where T10 : struct, IComponent
            where T11 : struct, IComponent
            where T12 : struct, IComponent
            where T13 : struct, IComponent
            where T14 : struct, IComponent
            where TV0 : unmanaged
            where TV1 : unmanaged
            where TV2 : unmanaged
            where TV3 : unmanaged
            where TV4 : unmanaged
            where TV5 : unmanaged
            where TV6 : unmanaged
            where TV7 : unmanaged
            where TV8 : unmanaged
            where TV9 : unmanaged
            where TV10 : unmanaged
            where TV11 : unmanaged
            where TV12 : unmanaged
            where TV13 : unmanaged
            where TV14 : unmanaged
			where TQ : IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8, TV9, TV10, TV11, TV12, TV13, TV14>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;
		    var c1 = ComponentID<T1>.ID;
		    var c2 = ComponentID<T2>.ID;
		    var c3 = ComponentID<T3>.ID;
		    var c4 = ComponentID<T4>.ID;
		    var c5 = ComponentID<T5>.ID;
		    var c6 = ComponentID<T6>.ID;
		    var c7 = ComponentID<T7>.ID;
		    var c8 = ComponentID<T8>.ID;
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;
		    var c11 = ComponentID<T11>.ID;
		    var c12 = ComponentID<T12>.ID;
		    var c13 = ComponentID<T13>.ID;
		    var c14 = ComponentID<T14>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];
			Span<TV1> lvs1 = stackalloc TV1[Vector<TV1>.Count];
			Span<TV2> lvs2 = stackalloc TV2[Vector<TV2>.Count];
			Span<TV3> lvs3 = stackalloc TV3[Vector<TV3>.Count];
			Span<TV4> lvs4 = stackalloc TV4[Vector<TV4>.Count];
			Span<TV5> lvs5 = stackalloc TV5[Vector<TV5>.Count];
			Span<TV6> lvs6 = stackalloc TV6[Vector<TV6>.Count];
			Span<TV7> lvs7 = stackalloc TV7[Vector<TV7>.Count];
			Span<TV8> lvs8 = stackalloc TV8[Vector<TV8>.Count];
			Span<TV9> lvs9 = stackalloc TV9[Vector<TV9>.Count];
			Span<TV10> lvs10 = stackalloc TV10[Vector<TV10>.Count];
			Span<TV11> lvs11 = stackalloc TV11[Vector<TV11>.Count];
			Span<TV12> lvs12 = stackalloc TV12[Vector<TV12>.Count];
			Span<TV13> lvs13 = stackalloc TV13[Vector<TV13>.Count];
			Span<TV14> lvs14 = stackalloc TV14[Vector<TV14>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					var tc1 = chunk.GetSpan<T1>(c1);
					var ts1 = MemoryMarshal.Cast<T1, TV1>(tc1);
					var tv1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(ts1);
					if (tv1.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc2 = chunk.GetSpan<T2>(c2);
					var ts2 = MemoryMarshal.Cast<T2, TV2>(tc2);
					var tv2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(ts2);
					if (tv2.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc3 = chunk.GetSpan<T3>(c3);
					var ts3 = MemoryMarshal.Cast<T3, TV3>(tc3);
					var tv3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(ts3);
					if (tv3.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc4 = chunk.GetSpan<T4>(c4);
					var ts4 = MemoryMarshal.Cast<T4, TV4>(tc4);
					var tv4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(ts4);
					if (tv4.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc5 = chunk.GetSpan<T5>(c5);
					var ts5 = MemoryMarshal.Cast<T5, TV5>(tc5);
					var tv5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(ts5);
					if (tv5.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc6 = chunk.GetSpan<T6>(c6);
					var ts6 = MemoryMarshal.Cast<T6, TV6>(tc6);
					var tv6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(ts6);
					if (tv6.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc7 = chunk.GetSpan<T7>(c7);
					var ts7 = MemoryMarshal.Cast<T7, TV7>(tc7);
					var tv7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(ts7);
					if (tv7.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc8 = chunk.GetSpan<T8>(c8);
					var ts8 = MemoryMarshal.Cast<T8, TV8>(tc8);
					var tv8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(ts8);
					if (tv8.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc9 = chunk.GetSpan<T9>(c9);
					var ts9 = MemoryMarshal.Cast<T9, TV9>(tc9);
					var tv9 = MemoryMarshal.Cast<TV9, Vector<TV9>>(ts9);
					if (tv9.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc10 = chunk.GetSpan<T10>(c10);
					var ts10 = MemoryMarshal.Cast<T10, TV10>(tc10);
					var tv10 = MemoryMarshal.Cast<TV10, Vector<TV10>>(ts10);
					if (tv10.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc11 = chunk.GetSpan<T11>(c11);
					var ts11 = MemoryMarshal.Cast<T11, TV11>(tc11);
					var tv11 = MemoryMarshal.Cast<TV11, Vector<TV11>>(ts11);
					if (tv11.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc12 = chunk.GetSpan<T12>(c12);
					var ts12 = MemoryMarshal.Cast<T12, TV12>(tc12);
					var tv12 = MemoryMarshal.Cast<TV12, Vector<TV12>>(ts12);
					if (tv12.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc13 = chunk.GetSpan<T13>(c13);
					var ts13 = MemoryMarshal.Cast<T13, TV13>(tc13);
					var tv13 = MemoryMarshal.Cast<TV13, Vector<TV13>>(ts13);
					if (tv13.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc14 = chunk.GetSpan<T14>(c14);
					var ts14 = MemoryMarshal.Cast<T14, TV14>(tc14);
					var tv14 = MemoryMarshal.Cast<TV14, Vector<TV14>>(ts14);
					if (tv14.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					// Execute the vectorised part
					q.Execute(tv0, tv1, tv2, tv3, tv4, tv5, tv6, tv7, tv8, tv9, tv10, tv11, tv12, tv13, tv14, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);
					lvs1.Clear();
					ts1[^leftover..].CopyTo(lvs1);
					var ls1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(lvs1);
					lvs2.Clear();
					ts2[^leftover..].CopyTo(lvs2);
					var ls2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(lvs2);
					lvs3.Clear();
					ts3[^leftover..].CopyTo(lvs3);
					var ls3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(lvs3);
					lvs4.Clear();
					ts4[^leftover..].CopyTo(lvs4);
					var ls4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(lvs4);
					lvs5.Clear();
					ts5[^leftover..].CopyTo(lvs5);
					var ls5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(lvs5);
					lvs6.Clear();
					ts6[^leftover..].CopyTo(lvs6);
					var ls6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(lvs6);
					lvs7.Clear();
					ts7[^leftover..].CopyTo(lvs7);
					var ls7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(lvs7);
					lvs8.Clear();
					ts8[^leftover..].CopyTo(lvs8);
					var ls8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(lvs8);
					lvs9.Clear();
					ts9[^leftover..].CopyTo(lvs9);
					var ls9 = MemoryMarshal.Cast<TV9, Vector<TV9>>(lvs9);
					lvs10.Clear();
					ts10[^leftover..].CopyTo(lvs10);
					var ls10 = MemoryMarshal.Cast<TV10, Vector<TV10>>(lvs10);
					lvs11.Clear();
					ts11[^leftover..].CopyTo(lvs11);
					var ls11 = MemoryMarshal.Cast<TV11, Vector<TV11>>(lvs11);
					lvs12.Clear();
					ts12[^leftover..].CopyTo(lvs12);
					var ls12 = MemoryMarshal.Cast<TV12, Vector<TV12>>(lvs12);
					lvs13.Clear();
					ts13[^leftover..].CopyTo(lvs13);
					var ls13 = MemoryMarshal.Cast<TV13, Vector<TV13>>(lvs13);
					lvs14.Clear();
					ts14[^leftover..].CopyTo(lvs14);
					var ls14 = MemoryMarshal.Cast<TV14, Vector<TV14>>(lvs14);

					// Execute the leftover at the end
					q.Execute(ls0, ls1, ls2, ls3, ls4, ls5, ls6, ls7, ls8, ls9, ls10, ls11, ls12, ls13, ls14, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
                        ts1[ts1.Length - leftover + i] = lvs1[i];
                        ts2[ts2.Length - leftover + i] = lvs2[i];
                        ts3[ts3.Length - leftover + i] = lvs3[i];
                        ts4[ts4.Length - leftover + i] = lvs4[i];
                        ts5[ts5.Length - leftover + i] = lvs5[i];
                        ts6[ts6.Length - leftover + i] = lvs6[i];
                        ts7[ts7.Length - leftover + i] = lvs7[i];
                        ts8[ts8.Length - leftover + i] = lvs8[i];
                        ts9[ts9.Length - leftover + i] = lvs9[i];
                        ts10[ts10.Length - leftover + i] = lvs10[i];
                        ts11[ts11.Length - leftover + i] = lvs11[i];
                        ts12[ts12.Length - leftover + i] = lvs12[i];
                        ts13[ts13.Length - leftover + i] = lvs13[i];
                        ts14[ts14.Length - leftover + i] = lvs14[i];
					}
				}
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Execute over a vector of values, taken from components. Mapping from components to vectorised values
	/// depends on the exact alignment of the types selected to the component and the vector. For example if
	/// a component is a Vector3 and the vector is float then the values in the vector will be the individual
	/// vector elements in sequence.
	/// </summary>
	public interface IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8, TV9, TV10, TV11, TV12, TV13, TV14, TV15>
		where TV0 : unmanaged
        where TV1 : unmanaged
        where TV2 : unmanaged
        where TV3 : unmanaged
        where TV4 : unmanaged
        where TV5 : unmanaged
        where TV6 : unmanaged
        where TV7 : unmanaged
        where TV8 : unmanaged
        where TV9 : unmanaged
        where TV10 : unmanaged
        where TV11 : unmanaged
        where TV12 : unmanaged
        where TV13 : unmanaged
        where TV14 : unmanaged
        where TV15 : unmanaged
	{
		/// <summary>
        /// Execute work over SIMD vectors
        /// </summary>
		/// <param name="offset">How many far through a component does the first vector start. For example
		/// if the component is a vector3 then a value of 2 would indicate that Z is the first element.</param>
		/// <param name="padding">How many items at the end of the last vector in the span are padding items</param>
		/// <param name="t0">Span of vectors of values, reinterpreted from component 0</param>
		/// <param name="t1">Span of vectors of values, reinterpreted from component 1</param>
		/// <param name="t2">Span of vectors of values, reinterpreted from component 2</param>
		/// <param name="t3">Span of vectors of values, reinterpreted from component 3</param>
		/// <param name="t4">Span of vectors of values, reinterpreted from component 4</param>
		/// <param name="t5">Span of vectors of values, reinterpreted from component 5</param>
		/// <param name="t6">Span of vectors of values, reinterpreted from component 6</param>
		/// <param name="t7">Span of vectors of values, reinterpreted from component 7</param>
		/// <param name="t8">Span of vectors of values, reinterpreted from component 8</param>
		/// <param name="t9">Span of vectors of values, reinterpreted from component 9</param>
		/// <param name="t10">Span of vectors of values, reinterpreted from component 10</param>
		/// <param name="t11">Span of vectors of values, reinterpreted from component 11</param>
		/// <param name="t12">Span of vectors of values, reinterpreted from component 12</param>
		/// <param name="t13">Span of vectors of values, reinterpreted from component 13</param>
		/// <param name="t14">Span of vectors of values, reinterpreted from component 14</param>
		/// <param name="t15">Span of vectors of values, reinterpreted from component 15</param>
		public void Execute(Span<Vector<TV0>> t0, Span<Vector<TV1>> t1, Span<Vector<TV2>> t2, Span<Vector<TV3>> t3, Span<Vector<TV4>> t4, Span<Vector<TV5>> t5, Span<Vector<TV6>> t6, Span<Vector<TV7>> t7, Span<Vector<TV8>> t8, Span<Vector<TV9>> t9, Span<Vector<TV10>> t10, Span<Vector<TV11>> t11, Span<Vector<TV12>> t12, Span<Vector<TV13>> t13, Span<Vector<TV14>> t14, Span<Vector<TV15>> t15, int offset, int padding);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">The type of the component 0</typeparam>
		/// <typeparam name="TV0">The type component 0 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T1">The type of the component 1</typeparam>
		/// <typeparam name="TV1">The type component 1 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T2">The type of the component 2</typeparam>
		/// <typeparam name="TV2">The type component 2 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T3">The type of the component 3</typeparam>
		/// <typeparam name="TV3">The type component 3 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T4">The type of the component 4</typeparam>
		/// <typeparam name="TV4">The type component 4 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T5">The type of the component 5</typeparam>
		/// <typeparam name="TV5">The type component 5 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T6">The type of the component 6</typeparam>
		/// <typeparam name="TV6">The type component 6 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T7">The type of the component 7</typeparam>
		/// <typeparam name="TV7">The type component 7 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T8">The type of the component 8</typeparam>
		/// <typeparam name="TV8">The type component 8 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T9">The type of the component 9</typeparam>
		/// <typeparam name="TV9">The type component 9 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T10">The type of the component 10</typeparam>
		/// <typeparam name="TV10">The type component 10 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T11">The type of the component 11</typeparam>
		/// <typeparam name="TV11">The type component 11 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T12">The type of the component 12</typeparam>
		/// <typeparam name="TV12">The type component 12 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T13">The type of the component 13</typeparam>
		/// <typeparam name="TV13">The type component 13 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T14">The type of the component 14</typeparam>
		/// <typeparam name="TV14">The type component 14 will be cast to in the SIMD vectors</typeparam>
        /// <typeparam name="T15">The type of the component 15</typeparam>
		/// <typeparam name="TV15">The type component 15 will be cast to in the SIMD vectors</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		[ExcludeFromCodeCoverage]
		public int ExecuteVectorChunk<TQ, T0, TV0, T1, TV1, T2, TV2, T3, TV3, T4, TV4, T5, TV5, T6, TV6, T7, TV7, T8, TV8, T9, TV9, T10, TV10, T11, TV11, T12, TV12, T13, TV13, T14, TV14, T15, TV15>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : struct, IComponent
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where T3 : struct, IComponent
            where T4 : struct, IComponent
            where T5 : struct, IComponent
            where T6 : struct, IComponent
            where T7 : struct, IComponent
            where T8 : struct, IComponent
            where T9 : struct, IComponent
            where T10 : struct, IComponent
            where T11 : struct, IComponent
            where T12 : struct, IComponent
            where T13 : struct, IComponent
            where T14 : struct, IComponent
            where T15 : struct, IComponent
            where TV0 : unmanaged
            where TV1 : unmanaged
            where TV2 : unmanaged
            where TV3 : unmanaged
            where TV4 : unmanaged
            where TV5 : unmanaged
            where TV6 : unmanaged
            where TV7 : unmanaged
            where TV8 : unmanaged
            where TV9 : unmanaged
            where TV10 : unmanaged
            where TV11 : unmanaged
            where TV12 : unmanaged
            where TV13 : unmanaged
            where TV14 : unmanaged
            where TV15 : unmanaged
			where TQ : IVectorChunkQuery<TV0, TV1, TV2, TV3, TV4, TV5, TV6, TV7, TV8, TV9, TV10, TV11, TV12, TV13, TV14, TV15>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

		    var c0 = ComponentID<T0>.ID;
		    var c1 = ComponentID<T1>.ID;
		    var c2 = ComponentID<T2>.ID;
		    var c3 = ComponentID<T3>.ID;
		    var c4 = ComponentID<T4>.ID;
		    var c5 = ComponentID<T5>.ID;
		    var c6 = ComponentID<T6>.ID;
		    var c7 = ComponentID<T7>.ID;
		    var c8 = ComponentID<T8>.ID;
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;
		    var c11 = ComponentID<T11>.ID;
		    var c12 = ComponentID<T12>.ID;
		    var c13 = ComponentID<T13>.ID;
		    var c14 = ComponentID<T14>.ID;
		    var c15 = ComponentID<T15>.ID;

			// Allocate some spans we need to copy the "leftover" values
			Span<TV0> lvs0 = stackalloc TV0[Vector<TV0>.Count];
			Span<TV1> lvs1 = stackalloc TV1[Vector<TV1>.Count];
			Span<TV2> lvs2 = stackalloc TV2[Vector<TV2>.Count];
			Span<TV3> lvs3 = stackalloc TV3[Vector<TV3>.Count];
			Span<TV4> lvs4 = stackalloc TV4[Vector<TV4>.Count];
			Span<TV5> lvs5 = stackalloc TV5[Vector<TV5>.Count];
			Span<TV6> lvs6 = stackalloc TV6[Vector<TV6>.Count];
			Span<TV7> lvs7 = stackalloc TV7[Vector<TV7>.Count];
			Span<TV8> lvs8 = stackalloc TV8[Vector<TV8>.Count];
			Span<TV9> lvs9 = stackalloc TV9[Vector<TV9>.Count];
			Span<TV10> lvs10 = stackalloc TV10[Vector<TV10>.Count];
			Span<TV11> lvs11 = stackalloc TV11[Vector<TV11>.Count];
			Span<TV12> lvs12 = stackalloc TV12[Vector<TV12>.Count];
			Span<TV13> lvs13 = stackalloc TV13[Vector<TV13>.Count];
			Span<TV14> lvs14 = stackalloc TV14[Vector<TV14>.Count];
			Span<TV15> lvs15 = stackalloc TV15[Vector<TV15>.Count];

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				archetype.Block();

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					// Get spans:
					// - tc: component span
					// - ts: span of components, reinterpreted as TV
					// - tv: span of reinterpreted values, as Vector<TV>

					var tc0 = chunk.GetSpan<T0>(c0);
					var ts0 = MemoryMarshal.Cast<T0, TV0>(tc0);
					var tv0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(ts0);

					var tc1 = chunk.GetSpan<T1>(c1);
					var ts1 = MemoryMarshal.Cast<T1, TV1>(tc1);
					var tv1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(ts1);
					if (tv1.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc2 = chunk.GetSpan<T2>(c2);
					var ts2 = MemoryMarshal.Cast<T2, TV2>(tc2);
					var tv2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(ts2);
					if (tv2.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc3 = chunk.GetSpan<T3>(c3);
					var ts3 = MemoryMarshal.Cast<T3, TV3>(tc3);
					var tv3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(ts3);
					if (tv3.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc4 = chunk.GetSpan<T4>(c4);
					var ts4 = MemoryMarshal.Cast<T4, TV4>(tc4);
					var tv4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(ts4);
					if (tv4.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc5 = chunk.GetSpan<T5>(c5);
					var ts5 = MemoryMarshal.Cast<T5, TV5>(tc5);
					var tv5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(ts5);
					if (tv5.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc6 = chunk.GetSpan<T6>(c6);
					var ts6 = MemoryMarshal.Cast<T6, TV6>(tc6);
					var tv6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(ts6);
					if (tv6.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc7 = chunk.GetSpan<T7>(c7);
					var ts7 = MemoryMarshal.Cast<T7, TV7>(tc7);
					var tv7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(ts7);
					if (tv7.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc8 = chunk.GetSpan<T8>(c8);
					var ts8 = MemoryMarshal.Cast<T8, TV8>(tc8);
					var tv8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(ts8);
					if (tv8.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc9 = chunk.GetSpan<T9>(c9);
					var ts9 = MemoryMarshal.Cast<T9, TV9>(tc9);
					var tv9 = MemoryMarshal.Cast<TV9, Vector<TV9>>(ts9);
					if (tv9.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc10 = chunk.GetSpan<T10>(c10);
					var ts10 = MemoryMarshal.Cast<T10, TV10>(tc10);
					var tv10 = MemoryMarshal.Cast<TV10, Vector<TV10>>(ts10);
					if (tv10.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc11 = chunk.GetSpan<T11>(c11);
					var ts11 = MemoryMarshal.Cast<T11, TV11>(tc11);
					var tv11 = MemoryMarshal.Cast<TV11, Vector<TV11>>(ts11);
					if (tv11.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc12 = chunk.GetSpan<T12>(c12);
					var ts12 = MemoryMarshal.Cast<T12, TV12>(tc12);
					var tv12 = MemoryMarshal.Cast<TV12, Vector<TV12>>(ts12);
					if (tv12.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc13 = chunk.GetSpan<T13>(c13);
					var ts13 = MemoryMarshal.Cast<T13, TV13>(tc13);
					var tv13 = MemoryMarshal.Cast<TV13, Vector<TV13>>(ts13);
					if (tv13.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc14 = chunk.GetSpan<T14>(c14);
					var ts14 = MemoryMarshal.Cast<T14, TV14>(tc14);
					var tv14 = MemoryMarshal.Cast<TV14, Vector<TV14>>(ts14);
					if (tv14.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					var tc15 = chunk.GetSpan<T15>(c15);
					var ts15 = MemoryMarshal.Cast<T15, TV15>(tc15);
					var tv15 = MemoryMarshal.Cast<TV15, Vector<TV15>>(ts15);
					if (tv15.Length != tv0.Length)
						throw new InvalidOperationException("Mismatched vector lengths");

					// Execute the vectorised part
					q.Execute(tv0, tv1, tv2, tv3, tv4, tv5, tv6, tv7, tv8, tv9, tv10, tv11, tv12, tv13, tv14, tv15, 0, 0);

					// Copy data into a vector to handle the "leftover" at the end
					// which can't fill a whole vector
					var vectored = Vector<TV0>.Count * tv0.Length;
					var leftover = ts0.Length - vectored;
					var itemsPerComponent = ts0.Length / tc0.Length;
					var offset = vectored % itemsPerComponent;

					if (leftover == 0)
						continue;

					lvs0.Clear();
					ts0[^leftover..].CopyTo(lvs0);
					var ls0 = MemoryMarshal.Cast<TV0, Vector<TV0>>(lvs0);
					lvs1.Clear();
					ts1[^leftover..].CopyTo(lvs1);
					var ls1 = MemoryMarshal.Cast<TV1, Vector<TV1>>(lvs1);
					lvs2.Clear();
					ts2[^leftover..].CopyTo(lvs2);
					var ls2 = MemoryMarshal.Cast<TV2, Vector<TV2>>(lvs2);
					lvs3.Clear();
					ts3[^leftover..].CopyTo(lvs3);
					var ls3 = MemoryMarshal.Cast<TV3, Vector<TV3>>(lvs3);
					lvs4.Clear();
					ts4[^leftover..].CopyTo(lvs4);
					var ls4 = MemoryMarshal.Cast<TV4, Vector<TV4>>(lvs4);
					lvs5.Clear();
					ts5[^leftover..].CopyTo(lvs5);
					var ls5 = MemoryMarshal.Cast<TV5, Vector<TV5>>(lvs5);
					lvs6.Clear();
					ts6[^leftover..].CopyTo(lvs6);
					var ls6 = MemoryMarshal.Cast<TV6, Vector<TV6>>(lvs6);
					lvs7.Clear();
					ts7[^leftover..].CopyTo(lvs7);
					var ls7 = MemoryMarshal.Cast<TV7, Vector<TV7>>(lvs7);
					lvs8.Clear();
					ts8[^leftover..].CopyTo(lvs8);
					var ls8 = MemoryMarshal.Cast<TV8, Vector<TV8>>(lvs8);
					lvs9.Clear();
					ts9[^leftover..].CopyTo(lvs9);
					var ls9 = MemoryMarshal.Cast<TV9, Vector<TV9>>(lvs9);
					lvs10.Clear();
					ts10[^leftover..].CopyTo(lvs10);
					var ls10 = MemoryMarshal.Cast<TV10, Vector<TV10>>(lvs10);
					lvs11.Clear();
					ts11[^leftover..].CopyTo(lvs11);
					var ls11 = MemoryMarshal.Cast<TV11, Vector<TV11>>(lvs11);
					lvs12.Clear();
					ts12[^leftover..].CopyTo(lvs12);
					var ls12 = MemoryMarshal.Cast<TV12, Vector<TV12>>(lvs12);
					lvs13.Clear();
					ts13[^leftover..].CopyTo(lvs13);
					var ls13 = MemoryMarshal.Cast<TV13, Vector<TV13>>(lvs13);
					lvs14.Clear();
					ts14[^leftover..].CopyTo(lvs14);
					var ls14 = MemoryMarshal.Cast<TV14, Vector<TV14>>(lvs14);
					lvs15.Clear();
					ts15[^leftover..].CopyTo(lvs15);
					var ls15 = MemoryMarshal.Cast<TV15, Vector<TV15>>(lvs15);

					// Execute the leftover at the end
					q.Execute(ls0, ls1, ls2, ls3, ls4, ls5, ls6, ls7, ls8, ls9, ls10, ls11, ls12, ls13, ls14, ls15, offset, leftover);

					// Copy leftover back into place
                    for (var i = 0; i < leftover; i++)
					{
                        ts0[ts0.Length - leftover + i] = lvs0[i];
                        ts1[ts1.Length - leftover + i] = lvs1[i];
                        ts2[ts2.Length - leftover + i] = lvs2[i];
                        ts3[ts3.Length - leftover + i] = lvs3[i];
                        ts4[ts4.Length - leftover + i] = lvs4[i];
                        ts5[ts5.Length - leftover + i] = lvs5[i];
                        ts6[ts6.Length - leftover + i] = lvs6[i];
                        ts7[ts7.Length - leftover + i] = lvs7[i];
                        ts8[ts8.Length - leftover + i] = lvs8[i];
                        ts9[ts9.Length - leftover + i] = lvs9[i];
                        ts10[ts10.Length - leftover + i] = lvs10[i];
                        ts11[ts11.Length - leftover + i] = lvs11[i];
                        ts12[ts12.Length - leftover + i] = lvs12[i];
                        ts13[ts13.Length - leftover + i] = lvs13[i];
                        ts14[ts14.Length - leftover + i] = lvs14[i];
                        ts15[ts15.Length - leftover + i] = lvs15[i];
					}
				}
			}

			return count;
		}
	}
}



