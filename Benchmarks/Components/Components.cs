using Myriad.ECS;

namespace Benchmarks.Components;

public partial record struct ComponentByte(byte Value) : IComponent;
public partial record struct ComponentInt16(short Value) : IComponent;
public partial record struct ComponentFloat(float Value) : IComponent;
public partial record struct ComponentInt32(int Value) : IComponent;
public partial record struct ComponentInt64(long Value) : IComponent;