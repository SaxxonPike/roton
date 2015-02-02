using System.Drawing;
using System.Windows.Forms;

namespace Torch
{
    /// <summary>
    /// A panel designed to NOT automatically scroll to contained controls.
    /// </summary>
    public partial class ScrollPanel : Panel
    {
        public ScrollPanel()
        {
            InitializeComponent();
            this.HScroll = true;
            this.VScroll = true;
        }

        protected override Point ScrollToControl(Control activeControl)
        {
            return this.AutoScrollPosition;
        }
    }
}
