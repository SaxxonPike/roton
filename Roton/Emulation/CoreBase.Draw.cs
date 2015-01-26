using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    abstract internal partial class CoreBase
    {
        virtual public AnsiChar Draw(Location location)
        {
            Tile tile = Tiles[location];
            Element element = Elements[tile.Id];
            int elementCount = Elements.Count;

            if (tile.Id == Elements.EmptyId)
            {
                return new AnsiChar(0x20, 0x0F);
            }
            else if (element.DrawCodeEnable)
            {
                return element.Draw(location);
            }
            else if (tile.Id < elementCount - 7)
            {
                return new AnsiChar(element.Character, tile.Color);
            }
            else
            {
                if (tile.Id != elementCount - 1)
                {
                    return new AnsiChar(tile.Color, ((tile.Id - (elementCount - 8)) << 4) | 0x0F);
                }
                else
                {
                    return new AnsiChar(tile.Color, 0x0F);
                }
            }
        }

        virtual public AnsiChar Draw_BlinkWall(Location location)
        {
            return new AnsiChar(0xCE, Tiles[location].Color);
        }

        virtual public AnsiChar Draw_Bomb(Location location)
        {
            int p1 = Actors[ActorIndexAt(location)].P1;
            return new AnsiChar((p1 > 1) ? (0x30 + p1) : 0x0B, Tiles[location].Color);
        }

        virtual public AnsiChar Draw_Clockwise(Location location)
        {
            switch ((GameCycle / Elements[Elements.ClockwiseId].Cycle) & 0x3)
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

        virtual public AnsiChar Draw_Counter(Location location)
        {
            switch ((GameCycle / Elements[Elements.CounterId].Cycle) & 0x3)
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

        virtual public AnsiChar Draw_DragonPup(Location location)
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

        virtual public AnsiChar Draw_Duplicator(Location location)
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

        virtual public AnsiChar Draw_Line(Location location)
        {
            return new AnsiChar(LineChars[Adjacent(location, Elements.LineId)], Tiles[location].Color);
        }

        virtual public AnsiChar Draw_Object(Location location)
        {
            return new AnsiChar(Actors[ActorIndexAt(location)].P1, Tiles[location].Color);
        }

        virtual public AnsiChar Draw_Pusher(Location location)
        {
            var actor = Actors[ActorIndexAt(location)];
            if (actor.Vector.X == 1)
                return new AnsiChar(0x10, Tiles[location].Color);
            else if (actor.Vector.X == -1)
                return new AnsiChar(0x11, Tiles[location].Color);
            else if (actor.Vector.Y == -1)
                return new AnsiChar(0x1E, Tiles[location].Color);
            else
                return new AnsiChar(0x1F, Tiles[location].Color);
        }

        virtual public AnsiChar Draw_SpinningGun(Location location)
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

        virtual public AnsiChar Draw_Star(Location location)
        {
            var tile = Tiles[location];
            tile.Color++;
            if (tile.Color > 15)
                tile.Color = 9;
            return new AnsiChar(StarChars[GameCycle & 0x3], tile.Color);
        }

        virtual public AnsiChar Draw_Stone(Location location)
        {
            return new AnsiChar(0x41 + RandomNumber(0x1A), Tiles[location].Color);
        }

        virtual public AnsiChar Draw_Transporter(Location location)
        {
            var actor = Actors[ActorIndexAt(location)];
            int index;

            if (actor.Vector.X == 0)
            {
                if (actor.Cycle > 0)
                    index = ((GameCycle / actor.Cycle) & 0x3);
                else
                    index = 0;
                index += (actor.Vector.Y << 1) + 2;
                return new AnsiChar(TransporterVChars[index], Tiles[location].Color);
            }
            else
            {
                if (actor.Cycle > 0)
                    index = ((GameCycle / actor.Cycle) & 0x3);
                else
                    index = 0;
                index += (actor.Vector.X << 1) + 2;
                return new AnsiChar(TransporterHChars[index], Tiles[location].Color);
            }
        }

        virtual public AnsiChar Draw_Web(Location location)
        {
            return new AnsiChar(WebChars[Adjacent(location, Elements.WebId)], Tiles[location].Color);
        }
    }
}
