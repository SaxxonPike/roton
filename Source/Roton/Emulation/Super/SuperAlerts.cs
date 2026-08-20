using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperAlerts(IMemory memory, IColors colors, IFacts facts) : Alerts
{
    public override IMessage AmmoMessage => new Message("Ammunition:", $"{facts.AmmoPerPickup} shots");

    public override bool AmmoPickup
    {
        get => memory.ReadBool(0x7C0B);
        set => memory.WriteBool(0x7C0B, value);
    }

    public override IMessage BombMessage { get; } = new Message("Bomb activated!");

    public override bool CantShootHere
    {
        get => memory.ReadBool(0x7C0D);
        set => memory.WriteBool(0x7C0D, value);
    }

    public override bool Dark
    {
        get => false;
        set { }
    }

    public override IMessage DarkMessage { get; } = new Message();

    public override IMessage DoorLockedMessage(int color)
    {
        return new Message($"The {colors[color]} door", "is locked!");
    }

    public override IMessage DoorOpenMessage(int color)
    {
        return new Message($"The {colors[color]} door", "is now open.");
    }

    public override IMessage EnergizerMessage { get; } = new Message("Shield:", "You are invincible");

    public override bool EnergizerPickup
    {
        get => memory.ReadBool(0x7C11);
        set => memory.WriteBool(0x7C11, value);
    }

    public override IMessage ErrorMessage(ReadOnlySpan<char> error)
    {
        return new Message($"ERR: {error.ToString()}");
    }

    public override IMessage FakeMessage { get; } = new Message("A fake wall:", "secret passage!");

    public override bool FakeWall
    {
        get => memory.ReadBool(0x7C0F);
        set => memory.WriteBool(0x7C0F, value);
    }

    public override bool Forest
    {
        get => memory.ReadBool(0x7C0E);
        set => memory.WriteBool(0x7C0E, value);
    }

    public override IMessage ForestMessage { get; } = new Message("A path is cleared", "through the forest.");
    public override IMessage GameOverMessage { get; } = new Message("Game over", "-- Press ESCAPE --");
    public override IMessage GemMessage { get; } = new Message("Gems give you health!");

    public override bool GemPickup
    {
        get => memory.ReadBool(0x7C10);
        set => memory.WriteBool(0x7C10, value);
    }

    public override IMessage InvisibleMessage { get; } = new Message("You are blocked", "by an invisible wall.");

    public override IMessage KeyAlreadyMessage(int color)
    {
        return new Message("You already have a", $"{colors[color]} key!");
    }

    public override IMessage KeyPickupMessage(int color)
    {
        return new Message("You now have the", $"{colors[color]} key.");
    }

    public override IMessage NoAmmoMessage { get; } = new Message("You don't have", "any ammo!");
    public override IMessage NoShootMessage { get; } = new Message("Can't shoot", "in this place!");

    public override bool NotDark
    {
        get => false;
        set { }
    }

    public override IMessage NotDarkMessage { get; } = new Message();

    public override bool NoTorches
    {
        get => false;
        set { }
    }

    public override IMessage NoTorchMessage { get; } = new Message();

    public override IMessage OuchMessage { get; } = new Message("Ouch!");

    public override bool OutOfAmmo
    {
        get => memory.ReadBool(0x7C0C);
        set => memory.WriteBool(0x7C0C, value);
    }

    public override IMessage StoneMessage { get; } = new Message("You have found a", "Stone of Power!");
    public override IMessage TimeMessage { get; } = new Message("Running out of time!");
    public override IMessage TorchMessage { get; } = new Message();

    public override bool TorchPickup
    {
        get => false;
        set { }
    }

    public override IMessage WaterMessage { get; } = new Message("Your way is", "blocked by lava.");
}