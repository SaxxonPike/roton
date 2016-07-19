namespace Roton.Emulation
{
    internal abstract class SoundsBase
    {
        public static readonly byte[] Silence = new byte[0];

        public virtual byte[] Ammo => Silence;
        public virtual byte[] BombActivate => Silence;
        public virtual byte[] BombExplode => Silence;
        public virtual byte[] BombTick => Silence;
        public virtual byte[] BombTock => Silence;
        public virtual byte[] BulletDie => Silence;
        public virtual byte[] Cheat => Silence;
        public virtual byte[] DoorLocked => Silence;
        public virtual byte[] DoorOpen => Silence;
        public virtual byte[] Duplicate => Silence;
        public virtual byte[] DuplicateFail => Silence;
        public virtual byte[] EnemyDie => Silence;
        public virtual byte[] EnemyShoot => Silence;
        public virtual byte[] EnemySuicide => Silence;
        public virtual byte[] Energizer => Silence;
        public virtual byte[] EnergyOut => Silence;
        public virtual byte[] Error => Silence;
        public virtual byte[] Forest => Silence;
        public virtual byte[] GameOver => Silence;
        public virtual byte[] Gem => Silence;
        public virtual byte[] Invisible => Silence;
        public virtual byte[] Key => Silence;
        public virtual byte[] KeyAlready => Silence;
        public virtual byte[] Ouch => Silence;
        public virtual byte[] Passage => Silence;
        public virtual byte[] Push => Silence;
        public virtual byte[] Ricochet => Silence;
        public virtual byte[] Shoot => Silence;
        public virtual byte[] SlimeDie => Silence;
        public virtual byte[] TimeLow => Silence;
        public virtual byte[] TimeOut => Silence;
        public virtual byte[] Torch => Silence;
        public virtual byte[] TorchOut => Silence;
        public virtual byte[] Transporter => Silence;
        public virtual byte[] Water => Silence;
    }
}