using System.IO;
using System.Linq;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class WorldManager(
    IHud hud,
    IWorld world,
    IState state,
    IBoardList boards,
    IGameSerializer gameSerializer,
    ITiles tiles,
    IFileSystem fileSystem,
    IScrollFormatter scrollFormatter,
    IFileDialog fileDialog,
    IBoard board,
    IConfig config,
    IAlerts alerts,
    IFacts facts,
    IElementList elements,
    IActorList actors,
    IFileTitles fileTitles,
    IExits exits,
    IScroll scroll,
    IScrollContent scrollContent)
    : IWorldManager
{
    private string GetFileName(string name, bool savedGame) =>
        savedGame
            ? $"{name}.{facts.SavedGameExtension}"
            : $"{name}.{facts.WorldFileExtension}";

    public bool LoadWorld(string name, bool savedGame)
    {
        var worldData = TryLoadWorld();

        if (worldData == null || worldData.Length == 0)
        {
            ShowDosError();
            return false;
        }

        using (var stream = new MemoryStream(worldData))
        {
            if (stream.Length == 0)
                return false;

            using var reader = new BinaryReader(stream);
            var type = reader.ReadInt16();
            if (type != world.WorldType)
            {
                hud.FailToLoadWorld();
                return false;
            }

            var numBoards = reader.ReadInt16();
            if (numBoards < 0)
                throw new RotonException("Board count must be zero or greater.");

            state.BoardCount = numBoards;
            gameSerializer.LoadWorld(stream);

            var newBoards = Enumerable
                .Range(0, numBoards + 1)
                .Select(_ => new PackedBoard(gameSerializer.LoadBoardData(stream)))
                .ToList();

            boards.Clear();

            foreach (var rawBoard in newBoards)
                boards.Add(rawBoard);
        }

        hud.CreateStatusWorld();
        UnpackBoard(world.BoardIndex);
        state.WorldLoaded = true;
        return true;

        byte[]? TryLoadWorld()
        {
            try
            {
                return fileSystem.GetFile(GetFileName(name, savedGame));
            }
            catch (IOException e)
            {
                ShowFormattedScroll(e.ToString());
                return [];
            }
        }
    }

    public void SaveWorld(string name)
    {
        // Make sure the packed board data is up to date.

        PackBoard();

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Write common world header.

        var type = (short)world.WorldType;
        var numBoards = (short)(boards.Count - 1);

        writer.Write(type);
        writer.Write(numBoards);

        // Write world data.

        gameSerializer.SaveWorld(stream);

        // Write each packed board.

        foreach (var item in boards)
            gameSerializer.SaveBoardData(stream, item.Data);

        stream.Flush();

        // Save to disk. Extension depends on whether the game world has been
        // modified in-game.

        var fileName = GetFileName(name, world.IsLocked);
        fileSystem.PutFile(fileName, stream.ToArray());
    }

    public void ClearWorld()
    {
        state.BoardCount = 0;
        boards.Clear();

        if (config.NoPesterMode)
            alerts.SetAll();
        else
            alerts.Reset();

        ClearBoard();
        boards.Add(new PackedBoard(gameSerializer.PackBoard(tiles)));
        world.BoardIndex = 0;
        world.Ammo = facts.DefaultAmmo;
        world.Gems = facts.DefaultGems;
        world.Health = facts.DefaultHealth;
        world.EnergyCycles = facts.DefaultEnergyCycles;
        world.Torches = facts.DefaultTorches;
        world.TorchCycles = facts.DefaultTorchCycles;
        world.Score = facts.DefaultScore;
        world.TimePassed = facts.DefaultTimePassed;
        world.Stones = facts.DefaultStones;
        world.Keys.Clear();
        world.Flags.Clear();
        SetBoard(0);
        board.Name = facts.DefaultBoardTitle;
        world.Name = facts.DefaultWorldTitle;
        state.WorldFileName = string.Empty;
    }

    public void OpenWorld()
    {
        var name = ShowLoad(facts.WorldFileWindowTitle, facts.WorldFileExtension, true);
        if (string.IsNullOrEmpty(name))
            return;

        LoadWorld(name!, false);
        state.StartBoard = world.BoardIndex;
        SetBoard(0);

        var element = elements[state.PlayerElement];
        tiles[actors.Player.Location] = new Tile(element.Id, element.Color);

        hud.FadeBoard(facts.FadeTile);
        hud.RedrawBoard();
    }

    public bool RestoreWorld()
    {
        var name = ShowLoad(facts.SavedGameWindowTitle, facts.SavedGameExtension, false);
        if (string.IsNullOrEmpty(name))
            return false;

        if (!LoadWorld(name!, true))
            return false;

        state.StartBoard = world.BoardIndex;
        world.IsLocked = false;
        SetBoard(state.StartBoard);
        return true;
    }

    public void PackBoard()
    {
        var packed = new PackedBoard(gameSerializer.PackBoard(tiles));
        PackBoard(world.BoardIndex, packed);
    }

    private void PackBoard(int boardIndex, IPackedBoard packed)
    {
        // bit of a hack to make sure we don't go out of bounds
        while (boards.Count <= boardIndex)
            boards.Add(new PackedBoard([]));

        state.BoardCount = boards.Count - 1;
        boards[world.BoardIndex] = packed;
    }

    public void UnpackBoard(int boardIndex)
    {
        gameSerializer.UnpackBoard(tiles, boards[boardIndex].Data);
        world.BoardIndex = boardIndex;
    }

    public void SetBoard(int boardIndex)
    {
        var element = elements.Player();
        tiles[actors.Player.Location] = new Tile(element.Id, element.Color);
        PackBoard();
        UnpackBoard(boardIndex);
    }

    public void ClearBoard()
    {
        var emptyId = elements.EmptyId;
        var boardEdgeId = state.EdgeTile.Id;
        var boardBorderId = state.BorderTile.Id;
        var boardBorderColor = state.BorderTile.Color;

        // board properties
        board.Name = string.Empty;
        state.Message = string.Empty;
        board.MaximumShots = facts.DefaultMaximumShots;
        board.IsDark = false;
        board.RestartOnZap = false;
        board.TimeLimit = 0;
        exits.East = 0;
        exits.North = 0;
        exits.South = 0;
        exits.West = 0;

        // build board edges
        for (var y = 0; y <= tiles.Height + 1; y++)
        {
            tiles[new Location(0, y)].Id = boardEdgeId;
            tiles[new Location(tiles.Width + 1, y)].Id = boardEdgeId;
        }

        for (var x = 0; x <= tiles.Width + 1; x++)
        {
            tiles[new Location(x, 0)].Id = boardEdgeId;
            tiles[new Location(x, tiles.Height + 1)].Id = boardEdgeId;
        }

        // clear out board
        for (var x = 1; x <= tiles.Width; x++)
        for (var y = 1; y <= tiles.Height; y++)
            tiles[new Location(x, y)] = new Tile(emptyId, 0);

        // build border
        for (var y = 1; y <= tiles.Height; y++)
        {
            tiles[new Location(1, y)] = new Tile(boardBorderId, boardBorderColor);
            tiles[new Location(tiles.Width, y)] = new Tile(boardBorderId, boardBorderColor);
        }

        for (var x = 1; x <= tiles.Width; x++)
        {
            tiles[new Location(x, 1)] = new Tile(boardBorderId, boardBorderColor);
            tiles[new Location(x, tiles.Height)] = new Tile(boardBorderId, boardBorderColor);
        }

        // generate player actor
        var element = elements.Player();
        state.ActorCount = 0;
        actors.Player.Location = new Location(tiles.Width / 2, tiles.Height / 2);
        tiles[actors.Player.Location] = new Tile(element.Id, element.Color);
        actors.Player.Cycle = 1;
        actors.Player.UnderTile = new Tile(0, 0);
        actors.Player.Pointer = 0;
        actors.Player.Length = 0;
    }

    private string? ShowLoad(string title, string extension, bool useTitles) =>
        fileDialog.Open(title, extension, useTitles ? fileTitles : null);

    private void ShowDosError()
    {
        scrollContent.AddLines(
            "$DOS Error:",
            string.Empty,
            "This may be caused by missing",
            "files or a bad disk. If you",
            "are trying to save a game,",
            "your disk may be full -- try",
            "using a blank, formatted disk",
            "for saving the game!"
        );

        scroll.ShowMessage("Error", false, 0);
    }

    private void ShowFormattedScroll(string error)
    {
        scrollContent.AddLines(scrollFormatter.Format(error));
        scroll.ShowMessage("Roton Error", false, 0);
    }
}