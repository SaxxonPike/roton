using System.IO;
using Roton.Infrastructure;

namespace Roton;

internal static class StreamExtensions
{
    public static void Read(this Stream stream, TempMemory<byte> temp)
    {
        var remaining = temp.Span.Length;
        var offset = 0;

        while (remaining > 0)
        {
            var amount = stream.Read(temp.Raw, offset, remaining);
            if (amount <= 0)
                throw new EndOfStreamException();

            offset += amount;
            remaining -= amount;
        }
    }

    public static void Write(this Stream stream, TempMemory<byte> temp) => 
        stream.Write(temp.Raw, 0, temp.Span.Length);
}