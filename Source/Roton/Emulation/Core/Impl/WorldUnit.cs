using System.IO;
using System.Linq;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Startup)]
public class WorldUnit(
    IHud hud,
    IWorld world,
    IState state,
    IBoardList boardList,
    //IFeatures features,
    IGameSerializer gameSerializer,
    ITiles tiles,
    IFileSystem fileSystem,
    IScrollFormatter scrollFormatter,
    IFileDialog fileDialog,
    IBoard board,
    IConfig config,
    IAlerts alerts,
    IFacts facts,
    IElementList elementList,
    IActorList actorList)
    : IWorldUnit
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

            boardList.Clear();

            foreach (var rawBoard in newBoards)
                boardList.Add(rawBoard);
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
        var numBoards = (short)(boardList.Count - 1);

        writer.Write(type);
        writer.Write(numBoards);

        // Write world data.

        gameSerializer.SaveWorld(stream);

        // Write each packed board.

        foreach (var item in boardList)
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
        boardList.Clear();

        if (config.NoPesterMode)
            alerts.SetAll();
        else
            alerts.Reset();

        ClearBoard();
        boardList.Add(new PackedBoard(gameSerializer.PackBoard(tiles)));
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
        var name = ShowLoad(facts.WorldFileWindowTitle, facts.WorldFileExtension);
        if (string.IsNullOrEmpty(name))
            return;

        LoadWorld(name!, false);
        state.StartBoard = world.BoardIndex;
        SetBoard(0);

        var element = elementList[state.PlayerElement];
        tiles[actorList.Player.Location] = new Tile(element.Id, element.Color);

        hud.FadeBoard(facts.FadeTile);
        hud.RedrawBoard();
    }

    public bool RestoreWorld()
    {
        var name = ShowLoad(facts.SavedGameWindowTitle, facts.SavedGameExtension);
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
        var board = new PackedBoard(gameSerializer.PackBoard(tiles));
        PackBoard(world.BoardIndex, board);
    }

    private void PackBoard(int boardIndex, IPackedBoard board)
    {
        // bit of a hack to make sure we don't go out of bounds
        while (boardList.Count <= boardIndex)
            boardList.Add(new PackedBoard([]));

        state.BoardCount = boardList.Count - 1;
        boardList[world.BoardIndex] = board;
    }

    public void UnpackBoard(int boardIndex)
    {
        gameSerializer.UnpackBoard(tiles, boardList[boardIndex].Data);
        world.BoardIndex = boardIndex;
    }

    public void SetBoard(int boardIndex)
    {
        var element = elementList.Player();
        tiles[actorList.Player.Location] = new Tile(element.Id, element.Color);
        PackBoard();
        UnpackBoard(boardIndex);
    }

    public void ClearBoard()
    {
        var emptyId = elementList.EmptyId;
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
        board.Exits.East = 0;
        board.Exits.North = 0;
        board.Exits.South = 0;
        board.Exits.West = 0;

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
        var element = elementList.Player();
        state.ActorCount = 0;
        actorList.Player.Location = new Location(tiles.Width / 2, tiles.Height / 2);
        tiles[actorList.Player.Location] = new Tile(element.Id, element.Color);
        actorList.Player.Cycle = 1;
        actorList.Player.UnderTile = new Tile(0, 0);
        actorList.Player.Pointer = 0;
        actorList.Player.Length = 0;
    }

    public string? ShowLoad(string title, string extension)
    {
        return fileDialog.Open(title, extension);
    }

    private void ShowDosError()
    {
        hud.ShowScroll(false, "Error",
            [
                "$DOS Error:",
                string.Empty,
                "This may be caused by missing",
                "files or a bad disk. If you",
                "are trying to save a game,",
                "your disk may be full -- try",
                "using a blank, formatted disk",
                "for saving the game!"
            ]
        );
    }
    
    private void ShowFormattedScroll(string error) =>
        hud.ShowScroll(false, "Roton Error", scrollFormatter.Format(error));

}