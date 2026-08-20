using System;

namespace Roton.Emulation.Data.Impl;

public abstract class Alerts : IAlerts
{
    public abstract IMessage AmmoMessage { get; }
    public abstract IMessage BombMessage { get; }
    public abstract IMessage DarkMessage { get; }
    public abstract IMessage EnergizerMessage { get; }
    public abstract IMessage FakeMessage { get; }
    public abstract IMessage ForestMessage { get; }
    public abstract IMessage GameOverMessage { get; }
    public abstract IMessage GemMessage { get; }
    public abstract IMessage InvisibleMessage { get; }
    public abstract IMessage NoAmmoMessage { get; }
    public abstract IMessage NoShootMessage { get; }
    public abstract IMessage NotDarkMessage { get; }
    public abstract IMessage NoTorchMessage { get; }
    public abstract IMessage OuchMessage { get; }
    public abstract IMessage StoneMessage { get; }
    public abstract IMessage TimeMessage { get; }
    public abstract IMessage TorchMessage { get; }
    public abstract IMessage WaterMessage { get; }
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
    public abstract IMessage DoorLockedMessage(int color);
    public abstract IMessage DoorOpenMessage(int color);
    public abstract IMessage ErrorMessage(ReadOnlySpan<char> error);
    public abstract IMessage KeyAlreadyMessage(int color);
    public abstract IMessage KeyPickupMessage(int color);

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