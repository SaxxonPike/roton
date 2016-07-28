﻿using Roton.Core;

namespace Roton.Emulation.Mapping
{
    public class SearchContext : ISearchContext
    {
        public SearchContext(IEngine engine)
        {
            Engine = engine;
        }

        public IEngine Engine { get; }
        public int SearchIndex { get; set; }

        public int SearchOffset { get; set; }
        public string SearchTarget { get; set; }
    }
}