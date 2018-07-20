using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public class ScrollFormatter : IScrollFormatter
    {
        private readonly IScroll _scroll;

        public ScrollFormatter(IScroll scroll)
        {
            _scroll = scroll;
        }
        
        public string[] Format(string text)
        {
            var output = new List<string>();
            var lines = text
                .Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var outLine = new StringBuilder();
                foreach (var word in line.Split(' '))
                {
                    if (word.Length + 1 > _scroll.TextWidth)
                    {
                        output.Add(outLine.ToString());
                        outLine.Clear();
                    }

                    if (outLine.Length > 0)
                        outLine.Append(' ');
                    outLine.Append(word);
                }
                output.Add(outLine.ToString());
            }

            return output.ToArray();
        }
    }
}