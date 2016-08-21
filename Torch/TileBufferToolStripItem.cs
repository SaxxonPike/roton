using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Roton.Interface.Video;
using Roton.Interface.Video.Terminals;

namespace Torch
{
    public partial class TileBufferToolStripItem : ToolStripButton
    {
        public TileBufferToolStripItem()
        {
            InitializeComponent();
            DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            ImageScaling = ToolStripItemImageScaling.None;
            TextAlign = ContentAlignment.MiddleLeft;
            TextImageRelation = TextImageRelation.TextBeforeImage;
            Text = "Buffer";
            AutoSize = true;
        }

        public TileBufferToolStripItem(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public OpenGlTerminal OpenGlTerminal { get; set; }
    }
}