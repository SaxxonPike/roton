﻿using System;
using Roton.Emulation.Core;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Data.Impl
{
    public sealed class RandomState : IRandomState
    {
        public RandomState()
        {
            var time = DateTimeOffset.Now;
            var seed = (time.Second << 24) |
                       ((time.Millisecond / 10) << 16) |
                       (time.Hour << 8) |
                       time.Minute;
            Seed = seed;
            State = seed;
        }

        public RandomState(int seed)
        {
            Seed = seed;
            State = seed;
        }

        public int Seed { get; }
        public int State { get; set; }
    }
}