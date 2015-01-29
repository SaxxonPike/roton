namespace Roton.OpenGL {
    partial class Terminal {
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
            this.timerDaemon = new Roton.Windows.TimerDaemon(this.components);
            this.SuspendLayout();
            // 
            // timerDaemon
            // 
            this.timerDaemon.Paused = false;
            // 
            // Terminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Name = "Terminal";
            this.Size = new System.Drawing.Size(640, 350);
            this.ResumeLayout(false);

        }

        #endregion

        private Windows.TimerDaemon timerDaemon;
    }
}
