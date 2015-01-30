using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    abstract internal partial class CoreBase
    {
        virtual internal string AmmoMessage
        {
            get { return @"Ammunition - 5 shots per container."; }
        }

        virtual internal string BombMessage
        {
            get { return @"Bomb activated!"; }
        }

        virtual internal string DarkMessage
        {
            get { return @"Room is dark - you need to light a torch!"; }
        }

        virtual internal string DoorClosedMessage(int color)
        {
            return @"The " + Colors[color] + " door is locked!";
        }

        virtual internal string DoorOpenMessage(int color)
        {
            return @"The " + Colors[color] + " door is now open.";
        }

        virtual internal string EnergizerMessage
        {
            get { return @"Energizer - You are invincible"; }
        }

        virtual internal string FakeMessage
        {
            get { return @"A fake wall - secret passage!"; }
        }

        virtual internal string ForestMessage
        {
            get { return @"A path is cleared through the forest."; }
        }

        virtual internal string GameOverMessage
        {
            get { return @" Game over  -  Press ESCAPE"; }
        }

        virtual internal string GemMessage
        {
            get { return @"Gems give you Health!"; }
        }

        virtual internal string InvisibleMessage
        {
            get { return @"You are blocked by an invisible wall."; }
        }

        virtual internal string KeyAlreadyMessage(int color)
        {
            return @"You already have a " + Colors[color] + @" key!";
        }

        virtual internal string KeyMessage(int color)
        {
            return @"You now have the " + Colors[color] + " key.";
        }

        virtual internal string NotDarkMessage
        {
            get { return @"Don't need torch - room is not dark!"; }
        }

        virtual internal string NoTorchMessage
        {
            get { return @"You don't have any torches!"; }
        }

        virtual public string StoneText
        {
            get
            {
                if (StonesEnabled)
                {
                    for (int i = 0; i < Flags.Count; i++)
                    {
                        var flag = Flags[i].ToUpper();
                        if (flag.Length > 0 && flag.StartsWith("Z"))
                        {
                            return flag.Substring(1);
                        }
                    }
                }
                return "";
            }
        }

        virtual internal string TimeMessage
        {
            get { return @"Running out of time!"; }
        }

        virtual internal string TorchMessage
        {
            get { return @"Torch - used for lighting in the underground."; }
        }

        virtual internal string WaterMessage
        {
            get { return @"Your way is blocked by water."; }
        }
    }
}
