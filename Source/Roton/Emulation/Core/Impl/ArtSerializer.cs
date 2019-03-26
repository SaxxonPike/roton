using System;
using System.Diagnostics;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [Context(Context.Super)]
    public sealed class ArtSerializer : IArtSerializer
    {
        private readonly Lazy<IMemory> _memory;
        private readonly Lazy<ITerminal> _terminal;

        public ArtSerializer(Lazy<IMemory> memory, Lazy<ITerminal> terminal)
        {
            _memory = memory;
            _terminal = terminal;
        }

        private IMemory Memory
        {
            [DebuggerStepThroughAttribute] get => _memory.Value;
        }

        private ITerminal Terminal
        {
            [DebuggerStepThroughAttribute] get => _terminal.Value;
        }

        public void Deserialize(int startOffset)
        {
            var offset = startOffset;
            var x = 0;
            var y = 0;
            var count = 0;
            var output = new AnsiChar(0, 0);

            while (y < 25)
            {
                if (count > 0)
                {
                    Terminal.Plot(x, y, output);
                    count--;
                    x++;
                    continue;
                }

                var data = Memory.Read8(offset++);

                if (data >= 0x00 && data <= 0x0F)
                {
                    output = new AnsiChar(output.Char, (output.Color & 0xF0) | data);
                    continue;
                }

                if (data >= 0x10 && data <= 0x17)
                {
                    output = new AnsiChar(output.Char, (output.Color & 0x0F) | ((data & 0x0F) << 4));
                    continue;
                }

                switch (data)
                {
                    case 0x18:
                        x = 0;
                        y++;
                        break;
                    case 0x19:
                        count = Memory.Read8(offset++) + 1;
                        output = new AnsiChar(0x20, output.Color);
                        break;
                    case 0x1A:
                        count = Memory.Read8(offset++) + 1;
                        output = new AnsiChar(Memory.Read8(offset++), output.Color);
                        break;
                    default:
                        count = 1;
                        output = new AnsiChar(data, output.Color);
                        break;
                }
            }
        }
    }
}