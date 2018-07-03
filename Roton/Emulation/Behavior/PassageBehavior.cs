using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class PassageBehavior : ElementBehavior
    {
        private readonly bool _enableUnderTileBug;

        public PassageBehavior(bool enableUnderTileBug)
        {
            _enableUnderTileBug = enableUnderTileBug;
        }

        public override string KnownName => "Passage";

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            var searchColor = engine.Tiles[location].Color;
            var passageIndex = engine.ActorIndexAt(location);
            var passageTarget = engine.Actors[passageIndex].P3;
            engine.SetBoard(passageTarget);
            var target = new Location();

            for (var x = 1; x <= engine.Tiles.Width; x++)
            {
                for (var y = 1; y <= engine.Tiles.Height; y++)
                {
                    if (engine.TileAt(x, y).Id == engine.Elements.PassageId)
                    {
                        if (engine.TileAt(x, y).Color == searchColor)
                        {
                            target.SetTo(x, y);
                        }
                    }
                }
            }

            if (_enableUnderTileBug)
            {
                // this is what causes the black holes when using passages
                engine.Tiles[engine.Player.Location].SetTo(engine.Elements.EmptyId, 0);
            }
            else
            {
                // Passage holes were fixed in Super ZZT
                engine.Tiles[engine.Player.Location].CopyFrom(engine.Player.UnderTile);
            }

            if (target.X != 0)
            {
                engine.Player.Location.CopyFrom(target);
            }
            engine.State.GamePaused = true;
            engine.PlaySound(4, engine.SoundSet.Passage);
            engine.FadePurple();
            engine.EnterBoard();
            vector.SetTo(0, 0);
        }
    }
}