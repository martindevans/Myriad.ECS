using Myriad.ECS.Components;

namespace Myriad.ECS.Tests;

public record struct ComponentByte(byte Value) : IComponent;
public record struct ComponentInt16(short Value) : IComponent;
public record struct ComponentFloat(float Value) : IComponent;
public record struct ComponentInt32(int Value) : IComponent;
public record struct ComponentInt64(long Value) : IComponent;
public record struct ComponentObject(object Value) : IComponent;

public record struct Component0 : IComponent;
public record struct Component1 : IComponent;
public record struct Component2 : IComponent;
public record struct Component3 : IComponent;
public record struct Component4 : IComponent;
public record struct Component5 : IComponent;
public record struct Component6 : IComponent;
public record struct Component7 : IComponent;
public record struct Component8 : IComponent;
public record struct Component9 : IComponent;
public record struct Component10 : IComponent;
public record struct Component11 : IComponent;
public record struct Component12 : IComponent;
public record struct Component13 : IComponent;
public record struct Component14 : IComponent;
public record struct Component15 : IComponent;
public record struct Component16 : IComponent;
public record struct Component17 : IComponent;

public record struct TestPhantom0 : IPhantomComponent;
public record struct TestPhantom1 : IPhantomComponent;
public record struct TestPhantom2 : IPhantomComponent;

public class BoxedInt
{
    public int Value;
}

public record struct TestDisposable
    : IDisposableComponent
{
    private readonly BoxedInt _box;

    public TestDisposable(BoxedInt box)
    {
        _box = box;
    }

    public void Dispose()
    {
        _box.Value++;
    }
}