namespace XleMapEditor
{
	partial class TilePicker
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
			this.target = new AgateLib.WinForms.AgateRenderTarget();
			this.sbDown = new System.Windows.Forms.VScrollBar();
			this.SuspendLayout();
			// 
			// target
			// 
			this.target.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.target.Location = new System.Drawing.Point(0, 0);
			this.target.Name = "target";
			this.target.Size = new System.Drawing.Size(107, 188);
			this.target.TabIndex = 0;
			this.target.Paint += new System.Windows.Forms.PaintEventHandler(this.target_Paint);
			this.target.MouseDown += new System.Windows.Forms.MouseEventHandler(this.target_MouseDown);
			// 
			// sbDown
			// 
			this.sbDown.Dock = System.Windows.Forms.DockStyle.Right;
			this.sbDown.Location = new System.Drawing.Point(110, 0);
			this.sbDown.Name = "sbDown";
			this.sbDown.Size = new System.Drawing.Size(17, 188);
			this.sbDown.TabIndex = 0;
			this.sbDown.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sbDown_Scroll);
			// 
			// TilePicker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.sbDown);
			this.Controls.Add(this.target);
			this.Name = "TilePicker";
			this.Size = new System.Drawing.Size(127, 188);
			this.ResumeLayout(false);

		}

		#endregion

		private AgateLib.WinForms.AgateRenderTarget target;
		private System.Windows.Forms.VScrollBar sbDown;
	}
}
