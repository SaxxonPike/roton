namespace Roton.Core
{
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
        bool AmmoPickup { get; set; }
        bool CantShootHere { get; set; }
        bool Dark { get; set; }
        bool EnergizerPickup { get; set; }
        bool FakeWall { get; set; }
        bool Forest { get; set; }
        bool GemPickup { get; set; }
        bool NotDark { get; set; }
        bool NoTorches { get; set; }
        bool OutOfAmmo { get; set; }
        bool TorchPickup { get; set; }

        IMessage DoorLockedMessage(int color);
        IMessage DoorOpenMessage(int color);
        IMessage ErrorMessage(string error);
        IMessage KeyAlreadyMessage(int color);
        IMessage KeyPickupMessage(int color);
    }
}