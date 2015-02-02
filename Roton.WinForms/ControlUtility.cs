using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Roton.WinForms
{
    static public class ControlUtility
    {
        static public IList<Control> GetAllChildren(this Control parent)
        {
            List<Control> result = new List<Control>();
            foreach (Control control in parent.Controls)
            {
                result.Add(control);
                result.AddRange(control.GetAllChildren());
            }
            return result;
        }

        static public void ResumeAllLayout(this Control parent)
        {
            foreach (Control control in parent.GetAllChildren().Reverse())
            {
                control.ResumeLayout();
            }
            parent.ResumeLayout();
        }

        static public void SuspendAllLayout(this Control parent)
        {
            parent.SuspendLayout();
            foreach (Control control in parent.GetAllChildren())
            {
                control.SuspendLayout();
            }
        }
    }
}
