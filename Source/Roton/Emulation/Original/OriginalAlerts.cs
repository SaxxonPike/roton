using System;
using Roton.Emulation.Colors;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalAlerts(IMemory memory, IColorList colors, IFacts facts) : Alerts
{
    private string GetColorName(int color) =>
        colors.Get(color)?.Name ?? "";

    public override IMessage AmmoMessage =>
        new Message($"Ammunition - {facts.AmmoPerPickup} shots per container.");

    public override ref Bool AmmoPickup =>
        ref memory.GetRef<Bool>(0x4AAB);

    public override IMessage BombMessage { get; } =
        new Message("Bomb activated!");

    public override ref Bool CantShootHere =>
        ref memory.GetRef<Bool>(0x4AAD);

    public override ref Bool Dark =>
        ref memory.GetRef<Bool>(0x4AB1);

    public override IMessage DarkMessage { get; } =
        new Message("Room is dark - you need to light a torch!");

    public override IMessage DoorLockedMessage(int color) =>
        new Message($"The {GetColorName(color)} door is locked!");

    public override IMessage DoorOpenMessage(int color) =>
        new Message($"The {GetColorName(color)} door is now open.");

    public override IMessage EnergizerMessage { get; } =
        new Message("Energizer - You are invincible");

    public override ref Bool EnergizerPickup =>
        ref memory.GetRef<Bool>(0x4AB5);

    public override IMessage ErrorMessage(ReadOnlySpan<char> error) =>
        new Message($"ERR: {error.ToString()}");

    public override IMessage FakeMessage { get; } =
        new Message("A fake wall - secret passage!");

    public override ref Bool FakeWall =>
        ref memory.GetRef<Bool>(0x4AB3);

    public override ref Bool Forest =>
        ref memory.GetRef<Bool>(0x4AB2);

    public override IMessage ForestMessage { get; } =
        new Message("A path is cleared through the forest.");

    public override IMessage GameOverMessage { get; } =
        new Message("Game over  -  Press ESCAPE");

    public override IMessage GemMessage { get; } =
        new Message("Gems give you health!");

    public override ref Bool GemPickup =>
        ref memory.GetRef<Bool>(0x4AB4);

    public override IMessage InvisibleMessage { get; } =
        new Message("You are blocked by an invisible wall.");

    public override IMessage KeyAlreadyMessage(int color) =>
        new Message($"You already have a {GetColorName(color)} key!");

    public override IMessage KeyPickupMessage(int color) =>
        new Message($"You now have the {GetColorName(color)} key.");

    public override IMessage NoAmmoMessage { get; } =
        new Message("You don't have any ammo!");

    public override IMessage NoShootMessage { get; } =
        new Message("Can't shoot in this place!");

    public override ref Bool NotDark =>
        ref memory.GetRef<Bool>(0x4AB1);

    public override IMessage NotDarkMessage { get; } =
        new Message("Don't need torch - room is not dark!");

    public override ref Bool NoTorches =>
        ref memory.GetRef<Bool>(0x4AAF);

    public override IMessage NoTorchMessage { get; } =
        new Message("You don't have any torches!");

    public override IMessage OuchMessage { get; } =
        new Message("Ouch!");

    public override ref Bool OutOfAmmo =>
        ref memory.GetRef<Bool>(0x4AAC);

    public override IMessage StoneMessage { get; } =
        new Message();

    public override IMessage TimeMessage { get; } =
        new Message("Running out of time!");

    public override IMessage TorchMessage { get; } =
        new Message("Torch - used for lighting in the underground.");

    public override ref Bool TorchPickup =>
        ref memory.GetRef<Bool>(0x4AAE);

    public override IMessage WaterMessage { get; } =
        new Message("Your way is blocked by water.");
}