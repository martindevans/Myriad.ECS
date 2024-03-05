using Myriad.ECS.Command;

namespace Myriad.ECS.Components;

public static class Extensions
{
	public static CommandBuffer.BufferedEntity AddSharding(this CommandBuffer.BufferedEntity buffered, int index)
	{
		switch (index % 16)
		{
			case 0:
				return buffered.Set(new Shard0(), true);
			case 1:
				return buffered.Set(new Shard1(), true);
			case 2:
				return buffered.Set(new Shard2(), true);
			case 3:
				return buffered.Set(new Shard3(), true);
			case 4:
				return buffered.Set(new Shard4(), true);
			case 5:
				return buffered.Set(new Shard5(), true);
			case 6:
				return buffered.Set(new Shard6(), true);
			case 7:
				return buffered.Set(new Shard7(), true);
			case 8:
				return buffered.Set(new Shard8(), true);
			case 9:
				return buffered.Set(new Shard9(), true);
			case 10:
				return buffered.Set(new Shard10(), true);
			case 11:
				return buffered.Set(new Shard11(), true);
			case 12:
				return buffered.Set(new Shard12(), true);
			case 13:
				return buffered.Set(new Shard13(), true);
			case 14:
				return buffered.Set(new Shard14(), true);
			case 15:
				return buffered.Set(new Shard15(), true);

/* dotcover disable */
			default:
				throw new InvalidOperationException("Cannot add shard > 16");
/* dotcover enable */
		}
	}

	public static void AddSharding(this CommandBuffer buffer, Entity entity, int index)
	{
		switch (index % 16)
		{
			case 0:
				buffer.Set(entity, new Shard0());
				break;
			case 1:
				buffer.Set(entity, new Shard1());
				break;
			case 2:
				buffer.Set(entity, new Shard2());
				break;
			case 3:
				buffer.Set(entity, new Shard3());
				break;
			case 4:
				buffer.Set(entity, new Shard4());
				break;
			case 5:
				buffer.Set(entity, new Shard5());
				break;
			case 6:
				buffer.Set(entity, new Shard6());
				break;
			case 7:
				buffer.Set(entity, new Shard7());
				break;
			case 8:
				buffer.Set(entity, new Shard8());
				break;
			case 9:
				buffer.Set(entity, new Shard9());
				break;
			case 10:
				buffer.Set(entity, new Shard10());
				break;
			case 11:
				buffer.Set(entity, new Shard11());
				break;
			case 12:
				buffer.Set(entity, new Shard12());
				break;
			case 13:
				buffer.Set(entity, new Shard13());
				break;
			case 14:
				buffer.Set(entity, new Shard14());
				break;
			case 15:
				buffer.Set(entity, new Shard15());
				break;

/* dotcover disable */
			default:
				throw new InvalidOperationException("Cannot add shard > 16");
/* dotcover enable */
		}
	}

	public static void RemoveSharding(this CommandBuffer buffer, Entity entity)
	{
		buffer.Remove<Shard0>(entity);
		buffer.Remove<Shard1>(entity);
		buffer.Remove<Shard2>(entity);
		buffer.Remove<Shard3>(entity);
		buffer.Remove<Shard4>(entity);
		buffer.Remove<Shard5>(entity);
		buffer.Remove<Shard6>(entity);
		buffer.Remove<Shard7>(entity);
		buffer.Remove<Shard8>(entity);
		buffer.Remove<Shard9>(entity);
		buffer.Remove<Shard10>(entity);
		buffer.Remove<Shard11>(entity);
		buffer.Remove<Shard12>(entity);
		buffer.Remove<Shard13>(entity);
		buffer.Remove<Shard14>(entity);
		buffer.Remove<Shard15>(entity);
	}
}

internal record struct Shard0 : IComponent;
internal record struct Shard1 : IComponent;
internal record struct Shard2 : IComponent;
internal record struct Shard3 : IComponent;
internal record struct Shard4 : IComponent;
internal record struct Shard5 : IComponent;
internal record struct Shard6 : IComponent;
internal record struct Shard7 : IComponent;
internal record struct Shard8 : IComponent;
internal record struct Shard9 : IComponent;
internal record struct Shard10 : IComponent;
internal record struct Shard11 : IComponent;
internal record struct Shard12 : IComponent;
internal record struct Shard13 : IComponent;
internal record struct Shard14 : IComponent;
internal record struct Shard15 : IComponent;
