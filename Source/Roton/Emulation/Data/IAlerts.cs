using System;

namespace Roton.Emulation.Data;

public interface IAlerts
{
    IMessage AmmoMessage { get; }
    IMessage BombMessage { get; }
    IMessage DarkMessage { get; }
    IMessage EnergizerMessage { get; }
    IMessage FakeMessage { get; }
    IMessage ForestMessage { get; }
    IMessage GameOverMessage { get; }
    IMessage GemMessage { get; }
    IMessage InvisibleMessage { get; }
    IMessage NoAmmoMessage { get; }
    IMessage NoShootMessage { get; }
    IMessage NotDarkMessage { get; }
    IMessage NoTorchMessage { get; }
    IMessage OuchMessage { get; }
    IMessage StoneMessage { get; }
    IMessage TimeMessage { get; }
    IMessage TorchMessage { get; }
    IMessage WaterMessage { get; }
    ref Bool AmmoPickup { get; }
    ref Bool CantShootHere { get; }
    ref Bool Dark { get; }
    ref Bool EnergizerPickup { get; }
    ref Bool FakeWall { get; }
    ref Bool Forest { get; }
    ref Bool GemPickup { get; }
    ref Bool NotDark { get; }
    ref Bool NoTorches { get; }
    ref Bool OutOfAmmo { get; }
    ref Bool TorchPickup { get; }

    IMessage DoorLockedMessage(int color);
    IMessage DoorOpenMessage(int color);
    IMessage ErrorMessage(ReadOnlySpan<char> error);
    IMessage KeyAlreadyMessage(int color);
    IMessage KeyPickupMessage(int color);

    void Reset();
    void SetAll();
}