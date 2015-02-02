namespace Roton.Windows
{
    partial class Terminal
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerDaemon = new Roton.Windows.TimerDaemon(this.components);
            this.displayTimer = new System.Windows.Forms.Timer(this.components);
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
            // Terminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Name = "Terminal";
            this.Size = new System.Drawing.Size(640, 360);
            this.ResumeLayout(false);

        }

        #endregion

        private TimerDaemon timerDaemon;
        private System.Windows.Forms.Timer displayTimer;
    }
}
