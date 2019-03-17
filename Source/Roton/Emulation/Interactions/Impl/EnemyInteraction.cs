using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl
{
    [Context(Context.Original, 0x0F)]
    [Context(Context.Original, 0x12)]
    [Context(Context.Original, 0x22)]
    [Context(Context.Original, 0x23)]
    [Context(Context.Original, 0x29)]
    [Context(Context.Original, 0x2A)]
    [Context(Context.Original, 0x2C)]
    [Context(Context.Original, 0x2D)]
    [Context(Context.Super, 0x22)]
    [Context(Context.Super, 0x23)]
    [Context(Context.Super, 0x29)]
    [Context(Context.Super, 0x2A)]
    [Context(Context.Super, 0x2C)]
    [Context(Context.Super, 0x2D)]
    [Context(Context.Super, 0x3B)]
    [Context(Context.Super, 0x3C)]
    [Context(Context.Super, 0x3D)]
    [Context(Context.Super, 0x3E)]
    [Context(Context.Super, 0x45)]
    [Context(Context.Super, 0x48)]
    public sealed class EnemyInteraction : IInteraction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public EnemyInteraction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            Engine.Attack(index, location);
        }
    }
}