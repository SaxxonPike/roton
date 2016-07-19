using System.Windows.Forms;

namespace Torch
{
    public static class ToolStripUtility
    {
        public static ToolStripItem[] ToArray(this ToolStripItemCollection collection)
        {
            var result = new ToolStripItem[collection.Count];
            collection.CopyTo(result, 0);
            return result;
        }
    }
}