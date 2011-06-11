

namespace XleMapEditor
{
	public partial class frmProperties
	{
		#region "Windows Form Designer generated code "
		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool Disposing)
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
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.grpTileset = new System.Windows.Forms.GroupBox();
			this.txtDefaultTile = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.cboTileset = new System.Windows.Forms.ComboBox();
			this.Frame1 = new System.Windows.Forms.GroupBox();
			this.cboTypes = new System.Windows.Forms.ComboBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.txtWidth = new System.Windows.Forms.TextBox();
			this.txtHeight = new System.Windows.Forms.TextBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.Label8 = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.Label1 = new System.Windows.Forms.Label();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.grpTileset.SuspendLayout();
			this.Frame1.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpTileset
			// 
			this.grpTileset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpTileset.BackColor = System.Drawing.SystemColors.Control;
			this.grpTileset.Controls.Add(this.txtDefaultTile);
			this.grpTileset.Controls.Add(this.label4);
			this.grpTileset.Controls.Add(this.label9);
			this.grpTileset.Controls.Add(this.cboTileset);
			this.grpTileset.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.grpTileset.ForeColor = System.Drawing.SystemColors.ControlText;
			this.grpTileset.Location = new System.Drawing.Point(8, 168);
			this.grpTileset.Name = "grpTileset";
			this.grpTileset.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.grpTileset.Size = new System.Drawing.Size(296, 82);
			this.grpTileset.TabIndex = 36;
			this.grpTileset.TabStop = false;
			this.grpTileset.Text = "Tile Set";
			// 
			// txtDefaultTile
			// 
			this.txtDefaultTile.AcceptsReturn = true;
			this.txtDefaultTile.BackColor = System.Drawing.SystemColors.Window;
			this.txtDefaultTile.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtDefaultTile.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtDefaultTile.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtDefaultTile.Location = new System.Drawing.Point(79, 47);
			this.txtDefaultTile.MaxLength = 0;
			this.txtDefaultTile.Name = "txtDefaultTile";
			this.txtDefaultTile.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtDefaultTile.Size = new System.Drawing.Size(89, 20);
			this.txtDefaultTile.TabIndex = 36;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.SystemColors.Control;
			this.label4.Cursor = System.Windows.Forms.Cursors.Default;
			this.label4.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label4.Location = new System.Drawing.Point(13, 50);
			this.label4.Name = "label4";
			this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label4.Size = new System.Drawing.Size(60, 14);
			this.label4.TabIndex = 38;
			this.label4.Text = "Default Tile";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.BackColor = System.Drawing.SystemColors.Control;
			this.label9.Cursor = System.Windows.Forms.Cursors.Default;
			this.label9.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label9.Location = new System.Drawing.Point(35, 22);
			this.label9.Name = "label9";
			this.label9.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label9.Size = new System.Drawing.Size(38, 14);
			this.label9.TabIndex = 34;
			this.label9.Text = "Tileset";
			// 
			// cboTileset
			// 
			this.cboTileset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboTileset.FormattingEnabled = true;
			this.cboTileset.Items.AddRange(new object[] {
            "Tiles.bmp",
            "TownTiles.bmp",
            "CastleTiles.bmp",
            "LOB Tiles.bmp",
            "LOB TownTiles.bmp"});
			this.cboTileset.Location = new System.Drawing.Point(79, 19);
			this.cboTileset.Name = "cboTileset";
			this.cboTileset.Size = new System.Drawing.Size(198, 22);
			this.cboTileset.TabIndex = 4;
			// 
			// Frame1
			// 
			this.Frame1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.Frame1.BackColor = System.Drawing.SystemColors.Control;
			this.Frame1.Controls.Add(this.cboTypes);
			this.Frame1.Controls.Add(this.Label2);
			this.Frame1.Controls.Add(this.txtWidth);
			this.Frame1.Controls.Add(this.txtHeight);
			this.Frame1.Controls.Add(this.txtName);
			this.Frame1.Controls.Add(this.Label8);
			this.Frame1.Controls.Add(this.Label3);
			this.Frame1.Controls.Add(this.Label1);
			this.Frame1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Frame1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Frame1.Location = new System.Drawing.Point(8, 8);
			this.Frame1.Name = "Frame1";
			this.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Frame1.Size = new System.Drawing.Size(296, 153);
			this.Frame1.TabIndex = 16;
			this.Frame1.TabStop = false;
			this.Frame1.Text = "Map Type";
			// 
			// cboTypes
			// 
			this.cboTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTypes.FormattingEnabled = true;
			this.cboTypes.Location = new System.Drawing.Point(76, 51);
			this.cboTypes.Name = "cboTypes";
			this.cboTypes.Size = new System.Drawing.Size(201, 22);
			this.cboTypes.TabIndex = 1;
			this.cboTypes.SelectedIndexChanged += new System.EventHandler(this.cboTypes_SelectedIndexChanged);
			// 
			// Label2
			// 
			this.Label2.AutoSize = true;
			this.Label2.BackColor = System.Drawing.SystemColors.Control;
			this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label2.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label2.Location = new System.Drawing.Point(16, 54);
			this.Label2.Name = "Label2";
			this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label2.Size = new System.Drawing.Size(54, 14);
			this.Label2.TabIndex = 32;
			this.Label2.Text = "Map Type";
			// 
			// txtWidth
			// 
			this.txtWidth.AcceptsReturn = true;
			this.txtWidth.BackColor = System.Drawing.SystemColors.Window;
			this.txtWidth.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtWidth.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtWidth.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtWidth.Location = new System.Drawing.Point(76, 94);
			this.txtWidth.MaxLength = 0;
			this.txtWidth.Name = "txtWidth";
			this.txtWidth.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtWidth.Size = new System.Drawing.Size(89, 20);
			this.txtWidth.TabIndex = 2;
			this.txtWidth.TextChanged += new System.EventHandler(this.txtWidth_TextChanged);
			// 
			// txtHeight
			// 
			this.txtHeight.AcceptsReturn = true;
			this.txtHeight.BackColor = System.Drawing.SystemColors.Window;
			this.txtHeight.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtHeight.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtHeight.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtHeight.Location = new System.Drawing.Point(76, 118);
			this.txtHeight.MaxLength = 0;
			this.txtHeight.Name = "txtHeight";
			this.txtHeight.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtHeight.Size = new System.Drawing.Size(89, 20);
			this.txtHeight.TabIndex = 3;
			this.txtHeight.TextChanged += new System.EventHandler(this.txtHeight_TextChanged);
			// 
			// txtName
			// 
			this.txtName.AcceptsReturn = true;
			this.txtName.BackColor = System.Drawing.SystemColors.Window;
			this.txtName.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtName.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtName.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtName.Location = new System.Drawing.Point(76, 26);
			this.txtName.MaxLength = 16;
			this.txtName.Name = "txtName";
			this.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtName.Size = new System.Drawing.Size(201, 20);
			this.txtName.TabIndex = 0;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// Label8
			// 
			this.Label8.AutoSize = true;
			this.Label8.BackColor = System.Drawing.SystemColors.Control;
			this.Label8.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label8.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label8.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label8.Location = new System.Drawing.Point(33, 118);
			this.Label8.Name = "Label8";
			this.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label8.Size = new System.Drawing.Size(37, 14);
			this.Label8.TabIndex = 31;
			this.Label8.Text = "Height";
			// 
			// Label3
			// 
			this.Label3.AutoSize = true;
			this.Label3.BackColor = System.Drawing.SystemColors.Control;
			this.Label3.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label3.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label3.Location = new System.Drawing.Point(36, 97);
			this.Label3.Name = "Label3";
			this.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label3.Size = new System.Drawing.Size(34, 14);
			this.Label3.TabIndex = 29;
			this.Label3.Text = "Width";
			// 
			// Label1
			// 
			this.Label1.AutoSize = true;
			this.Label1.BackColor = System.Drawing.SystemColors.Control;
			this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label1.Location = new System.Drawing.Point(13, 29);
			this.Label1.Name = "Label1";
			this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label1.Size = new System.Drawing.Size(57, 14);
			this.Label1.TabIndex = 17;
			this.Label1.Text = "Map Name";
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.BackColor = System.Drawing.SystemColors.Control;
			this.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdCancel.Location = new System.Drawing.Point(239, 444);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdCancel.Size = new System.Drawing.Size(65, 25);
			this.cmdCancel.TabIndex = 17;
			this.cmdCancel.TabStop = false;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOK.BackColor = System.Drawing.SystemColors.Control;
			this.cmdOK.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdOK.Location = new System.Drawing.Point(168, 444);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdOK.Size = new System.Drawing.Size(65, 25);
			this.cmdOK.TabIndex = 18;
			this.cmdOK.TabStop = false;
			this.cmdOK.Text = "OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.propertyGrid1.Location = new System.Drawing.Point(12, 256);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(292, 182);
			this.propertyGrid1.TabIndex = 35;
			// 
			// frmProperties
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(316, 481);
			this.Controls.Add(this.propertyGrid1);
			this.Controls.Add(this.grpTileset);
			this.Controls.Add(this.Frame1);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Location = new System.Drawing.Point(4, 23);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmProperties";
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Map Properties";
			this.Load += new System.EventHandler(this.frmProperties_Load);
			this.grpTileset.ResumeLayout(false);
			this.grpTileset.PerformLayout();
			this.Frame1.ResumeLayout(false);
			this.Frame1.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.ComboBox cboTileset;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolTip ToolTip1;
		private System.Windows.Forms.GroupBox grpTileset;
		private System.Windows.Forms.TextBox txtWidth;
		private System.Windows.Forms.TextBox txtHeight;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label Label8;
		private System.Windows.Forms.Label Label3;
		private System.Windows.Forms.Label Label1;
		private System.Windows.Forms.GroupBox Frame1;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.ComboBox cboTypes;
		private System.Windows.Forms.Label Label2;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.TextBox txtDefaultTile;
		private System.Windows.Forms.Label label4;
	}
}
