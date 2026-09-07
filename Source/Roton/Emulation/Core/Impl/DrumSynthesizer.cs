using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class DrumSynthesizer(
    IRandomizer randomizer)
    : IDrumSynthesizer
{
    public void Synthesize(int id, Span<Word> buffer)
    {
        int len;

        switch (id)
        {
            // "Tick"
            case 0:
            {
                len = 1;
                buffer[1] = 3200;
                break;
            }
            
            // "Tweet"
            case 1:
            {
                len = 14;
                for (var i = 1; i <= 14; i++)
                    buffer[i] = i * 100 + 1000;
                break;
            }
            
            // "Cowbell"
            case 2:
            {
                // There is a typo in the original code that fills past
                // the end of the sound data, which is why it's 16 instead of 14.

                len = 14;
                for (var i = 1; i <= 16; i++)
                    buffer[i] = (i & 1) * 1600 + 1600 + (i & 3) * 1600;
                break;
            }
            
            // "High Snare"
            case 4:
            {
                len = 14;
                for (var i = 1; i <= 14; i++)
                    buffer[i] = randomizer.GetNext(5000) + 500;
                break;
            }
            
            // "High woodblock"
            case 5:
            {
                // There is a typo in the original code that fills past
                // the end of the sound data, which is why it's 8 instead of 7.

                len = 14;
                for (var i = 1; i <= 8; i++)
                {
                    buffer[i * 2 - 1] = 1600;
                    buffer[i * 2] = randomizer.GetNext(1600) + 800;
                }

                break;
            }
            
            // "Low snare"
            case 6:
            {
                len = 14;
                for (var i = 1; i <= 14; i++)
                    buffer[i] = (i & 1) * 880 + 880 + i % 3 * 440;
                break;
            }
            
            // "Low tom"
            case 7:
            {
                len = 14;
                for (var i = 1; i <= 14; i++)
                    buffer[i] = 700 - i * 12;
                break;
            }
            
            // "Low woodblock"
            case 8:
            {
                len = 14;
                for (var i = 1; i <= 14; i++)
                    buffer[i] = i * 20 + 1200 - randomizer.GetNext(i * 40);
                break;
            }
            
            // "Bass drum"
            case 9:
            {
                len = 14;
                for (var i = 1; i <= 14; i++)
                    buffer[i] = randomizer.GetNext(440) + 220;
                break;
            }
            default:
            {
                // Buffer left intentionally uninitialized.
                // This drum cannot be played by normal means.

                len = 14;
                break;
            }
        }

        buffer[0] = len;
    }
}