using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    internal class SuperZztAlerts : IAlerts
    {
        private readonly IMemory _memory;

        public SuperZztAlerts(IMemory memory)
        {
            _memory = memory;
        }

        public bool AlertAmmo
        {
            get { return _memory.ReadBool(0x7C0B); }
            set { _memory.WriteBool(0x7C0B, value); }
        }

        public bool AlertDark
        {
            get { return false; }
            set { }
        }

        public bool AlertEnergy
        {
            get { return _memory.ReadBool(0x7C11); }
            set { _memory.WriteBool(0x7C11, value); }
        }

        public bool AlertFake
        {
            get { return _memory.ReadBool(0x7C0F); }
            set { _memory.WriteBool(0x7C0F, value); }
        }

        public bool AlertForest
        {
            get { return _memory.ReadBool(0x7C0E); }
            set { _memory.WriteBool(0x7C0E, value); }
        }

        public bool AlertGem
        {
            get { return _memory.ReadBool(0x7C10); }
            set { _memory.WriteBool(0x7C10, value); }
        }

        public bool AlertNoAmmo
        {
            get { return _memory.ReadBool(0x7C0C); }
            set { _memory.WriteBool(0x7C0C, value); }
        }

        public bool AlertNoShoot
        {
            get { return _memory.ReadBool(0x7C0D); }
            set { _memory.WriteBool(0x7C0D, value); }
        }

        public bool AlertNotDark
        {
            get { return false; }
            set { }
        }

        public bool AlertNoTorch
        {
            get { return false; }
            set { }
        }

        public bool AlertTorch
        {
            get { return false; }
            set { }
        }
    }
}
