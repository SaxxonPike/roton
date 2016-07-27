using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Emulation.Mapping
{
    public class SearchContext : ISearchContext
    {
        public SearchContext(IEngine engine)
        {
            Engine = engine;
        }

        public int SearchOffset { get; set; }
        public IEngine Engine { get; }
        public int SearchIndex { get; set; }
        public string SearchTarget { get; set; }
    }
}
