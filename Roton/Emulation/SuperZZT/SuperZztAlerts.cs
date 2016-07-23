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

        public IMessage AmmoMessage => new SuperZztMessage("Ammunition:", "20 shots");

        public bool AmmoPickup
        {
            get { return _memory.ReadBool(0x7C0B); }
            set { _memory.WriteBool(0x7C0B, value); }
        }

        public IMessage BombMessage => new SuperZztMessage("Bomb activated!");

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

        public IMessage DarkMessage => new SuperZztMessage();

        public IMessage DoorLockedMessage(int color)
        {
            return new SuperZztMessage($"The {_colors[color]} door", "is locked!");
        }

        public IMessage DoorOpenMessage(int color)
        {
            return new SuperZztMessage($"The {_colors[color]} door", "is now open.");
        }

        public IMessage EnergizerMessage => new SuperZztMessage("Shield:", "You are invincible");

        public bool EnergizerPickup
        {
            get { return _memory.ReadBool(0x7C11); }
            set { _memory.WriteBool(0x7C11, value); }
        }

        public IMessage FakeMessage => new SuperZztMessage("A fake wall:", "secret passage!");

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

        public IMessage ForestMessage => new SuperZztMessage("A path is cleared", "through the forest.");
        public IMessage GameOverMessage => new SuperZztMessage("Game over", "-- Press ESCAPE --");
        public IMessage GemMessage => new SuperZztMessage("Gems give you health!");

        public bool GemPickup
        {
            get { return _memory.ReadBool(0x7C10); }
            set { _memory.WriteBool(0x7C10, value); }
        }

        public IMessage InvisibleMessage => new SuperZztMessage("You are blocked", "by an invisible wall.");

        public IMessage KeyAlreadyMessage(int color)
        {
            return new SuperZztMessage("You already have a", $"{_colors[color]} key!");
        }

        public IMessage KeyPickupMessage(int color)
        {
            return new SuperZztMessage("You now have the", $"{_colors[color]} key.");
        }

        public IMessage NoAmmoMessage => new SuperZztMessage("You don't have", "any ammo!");
        public IMessage NoShootMessage => new SuperZztMessage("Can't shoot", "in this place!");

        public bool NotDark
        {
            get { return false; }
            set { }
        }

        public IMessage NotDarkMessage => new SuperZztMessage();

        public bool NoTorches
        {
            get { return false; }
            set { }
        }

        public IMessage NoTorchMessage => new SuperZztMessage();

        public bool OutOfAmmo
        {
            get { return _memory.ReadBool(0x7C0C); }
            set { _memory.WriteBool(0x7C0C, value); }
        }

        public IMessage StoneMessage => new SuperZztMessage("You have found a", "Stone of Power!");
        public IMessage TimeMessage => new SuperZztMessage("Running out of time!");
        public IMessage TorchMessage => new SuperZztMessage();

        public bool TorchPickup
        {
            get { return false; }
            set { }
        }

        public IMessage WaterMessage => new SuperZztMessage("Your way is", "blocked by lava.");
    }
}