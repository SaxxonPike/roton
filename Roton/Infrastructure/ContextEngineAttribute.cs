using System;
using Roton.Emulation.Data.Impl;

namespace Roton.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ContextEngineAttribute : Attribute
    {
        public ContextEngine ContextEngine { get; }
        public string Name { get; }
        public int Id { get; }

        public ContextEngineAttribute(ContextEngine contextEngine)
        {
            ContextEngine = contextEngine;
            Name = string.Empty;
            Id = -1;
        }
        
        public ContextEngineAttribute(ContextEngine contextEngine, string name)
        {
            ContextEngine = contextEngine;
            Name = name;
            Id = -1;
        }
        
        public ContextEngineAttribute(ContextEngine contextEngine, int id)
        {
            ContextEngine = contextEngine;
            Name = string.Empty;
            Id = id;
        }
        
        public ContextEngineAttribute(ContextEngine contextEngine, string name, int id)
        {
            ContextEngine = contextEngine;
            Name = name;
            Id = id;
        }
    }
}