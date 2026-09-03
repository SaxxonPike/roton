using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Super)]
internal sealed class ArtSerializer(IMemory memory, ITerminal terminal) : IArtSerializer
{
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
                terminal.Plot(x, y, output);
                count--;
                x++;
                continue;
            }

            var data = memory.Read8(offset++);

            switch (data)
            {
                case >= 0x00 and <= 0x0F:
                {
                    output = new AnsiChar(output.Char, (output.Color & 0xF0) | data);
                    continue;
                }
                case >= 0x10 and <= 0x17:
                {
                    output = new AnsiChar(output.Char, (output.Color & 0x0F) | ((data & 0x0F) << 4));
                    continue;
                }
                case 0x18:
                {
                    x = 0;
                    y++;
                    break;
                }
                case 0x19:
                {
                    count = memory.Read8(offset++) + 1;
                    output = new AnsiChar(0x20, output.Color);
                    break;
                }
                case 0x1A:
                {
                    count = memory.Read8(offset++) + 1;
                    output = new AnsiChar(memory.Read8(offset++), output.Color);
                    break;
                }
                default:
                {
                    count = 1;
                    output = new AnsiChar(data, output.Color);
                    break;
                }
            }
        }
    }
}