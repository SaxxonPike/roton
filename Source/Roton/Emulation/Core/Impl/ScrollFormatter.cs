using System;
using System.Collections.Generic;
using System.Diagnostics;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class ScrollFormatter(IScroll scroll) : IScrollFormatter
{
    private readonly char[] _newLineChars = Environment.NewLine.ToCharArray();

    private IScroll Scroll
    {
        [DebuggerStepThrough] get => scroll;
    }
        
    public string[] Format(string text)
    {
        var output = new List<string>();
        var lines = text
            .Split(_newLineChars, StringSplitOptions.RemoveEmptyEntries);

        var sb = StringBuilderPool.Rent();

        foreach (var line in lines)
        {
            sb.Clear();
            foreach (var word in line.Split(' '))
            {
                if (word.Length + 1 > Scroll.TextWidth)
                {
                    output.Add(sb.ToString());
                    sb.Clear();
                }

                if (sb.Length > 0)
                    sb.Append(' ');
                sb.Append(word);
            }

            output.Add(sb.ToString());
            sb.Clear();
        }

        StringBuilderPool.Return(sb);
        return [.. output];
    }
}