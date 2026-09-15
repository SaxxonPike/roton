using System.IO;
using System.Linq;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using static System.Buffers.Binary.BinaryPrimitives;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class WorldImporter(
    IState state,
    IGameSerializer gameSerializer,
    IBoardList boards)
    : IWorldImporter
{
    public bool ImportWorldData(int worldType, Stream stream)
    {
        var numBoardsBytes = new byte[2];
        if (stream.Read(numBoardsBytes, 0, 2) < 2)
            throw new EndOfStreamException();
        
        var numBoards = ReadInt16LittleEndian(numBoardsBytes);
        if (numBoards < 0)
            return false;
        //throw new RotonException("Board count must be zero or greater.");

        state.BoardCount = numBoards;
        gameSerializer.LoadWorldData(stream);

        var newBoards = Enumerable
            .Range(0, numBoards + 1)
            .Select(_ => new PackedBoard(gameSerializer.LoadBoardData(stream)))
            .ToList();

        boards.Clear();

        foreach (var rawBoard in newBoards)
            boards.Add(rawBoard);

        return true;
    }

    public bool ExportWorldData(Stream stream)
    {
        var numBoardsBytes = new byte[2];
        WriteInt16LittleEndian(numBoardsBytes, state.BoardCount);
        stream.Write(numBoardsBytes, 0, 2);
        
        gameSerializer.SaveWorldData(stream);

        foreach (var item in boards)
            gameSerializer.SaveBoardData(stream, item.Data);

        stream.Flush();
        return true;
    }
}