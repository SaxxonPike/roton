using Roton.Emulation.Core.Impl;

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