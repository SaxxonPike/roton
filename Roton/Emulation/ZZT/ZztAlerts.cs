using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.ZZT
{
    internal class ZztAlerts : IAlerts
    {
        private readonly int _ammoPerPickup;
        private readonly IColorList _colors;
        private readonly IMemory _memory;

        public ZztAlerts(IMemory memory, IColorList colors, int ammoPerPickup)
        {
            _memory = memory;
            _colors = colors;
            _ammoPerPickup = ammoPerPickup;
        }

        public IMessage AmmoMessage => new ZztMessage($"Ammunition - {_ammoPerPickup} shots per container.");

        public bool AmmoPickup
        {
            get { return _memory.ReadBool(0x4AAB); }
            set { _memory.WriteBool(0x4AAB, value); }
        }

        public IMessage BombMessage { get; } = new ZztMessage("Bomb activated!");

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

        public IMessage DarkMessage { get; } = new ZztMessage("Room is dark - you need to light a torch!");

        public IMessage DoorLockedMessage(int color)
        {
            return new ZztMessage($"The {_colors[color]} door is locked!");
        }

        public IMessage DoorOpenMessage(int color)
        {
            return new ZztMessage($"The {_colors[color]} door is now open.");
        }

        public IMessage EnergizerMessage { get; } = new ZztMessage("Energizer - You are invincible");

        public bool EnergizerPickup
        {
            get { return _memory.ReadBool(0x4AB5); }
            set { _memory.WriteBool(0x4AB5, value); }
        }

        public IMessage ErrorMessage(string error)
        {
            return new ZztMessage($"ERR: {error}");
        }

        public IMessage FakeMessage { get; } = new ZztMessage("A fake wall - secret passage!");

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

        public IMessage ForestMessage { get; } = new ZztMessage("A path is cleared through the forest.");
        public IMessage GameOverMessage { get; } = new ZztMessage("Game over  -  Press ESCAPE");
        public IMessage GemMessage { get; } = new ZztMessage("Gems give you health!");

        public bool GemPickup
        {
            get { return _memory.ReadBool(0x4AB4); }
            set { _memory.WriteBool(0x4AB4, value); }
        }

        public IMessage InvisibleMessage { get; } = new ZztMessage("You are blocked by an invisible wall.");

        public IMessage KeyAlreadyMessage(int color)
        {
            return new ZztMessage($"You already have a {_colors[color]} key!");
        }

        public IMessage KeyPickupMessage(int color)
        {
            return new ZztMessage($"You now have the {_colors[color]} key.");
        }

        public IMessage NoAmmoMessage { get; } = new ZztMessage("You don't have any ammo!");
        public IMessage NoShootMessage { get; } = new ZztMessage("Can't shoot in this place!");

        public bool NotDark
        {
            get { return _memory.ReadBool(0x4AB1); }
            set { _memory.WriteBool(0x4AB1, value); }
        }

        public IMessage NotDarkMessage { get; } = new ZztMessage("Don't need torch - room is not dark!");

        public bool NoTorches
        {
            get { return _memory.ReadBool(0x4AAF); }
            set { _memory.WriteBool(0x4AAF, value); }
        }

        public IMessage NoTorchMessage { get; } = new ZztMessage("You don't have any torches!");

        public IMessage OuchMessage { get; } = new ZztMessage("Ouch!");

        public bool OutOfAmmo
        {
            get { return _memory.ReadBool(0x4AAC); }
            set { _memory.WriteBool(0x4AAC, value); }
        }

        public IMessage StoneMessage { get; } = new ZztMessage();
        public IMessage TimeMessage { get; } = new ZztMessage("Running out of time!");
        public IMessage TorchMessage { get; } = new ZztMessage("Torch - used for lighting in the underground.");

        public bool TorchPickup
        {
            get { return _memory.ReadBool(0x4AAE); }
            set { _memory.WriteBool(0x4AAE, value); }
        }

        public IMessage WaterMessage { get; } = new ZztMessage("Your way is blocked by water.");
    }
}