using System;
using JetBrains.Annotations;

namespace Roton.Infrastructure;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
[MeansImplicitUse]
public sealed class ContextAttribute(Context context, string name, int id = -1) : Attribute
{
    public Context Context { get; } = context;
    public string Name { get; } = name;
    public int Id { get; } = id;

    public ContextAttribute(Context context) : this(context, string.Empty)
    {
    }

    public ContextAttribute(Context context, int id) : this(context, string.Empty, id)
    {
    }
}