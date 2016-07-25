﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Emulation.Behavior
{
    internal abstract class EnemyBehavior : ElementBehavior
    {
        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            engine.Attack(index, location);
        }
    }
}