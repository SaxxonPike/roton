using System;
using System.Collections.Generic;
using Roton.Emulation.Colors;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalAlerts(
    IMemory memory, 
    IColorList colors,
    IFacts facts) 
    : Alerts
{
    private string GetColorName(int color) =>
        colors.Get(color)?.Name ?? string.Empty;

    public override IReadOnlyList<string> AmmoMessage =>
        [$"Ammunition - {facts.AmmoPerPickup} shots per container."];

    public override ref Bool AmmoPickup =>
        ref memory.GetRef<Bool>(0x4AAB);

    public override IReadOnlyList<string> BombMessage { get; } =
        ["Bomb activated!"];

    public override ref Bool CantShootHere =>
        ref memory.GetRef<Bool>(0x4AAD);

    public override ref Bool Dark =>
        ref memory.GetRef<Bool>(0x4AB1);

    public override IReadOnlyList<string> DarkMessage { get; } =
        ["Room is dark - you need to light a torch!"];

    public override IReadOnlyList<string> DoorLockedMessage(int color) =>
        [$"The {GetColorName(color)} door is locked!"];

    public override IReadOnlyList<string> DoorOpenMessage(int color) =>
        [$"The {GetColorName(color)} door is now open."];

    public override IReadOnlyList<string> EnergizerMessage { get; } =
        ["Energizer - You are invincible"];

    public override ref Bool EnergizerPickup =>
        ref memory.GetRef<Bool>(0x4AB5);

    public override IReadOnlyList<string> ErrorMessage(ReadOnlySpan<char> error) =>
        [$"ERR: {error.ToString()}"];

    public override IReadOnlyList<string> FakeMessage { get; } =
        ["A fake wall - secret passage!"];

    public override ref Bool FakeWall =>
        ref memory.GetRef<Bool>(0x4AB3);

    public override ref Bool Forest =>
        ref memory.GetRef<Bool>(0x4AB2);

    public override IReadOnlyList<string> ForestMessage { get; } =
        ["A path is cleared through the forest."];

    public override IReadOnlyList<string> GameOverMessage { get; } =
        ["Game over  -  Press ESCAPE"];

    public override IReadOnlyList<string> GemMessage { get; } =
        ["Gems give you health!"];

    public override ref Bool GemPickup =>
        ref memory.GetRef<Bool>(0x4AB4);

    public override IReadOnlyList<string> InvisibleMessage { get; } =
        ["You are blocked by an invisible wall."];

    public override IReadOnlyList<string> KeyAlreadyMessage(int color) =>
        [$"You already have a {GetColorName(color)} key!"];

    public override IReadOnlyList<string> KeyPickupMessage(int color) =>
        [$"You now have the {GetColorName(color)} key."];

    public override IReadOnlyList<string> NoAmmoMessage { get; } =
        ["You don't have any ammo!"];

    public override IReadOnlyList<string> NoShootMessage { get; } =
        ["Can't shoot in this place!"];

    public override ref Bool NotDark =>
        ref memory.GetRef<Bool>(0x4AB1);

    public override IReadOnlyList<string> NotDarkMessage { get; } =
        ["Don't need torch - room is not dark!"];

    public override ref Bool NoTorches =>
        ref memory.GetRef<Bool>(0x4AAF);

    public override IReadOnlyList<string> NoTorchMessage { get; } =
        ["You don't have any torches!"];

    public override IReadOnlyList<string> OuchMessage { get; } =
        ["Ouch!"];

    public override ref Bool OutOfAmmo =>
        ref memory.GetRef<Bool>(0x4AAC);

    public override IReadOnlyList<string> StoneMessage { get; } =
        [];

    public override IReadOnlyList<string> TimeMessage { get; } =
        ["Running out of time!"];

    public override IReadOnlyList<string> TorchMessage { get; } =
        ["Torch - used for lighting in the underground."];

    public override ref Bool TorchPickup =>
        ref memory.GetRef<Bool>(0x4AAE);

    public override IReadOnlyList<string> WaterMessage { get; } =
        ["Your way is blocked by water."];
}