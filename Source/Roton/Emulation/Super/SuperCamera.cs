using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperCamera(
    ITerminal terminal,
    ITiles tiles,
    IActorList actors,
    IBoard board,
    IBoardUpdater boardUpdater)
    : ICamera
{
    private const int WindowWidth = 24;
    private const int WindowHeight = 20;
    private const int WindowLeft = 14;
    private const int WindowTop = 2;
    private const int WindowRight = WindowLeft + WindowWidth - 1;
    private const int WindowBottom = WindowTop + WindowHeight - 1;

    private Location OldPlayerLocation { get; set; } = new(short.MinValue, short.MinValue);

    public bool UpdateCamera()
    {
        var upperLeft = new Location(WindowLeft, WindowTop);
        const int viewCenterX = WindowWidth / 2;
        const int viewCenterY = WindowHeight / 2;

        // Thresholds are the number of tiles that the camera will try to keep in view relative to the player.
        // The 8/6 mismatch on the Y axis is a bug in the Super engine itself. A perfectly centered camera
        // would use 7 for both top and bottom.

        const int scrollThresholdLeft = 9;
        const int scrollThresholdRight = 9;
        const int scrollThresholdTop = 8;
        const int scrollThresholdBottom = 6;

        // Max bounds of the camera (so that the scroll doesn't go off the right or bottom of the board.)

        var maxCameraX = tiles.Width - WindowWidth + 1;
        var maxCameraY = tiles.Height - WindowHeight + 1;

        var player = actors.Player.Location;
        var newCamera = new Location16(board.Camera.X, board.Camera.Y);
        var redrawRequired = false;

        var relativeX = player.X - newCamera.X;
        if (relativeX < scrollThresholdLeft && newCamera.X > 1)
        {
            if (player.X == OldPlayerLocation.X - 1)
            {
                newCamera.X--;
                board.Camera = newCamera;
                VideoScroll(upperLeft, WindowWidth, WindowHeight, Vector.East);
                for (var y = 0; y < WindowHeight; y++)
                    boardUpdater.UpdateBoard(new Location(newCamera.X, newCamera.Y + y));
            }
            else
            {
                newCamera.X = player.X - viewCenterX;

                if (newCamera.X < 1)
                    newCamera.X = 1;
                else if (newCamera.X > maxCameraX)
                    newCamera.X = maxCameraX;

                redrawRequired = true;
            }
        }
        else if (relativeX >= WindowWidth - scrollThresholdRight && newCamera.X < maxCameraX)
        {
            if (player.X == OldPlayerLocation.X + 1)
            {
                newCamera.X++;
                board.Camera = newCamera;
                VideoScroll(upperLeft, WindowWidth, WindowHeight, Vector.West);
                for (var y = 0; y < WindowHeight; y++)
                    boardUpdater.UpdateBoard(new Location(newCamera.X + WindowWidth - 1, newCamera.Y + y));
            }
            else
            {
                newCamera.X = player.X - viewCenterX;

                if (newCamera.X < 1)
                    newCamera.X = 1;
                else if (newCamera.X > maxCameraX)
                    newCamera.X = maxCameraX;

                redrawRequired = true;
            }
        }

        var relativeY = player.Y - newCamera.Y;
        if (relativeY < scrollThresholdTop && newCamera.Y > 1)
        {
            if (player.Y == OldPlayerLocation.Y - 1)
            {
                newCamera.Y--;
                board.Camera = newCamera;
                VideoScroll(upperLeft, WindowWidth, WindowHeight, Vector.South);
                for (var x = 0; x < WindowWidth; x++)
                    boardUpdater.UpdateBoard(new Location(newCamera.X + x, newCamera.Y));
            }
            else
            {
                newCamera.Y = player.Y - viewCenterY;

                if (newCamera.Y < 1)
                    newCamera.Y = 1;
                else if (newCamera.Y > maxCameraY)
                    newCamera.Y = maxCameraY;

                redrawRequired = true;
            }
        }
        else if (relativeY >= WindowHeight - scrollThresholdBottom && newCamera.Y < maxCameraY)
        {
            if (player.Y == OldPlayerLocation.Y + 1)
            {
                newCamera.Y++;
                board.Camera = newCamera;
                VideoScroll(upperLeft, WindowWidth, WindowHeight, Vector.North);
                for (var x = 0; x < WindowWidth; x++)
                    boardUpdater.UpdateBoard(new Location(newCamera.X + x, newCamera.Y + WindowHeight - 1));
            }
            else
            {
                newCamera.Y = player.Y - viewCenterY;

                if (newCamera.Y < 1)
                    newCamera.Y = 1;
                else if (newCamera.Y > maxCameraY)
                    newCamera.Y = maxCameraY;

                redrawRequired = true;
            }
        }

        OldPlayerLocation = player;
        if (newCamera == board.Camera && !redrawRequired)
            return false;

        board.Camera = newCamera;
        return redrawRequired;
    }

    private void VideoScroll(Location pos, int width, int height, Vector dir)
    {
        var buffer = new AnsiChar[width * height];
        var bufIdx = 0;

        var minX = pos.X;
        var minY = pos.Y;
        var maxX = pos.X + width;
        var maxY = pos.Y + height;

        // Copy source into memory.

        for (var iy = 0; iy < height; iy++)
        for (var ix = 0; ix < width; ix++)
            buffer[bufIdx++] = terminal.Read(ix + pos.X, iy + pos.Y);

        // Blit it back out where it goes.

        bufIdx = 0;
        var finalX = pos.X + dir.X;
        var finalY = pos.Y + dir.Y;
        for (var iy = 0; iy < height; iy++)
        for (var ix = 0; ix < width; ix++)
        {
            var data = buffer[bufIdx++];
            var px = ix + finalX;
            var py = iy + finalY;

            if (px >= minX && px < maxX && py >= minY && py < maxY)
                terminal.Plot(px, py, data);
        }
    }
}