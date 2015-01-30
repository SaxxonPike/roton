namespace Lyon
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (this.Context is Roton.Context)
            {
                this.Context.Stop();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            Roton.Windows.Font font1 = new Roton.Windows.Font();
            Roton.Windows.Palette palette1 = new Roton.Windows.Palette();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton3 = new System.Windows.Forms.ToolStripDropDownButton();
            this.openWorldMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveWorldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.restartCoreMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartZZTMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartSuperZZTMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpRAMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.scale1xMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scale2xMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scale3xMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speaker = new Roton.Windows.Speaker(this.components);
            this.mainPanel = new System.Windows.Forms.Panel();
            this.terminal = new Roton.OpenGL.Terminal();
            this.toolStrip1.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton3,
            this.toolStripDropDownButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(330, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton3
            // 
            this.toolStripDropDownButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openWorldMenuItem,
            this.saveWorldToolStripMenuItem,
            this.toolStripMenuItem2,
            this.restartCoreMenuItem,
            this.restartZZTMenuItem,
            this.restartSuperZZTMenuItem,
            this.dumpRAMToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitMenuItem});
            this.toolStripDropDownButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton3.Image")));
            this.toolStripDropDownButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            this.toolStripDropDownButton3.Size = new System.Drawing.Size(67, 22);
            this.toolStripDropDownButton3.Text = "&Game";
            // 
            // openWorldMenuItem
            // 
            this.openWorldMenuItem.Name = "openWorldMenuItem";
            this.openWorldMenuItem.Size = new System.Drawing.Size(214, 22);
            this.openWorldMenuItem.Text = "&Open World...";
            // 
            // saveWorldToolStripMenuItem
            // 
            this.saveWorldToolStripMenuItem.Name = "saveWorldToolStripMenuItem";
            this.saveWorldToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.saveWorldToolStripMenuItem.Text = "&Save World...";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(211, 6);
            // 
            // restartCoreMenuItem
            // 
            this.restartCoreMenuItem.Name = "restartCoreMenuItem";
            this.restartCoreMenuItem.Size = new System.Drawing.Size(214, 22);
            this.restartCoreMenuItem.Text = "&Restart Core";
            // 
            // restartZZTMenuItem
            // 
            this.restartZZTMenuItem.Name = "restartZZTMenuItem";
            this.restartZZTMenuItem.Size = new System.Drawing.Size(214, 22);
            this.restartZZTMenuItem.Text = "Restart in &ZZT mode";
            // 
            // restartSuperZZTMenuItem
            // 
            this.restartSuperZZTMenuItem.Name = "restartSuperZZTMenuItem";
            this.restartSuperZZTMenuItem.Size = new System.Drawing.Size(214, 22);
            this.restartSuperZZTMenuItem.Text = "Restart in &Super ZZT mode";
            // 
            // dumpRAMToolStripMenuItem
            // 
            this.dumpRAMToolStripMenuItem.Name = "dumpRAMToolStripMenuItem";
            this.dumpRAMToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.dumpRAMToolStripMenuItem.Text = "&Dump RAM...";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(211, 6);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(214, 22);
            this.exitMenuItem.Text = "E&xit";
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scale1xMenuItem,
            this.scale2xMenuItem,
            this.scale3xMenuItem});
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(61, 22);
            this.toolStripDropDownButton2.Text = "&View";
            // 
            // scale1xMenuItem
            // 
            this.scale1xMenuItem.Name = "scale1xMenuItem";
            this.scale1xMenuItem.Size = new System.Drawing.Size(115, 22);
            this.scale1xMenuItem.Text = "Scale &1x";
            // 
            // scale2xMenuItem
            // 
            this.scale2xMenuItem.Name = "scale2xMenuItem";
            this.scale2xMenuItem.Size = new System.Drawing.Size(115, 22);
            this.scale2xMenuItem.Text = "Scale &2x";
            // 
            // scale3xMenuItem
            // 
            this.scale3xMenuItem.Name = "scale3xMenuItem";
            this.scale3xMenuItem.Size = new System.Drawing.Size(115, 22);
            this.scale3xMenuItem.Text = "Scale &3x";
            // 
            // mainPanel
            // 
            this.mainPanel.AutoSize = true;
            this.mainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainPanel.Controls.Add(this.terminal);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 25);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(330, 275);
            this.mainPanel.TabIndex = 1;
            // 
            // terminal
            // 
            this.terminal.BackColor = System.Drawing.Color.Black;
            this.terminal.Location = new System.Drawing.Point(0, 0);
            this.terminal.Name = "terminal";
            this.terminal.Size = new System.Drawing.Size(640, 350);
            this.terminal.TabIndex = 0;
            this.terminal.AutoSize = true;
            this.terminal.TerminalFont = font1;
            this.terminal.TerminalPalette = palette1;
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(330, 300);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.toolStrip1);
            this.Name = "GameForm";
            this.Text = "Lyon";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton3;
        private System.Windows.Forms.ToolStripMenuItem openWorldMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem restartCoreMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartZZTMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartSuperZZTMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem scale1xMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale2xMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale3xMenuItem;
        private Roton.Windows.Speaker speaker;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.ToolStripMenuItem dumpRAMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveWorldToolStripMenuItem;
        private Roton.OpenGL.Terminal terminal;
    }
}

