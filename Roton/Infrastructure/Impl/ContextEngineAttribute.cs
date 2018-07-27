using System;
using Roton.Emulation.Data.Impl;

namespace Roton.Infrastructure.Impl
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ContextAttribute : Attribute
    {
        public Context Context { get; }
        public string Name { get; }
        public int Id { get; }

        public ContextAttribute(Context context)
        {
            Context = context;
            Name = string.Empty;
            Id = -1;
        }
        
        public ContextAttribute(Context context, string name)
        {
            Context = context;
            Name = name;
            Id = -1;
        }
        
        public ContextAttribute(Context context, int id)
        {
            Context = context;
            Name = string.Empty;
            Id = id;
        }
        
        public ContextAttribute(Context context, string name, int id)
        {
            Context = context;
            Name = name;
            Id = id;
        }
    }
}