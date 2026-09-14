using System;
using System.Collections.Generic;

namespace Roton.Emulation.Data.Impl;

public abstract class Alerts : IAlerts
{
    public abstract IReadOnlyList<string> AmmoMessage { get; }
    public abstract IReadOnlyList<string> BombMessage { get; }
    public abstract IReadOnlyList<string> DarkMessage { get; }
    public abstract IReadOnlyList<string> EnergizerMessage { get; }
    public abstract IReadOnlyList<string> FakeMessage { get; }
    public abstract IReadOnlyList<string> ForestMessage { get; }
    public abstract IReadOnlyList<string> GameOverMessage { get; }
    public abstract IReadOnlyList<string> GemMessage { get; }
    public abstract IReadOnlyList<string> InvisibleMessage { get; }
    public abstract IReadOnlyList<string> NoAmmoMessage { get; }
    public abstract IReadOnlyList<string> NoShootMessage { get; }
    public abstract IReadOnlyList<string> NotDarkMessage { get; }
    public abstract IReadOnlyList<string> NoTorchMessage { get; }
    public abstract IReadOnlyList<string> OuchMessage { get; }
    public abstract IReadOnlyList<string> StoneMessage { get; }
    public abstract IReadOnlyList<string> TimeMessage { get; }
    public abstract IReadOnlyList<string> TorchMessage { get; }
    public abstract IReadOnlyList<string> WaterMessage { get; }
    public abstract ref Bool AmmoPickup { get; }
    public abstract ref Bool CantShootHere { get; }
    public abstract ref Bool Dark { get; }
    public abstract ref Bool EnergizerPickup { get; }
    public abstract ref Bool FakeWall { get; }
    public abstract ref Bool Forest { get; }
    public abstract ref Bool GemPickup { get; }
    public abstract ref Bool NotDark { get; }
    public abstract ref Bool NoTorches { get; }
    public abstract ref Bool OutOfAmmo { get; }
    public abstract ref Bool TorchPickup { get; }
    public abstract IReadOnlyList<string> DoorLockedMessage(int color);
    public abstract IReadOnlyList<string> DoorOpenMessage(int color);
    public abstract IReadOnlyList<string> ErrorMessage(ReadOnlySpan<char> error);
    public abstract IReadOnlyList<string> KeyAlreadyMessage(int color);
    public abstract IReadOnlyList<string> KeyPickupMessage(int color);

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