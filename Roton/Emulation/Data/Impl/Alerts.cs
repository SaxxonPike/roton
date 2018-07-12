namespace Roton.Emulation.Data.Impl
{
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
        public abstract bool AmmoPickup { get; set; }
        public abstract bool CantShootHere { get; set; }
        public abstract bool Dark { get; set; }
        public abstract bool EnergizerPickup { get; set; }
        public abstract bool FakeWall { get; set; }
        public abstract bool Forest { get; set; }
        public abstract bool GemPickup { get; set; }
        public abstract bool NotDark { get; set; }
        public abstract bool NoTorches { get; set; }
        public abstract bool OutOfAmmo { get; set; }
        public abstract bool TorchPickup { get; set; }
        public abstract IMessage DoorLockedMessage(int color);
        public abstract IMessage DoorOpenMessage(int color);
        public abstract IMessage ErrorMessage(string error);
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
    }
}