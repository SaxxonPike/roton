using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalAlerts(IMemory memory, IColors colors, IFacts facts) : Alerts
{
    public override IMessage AmmoMessage => new Message($"Ammunition - {facts.AmmoPerPickup} shots per container.");

    public override bool AmmoPickup
    {
        get => memory.ReadBool(0x4AAB);
        set => memory.WriteBool(0x4AAB, value);
    }

    public override IMessage BombMessage { get; } = new Message("Bomb activated!");

    public override bool CantShootHere
    {
        get => memory.ReadBool(0x4AAD);
        set => memory.WriteBool(0x4AAD, value);
    }

    public override bool Dark
    {
        get => memory.ReadBool(0x4AB1);
        set => memory.WriteBool(0x4AB1, value);
    }

    public override IMessage DarkMessage { get; } = new Message("Room is dark - you need to light a torch!");

    public override IMessage DoorLockedMessage(int color)
    {
        return new Message($"The {colors[color]} door is locked!");
    }

    public override IMessage DoorOpenMessage(int color)
    {
        return new Message($"The {colors[color]} door is now open.");
    }

    public override IMessage EnergizerMessage { get; } = new Message("Energizer - You are invincible");

    public override bool EnergizerPickup
    {
        get => memory.ReadBool(0x4AB5);
        set => memory.WriteBool(0x4AB5, value);
    }

    public override IMessage ErrorMessage(ReadOnlySpan<char> error)
    {
        return new Message($"ERR: {error.ToString()}");
    }

    public override IMessage FakeMessage { get; } = new Message("A fake wall - secret passage!");

    public override bool FakeWall
    {
        get => memory.ReadBool(0x4AB3);
        set => memory.WriteBool(0x4AB3, value);
    }

    public override bool Forest
    {
        get => memory.ReadBool(0x4AB2);
        set => memory.WriteBool(0x4AB2, value);
    }

    public override IMessage ForestMessage { get; } = new Message("A path is cleared through the forest.");
    public override IMessage GameOverMessage { get; } = new Message("Game over  -  Press ESCAPE");
    public override IMessage GemMessage { get; } = new Message("Gems give you health!");

    public override bool GemPickup
    {
        get => memory.ReadBool(0x4AB4);
        set => memory.WriteBool(0x4AB4, value);
    }

    public override IMessage InvisibleMessage { get; } = new Message("You are blocked by an invisible wall.");

    public override IMessage KeyAlreadyMessage(int color)
    {
        return new Message($"You already have a {colors[color]} key!");
    }

    public override IMessage KeyPickupMessage(int color)
    {
        return new Message($"You now have the {colors[color]} key.");
    }

    public override IMessage NoAmmoMessage { get; } = new Message("You don't have any ammo!");
    public override IMessage NoShootMessage { get; } = new Message("Can't shoot in this place!");

    public override bool NotDark
    {
        get => memory.ReadBool(0x4AB1);
        set => memory.WriteBool(0x4AB1, value);
    }

    public override IMessage NotDarkMessage { get; } = new Message("Don't need torch - room is not dark!");

    public override bool NoTorches
    {
        get => memory.ReadBool(0x4AAF);
        set => memory.WriteBool(0x4AAF, value);
    }

    public override IMessage NoTorchMessage { get; } = new Message("You don't have any torches!");

    public override IMessage OuchMessage { get; } = new Message("Ouch!");

    public override bool OutOfAmmo
    {
        get => memory.ReadBool(0x4AAC);
        set => memory.WriteBool(0x4AAC, value);
    }

    public override IMessage StoneMessage { get; } = new Message();
    public override IMessage TimeMessage { get; } = new Message("Running out of time!");
    public override IMessage TorchMessage { get; } = new Message("Torch - used for lighting in the underground.");

    public override bool TorchPickup
    {
        get => memory.ReadBool(0x4AAE);
        set => memory.WriteBool(0x4AAE, value);
    }

    public override IMessage WaterMessage { get; } = new Message("Your way is blocked by water.");
}