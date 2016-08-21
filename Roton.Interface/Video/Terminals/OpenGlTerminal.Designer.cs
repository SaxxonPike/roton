using Roton.Interface.Synchronization;

namespace Roton.Interface.Video.Terminals {
    partial class OpenGlTerminal {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.timerDaemon = new TimerDaemon(this.components);
            this.displayTimer = new System.Windows.Forms.Timer(this.components);
            this.glControl = new OpenTK.GLControl();
            this.SuspendLayout();
            // 
            // timerDaemon
            // 
            this.timerDaemon.Paused = false;
            // 
            // displayTimer
            // 
            this.displayTimer.Interval = 1;
            this.displayTimer.Tick += new System.EventHandler(this.displayTimer_Tick);
            // 
            // glControl
            // 
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl.Location = new System.Drawing.Point(0, 0);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(640, 350);
            this.glControl.TabIndex = 0;
            this.glControl.VSync = false;
            this.glControl.Load += new System.EventHandler(this.glControl_Load);
            // 
            // OpenGlTerminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.glControl);
            this.Name = "OpenGlTerminal";
            this.Size = new System.Drawing.Size(640, 350);
            this.ResumeLayout(false);

        }

        #endregion

        private TimerDaemon timerDaemon;
        private System.Windows.Forms.Timer displayTimer;
        private OpenTK.GLControl glControl;
    }
}
