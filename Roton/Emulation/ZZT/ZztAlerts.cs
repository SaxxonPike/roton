using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.ZZT
{
    internal class ZztAlerts : IAlerts
    {
        private readonly IMemory _memory;

        public ZztAlerts(IMemory memory)
        {
            _memory = memory;
        }

        public bool AlertAmmo
        {
            get { return _memory.ReadBool(0x4AAB); }
            set { _memory.WriteBool(0x4AAB, value); }
        }

        public bool AlertDark
        {
            get { return _memory.ReadBool(0x4AB1); }
            set { _memory.WriteBool(0x4AB1, value); }
        }

        public bool AlertEnergy
        {
            get { return _memory.ReadBool(0x4AB5); }
            set { _memory.WriteBool(0x4AB5, value); }
        }

        public bool AlertFake
        {
            get { return _memory.ReadBool(0x4AB3); }
            set { _memory.WriteBool(0x4AB3, value); }
        }

        public bool AlertForest
        {
            get { return _memory.ReadBool(0x4AB2); }
            set { _memory.WriteBool(0x4AB2, value); }
        }

        public bool AlertGem
        {
            get { return _memory.ReadBool(0x4AB4); }
            set { _memory.WriteBool(0x4AB4, value); }
        }

        public bool AlertNoAmmo
        {
            get { return _memory.ReadBool(0x4AAC); }
            set { _memory.WriteBool(0x4AAC, value); }
        }

        public bool AlertNoShoot
        {
            get { return _memory.ReadBool(0x4AAD); }
            set { _memory.WriteBool(0x4AAD, value); }
        }

        public bool AlertNotDark
        {
            get { return _memory.ReadBool(0x4AB1); }
            set { _memory.WriteBool(0x4AB1, value); }
        }

        public bool AlertNoTorch
        {
            get { return _memory.ReadBool(0x4AAF); }
            set { _memory.WriteBool(0x4AAF, value); }
        }

        public bool AlertTorch
        {
            get { return _memory.ReadBool(0x4AAE); }
            set { _memory.WriteBool(0x4AAE, value); }
        }
    }
}
