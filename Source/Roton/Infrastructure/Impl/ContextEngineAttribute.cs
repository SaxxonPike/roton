using System;
using Roton.Emulation.Data.Impl;

namespace Roton.Infrastructure.Impl;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ContextAttribute(Context context, string name, int id) : Attribute
{
    public Context Context { get; } = context;
    public string Name { get; } = name;
    public int Id { get; } = id;

    public ContextAttribute(Context context) : this(context, string.Empty, -1)
    {
    }
        
    public ContextAttribute(Context context, string name) : this(context, name, -1)
    {
    }
        
    public ContextAttribute(Context context, int id) : this(context, string.Empty, id)
    {
    }
}