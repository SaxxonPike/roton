using System.Linq;

namespace Roton.Emulation.Execution
{
    internal abstract partial class CoreBase
    {
        internal virtual string AmmoMessage => @"Ammunition - 5 shots per container.";

        internal virtual string BombMessage => @"Bomb activated!";

        internal virtual string DarkMessage => @"Room is dark - you need to light a torch!";

        internal virtual string DoorClosedMessage(int color)
        {
            return @"The " + Colors[color] + " door is locked!";
        }

        internal virtual string DoorOpenMessage(int color)
        {
            return @"The " + Colors[color] + " door is now open.";
        }

        internal virtual string EnergizerMessage => @"Energizer - You are invincible";

        internal virtual string FakeMessage => @"A fake wall - secret passage!";

        internal virtual string ForestMessage => @"A path is cleared through the forest.";

        internal virtual string GameOverMessage => @" Game over  -  Press ESCAPE";

        internal virtual string GemMessage => @"Gems give you Health!";

        internal virtual string InvisibleMessage => @"You are blocked by an invisible wall.";

        internal virtual string KeyAlreadyMessage(int color)
        {
            return @"You already have a " + Colors[color] + @" key!";
        }

        internal virtual string KeyMessage(int color)
        {
            return @"You now have the " + Colors[color] + " key.";
        }

        internal virtual string NoAmmoMessage => @"You don't have any ammo!";

        internal virtual string NotDarkMessage => @"Don't need torch - room is not dark!";

        internal virtual string NoShootMessage => @"Can't shoot in this place!";

        internal virtual string NoTorchMessage => @"You don't have any torches!";

        public virtual string StoneText
        {
            get
            {
                if (!StonesEnabled)
                    return string.Empty;

                foreach (var flag in Flags.Select(f => f.ToUpperInvariant()))
                {
                    if (flag.Length > 0 && flag.StartsWith("Z"))
                    {
                        return flag.Substring(1);
                    }
                }
                return string.Empty;
            }
        }

        internal virtual string TimeMessage => @"Running out of time!";

        internal virtual string TorchMessage => @"Torch - used for lighting in the underground.";

        internal virtual string WaterMessage => @"Your way is blocked by water.";
    }
}