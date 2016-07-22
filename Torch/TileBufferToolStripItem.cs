using Roton.WinForms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Roton.Core;

namespace Torch
{
    public partial class TileBufferToolStripItem : ToolStripButton
    {
        public TileBufferToolStripItem()
        {
            InitializeComponent();
            DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            ImageScaling = ToolStripItemImageScaling.None;
            TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            TextImageRelation = TextImageRelation.TextBeforeImage;
            Text = "Buffer";
            AutoSize = true;
        }

        public TileBufferToolStripItem(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public Terminal Terminal { get; set; }
    }
}