using System.Linq;

namespace Roton.Emulation.Data.Impl;

public abstract class Flags(IMemory memory, int offset) : FixedStringSet(memory, offset, true), IFlags
{

    public string StoneText
    {
        get
        {
            foreach (var flag in this.Select(f => f.ToUpperInvariant()))
            {
                if (flag.Length > 0 && flag[0] == 'Z')
                {
                    return flag.Substring(1);
                }
            }

            return string.Empty;
        }
    }
}