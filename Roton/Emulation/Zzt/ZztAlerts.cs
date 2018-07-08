using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.ZZT
{
    public sealed class ZztAlerts : Alerts
    {
        private readonly IColors _colors;
        private readonly IConfig _config;
        private readonly IMemory _memory;

        public ZztAlerts(IMemory memory, IColors colors, IConfig config)
        {
            _memory = memory;
            _colors = colors;
            _config = config;
        }

        public override IMessage AmmoMessage => new ZztMessage($"Ammunition - {_config.AmmoPerPickup} shots per container.");

        public override bool AmmoPickup
        {
            get { return _memory.ReadBool(0x4AAB); }
            set { _memory.WriteBool(0x4AAB, value); }
        }

        public override IMessage BombMessage { get; } = new ZztMessage("Bomb activated!");

        public override bool CantShootHere
        {
            get { return _memory.ReadBool(0x4AAD); }
            set { _memory.WriteBool(0x4AAD, value); }
        }

        public override bool Dark
        {
            get { return _memory.ReadBool(0x4AB1); }
            set { _memory.WriteBool(0x4AB1, value); }
        }

        public override IMessage DarkMessage { get; } = new ZztMessage("Room is dark - you need to light a torch!");

        public override IMessage DoorLockedMessage(int color)
        {
            return new ZztMessage($"The {_colors[color]} door is locked!");
        }

        public override IMessage DoorOpenMessage(int color)
        {
            return new ZztMessage($"The {_colors[color]} door is now open.");
        }

        public override IMessage EnergizerMessage { get; } = new ZztMessage("Energizer - You are invincible");

        public override bool EnergizerPickup
        {
            get { return _memory.ReadBool(0x4AB5); }
            set { _memory.WriteBool(0x4AB5, value); }
        }

        public override IMessage ErrorMessage(string error)
        {
            return new ZztMessage($"ERR: {error}");
        }

        public override IMessage FakeMessage { get; } = new ZztMessage("A fake wall - secret passage!");

        public override bool FakeWall
        {
            get { return _memory.ReadBool(0x4AB3); }
            set { _memory.WriteBool(0x4AB3, value); }
        }

        public override bool Forest
        {
            get { return _memory.ReadBool(0x4AB2); }
            set { _memory.WriteBool(0x4AB2, value); }
        }

        public override IMessage ForestMessage { get; } = new ZztMessage("A path is cleared through the forest.");
        public override IMessage GameOverMessage { get; } = new ZztMessage("Game over  -  Press ESCAPE");
        public override IMessage GemMessage { get; } = new ZztMessage("Gems give you health!");

        public override bool GemPickup
        {
            get { return _memory.ReadBool(0x4AB4); }
            set { _memory.WriteBool(0x4AB4, value); }
        }

        public override IMessage InvisibleMessage { get; } = new ZztMessage("You are blocked by an invisible wall.");

        public override IMessage KeyAlreadyMessage(int color)
        {
            return new ZztMessage($"You already have a {_colors[color]} key!");
        }

        public override IMessage KeyPickupMessage(int color)
        {
            return new ZztMessage($"You now have the {_colors[color]} key.");
        }

        public override IMessage NoAmmoMessage { get; } = new ZztMessage("You don't have any ammo!");
        public override IMessage NoShootMessage { get; } = new ZztMessage("Can't shoot in this place!");

        public override bool NotDark
        {
            get { return _memory.ReadBool(0x4AB1); }
            set { _memory.WriteBool(0x4AB1, value); }
        }

        public override IMessage NotDarkMessage { get; } = new ZztMessage("Don't need torch - room is not dark!");

        public override bool NoTorches
        {
            get { return _memory.ReadBool(0x4AAF); }
            set { _memory.WriteBool(0x4AAF, value); }
        }

        public override IMessage NoTorchMessage { get; } = new ZztMessage("You don't have any torches!");

        public override IMessage OuchMessage { get; } = new ZztMessage("Ouch!");

        public override bool OutOfAmmo
        {
            get { return _memory.ReadBool(0x4AAC); }
            set { _memory.WriteBool(0x4AAC, value); }
        }

        public override IMessage StoneMessage { get; } = new ZztMessage();
        public override IMessage TimeMessage { get; } = new ZztMessage("Running out of time!");
        public override IMessage TorchMessage { get; } = new ZztMessage("Torch - used for lighting in the underground.");

        public override bool TorchPickup
        {
            get { return _memory.ReadBool(0x4AAE); }
            set { _memory.WriteBool(0x4AAE, value); }
        }

        public override IMessage WaterMessage { get; } = new ZztMessage("Your way is blocked by water.");
    }
}