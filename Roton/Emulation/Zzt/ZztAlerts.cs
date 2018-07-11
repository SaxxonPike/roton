using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Zzt
{
    [ContextEngine(ContextEngine.Zzt)]
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

        public override IMessage AmmoMessage => new Message($"Ammunition - {_config.AmmoPerPickup} shots per container.");

        public override bool AmmoPickup
        {
            get => _memory.ReadBool(0x4AAB);
            set => _memory.WriteBool(0x4AAB, value);
        }

        public override IMessage BombMessage { get; } = new Message("Bomb activated!");

        public override bool CantShootHere
        {
            get => _memory.ReadBool(0x4AAD);
            set => _memory.WriteBool(0x4AAD, value);
        }

        public override bool Dark
        {
            get => _memory.ReadBool(0x4AB1);
            set => _memory.WriteBool(0x4AB1, value);
        }

        public override IMessage DarkMessage { get; } = new Message("Room is dark - you need to light a torch!");

        public override IMessage DoorLockedMessage(int color)
        {
            return new Message($"The {_colors[color]} door is locked!");
        }

        public override IMessage DoorOpenMessage(int color)
        {
            return new Message($"The {_colors[color]} door is now open.");
        }

        public override IMessage EnergizerMessage { get; } = new Message("Energizer - You are invincible");

        public override bool EnergizerPickup
        {
            get => _memory.ReadBool(0x4AB5);
            set => _memory.WriteBool(0x4AB5, value);
        }

        public override IMessage ErrorMessage(string error)
        {
            return new Message($"ERR: {error}");
        }

        public override IMessage FakeMessage { get; } = new Message("A fake wall - secret passage!");

        public override bool FakeWall
        {
            get => _memory.ReadBool(0x4AB3);
            set => _memory.WriteBool(0x4AB3, value);
        }

        public override bool Forest
        {
            get => _memory.ReadBool(0x4AB2);
            set => _memory.WriteBool(0x4AB2, value);
        }

        public override IMessage ForestMessage { get; } = new Message("A path is cleared through the forest.");
        public override IMessage GameOverMessage { get; } = new Message("Game over  -  Press ESCAPE");
        public override IMessage GemMessage { get; } = new Message("Gems give you health!");

        public override bool GemPickup
        {
            get => _memory.ReadBool(0x4AB4);
            set => _memory.WriteBool(0x4AB4, value);
        }

        public override IMessage InvisibleMessage { get; } = new Message("You are blocked by an invisible wall.");

        public override IMessage KeyAlreadyMessage(int color)
        {
            return new Message($"You already have a {_colors[color]} key!");
        }

        public override IMessage KeyPickupMessage(int color)
        {
            return new Message($"You now have the {_colors[color]} key.");
        }

        public override IMessage NoAmmoMessage { get; } = new Message("You don't have any ammo!");
        public override IMessage NoShootMessage { get; } = new Message("Can't shoot in this place!");

        public override bool NotDark
        {
            get => _memory.ReadBool(0x4AB1);
            set => _memory.WriteBool(0x4AB1, value);
        }

        public override IMessage NotDarkMessage { get; } = new Message("Don't need torch - room is not dark!");

        public override bool NoTorches
        {
            get => _memory.ReadBool(0x4AAF);
            set => _memory.WriteBool(0x4AAF, value);
        }

        public override IMessage NoTorchMessage { get; } = new Message("You don't have any torches!");

        public override IMessage OuchMessage { get; } = new Message("Ouch!");

        public override bool OutOfAmmo
        {
            get => _memory.ReadBool(0x4AAC);
            set => _memory.WriteBool(0x4AAC, value);
        }

        public override IMessage StoneMessage { get; } = new Message();
        public override IMessage TimeMessage { get; } = new Message("Running out of time!");
        public override IMessage TorchMessage { get; } = new Message("Torch - used for lighting in the underground.");

        public override bool TorchPickup
        {
            get => _memory.ReadBool(0x4AAE);
            set => _memory.WriteBool(0x4AAE, value);
        }

        public override IMessage WaterMessage { get; } = new Message("Your way is blocked by water.");
    }
}