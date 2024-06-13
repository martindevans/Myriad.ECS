// ReSharper disable All
#pragma warning disable IDE0060 // Remove unused parameter
namespace System.Diagnostics.CodeAnalysis;

#if !NET6_0_OR_GREATER

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
internal class MemberNotNullAttribute
    : Attribute
{
    public MemberNotNullAttribute(string name)
    {
    }

    public MemberNotNullAttribute(params string[] name)
    {
    }
}

#endif