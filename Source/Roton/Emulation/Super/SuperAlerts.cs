using System;
using Roton.Emulation.Colors;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperAlerts(
    IMemory memory,
    IColorList colors,
    IFacts facts)
    : Alerts
{
    private Bool _dark;
    private Bool _notDark;
    private Bool _noTorches;
    private Bool _torchPickup;

    private string GetColorName(int color) =>
        colors.Get(color)?.Name ?? "";

    public override IMessage AmmoMessage =>
        new Message("Ammunition:", $"{facts.AmmoPerPickup} shots");

    public override ref Bool AmmoPickup =>
        ref memory.GetRef<Bool>(0x7C0B);

    public override IMessage BombMessage { get; } =
        new Message("Bomb activated!");

    public override ref Bool CantShootHere =>
        ref memory.GetRef<Bool>(0x7C0D);

    public override ref Bool Dark =>
        ref _dark;

    public override IMessage DarkMessage { get; } =
        new Message();

    public override IMessage DoorLockedMessage(int color) =>
        new Message($"The {GetColorName(color)} door", "is locked!");

    public override IMessage DoorOpenMessage(int color) =>
        new Message($"The {GetColorName(color)} door", "is now open.");

    public override IMessage EnergizerMessage { get; } =
        new Message("Shield:", "You are invincible");

    public override ref Bool EnergizerPickup =>
        ref memory.GetRef<Bool>(0x7C11);

    public override IMessage ErrorMessage(ReadOnlySpan<char> error) =>
        new Message($"ERR: {error.ToString()}");

    public override IMessage FakeMessage { get; } =
        new Message("A fake wall:", "secret passage!");

    public override ref Bool FakeWall =>
        ref memory.GetRef<Bool>(0x7C0F);

    public override ref Bool Forest =>
        ref memory.GetRef<Bool>(0x7C0E);

    public override IMessage ForestMessage { get; } =
        new Message("A path is cleared", "through the forest.");

    public override IMessage GameOverMessage { get; } =
        new Message("Game over", "-- Press ESCAPE --");

    public override IMessage GemMessage { get; } =
        new Message("Gems give you health!");

    public override ref Bool GemPickup =>
        ref memory.GetRef<Bool>(0x7C10);

    public override IMessage InvisibleMessage { get; } =
        new Message("You are blocked", "by an invisible wall.");

    public override IMessage KeyAlreadyMessage(int color) =>
        new Message("You already have a", $"{GetColorName(color)} key!");

    public override IMessage KeyPickupMessage(int color) =>
        new Message("You now have the", $"{GetColorName(color)} key.");

    public override IMessage NoAmmoMessage { get; } =
        new Message("You don't have", "any ammo!");

    public override IMessage NoShootMessage { get; } =
        new Message("Can't shoot", "in this place!");

    public override ref Bool NotDark =>
        ref _notDark;

    public override IMessage NotDarkMessage { get; } =
        new Message();

    public override ref Bool NoTorches =>
        ref _noTorches;

    public override IMessage NoTorchMessage { get; } =
        new Message();

    public override IMessage OuchMessage { get; } =
        new Message("Ouch!");

    public override ref Bool OutOfAmmo =>
        ref memory.GetRef<Bool>(0x7C0C);

    public override IMessage StoneMessage { get; } =
        new Message("You have found a", "Stone of Power!");

    public override IMessage TimeMessage { get; } =
        new Message("Running out of time!");

    public override IMessage TorchMessage { get; } =
        new Message();

    public override ref Bool TorchPickup =>
        ref _torchPickup;

    public override IMessage WaterMessage { get; } =
        new Message("Your way is", "blocked by lava.");
}