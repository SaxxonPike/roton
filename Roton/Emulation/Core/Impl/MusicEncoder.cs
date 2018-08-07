using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public class MusicEncoder : IMusicEncoder
    {
        public ISound Encode(string music)
        {
            var speed = 1;
            var octave = 3;
            var result = new List<int>();
            var isNote = false;
            var note = -1;

            foreach (var c in music.ToUpperInvariant())
            {
                if (!isNote)
                {
                    note = -1;
                }
                else
                {
                    switch (c)
                    {
                        case '!':
                            note--;
                            break;
                        case '#':
                            note++;
                            break;
                    }

                    isNote = false;
                    result.Add(note + (octave << 4));
                    result.Add(speed);
                }

                switch (c)
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
                        speed = speed / 3;
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
                        result.Add(0);
                        result.Add(speed);
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
                        result.Add(0xF0 | (c - 0x30));
                        result.Add(speed);
                        break;
                }
            }

            if (isNote)
            {
                result.Add(note + (octave << 4));
                result.Add(speed);
            }

            return new Sound(result.Take(254).ToArray());
        }
    }
}