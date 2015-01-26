using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    abstract internal class SoundsBase
    {
        static public byte[] Silence = new byte[0];

        virtual public byte[] Ammo { get { return Silence; } }
        virtual public byte[] BombActivate { get { return Silence; } }
        virtual public byte[] BombExplode { get { return Silence; } }
        virtual public byte[] BombTick { get { return Silence; } }
        virtual public byte[] BombTock { get { return Silence; } }
        virtual public byte[] BulletDie { get { return Silence; } }
        virtual public byte[] Cheat { get { return Silence; } }
        virtual public byte[] DoorLocked { get { return Silence; } }
        virtual public byte[] DoorOpen { get { return Silence; } }
        virtual public byte[] Duplicate { get { return Silence; } }
        virtual public byte[] DuplicateFail { get { return Silence; } }
        virtual public byte[] EnemyDie { get { return Silence; } }
        virtual public byte[] EnemyShoot { get { return Silence; } }
        virtual public byte[] EnemySuicide { get { return Silence; } }
        virtual public byte[] Energizer { get { return Silence; } }
        virtual public byte[] EnergyOut { get { return Silence; } }
        virtual public byte[] Error { get { return Silence; } }
        virtual public byte[] Forest { get { return Silence; } }
        virtual public byte[] GameOver { get { return Silence; } }
        virtual public byte[] Gem { get { return Silence; } }
        virtual public byte[] Invisible { get { return Silence; } }
        virtual public byte[] Key { get { return Silence; } }
        virtual public byte[] KeyAlready { get { return Silence; } }
        virtual public byte[] Ouch { get { return Silence; } }
        virtual public byte[] Passage { get { return Silence; } }
        virtual public byte[] Push { get { return Silence; } }
        virtual public byte[] Ricochet { get { return Silence; } }
        virtual public byte[] Shoot { get { return Silence; } }
        virtual public byte[] SlimeDie { get { return Silence; } }
        virtual public byte[] TimeLow { get { return Silence; } }
        virtual public byte[] TimeOut { get { return Silence; } }
        virtual public byte[] Torch { get { return Silence; } }
        virtual public byte[] TorchOut { get { return Silence; } }
        virtual public byte[] Transporter { get { return Silence; } }
        virtual public byte[] Water { get { return Silence; } }
    }
}
