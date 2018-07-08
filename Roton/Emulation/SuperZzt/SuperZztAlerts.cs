using Roton.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.ZZT;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    public sealed class SuperZztAlerts : Alerts
    {
        private readonly int _ammoPerPickup;
        private readonly IColors _colors;
        private readonly IMemory _memory;

        public SuperZztAlerts(IMemory memory, IColors colors, int ammoPerPickup)
        {
            _memory = memory;
            _colors = colors;
            _ammoPerPickup = ammoPerPickup;
        }

        public override IMessage AmmoMessage => new SuperZztMessage("Ammunition:", $"{_ammoPerPickup} shots");

        public override bool AmmoPickup
        {
            get { return _memory.ReadBool(0x7C0B); }
            set { _memory.WriteBool(0x7C0B, value); }
        }

        public override IMessage BombMessage { get; } = new SuperZztMessage("Bomb activated!");

        public override bool CantShootHere
        {
            get { return _memory.ReadBool(0x7C0D); }
            set { _memory.WriteBool(0x7C0D, value); }
        }

        public override bool Dark
        {
            get { return false; }
            set { }
        }

        public override IMessage DarkMessage { get; } = new SuperZztMessage();

        public override IMessage DoorLockedMessage(int color)
        {
            return new SuperZztMessage($"The {_colors[color]} door", "is locked!");
        }

        public override IMessage DoorOpenMessage(int color)
        {
            return new SuperZztMessage($"The {_colors[color]} door", "is now open.");
        }

        public override IMessage EnergizerMessage => new SuperZztMessage("Shield:", "You are invincible");

        public override bool EnergizerPickup
        {
            get { return _memory.ReadBool(0x7C11); }
            set { _memory.WriteBool(0x7C11, value); }
        }

        public override IMessage ErrorMessage(string error)
        {
            return new SuperZztMessage($"ERR: {error}");
        }

        public override IMessage FakeMessage { get; } = new SuperZztMessage("A fake wall:", "secret passage!");

        public override bool FakeWall
        {
            get { return _memory.ReadBool(0x7C0F); }
            set { _memory.WriteBool(0x7C0F, value); }
        }

        public override bool Forest
        {
            get { return _memory.ReadBool(0x7C0E); }
            set { _memory.WriteBool(0x7C0E, value); }
        }

        public override IMessage ForestMessage { get; } = new SuperZztMessage("A path is cleared", "through the forest.");
        public override IMessage GameOverMessage { get; } = new SuperZztMessage("Game over", "-- Press ESCAPE --");
        public override IMessage GemMessage { get; } = new SuperZztMessage("Gems give you health!");

        public override bool GemPickup
        {
            get { return _memory.ReadBool(0x7C10); }
            set { _memory.WriteBool(0x7C10, value); }
        }

        public override IMessage InvisibleMessage { get; } = new SuperZztMessage("You are blocked", "by an invisible wall.");

        public override IMessage KeyAlreadyMessage(int color)
        {
            return new SuperZztMessage("You already have a", $"{_colors[color]} key!");
        }

        public override IMessage KeyPickupMessage(int color)
        {
            return new SuperZztMessage("You now have the", $"{_colors[color]} key.");
        }

        public override IMessage NoAmmoMessage { get; } = new SuperZztMessage("You don't have", "any ammo!");
        public override IMessage NoShootMessage { get; } = new SuperZztMessage("Can't shoot", "in this place!");

        public override bool NotDark
        {
            get { return false; }
            set { }
        }

        public override IMessage NotDarkMessage { get; } = new SuperZztMessage();

        public override bool NoTorches
        {
            get { return false; }
            set { }
        }

        public override IMessage NoTorchMessage { get; } = new SuperZztMessage();

        public override IMessage OuchMessage { get; } = new SuperZztMessage("Ouch!");

        public override bool OutOfAmmo
        {
            get { return _memory.ReadBool(0x7C0C); }
            set { _memory.WriteBool(0x7C0C, value); }
        }

        public override IMessage StoneMessage { get; } = new SuperZztMessage("You have found a", "Stone of Power!");
        public override IMessage TimeMessage { get; } = new SuperZztMessage("Running out of time!");
        public override IMessage TorchMessage { get; } = new SuperZztMessage();

        public override bool TorchPickup
        {
            get { return false; }
            set { }
        }

        public override IMessage WaterMessage { get; } = new SuperZztMessage("Your way is", "blocked by lava.");
    }
}