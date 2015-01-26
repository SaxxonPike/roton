using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    internal class SoundsCommon : SoundsBase
    {
        public override byte[] Ammo
        {
            get
            {
                return new byte[]
                {
                    0x30, 0x01,
                    0x31, 0x01,
                    0x32, 0x01
                };
            }
        }

        public override byte[] BombActivate
        {
            get
            {
                return new byte[]
                {
                    0x30, 0x01,
                    0x35, 0x01,
                    0x40, 0x01,
                    0x45, 0x01,
                    0x50, 0x01
                };
            }
        }

        public override byte[] BombExplode
        {
            get
            {
                return new byte[]
                {
                    0x60, 0x01,
                    0x50, 0x01,
                    0x40, 0x01,
                    0x30, 0x01,
                    0x20, 0x01
                };
            }
        }

        public override byte[] BombTick
        {
            get
            {
                return new byte[]
                {
                    0xF8, 0x01
                };
            }
        }

        public override byte[] BombTock
        {
            get
            {
                return new byte[]
                {
                    0xF5, 0x01
                };
            }
        }

        public override byte[] BulletDie
        {
            get
            {
                return new byte[]
                {
                    0x20, 0x01
                };
            }
        }

        public override byte[] Cheat
        {
            get
            {
                return new byte[]
                {
                    0x27, 0x04
                };
            }
        }

        public override byte[] DoorLocked
        {
            get
            {
                return new byte[]
                {
                    0x17, 0x01,
                    0x10, 0x01
                };
            }
        }

        public override byte[] DoorOpen
        {
            get
            {
                return new byte[]
                {
                    0x30, 0x01,
                    0x37, 0x01,
                    0x3B, 0x01,
                    0x30, 0x01,
                    0x37, 0x01,
                    0x3B, 0x01,
                    0x40, 0x04
                };
            }
        }

        public override byte[] Duplicate
        {
            get
            {
                return new byte[]
                {
                    0x30, 0x02,
                    0x32, 0x02,
                    0x34, 0x02,
                    0x35, 0x02,
                    0x37, 0x02
                };
            }
        }

        public override byte[] DuplicateFail
        {
            get
            {
                return new byte[]
                {
                    0x18, 0x01,
                    0x16, 0x01
                };
            }
        }

        public override byte[] EnemyDie
        {
            get
            {
                return new byte[]
                {
                    0x40, 0x01,
                    0x10, 0x01,
                    0x50, 0x01,
                    0x30, 0x01
                };
            }
        }

        public override byte[] EnemyShoot
        {
            get
            {
                return new byte[]
                {
                    0x30, 0x01,
                    0x26, 0x01
                };
            }
        }

        public override byte[] EnemySuicide
        {
            get
            {
                return new byte[]
                {
                    0x10, 0x01
                };
            }
        }

        public override byte[] Energizer
        {
            get
            {
                return new byte[]
                {
                    0x20, 0x03,

                    0x23, 0x03,
                    0x24, 0x03,
                    0x25, 0x03,
                    0x35, 0x03,
                    0x25, 0x03,
                    0x23, 0x03,
                    0x20, 0x03,
                    0x30, 0x03,

                    0x23, 0x03,
                    0x24, 0x03,
                    0x25, 0x03,
                    0x35, 0x03,
                    0x25, 0x03,
                    0x23, 0x03,
                    0x20, 0x03,
                    0x30, 0x03,

                    0x23, 0x03,
                    0x24, 0x03,
                    0x25, 0x03,
                    0x35, 0x03,
                    0x25, 0x03,
                    0x23, 0x03,
                    0x20, 0x03,
                    0x30, 0x03,

                    0x23, 0x03,
                    0x24, 0x03,
                    0x25, 0x03,
                    0x35, 0x03,
                    0x25, 0x03,
                    0x23, 0x03,
                    0x20, 0x03,
                    0x30, 0x03,

                    0x23, 0x03,
                    0x24, 0x03,
                    0x25, 0x03,
                    0x35, 0x03,
                    0x25, 0x03,
                    0x23, 0x03,
                    0x20, 0x03,
                    0x30, 0x03,

                    0x23, 0x03,
                    0x24, 0x03,
                    0x25, 0x03,
                    0x35, 0x03,
                    0x25, 0x03,
                    0x23, 0x03,
                    0x20, 0x03,
                    0x30, 0x03,

                    0x23, 0x03,
                    0x24, 0x03,
                    0x25, 0x03,
                    0x35, 0x03,
                    0x25, 0x03,
                    0x23, 0x03,
                    0x20, 0x03
                };
            }
        }

        public override byte[] EnergyOut
        {
            get
            {
                return new byte[]
                {
                    0x20, 0x03,
                    0x1A, 0x03,
                    0x17, 0x03,
                    0x16, 0x03,
                    0x15, 0x03,
                    0x13, 0x03,
                    0x10, 0x03
                };
            }
        }

        public override byte[] Error
        {
            get
            {
                return new byte[]
                {
                    0x50, 0x0A
                };
            }
        }

        public override byte[] Forest
        {
            get
            {
                return new byte[]
                {
                    0x39, 0x01
                };
            }
        }

        public override byte[] GameOver
        {
            get
            {
                return new byte[]
                {
                    0x20, 0x03,
                    0x23, 0x03,
                    0x27, 0x03,
                    0x30, 0x03,
                    0x27, 0x03,
                    0x2A, 0x03,
                    0x32, 0x03,
                    0x37, 0x03,
                    0x35, 0x03,
                    0x38, 0x03,
                    0x40, 0x03,
                    0x45, 0x03,
                    0x10, 0x0A
                };
            }
        }

        public override byte[] Gem
        {
            get
            {
                return new byte[]
                {
                    0x40, 0x01,
                    0x37, 0x01,
                    0x34, 0x01,
                    0x30, 0x01
                };
            }
        }

        public override byte[] Invisible
        {
            get
            {
                return new byte[]
                {
                    0x12, 0x01,
                    0x10, 0x01
                };
            }
        }

        public override byte[] Key
        {
            get
            {
                return new byte[]
                {
                    0x40, 0x01,
                    0x44, 0x01,
                    0x47, 0x01,
                    0x40, 0x01,
                    0x44, 0x01,
                    0x47, 0x01,
                    0x40, 0x01,
                    0x44, 0x01,
                    0x47, 0x01,
                    0x50, 0x02
                };
            }
        }

        public override byte[] KeyAlready
        {
            get
            {
                return new byte[]
                {
                    0x30, 0x02,
                    0x20, 0x02
                };
            }
        }

        public override byte[] Ouch
        {
            get
            {
                return new byte[]
                {
                    0x10, 0x01,
                    0x20, 0x01,
                    0x13, 0x01,
                    0x23, 0x01
                };
            }
        }

        public override byte[] Passage
        {
            get
            {
                return new byte[]
                {
                    0x30, 0x01,
                    0x34, 0x01,
                    0x37, 0x01,
                    0x31, 0x01,
                    0x35, 0x01,
                    0x38, 0x01,
                    0x32, 0x01,
                    0x36, 0x01,
                    0x39, 0x01,
                    0x33, 0x01,
                    0x37, 0x01,
                    0x3A, 0x01,
                    0x34, 0x01,
                    0x38, 0x01,
                    0x40, 0x01
                };
            }
        }

        public override byte[] Push
        {
            get
            {
                return new byte[]
                {
                    0x15, 0x01
                };
            }
        }

        public override byte[] Ricochet
        {
            get
            {
                return new byte[]
                {
                    0xF9, 0x01
                };
            }
        }

        public override byte[] Shoot
        {
            get
            {
                return new byte[]
                {
                    0x40, 0x01,
                    0x30, 0x01,
                    0x20, 0x01
                };
            }
        }

        public override byte[] SlimeDie
        {
            get
            {
                return new byte[]
                {
                    0x20, 0x01,
                    0x23, 0x01
                };
            }
        }

        public override byte[] TimeLow
        {
            get
            {
                return new byte[]
                {
                    0x40, 0x06,
                    0x45, 0x06,
                    0x40, 0x06,
                    0x35, 0x06,
                    0x40, 0x06,
                    0x45, 0x06,
                    0x40, 0x0A
                };
            }
        }

        public override byte[] TimeOut
        {
            get
            {
                return new byte[]
                {
                    0x20, 0x01,
                    0x23, 0x01,
                    0x27, 0x01,
                    0x30, 0x01,
                    0x10, 0x01
                };
            }
        }

        public override byte[] Torch
        {
            get
            {
                return new byte[]
                {
                    0x30, 0x01,
                    0x39, 0x01,
                    0x34, 0x02
                };
            }
        }

        public override byte[] TorchOut
        {
            get
            {
                return new byte[]
                {
                    0x30, 0x01,
                    0x20, 0x01,
                    0x10, 0x01
                };
            }
        }

        public override byte[] Transporter
        {
            get
            {
                return new byte[]
                {
                    0x30, 0x01,
                    0x42, 0x01,
                    0x34, 0x01,
                    0x46, 0x01,
                    0x38, 0x01,
                    0x4A, 0x01,
                    0x40, 0x01,
                    0x52, 0x01
                };
            }
        }

        public override byte[] Water
        {
            get
            {
                return new byte[]
                {
                    0x40, 0x01,
                    0x50, 0x01
                };
            }
        }
    }
}
