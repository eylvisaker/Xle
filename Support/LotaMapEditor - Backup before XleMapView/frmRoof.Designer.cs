

namespace XleMapEditor
{
	public partial class frmRoof
	{
		#region "Windows Form Designer generated code "
		[System.Diagnostics.DebuggerNonUserCode()]public frmRoof()
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();
		}
		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool Disposing)
		{
			if (Disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(Disposing);
		}
        //Required by the Windows Form Designer
		public System.Windows.Forms.ToolTip ToolTip1;
		public System.Windows.Forms.Button cmdCopy;
        public System.Windows.Forms.CheckBox chkDrawGround;
		public System.Windows.Forms.Button cmdDone;
		public System.Windows.Forms.PictureBox Picture2;
		public System.Windows.Forms.PictureBox Picture1;
        public System.Windows.Forms.Label Label6;
		public System.Windows.Forms.Label lblEditing;
        public System.Windows.Forms.Label Label1;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmdCopy = new System.Windows.Forms.Button();
            this.chkDrawGround = new System.Windows.Forms.CheckBox();
            this.cmdDone = new System.Windows.Forms.Button();
            this.Picture2 = new System.Windows.Forms.PictureBox();
            this.Picture1 = new System.Windows.Forms.PictureBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.lblEditing = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.sbDown = new System.Windows.Forms.VScrollBar();
            this.sbRight = new System.Windows.Forms.HScrollBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDeleteRoof = new System.Windows.Forms.Button();
            this.btnCreateRoof = new System.Windows.Forms.Button();
            this.nudRoofIndex = new System.Windows.Forms.NumericUpDown();
            this.nudRoofY = new System.Windows.Forms.NumericUpDown();
            this.nudRoofX = new System.Windows.Forms.NumericUpDown();
            this.nudRoofHeight = new System.Windows.Forms.NumericUpDown();
            this.nudRoofWidth = new System.Windows.Forms.NumericUpDown();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.Picture2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Picture1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoofIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoofY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoofX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoofHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoofWidth)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCopy
            // 
            this.cmdCopy.BackColor = System.Drawing.SystemColors.Control;
            this.cmdCopy.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdCopy.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCopy.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdCopy.Location = new System.Drawing.Point(5, 433);
            this.cmdCopy.Margin = new System.Windows.Forms.Padding(2);
            this.cmdCopy.Name = "cmdCopy";
            this.cmdCopy.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdCopy.Size = new System.Drawing.Size(100, 39);
            this.cmdCopy.TabIndex = 22;
            this.cmdCopy.Text = "Copy From Ground";
            this.cmdCopy.UseVisualStyleBackColor = false;
            this.cmdCopy.Click += new System.EventHandler(this.cmdCopy_Click);
            // 
            // chkDrawGround
            // 
            this.chkDrawGround.BackColor = System.Drawing.SystemColors.Control;
            this.chkDrawGround.Checked = true;
            this.chkDrawGround.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDrawGround.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkDrawGround.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDrawGround.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkDrawGround.Location = new System.Drawing.Point(5, 103);
            this.chkDrawGround.Margin = new System.Windows.Forms.Padding(2);
            this.chkDrawGround.Name = "chkDrawGround";
            this.chkDrawGround.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkDrawGround.Size = new System.Drawing.Size(108, 23);
            this.chkDrawGround.TabIndex = 21;
            this.chkDrawGround.Text = "Draw Ground";
            this.chkDrawGround.UseVisualStyleBackColor = false;
            this.chkDrawGround.CheckStateChanged += new System.EventHandler(this.chkDrawGround_CheckStateChanged);
            // 
            // cmdDone
            // 
            this.cmdDone.BackColor = System.Drawing.SystemColors.Control;
            this.cmdDone.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdDone.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdDone.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdDone.Location = new System.Drawing.Point(220, 451);
            this.cmdDone.Margin = new System.Windows.Forms.Padding(2);
            this.cmdDone.Name = "cmdDone";
            this.cmdDone.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdDone.Size = new System.Drawing.Size(54, 23);
            this.cmdDone.TabIndex = 4;
            this.cmdDone.Text = "Done";
            this.cmdDone.UseVisualStyleBackColor = false;
            // 
            // Picture2
            // 
            this.Picture2.BackColor = System.Drawing.SystemColors.Control;
            this.Picture2.Cursor = System.Windows.Forms.Cursors.Default;
            this.Picture2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Picture2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Picture2.Location = new System.Drawing.Point(5, 130);
            this.Picture2.Margin = new System.Windows.Forms.Padding(2);
            this.Picture2.Name = "Picture2";
            this.Picture2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Picture2.Size = new System.Drawing.Size(256, 256);
            this.Picture2.TabIndex = 1;
            this.Picture2.TabStop = false;
            this.Picture2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Picture2_MouseDown);
            this.Picture2.Paint += new System.Windows.Forms.PaintEventHandler(this.Picture2_Paint);
            // 
            // Picture1
            // 
            this.Picture1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Picture1.BackColor = System.Drawing.SystemColors.Control;
            this.Picture1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Picture1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Picture1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Picture1.Location = new System.Drawing.Point(0, 0);
            this.Picture1.Margin = new System.Windows.Forms.Padding(2);
            this.Picture1.Name = "Picture1";
            this.Picture1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Picture1.Size = new System.Drawing.Size(436, 450);
            this.Picture1.TabIndex = 0;
            this.Picture1.TabStop = false;
            this.Picture1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Picture1_MouseMove);
            this.Picture1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Picture1_MouseDown);
            this.Picture1.Paint += new System.Windows.Forms.PaintEventHandler(this.Picture1_Paint);
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.BackColor = System.Drawing.SystemColors.Control;
            this.Label6.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label6.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label6.Location = new System.Drawing.Point(2, 59);
            this.Label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label6.Name = "Label6";
            this.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label6.Size = new System.Drawing.Size(74, 14);
            this.Label6.TabIndex = 18;
            this.Label6.Text = "Roof Location";
            // 
            // lblEditing
            // 
            this.lblEditing.AutoSize = true;
            this.lblEditing.BackColor = System.Drawing.SystemColors.Control;
            this.lblEditing.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblEditing.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEditing.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblEditing.Location = new System.Drawing.Point(2, 2);
            this.lblEditing.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEditing.Name = "lblEditing";
            this.lblEditing.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblEditing.Size = new System.Drawing.Size(64, 14);
            this.lblEditing.TabIndex = 8;
            this.lblEditing.Text = "Editing Roof";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.SystemColors.Control;
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label1.Location = new System.Drawing.Point(144, 59);
            this.Label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(88, 14);
            this.Label1.TabIndex = 7;
            this.Label1.Text = "Roof Dimensions";
            // 
            // sbDown
            // 
            this.sbDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sbDown.Location = new System.Drawing.Point(436, 0);
            this.sbDown.Name = "sbDown";
            this.sbDown.Size = new System.Drawing.Size(17, 452);
            this.sbDown.TabIndex = 25;
            this.sbDown.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sbDown_Scroll);
            // 
            // sbRight
            // 
            this.sbRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sbRight.Location = new System.Drawing.Point(0, 452);
            this.sbRight.Name = "sbRight";
            this.sbRight.Size = new System.Drawing.Size(436, 17);
            this.sbRight.TabIndex = 26;
            this.sbRight.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sbRight_Scroll);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnDeleteRoof);
            this.panel1.Controls.Add(this.btnCreateRoof);
            this.panel1.Controls.Add(this.cmdDone);
            this.panel1.Controls.Add(this.chkDrawGround);
            this.panel1.Controls.Add(this.cmdCopy);
            this.panel1.Controls.Add(this.Picture2);
            this.panel1.Controls.Add(this.nudRoofIndex);
            this.panel1.Controls.Add(this.nudRoofY);
            this.panel1.Controls.Add(this.nudRoofX);
            this.panel1.Controls.Add(this.nudRoofHeight);
            this.panel1.Controls.Add(this.nudRoofWidth);
            this.panel1.Controls.Add(this.lblEditing);
            this.panel1.Controls.Add(this.Label1);
            this.panel1.Controls.Add(this.Label6);
            this.panel1.Location = new System.Drawing.Point(471, 8);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(274, 474);
            this.panel1.TabIndex = 27;
            // 
            // btnDeleteRoof
            // 
            this.btnDeleteRoof.Location = new System.Drawing.Point(186, 33);
            this.btnDeleteRoof.Name = "btnDeleteRoof";
            this.btnDeleteRoof.Size = new System.Drawing.Size(85, 23);
            this.btnDeleteRoof.TabIndex = 35;
            this.btnDeleteRoof.Text = "Delete Roof";
            this.btnDeleteRoof.UseVisualStyleBackColor = true;
            this.btnDeleteRoof.Click += new System.EventHandler(this.btnDeleteRoof_Click);
            // 
            // btnCreateRoof
            // 
            this.btnCreateRoof.Location = new System.Drawing.Point(186, 4);
            this.btnCreateRoof.Name = "btnCreateRoof";
            this.btnCreateRoof.Size = new System.Drawing.Size(85, 23);
            this.btnCreateRoof.TabIndex = 34;
            this.btnCreateRoof.Text = "Create Roof";
            this.btnCreateRoof.UseVisualStyleBackColor = true;
            this.btnCreateRoof.Click += new System.EventHandler(this.btnCreateRoof_Click);
            // 
            // nudRoofIndex
            // 
            this.nudRoofIndex.Location = new System.Drawing.Point(3, 19);
            this.nudRoofIndex.Name = "nudRoofIndex";
            this.nudRoofIndex.Size = new System.Drawing.Size(78, 20);
            this.nudRoofIndex.TabIndex = 33;
            this.nudRoofIndex.ValueChanged += new System.EventHandler(this.nudRoofIndex_ValueChanged);
            // 
            // nudRoofY
            // 
            this.nudRoofY.Location = new System.Drawing.Point(62, 75);
            this.nudRoofY.Margin = new System.Windows.Forms.Padding(2);
            this.nudRoofY.Name = "nudRoofY";
            this.nudRoofY.Size = new System.Drawing.Size(53, 20);
            this.nudRoofY.TabIndex = 32;
            this.nudRoofY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRoofY.ValueChanged += new System.EventHandler(this.nudRoofY_ValueChanged);
            // 
            // nudRoofX
            // 
            this.nudRoofX.Location = new System.Drawing.Point(5, 75);
            this.nudRoofX.Margin = new System.Windows.Forms.Padding(2);
            this.nudRoofX.Name = "nudRoofX";
            this.nudRoofX.Size = new System.Drawing.Size(53, 20);
            this.nudRoofX.TabIndex = 31;
            this.nudRoofX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRoofX.ValueChanged += new System.EventHandler(this.nudRoofX_ValueChanged);
            // 
            // nudRoofHeight
            // 
            this.nudRoofHeight.Location = new System.Drawing.Point(208, 75);
            this.nudRoofHeight.Margin = new System.Windows.Forms.Padding(2);
            this.nudRoofHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRoofHeight.Name = "nudRoofHeight";
            this.nudRoofHeight.Size = new System.Drawing.Size(53, 20);
            this.nudRoofHeight.TabIndex = 30;
            this.nudRoofHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRoofHeight.ValueChanged += new System.EventHandler(this.nudRoofHeight_ValueChanged);
            // 
            // nudRoofWidth
            // 
            this.nudRoofWidth.Location = new System.Drawing.Point(147, 75);
            this.nudRoofWidth.Margin = new System.Windows.Forms.Padding(2);
            this.nudRoofWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRoofWidth.Name = "nudRoofWidth";
            this.nudRoofWidth.Size = new System.Drawing.Size(53, 20);
            this.nudRoofWidth.TabIndex = 29;
            this.nudRoofWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRoofWidth.ValueChanged += new System.EventHandler(this.nudRoofWidth_ValueChanged);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.Picture1);
            this.panel3.Controls.Add(this.sbRight);
            this.panel3.Controls.Add(this.sbDown);
            this.panel3.Location = new System.Drawing.Point(12, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(454, 469);
            this.panel3.TabIndex = 29;
            // 
            // frmRoof
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(756, 493);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Location = new System.Drawing.Point(4, 23);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmRoof";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Edit Roof";
            this.Load += new System.EventHandler(this.frmRoof_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmRoof_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.Picture2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Picture1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoofIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoofY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoofX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoofHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoofWidth)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		internal System.Windows.Forms.VScrollBar sbDown;
		internal System.Windows.Forms.HScrollBar sbRight;
		#endregion
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown nudRoofWidth;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.NumericUpDown nudRoofIndex;
        private System.Windows.Forms.NumericUpDown nudRoofY;
        private System.Windows.Forms.NumericUpDown nudRoofX;
        private System.Windows.Forms.NumericUpDown nudRoofHeight;
        private System.Windows.Forms.Button btnDeleteRoof;
        private System.Windows.Forms.Button btnCreateRoof;
	}
}
