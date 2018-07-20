using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;

namespace Roton.Test.Infrastructure
{
    public class TestKeyboard : Keyboard
    {
        public void Type(KeyPress keyPress)
        {
            Enqueue(keyPress);
        }
    }
}