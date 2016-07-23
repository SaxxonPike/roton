using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    internal class SuperZztAlerts : IAlerts
    {
        private readonly IColorList _colors;
        private readonly IMemory _memory;

        public SuperZztAlerts(IMemory memory, IColorList colors)
        {
            _memory = memory;
            _colors = colors;
        }

        public string AmmoMessage => "Ammunition:\r20 shots";

        public bool AmmoPickup
        {
            get { return _memory.ReadBool(0x7C0B); }
            set { _memory.WriteBool(0x7C0B, value); }
        }

        public string BombMessage => "Bomb activated!";

        public bool CantShootHere
        {
            get { return _memory.ReadBool(0x7C0D); }
            set { _memory.WriteBool(0x7C0D, value); }
        }

        public bool Dark
        {
            get { return false; }
            set { }
        }

        public string DarkMessage => string.Empty;

        public string DoorLockedMessage(int color)
        {
            return $"The {_colors[color]} door\ris locked!";
        }

        public string DoorOpenMessage(int color)
        {
            return $"The {_colors[color]} door \ris now open.";
        }

        public string EnergizerMessage => "Shield:\rYou are invincible";

        public bool EnergizerPickup
        {
            get { return _memory.ReadBool(0x7C11); }
            set { _memory.WriteBool(0x7C11, value); }
        }

        public string FakeMessage => "A fake wall:\rsecret passage!";

        public bool FakeWall
        {
            get { return _memory.ReadBool(0x7C0F); }
            set { _memory.WriteBool(0x7C0F, value); }
        }

        public bool Forest
        {
            get { return _memory.ReadBool(0x7C0E); }
            set { _memory.WriteBool(0x7C0E, value); }
        }

        public string ForestMessage => "A path is cleared\rthrough the forest.";
        public string GameOverMessage => "Game over\r-- Press ESCAPE --";
        public string GemMessage => "Gems give you health!";

        public bool GemPickup
        {
            get { return _memory.ReadBool(0x7C10); }
            set { _memory.WriteBool(0x7C10, value); }
        }

        public string InvisibleMessage => "You are blocked\rby an invisible wall.";

        public string KeyAleadyMessage(int color)
        {
            return $"You already have a\r{_colors[color]} key!";
        }

        public string KeyPickupMessage(int color)
        {
            return $"You now have the\r{_colors[color]} key.";
        }

        public string NoAmmoMessage => "You don't have\rany ammo!";
        public string NoShootMessage => "Can't shoot\rin this place!";

        public bool NotDark
        {
            get { return false; }
            set { }
        }

        public string NotDarkMessage => string.Empty;

        public bool NoTorches
        {
            get { return false; }
            set { }
        }

        public string NoTorchMessage => string.Empty;

        public bool OutOfAmmo
        {
            get { return _memory.ReadBool(0x7C0C); }
            set { _memory.WriteBool(0x7C0C, value); }
        }

        public string StoneMessage => "You have found a\rStone of Power!";
        public string TimeMessage => "Running out of time!";
        public string TorchMessage => string.Empty;

        public bool TorchPickup
        {
            get { return false; }
            set { }
        }

        public string WaterMessage => "Your way is\rblocked by lava.";
    }
}