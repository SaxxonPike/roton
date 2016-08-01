﻿using System;

namespace Roton.Emulation.Execution
{
    public class RandomState : IRandomState
    {
        public RandomState()
        {
            var time = DateTimeOffset.Now;
            var seed = (time.Second << 24) |
                       ((time.Millisecond/10) << 16) |
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