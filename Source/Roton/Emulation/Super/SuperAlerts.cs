using System;
using System.Collections.Generic;
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
        colors.Get(color)?.Name ?? string.Empty;

    public override IReadOnlyList<string> AmmoMessage =>
        ["Ammunition:", $"{facts.AmmoPerPickup} shots"];

    public override ref Bool AmmoPickup =>
        ref memory.GetRef<Bool>(0x7C0B);

    public override IReadOnlyList<string> BombMessage { get; } =
        ["Bomb activated!"];

    public override ref Bool CantShootHere =>
        ref memory.GetRef<Bool>(0x7C0D);

    public override ref Bool Dark =>
        ref _dark;

    public override IReadOnlyList<string> DarkMessage { get; } =
        [];

    public override IReadOnlyList<string> DoorLockedMessage(int color) =>
        [$"The {GetColorName(color)} door", "is locked!"];

    public override IReadOnlyList<string> DoorOpenMessage(int color) =>
        [$"The {GetColorName(color)} door", "is now open."];

    public override IReadOnlyList<string> EnergizerMessage { get; } =
        ["Shield:", "You are invincible"];

    public override ref Bool EnergizerPickup =>
        ref memory.GetRef<Bool>(0x7C11);

    public override IReadOnlyList<string> ErrorMessage(ReadOnlySpan<char> error) =>
        [$"ERR: {error.ToString()}"];

    public override IReadOnlyList<string> FakeMessage { get; } =
        ["A fake wall:", "secret passage!"];

    public override ref Bool FakeWall =>
        ref memory.GetRef<Bool>(0x7C0F);

    public override ref Bool Forest =>
        ref memory.GetRef<Bool>(0x7C0E);

    public override IReadOnlyList<string> ForestMessage { get; } =
        ["A path is cleared", "through the forest."];

    public override IReadOnlyList<string> GameOverMessage { get; } =
        ["Game over", "-- Press ESCAPE --"];

    public override IReadOnlyList<string> GemMessage { get; } =
        ["Gems give you health!"];

    public override ref Bool GemPickup =>
        ref memory.GetRef<Bool>(0x7C10);

    public override IReadOnlyList<string> InvisibleMessage { get; } =
        ["You are blocked", "by an invisible wall."];

    public override IReadOnlyList<string> KeyAlreadyMessage(int color) =>
        ["You already have a", $"{GetColorName(color)} key!"];

    public override IReadOnlyList<string> KeyPickupMessage(int color) =>
        ["You now have the", $"{GetColorName(color)} key."];

    public override IReadOnlyList<string> NoAmmoMessage { get; } =
        ["You don't have", "any ammo!"];

    public override IReadOnlyList<string> NoShootMessage { get; } =
        ["Can't shoot", "in this place!"];

    public override ref Bool NotDark =>
        ref _notDark;

    public override IReadOnlyList<string> NotDarkMessage { get; } =
        [];

    public override ref Bool NoTorches =>
        ref _noTorches;

    public override IReadOnlyList<string> NoTorchMessage { get; } =
        [];

    public override IReadOnlyList<string> OuchMessage { get; } =
        ["Ouch!"];

    public override ref Bool OutOfAmmo =>
        ref memory.GetRef<Bool>(0x7C0C);

    public override IReadOnlyList<string> StoneMessage { get; } =
        ["You have found a", "Stone of Power!"];

    public override IReadOnlyList<string> TimeMessage { get; } =
        ["Running out of time!"];

    public override IReadOnlyList<string> TorchMessage { get; } =
        [];

    public override ref Bool TorchPickup =>
        ref _torchPickup;

    public override IReadOnlyList<string> WaterMessage { get; } =
        ["Your way is", "blocked by lava."];
}