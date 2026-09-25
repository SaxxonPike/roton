using System;
using System.Collections.Generic;
using Roton.Emulation.Colors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperAlerts(
    IMemory memory,
    IColorList colors,
    IFacts facts)
    : IAlerts
{
    private Bool _dark;
    private Bool _notDark;
    private Bool _noTorches;
    private Bool _torchPickup;

    private string GetColorName(int color) =>
        colors.Get(color)?.Name ?? string.Empty;

    public IReadOnlyList<string> AmmoMessage =>
        ["Ammunition:", $"{facts.AmmoPerPickup} shots"];

    public ref Bool AmmoPickup =>
        ref memory.GetRef<Bool>(0x7C0B);

    public IReadOnlyList<string> BombMessage { get; } =
        ["Bomb activated!"];

    public ref Bool CantShootHere =>
        ref memory.GetRef<Bool>(0x7C0D);

    public ref Bool Dark =>
        ref _dark;

    public IReadOnlyList<string> DarkMessage { get; } =
        [];

    public IReadOnlyList<string> DoorLockedMessage(int color) =>
        [$"The {GetColorName(color)} door", "is locked!"];

    public IReadOnlyList<string> DoorOpenMessage(int color) =>
        [$"The {GetColorName(color)} door", "is now open."];

    public IReadOnlyList<string> EnergizerMessage { get; } =
        ["Shield:", "You are invincible"];

    public ref Bool EnergizerPickup =>
        ref memory.GetRef<Bool>(0x7C11);

    public IReadOnlyList<string> ErrorMessage(ReadOnlySpan<char> error) =>
        [$"ERR: {error.ToString()}"];

    public IReadOnlyList<string> FakeMessage { get; } =
        ["A fake wall:", "secret passage!"];

    public ref Bool FakeWall =>
        ref memory.GetRef<Bool>(0x7C0F);

    public ref Bool Forest =>
        ref memory.GetRef<Bool>(0x7C0E);

    public IReadOnlyList<string> ForestMessage { get; } =
        ["A path is cleared", "through the forest."];

    public IReadOnlyList<string> GameOverMessage { get; } =
        ["Game over", "-- Press ESCAPE --"];

    public IReadOnlyList<string> GemMessage { get; } =
        ["Gems give you health!"];

    public ref Bool GemPickup =>
        ref memory.GetRef<Bool>(0x7C10);

    public IReadOnlyList<string> InvisibleMessage { get; } =
        ["You are blocked", "by an invisible wall."];

    public IReadOnlyList<string> KeyAlreadyMessage(int color) =>
        ["You already have a", $"{GetColorName(color)} key!"];

    public IReadOnlyList<string> KeyPickupMessage(int color) =>
        ["You now have the", $"{GetColorName(color)} key."];

    public IReadOnlyList<string> NoAmmoMessage { get; } =
        ["You don't have", "any ammo!"];

    public IReadOnlyList<string> NoShootMessage { get; } =
        ["Can't shoot", "in this place!"];

    public ref Bool NotDark =>
        ref _notDark;

    public IReadOnlyList<string> NotDarkMessage { get; } =
        [];

    public ref Bool NoTorches =>
        ref _noTorches;

    public IReadOnlyList<string> NoTorchMessage { get; } =
        [];

    public IReadOnlyList<string> OuchMessage { get; } =
        ["Ouch!"];

    public ref Bool OutOfAmmo =>
        ref memory.GetRef<Bool>(0x7C0C);

    public IReadOnlyList<string> StoneMessage { get; } =
        ["You have found a", "Stone of Power!"];

    public IReadOnlyList<string> TimeMessage { get; } =
        ["Running out of time!"];

    public IReadOnlyList<string> TorchMessage { get; } =
        [];

    public ref Bool TorchPickup =>
        ref _torchPickup;

    public IReadOnlyList<string> WaterMessage { get; } =
        ["Your way is", "blocked by lava."];

    public void Reset()
    {
        AmmoPickup = true;
        Dark = true;
        EnergizerPickup = true;
        FakeWall = true;
        Forest = true;
        GemPickup = true;
        OutOfAmmo = true;
        CantShootHere = true;
        NotDark = true;
        NoTorches = true;
        TorchPickup = true;
    }

    public void SetAll()
    {
        AmmoPickup = false;
        Dark = false;
        EnergizerPickup = false;
        FakeWall = false;
        Forest = false;
        GemPickup = false;
        OutOfAmmo = false;
        CantShootHere = false;
        NotDark = false;
        NoTorches = false;
        TorchPickup = false;
    }
}