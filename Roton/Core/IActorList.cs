﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Core
{
    public interface IActorList : IList<IActor>
    {
        int Capacity { get; }
    }
}
