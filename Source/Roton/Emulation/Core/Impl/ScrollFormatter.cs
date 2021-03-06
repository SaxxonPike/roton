using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class ScrollFormatter : IScrollFormatter
    {
        private readonly Lazy<IScroll> _scroll;

        public ScrollFormatter(Lazy<IScroll> scroll)
        {
            _scroll = scroll;
        }

        private IScroll Scroll
        {
            [DebuggerStepThrough] get => _scroll.Value;
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
                    if (word.Length + 1 > Scroll.TextWidth)
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