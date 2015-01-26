using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Torch
{
    static public class ToolStripUtility
    {
        static public ToolStripItem[] ToArray(this ToolStripItemCollection collection)
        {
            ToolStripItem[] result = new ToolStripItem[collection.Count];
            collection.CopyTo(result, 0);
            return result;
        }
    }
}
