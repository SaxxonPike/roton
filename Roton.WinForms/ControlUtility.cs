using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Roton.WinForms
{
    public static class ControlUtility
    {
        public static IList<Control> GetAllChildren(this Control parent)
        {
            var result = new List<Control>();
            foreach (Control control in parent.Controls)
            {
                result.Add(control);
                result.AddRange(control.GetAllChildren());
            }
            return result;
        }

        public static void ResumeAllLayout(this Control parent)
        {
            foreach (var control in parent.GetAllChildren().Reverse())
            {
                control.ResumeLayout();
            }
            parent.ResumeLayout();
        }

        public static void SuspendAllLayout(this Control parent)
        {
            parent.SuspendLayout();
            foreach (var control in parent.GetAllChildren())
            {
                control.SuspendLayout();
            }
        }
    }
}