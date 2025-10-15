using System.Runtime.InteropServices;

namespace Myriad.ECS.Collections;

[StructLayout(LayoutKind.Explicit)]
internal struct Union64
{
    [FieldOffset(0)] public long Long;

    [FieldOffset(0)] public int I0;
    [FieldOffset(4)] public int I1;

    [FieldOffset(0)] public uint U0;
    [FieldOffset(4)] public uint U1;

    [FieldOffset(0)] public byte B0;
    [FieldOffset(1)] public byte B1;
    [FieldOffset(2)] public byte B2;
    [FieldOffset(3)] public byte B3;
    [FieldOffset(4)] public byte B4;
    [FieldOffset(5)] public byte B5;
    [FieldOffset(6)] public byte B6;
    [FieldOffset(7)] public byte B7;

    public override int GetHashCode()
    {
        // Because the data is all overlapped it doesn't really matter which fields are
        // used here, so long as **all** the ones from the **same** group are used.
        return Long.GetHashCode();
    }
}
