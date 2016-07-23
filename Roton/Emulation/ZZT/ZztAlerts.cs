using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.ZZT
{
    internal class ZztAlerts : IAlerts
    {
        private readonly IColorList _colors;
        private readonly IMemory _memory;

        public ZztAlerts(IMemory memory, IColorList colors)
        {
            _memory = memory;
            _colors = colors;
        }

        public string AmmoMessage => "Ammunition - 5 shots per container.";

        public bool AmmoPickup
        {
            get { return _memory.ReadBool(0x4AAB); }
            set { _memory.WriteBool(0x4AAB, value); }
        }

        public string BombMessage => "Bomb activated!";

        public bool CantShootHere
        {
            get { return _memory.ReadBool(0x4AAD); }
            set { _memory.WriteBool(0x4AAD, value); }
        }

        public bool Dark
        {
            get { return _memory.ReadBool(0x4AB1); }
            set { _memory.WriteBool(0x4AB1, value); }
        }

        public string DarkMessage => "Room is dark - you need to light a torch!";

        public string DoorLockedMessage(int color)
        {
            return $"The {_colors[color]} door is locked!";
        }

        public string DoorOpenMessage(int color)
        {
            return $"The {_colors[color]} door is now open.";
        }

        public string EnergizerMessage => "Energizer - You are invincible";

        public bool EnergizerPickup
        {
            get { return _memory.ReadBool(0x4AB5); }
            set { _memory.WriteBool(0x4AB5, value); }
        }

        public string FakeMessage => "A fake wall - secret passage!";

        public bool FakeWall
        {
            get { return _memory.ReadBool(0x4AB3); }
            set { _memory.WriteBool(0x4AB3, value); }
        }

        public bool Forest
        {
            get { return _memory.ReadBool(0x4AB2); }
            set { _memory.WriteBool(0x4AB2, value); }
        }

        public string ForestMessage => "A path is cleared through the forest.";
        public string GameOverMessage => "Game over  -  Press ESCAPE";
        public string GemMessage => "Gems give you health!";

        public bool GemPickup
        {
            get { return _memory.ReadBool(0x4AB4); }
            set { _memory.WriteBool(0x4AB4, value); }
        }

        public string InvisibleMessage => "You are blocked by an invisible wall.";

        public string KeyAleadyMessage(int color)
        {
            return $"You already have a {_colors[color]} key!";
        }

        public string KeyPickupMessage(int color)
        {
            return $"You now have the {_colors[color]} key.";
        }

        public string NoAmmoMessage => "You don't have any ammo!";
        public string NoShootMessage => "Can't shoot in this place!";

        public bool NotDark
        {
            get { return _memory.ReadBool(0x4AB1); }
            set { _memory.WriteBool(0x4AB1, value); }
        }

        public string NotDarkMessage => "Don't need torch - room is not dark!";

        public bool NoTorches
        {
            get { return _memory.ReadBool(0x4AAF); }
            set { _memory.WriteBool(0x4AAF, value); }
        }

        public string NoTorchMessage => "You don't have any torches!";

        public bool OutOfAmmo
        {
            get { return _memory.ReadBool(0x4AAC); }
            set { _memory.WriteBool(0x4AAC, value); }
        }

        public string StoneMessage => string.Empty;
        public string TimeMessage => "Running out of time!";
        public string TorchMessage => "Torch - used for lighting in the underground.";

        public bool TorchPickup
        {
            get { return _memory.ReadBool(0x4AAE); }
            set { _memory.WriteBool(0x4AAE, value); }
        }

        public string WaterMessage => "Your way is blocked by water.";
    }
}