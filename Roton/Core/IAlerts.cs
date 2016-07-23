namespace Roton.Core
{
    public interface IAlerts
    {
        string AmmoMessage { get; }
        string BombMessage { get; }
        string DarkMessage { get; }
        string EnergizerMessage { get; }
        string FakeMessage { get; }
        string ForestMessage { get; }
        string GameOverMessage { get; }
        string GemMessage { get; }
        string InvisibleMessage { get; }
        string NoAmmoMessage { get; }
        string NoShootMessage { get; }
        string NotDarkMessage { get; }
        string NoTorchMessage { get; }
        string StoneMessage { get; }
        string TimeMessage { get; }
        string TorchMessage { get; }
        string WaterMessage { get; }
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

        string DoorLockedMessage(int color);
        string DoorOpenMessage(int color);
        string KeyAleadyMessage(int color);
        string KeyPickupMessage(int color);
    }
}