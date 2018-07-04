﻿using System.Collections.Generic;

namespace Roton.Core
{
    public interface IFlags : IEnumerable<string>
    {
        string this[int index] { get; set; }
        void Add(string item);
        void Clear();
        bool Contains(string item);
        bool Remove(string item);
    }
}