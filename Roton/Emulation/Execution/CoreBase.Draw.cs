using Roton.Core;

namespace Roton.Emulation.Execution
{
    internal abstract partial class CoreBase
    {
        public virtual AnsiChar Draw(IXyPair location)
        {
            if (Dark && !ElementAt(location).Shown && (TorchCycles <= 0 || Distance(Player.Location, location) >= 50) && !EditorMode)
            {
                return new AnsiChar(0xB0, 0x07);
            }

            var tile = Tiles[location];
            var element = Elements[tile.Id];
            var elementCount = Elements.Count;

            if (tile.Id == Elements.EmptyId)
            {
                return new AnsiChar(0x20, 0x0F);
            }
            if (element.DrawCodeEnable)
            {
                return element.Draw(location);
            }
            if (tile.Id < elementCount - 7)
            {
                return new AnsiChar(element.Character, tile.Color);
            }
            if (tile.Id != elementCount - 1)
            {
                return new AnsiChar(tile.Color, ((tile.Id - (elementCount - 8)) << 4) | 0x0F);
            }
            return new AnsiChar(tile.Color, 0x0F);
        }

        public virtual AnsiChar Draw_BlinkWall(IXyPair location)
        {
            return new AnsiChar(0xCE, Tiles[location].Color);
        }

        public virtual AnsiChar Draw_Bomb(IXyPair location)
        {
            var p1 = Actors[ActorIndexAt(location)].P1;
            return new AnsiChar(p1 > 1 ? 0x30 + p1 : 0x0B, Tiles[location].Color);
        }

        public virtual AnsiChar Draw_Clockwise(IXyPair location)
        {
            switch ((GameCycle/Elements[Elements.ClockwiseId].Cycle) & 0x3)
            {
                case 0:
                    return new AnsiChar(0xB3, Tiles[location].Color);
                case 1:
                    return new AnsiChar(0x2F, Tiles[location].Color);
                case 2:
                    return new AnsiChar(0xC4, Tiles[location].Color);
                default:
                    return new AnsiChar(0x5C, Tiles[location].Color);
            }
        }

        public virtual AnsiChar Draw_Counter(IXyPair location)
        {
            switch ((GameCycle/Elements[Elements.CounterId].Cycle) & 0x3)
            {
                case 3:
                    return new AnsiChar(0xB3, Tiles[location].Color);
                case 2:
                    return new AnsiChar(0x2F, Tiles[location].Color);
                case 1:
                    return new AnsiChar(0xC4, Tiles[location].Color);
                default:
                    return new AnsiChar(0x5C, Tiles[location].Color);
            }
        }

        public virtual AnsiChar Draw_DragonPup(IXyPair location)
        {
            switch (GameCycle & 0x3)
            {
                case 0:
                case 2:
                    return new AnsiChar(0x94, Tiles[location].Color);
                case 1:
                    return new AnsiChar(0xA2, Tiles[location].Color);
                default:
                    return new AnsiChar(0x95, Tiles[location].Color);
            }
        }

        public virtual AnsiChar Draw_Duplicator(IXyPair location)
        {
            switch (Actors[ActorIndexAt(location)].P1)
            {
                case 2:
                    return new AnsiChar(0xF9, Tiles[location].Color);
                case 3:
                    return new AnsiChar(0xF8, Tiles[location].Color);
                case 4:
                    return new AnsiChar(0x6F, Tiles[location].Color);
                case 5:
                    return new AnsiChar(0x4F, Tiles[location].Color);
                default:
                    return new AnsiChar(0xFA, Tiles[location].Color);
            }
        }

        public virtual AnsiChar Draw_Line(IXyPair location)
        {
            return new AnsiChar(LineChars[Adjacent(location, Elements.LineId)], Tiles[location].Color);
        }

        public virtual AnsiChar Draw_Object(IXyPair location)
        {
            return new AnsiChar(Actors[ActorIndexAt(location)].P1, Tiles[location].Color);
        }

        public virtual AnsiChar Draw_Pusher(IXyPair location)
        {
            var actor = Actors[ActorIndexAt(location)];
            switch (actor.Vector.X)
            {
                case 1:
                    return new AnsiChar(0x10, Tiles[location].Color);
                case -1:
                    return new AnsiChar(0x11, Tiles[location].Color);
                default:
                    return actor.Vector.Y == -1
                        ? new AnsiChar(0x1E, Tiles[location].Color)
                        : new AnsiChar(0x1F, Tiles[location].Color);
            }
        }

        public virtual AnsiChar Draw_SpinningGun(IXyPair location)
        {
            switch (GameCycle & 0x7)
            {
                case 0:
                case 1:
                    return new AnsiChar(0x18, Tiles[location].Color);
                case 2:
                case 3:
                    return new AnsiChar(0x1A, Tiles[location].Color);
                case 4:
                case 5:
                    return new AnsiChar(0x19, Tiles[location].Color);
                default:
                    return new AnsiChar(0x1B, Tiles[location].Color);
            }
        }

        public virtual AnsiChar Draw_Star(IXyPair location)
        {
            var tile = Tiles[location];
            tile.Color++;
            if (tile.Color > 15)
                tile.Color = 9;
            return new AnsiChar(StarChars[GameCycle & 0x3], tile.Color);
        }

        public virtual AnsiChar Draw_Stone(IXyPair location)
        {
            return new AnsiChar(0x41 + RandomNumber(0x1A), Tiles[location].Color);
        }

        public virtual AnsiChar Draw_Transporter(IXyPair location)
        {
            var actor = Actors[ActorIndexAt(location)];
            int index;

            if (actor.Vector.X == 0)
            {
                if (actor.Cycle > 0)
                    index = (GameCycle/actor.Cycle) & 0x3;
                else
                    index = 0;
                index += (actor.Vector.Y << 1) + 2;
                return new AnsiChar(TransporterVChars[index], Tiles[location].Color);
            }
            if (actor.Cycle > 0)
                index = (GameCycle/actor.Cycle) & 0x3;
            else
                index = 0;
            index += (actor.Vector.X << 1) + 2;
            return new AnsiChar(TransporterHChars[index], Tiles[location].Color);
        }

        public virtual AnsiChar Draw_Web(IXyPair location)
        {
            return new AnsiChar(WebChars[Adjacent(location, Elements.WebId)], Tiles[location].Color);
        }
    }
}