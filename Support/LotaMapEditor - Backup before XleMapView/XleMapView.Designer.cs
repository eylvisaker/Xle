namespace XleMapEditor
{
    partial class XleMapView
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
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sbRight = new System.Windows.Forms.HScrollBar();
            this.sbDown = new System.Windows.Forms.VScrollBar();
            this.agateRenderTarget1 = new ERY.AgateLib.WinForms.AgateRenderTarget();
            this.SuspendLayout();
            // 
            // sbRight
            // 
            this.sbRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sbRight.Location = new System.Drawing.Point(0, 219);
            this.sbRight.Maximum = 32767;
            this.sbRight.Name = "sbRight";
            this.sbRight.Size = new System.Drawing.Size(216, 17);
            this.sbRight.TabIndex = 31;
            // 
            // sbDown
            // 
            this.sbDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sbDown.Location = new System.Drawing.Point(216, 0);
            this.sbDown.Maximum = 32767;
            this.sbDown.Name = "sbDown";
            this.sbDown.Size = new System.Drawing.Size(17, 219);
            this.sbDown.TabIndex = 32;
            // 
            // agateRenderTarget1
            // 
            this.agateRenderTarget1.Location = new System.Drawing.Point(0, 0);
            this.agateRenderTarget1.Name = "agateRenderTarget1";
            this.agateRenderTarget1.Size = new System.Drawing.Size(216, 219);
            this.agateRenderTarget1.TabIndex = 33;
            // 
            // XleMapView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.agateRenderTarget1);
            this.Controls.Add(this.sbRight);
            this.Controls.Add(this.sbDown);
            this.Name = "XleMapView";
            this.Size = new System.Drawing.Size(233, 236);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.HScrollBar sbRight;
        internal System.Windows.Forms.VScrollBar sbDown;
        private ERY.AgateLib.WinForms.AgateRenderTarget agateRenderTarget1;

    }
}
