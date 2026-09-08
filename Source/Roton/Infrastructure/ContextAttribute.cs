using System;
using JetBrains.Annotations;

namespace Roton.Infrastructure;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
[MeansImplicitUse(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public sealed class ContextAttribute(
    Context context,
    string? name = null,
    int id = -1)
    : Attribute
{
    public Context Context { get; } = context;
    public string Name { get; } = name ?? string.Empty;
    public int Id { get; } = id;

    public ContextAttribute(Context context, int id)
        : this(context, string.Empty, id)
    {
    }
}