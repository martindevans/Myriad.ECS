// ReSharper disable All
namespace System.Runtime.CompilerServices;

#if !NET6_0_OR_GREATER

using Diagnostics;
using Diagnostics.CodeAnalysis;

/// <summary>
/// Reserved to be used by the compiler for tracking metadata.
/// This class should not be used by developers in source code.
/// </summary>
[ExcludeFromCodeCoverage, DebuggerNonUserCode]
internal static class IsExternalInit;

#endif