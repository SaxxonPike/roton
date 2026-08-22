using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements;

public class BoardEdgeTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void BoardEdge_ShouldDoNothing_WhenNoNeighborExists()
    {
        // A board edge acts like a regular wall when attempting to enter it toward a
        // direction where no neighbor board is assigned.

        // Place the player.
        MovePlayerTo(10, 10);

        // Place the edge tile.
        PlotTo(11, 10, Elements.BoardEdgeId);

        // Move the player into the edge.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(10, 10).Id.Should().Be(Elements.PlayerId,
            "player should not have moved");
        BoardIndex.Should().Be(0,
            "board should not have changed");
    }

    [Theory]
    public void BoardEdge_ShouldChangeBoards_WhenNeighborExists_AndTargetIsNotBlocked(BoardEdgeDir dir)
    {
        // When entering a board edge toward a direction that has a neighbor board,
        // the player should be moved to a matching edge. For instance, when entering
        // an edge tile from the south, the destination will be on the north
        // neighbor, at the south edge. Vertical movement maintains X-position, and
        // horizontal movement maintains Y-position.

        var (dX, dY, tX, tY, key) = dir switch
        {
            BoardEdgeDir.North => (0, -1, 10, Tiles.Height, AnsiKey.Up),
            BoardEdgeDir.South => (0, 1, 10, 1, AnsiKey.Down),
            BoardEdgeDir.West => (-1, 0, Tiles.Width, 10, AnsiKey.Left),
            BoardEdgeDir.East => (1, 0, 1, 10, AnsiKey.Right),
            _ => default
        };

        // Set up board 1.
        GoToBoard(1);
        PlotTo(tX, tY, Elements.EmptyId);

        // Set up board 0.
        GoToBoard(0);
        PlotTo(10 + dX, 10 + dY, Elements.BoardEdgeId);
        Board.Exits[(int)dir] = 1;

        // Place the player.
        MovePlayerTo(10, 10);

        // Move the player into the edge.
        Type(key);
        StepAllKeys();

        // Assert.
        BoardIndex.Should().Be(1,
            "board should have changed");
        TileAt(tX, tY).Id.Should().Be(Elements.PlayerId,
            "player should have entered at the correct coordinate");
    }

    [Test]
    public void BoardEdge_ShouldNotChangeBoards_WhenNeighborExists_AndTargetIsBlocked()
    {
        // Before a board is entered via an edge tile, the destination tile
        // is checked to ensure that it can be entered. If not, the movement is prevented.

        // Set up board 1.
        GoToBoard(1);
        PlotTo(1, 10, Elements.SolidId);

        // Set up board 0.
        GoToBoard(0);
        PlotTo(11, 10, Elements.BoardEdgeId);
        Board.Exits.East = 1;

        // Place the player.
        MovePlayerTo(10, 10);

        // Move the player into the edge.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        BoardIndex.Should().Be(0,
            "board should not have changed");
        TileAt(10, 10).Id.Should().Be(Elements.PlayerId,
            "player should have been blocked from moving");
    }

    [Test]
    public void BoardEdge_ShouldCausePlayerInteraction_WhileChangingBoards()
    {
        // If a player is entering a board where the destination tile is not empty, the
        // player will interact normally with it. The board will properly be changed
        // if the player is not blocked by the destination tile - for instance, while
        // picking up a power-up.

        // Set up board 1. Place a key at the edge for the player to interact with.
        GoToBoard(1);
        PlotTo(1, 10, Elements.KeyId, 9);

        // Set up board 0.
        GoToBoard(0);
        PlotTo(11, 10, Elements.BoardEdgeId);
        Board.Exits.East = 1;

        // Place the player.
        MovePlayerTo(10, 10);

        // Move the player into the edge.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        BoardIndex.Should().Be(1,
            "board should have changed");
        TileAt(1, 10).Id.Should().Be(Elements.PlayerId,
            "player should have entered the board on the left side");
        Keys[0].Should().BeTrue(
            "player should have picked up the key");
    }

    [Test]
    public void BoardEdge_ShouldCausePlayerInteraction_WhenBlocked()
    {
        // If a player is entering a board where the destination tile is blocked, the
        // player still interacts normally with the tile, but the board will not be changed.
        // This can trigger object interactions and scrolls.

        // Set up board 1. This places an invisible wall that changes the destination tile
        // but still blocks the player.
        GoToBoard(1);
        PlotTo(1, 10, Elements.InvisibleId);

        // Set up board 0.
        GoToBoard(0);
        PlotTo(11, 10, Elements.BoardEdgeId);
        Board.Exits.East = 1;

        // Place the player.
        MovePlayerTo(10, 10);

        // Move the player into the edge.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert board 0.
        BoardIndex.Should().Be(0,
            "board should not have changed");

        // Assert board 1.
        GoToBoard(1);
        TileAt(1, 10).Id.Should().Be(Elements.NormalId,
            "player should have interacted with the blocking target tile");
    }

    [Test]
    public void BoardEdge_ShouldProhibitPassageToBeTaken_WhenTargetTileIsNotWalkable()
    {
        // Chaining an edge tile into a passage with a blocked target tile on board 2
        // causes board 2 to be shown, but board 0 to be reentered. In the Super engine,
        // the player will remain on board 2 if there is no matching passage.

        // Set up board 1. This is the neighbor board that contains the passage to board 2.
        GoToBoard(1);
        var passageIndex = SpawnTo(1, 10, Elements.PassageId, 1);
        Actors[passageIndex].P3 = 2;

        // Set up board 2.
        GoToBoard(2);

        // Set the player "under tile" on the target board. This is
        // something to check due to the Super engine behavior.
        Player.UnderTile = new(Elements.FakeId, 1);

        // Set up board 0.
        GoToBoard(0);
        PlotTo(11, 10, Elements.BoardEdgeId);
        Board.Exits.East = 1;

        // Place the player.
        MovePlayerTo(10, 10);

        // Move the player into the edge.
        Type(AnsiKey.Right);
        StepAllKeys();

        if (Context == Context.Super)
        {
            BoardIndex.Should().Be(2,
                "board should have changed");
            TileAt(1, 10).Id.Should().Be(Elements.FakeId);
        }
        else
        {
            BoardIndex.Should().Be(0,
                "board should not have changed");
            TileAt(10, 10).Id.Should().Be(Elements.PlayerId);
        }
    }

    [Test]
    public void BoardEdge_ShouldProhibitPassageToBeTaken_WhenTargetTileIsNotWalkable_WithMatchingPassage()
    {
        // Chaining an edge tile into a passage with a blocked target tile on board 2
        // and also a matching destination passage causes board 2 to be shown, but board 0
        // to be reentered.

        // Set up board 1. This is the neighbor board that contains the passage to board 2.
        GoToBoard(1);
        var p1Index = SpawnTo(1, 10, Elements.PassageId, 1);
        Actors[p1Index].P3 = 2;

        // Set up board 2. The target tile here is blocked, and a matching passage is created.
        GoToBoard(2);
        var p2Index = SpawnTo(1, 10, Elements.PassageId, 1);
        Actors[p2Index].P3 = 2;

        // Set up board 0.
        GoToBoard(0);
        PlotTo(11, 10, Elements.BoardEdgeId);
        Board.Exits.East = 1;

        // Place the player.
        MovePlayerTo(10, 10);

        // Move the player into the edge.
        Type(AnsiKey.Right);
        StepAllKeys();

        BoardIndex.Should().Be(0,
            "board should not have changed");
        TileAt(10, 10).Id.Should().Be(Elements.PlayerId);
    }

    [Test]
    public void BoardEdge_ShouldAllowPassageToBeTaken_WhenTargetTileIsWalkable()
    {
        // Chaining an edge tile into a passage with a walkable target tile on board 2
        // will cause board 2 to be shown and entered.

        // Set up board 1. This is the neighbor board that contains the passage to board 2.
        GoToBoard(1);
        var passageIndex = SpawnTo(1, 10, Elements.PassageId, 1);
        Actors[passageIndex].P3 = 2;

        // Set up board 2. The target tile here is walkable.
        GoToBoard(2);
        PlotTo(1, 10, Elements.EmptyId);

        // Set up board 0.
        GoToBoard(0);
        PlotTo(11, 10, Elements.BoardEdgeId);
        Board.Exits.East = 1;

        // Place the player.
        MovePlayerTo(10, 10);

        // Move the player into the edge.
        Type(AnsiKey.Right);
        StepAllKeys();

        BoardIndex.Should().Be(2,
            "board should have changed");
    }
}