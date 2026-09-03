using System;
using JetBrains.Annotations;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class MusicEncoder : IMusicEncoder
{
    [MustDisposeResource]
    public TempMemory<byte> Encode(ReadOnlySpan<char> music)
    {
        var buffer = (stackalloc byte[512]);
        var speed = 1;
        var octave = 3;
        var isNote = false;
        var note = -1;
        var len = 0;

        foreach (var c in music)
        {
            if (len >= buffer.Length)
                break;

            var ch = c.ToUpperCase();

            if (!isNote)
            {
                note = -1;
            }
            else
            {
                switch (ch)
                {
                    case '!':
                        note--;
                        break;
                    case '#':
                        note++;
                        break;
                }

                isNote = false;
                buffer[len++] = unchecked((byte)(note + (octave << 4)));
                buffer[len++] = unchecked((byte)speed);
            }

            switch (ch)
            {
                case 'T':
                    speed = 1;
                    break;
                case 'S':
                    speed = 2;
                    break;
                case 'I':
                    speed = 4;
                    break;
                case 'Q':
                    speed = 8;
                    break;
                case 'H':
                    speed = 16;
                    break;
                case 'W':
                    speed = 32;
                    break;
                case '.':
                    speed = speed * 3 / 2;
                    break;
                case '3':
                    speed /= 3;
                    break;
                case '+':
                    if (octave < 6)
                        octave++;
                    break;
                case '-':
                    if (octave > 1)
                        octave--;
                    break;
                case 'C':
                    note = 0;
                    isNote = true;
                    break;
                case 'D':
                    note = 2;
                    isNote = true;
                    break;
                case 'E':
                    note = 4;
                    isNote = true;
                    break;
                case 'F':
                    note = 5;
                    isNote = true;
                    break;
                case 'G':
                    note = 7;
                    isNote = true;
                    break;
                case 'A':
                    note = 9;
                    isNote = true;
                    break;
                case 'B':
                    note = 11;
                    isNote = true;
                    break;
                case 'X':
                    buffer[len++] = 0;
                    buffer[len++] = unchecked((byte)speed);
                    break;
                case '0':
                case '1':
                case '2':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    buffer[len++] = unchecked((byte)(0xF0 | (ch - 0x30)));
                    buffer[len++] = unchecked((byte)speed);
                    break;
            }
        }

        if (isNote && len < buffer.Length)
        {
            buffer[len++] = unchecked((byte)(note + (octave << 4)));
            buffer[len++] = unchecked((byte)speed);
        }

        return new TempMemory<byte>(buffer.Slice(0, len));
    }
}