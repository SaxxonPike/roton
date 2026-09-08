using System;
using System.Collections.Generic;
using System.IO;

namespace Roton.Editors;

public enum BoardId;

public enum ActorId;

public interface IGameEditor
{
    IWorldEditor World { get; }

    void Create(Context context);
}

public interface IWorldEditor
{
    Guid Id { get; }
    int Ammo { get; set; }
    int Gems { get; set; }
    IKeyEditor Keys { get; }
    int Health { get; set; }
    int StartingBoard { get; set; }
    int Torches { get; set; }
    int EnergizerTicks { get; set; }
    int TorchTicks { get; set; }
    int Score { get; set; }
    string Name { get; set; }
    TimeSpan TimePassed { get; set; }
    bool IsSavedGame { get; set; }
    int Stones { get; set; }

    IBoardEditorList Boards { get; }

    void ImportRaw(Stream stream);
    void ImportJson(Stream stream);
    int ExportRaw(Stream stream);
    int ExportJson(Stream stream);
    Span<byte> GetInfoBytes();
    Span<byte> GetHeaderBytes();
}

public interface IFlagEditor : IList<string>;

public interface IKeyEditor : IReadOnlyList<bool>
{
    new bool this[int index] { get; set; }
    bool Blue { get; set; }
    bool Green { get; set; }
    bool Cyan { get; set; }
    bool Red { get; set; }
    bool Purple { get; set; }
    bool Yellow { get; set; }
    bool White { get; set; }
}

public interface IBoardEditorList : IList<IBoardEditor>;

public interface IBoardEditor
{
    Guid Id { get; }
    int MaxShots { get; set; }
    IBoardNeighbors Neighbors { get; }
    bool ReEnterWhenZapped { get; set; }
    Vector Enter { get; set; }
    Vector Camera { get; set; }
    TimeSpan TimeLimit { get; set; }
    IActorEditorList Actors { get; }

    Span<byte> InfoBytes { get; set; }
    Span<byte> TileBytes { get; set; }

    void ImportRaw(Stream stream);
    void ImportJson(Stream stream);
    int ExportRaw(Stream stream);
    int ExportJson(Stream stream);
    Span<byte> GetInfoBytes();
}

public interface IBoardNeighbors : IReadOnlyList<int>
{
}

public interface IActorEditorList
{
    IActorEditor this[int index] { get; }
}

public interface IActorEditor
{
    Guid Id { get; }
}