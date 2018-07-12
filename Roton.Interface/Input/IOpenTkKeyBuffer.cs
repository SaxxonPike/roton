using OpenTK.Input;
using Roton.Emulation.Data;

namespace Roton.Interface.Input
{
    public interface IOpenTkKeyBuffer : IKeyboard
    {
        new bool Alt { get; set; }
        new bool Control { get; set; }
        new bool Shift { get; set; }
        bool Press(char data);
        bool Press(Key data);
    }
}