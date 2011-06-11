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
			this.target = new AgateLib.WinForms.AgateRenderTarget();
			this.SuspendLayout();
			// 
			// sbRight
			// 
			this.sbRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.sbRight.Location = new System.Drawing.Point(0, 225);
			this.sbRight.Maximum = 32767;
			this.sbRight.Name = "sbRight";
			this.sbRight.Size = new System.Drawing.Size(222, 17);
			this.sbRight.TabIndex = 31;
			this.sbRight.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sbRight_Scroll);
			// 
			// sbDown
			// 
			this.sbDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.sbDown.Location = new System.Drawing.Point(222, 0);
			this.sbDown.Maximum = 32767;
			this.sbDown.Name = "sbDown";
			this.sbDown.Size = new System.Drawing.Size(17, 225);
			this.sbDown.TabIndex = 32;
			this.sbDown.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sbDown_Scroll);
			// 
			// target
			// 
			this.target.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.target.Location = new System.Drawing.Point(0, 0);
			this.target.Name = "target";
			this.target.Size = new System.Drawing.Size(222, 225);
			this.target.TabIndex = 33;
			this.target.Paint += new System.Windows.Forms.PaintEventHandler(this.target_Paint);
			this.target.MouseMove += new System.Windows.Forms.MouseEventHandler(this.target_MouseMove);
			this.target.MouseDown += new System.Windows.Forms.MouseEventHandler(this.target_MouseDown);
			this.target.MouseUp += new System.Windows.Forms.MouseEventHandler(this.target_MouseUp);
			// 
			// XleMapView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.target);
			this.Controls.Add(this.sbRight);
			this.Controls.Add(this.sbDown);
			this.Name = "XleMapView";
			this.Size = new System.Drawing.Size(239, 242);
			this.Resize += new System.EventHandler(this.XleMapView_Resize);
			this.ResumeLayout(false);

		}

		#endregion

		internal System.Windows.Forms.HScrollBar sbRight;
		internal System.Windows.Forms.VScrollBar sbDown;
		private AgateLib.WinForms.AgateRenderTarget target;

	}
}
