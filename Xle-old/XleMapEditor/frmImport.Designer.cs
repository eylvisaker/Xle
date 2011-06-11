namespace XleMapEditor
{
	partial class frmImport
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImport));
			this.nudOffset = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.nudWidth = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.cboIntSize = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.nudHeight = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.cboMapType = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.cboTiles = new System.Windows.Forms.ComboBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnLineDown = new System.Windows.Forms.Button();
			this.btnLineUp = new System.Windows.Forms.Button();
			this.btnPageDown = new System.Windows.Forms.Button();
			this.btnPageUp = new System.Windows.Forms.Button();
			this.chkRLE = new System.Windows.Forms.CheckBox();
			this.cboTileSize = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.tilePicker = new XleMapEditor.TilePicker();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnLoadMapping = new System.Windows.Forms.ToolStripButton();
			this.btnSaveMapping = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnSaveMap = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.zoomIn = new System.Windows.Forms.ToolStripButton();
			this.zoomOut = new System.Windows.Forms.ToolStripButton();
			this.openFile = new System.Windows.Forms.OpenFileDialog();
			this.saveFile = new System.Windows.Forms.SaveFileDialog();
			this.xmfSave = new System.Windows.Forms.SaveFileDialog();
			this.mapView = new XleMapEditor.XleMapView();
			this.btnSaveToMap = new System.Windows.Forms.ToolStripButton();
			((System.ComponentModel.ISupportInitialize)(this.nudOffset)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
			this.panel1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// nudOffset
			// 
			this.nudOffset.Location = new System.Drawing.Point(6, 25);
			this.nudOffset.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
			this.nudOffset.Name = "nudOffset";
			this.nudOffset.Size = new System.Drawing.Size(90, 20);
			this.nudOffset.TabIndex = 4;
			this.nudOffset.ValueChanged += new System.EventHandler(this.nudOffset_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Start Offset";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 65);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Width";
			// 
			// nudWidth
			// 
			this.nudWidth.Location = new System.Drawing.Point(6, 81);
			this.nudWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.nudWidth.Name = "nudWidth";
			this.nudWidth.Size = new System.Drawing.Size(90, 20);
			this.nudWidth.TabIndex = 6;
			this.nudWidth.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
			this.nudWidth.ValueChanged += new System.EventHandler(this.nudWidth_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(99, 143);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(63, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Integer Size";
			// 
			// cboIntSize
			// 
			this.cboIntSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboIntSize.FormattingEnabled = true;
			this.cboIntSize.Items.AddRange(new object[] {
            "1",
            "2",
            "4",
            "8"});
			this.cboIntSize.Location = new System.Drawing.Point(102, 159);
			this.cboIntSize.Name = "cboIntSize";
			this.cboIntSize.Size = new System.Drawing.Size(60, 21);
			this.cboIntSize.TabIndex = 9;
			this.cboIntSize.SelectedIndexChanged += new System.EventHandler(this.cboIntSize_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 104);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(38, 13);
			this.label4.TabIndex = 11;
			this.label4.Text = "Height";
			// 
			// nudHeight
			// 
			this.nudHeight.Location = new System.Drawing.Point(6, 120);
			this.nudHeight.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.nudHeight.Name = "nudHeight";
			this.nudHeight.Size = new System.Drawing.Size(90, 20);
			this.nudHeight.TabIndex = 10;
			this.nudHeight.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
			this.nudHeight.ValueChanged += new System.EventHandler(this.nudHeight_ValueChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(99, 65);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(55, 13);
			this.label6.TabIndex = 14;
			this.label6.Text = "Map Type";
			// 
			// cboMapType
			// 
			this.cboMapType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMapType.FormattingEnabled = true;
			this.cboMapType.Location = new System.Drawing.Point(102, 81);
			this.cboMapType.Name = "cboMapType";
			this.cboMapType.Size = new System.Drawing.Size(164, 21);
			this.cboMapType.TabIndex = 15;
			this.cboMapType.SelectedIndexChanged += new System.EventHandler(this.cboMapType_SelectedIndexChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(99, 104);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(41, 13);
			this.label7.TabIndex = 16;
			this.label7.Text = "Tile set";
			// 
			// cboTiles
			// 
			this.cboTiles.FormattingEnabled = true;
			this.cboTiles.Location = new System.Drawing.Point(102, 120);
			this.cboTiles.Name = "cboTiles";
			this.cboTiles.Size = new System.Drawing.Size(164, 21);
			this.cboTiles.TabIndex = 17;
			this.cboTiles.SelectedIndexChanged += new System.EventHandler(this.cboTiles_SelectedIndexChanged);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.btnLineDown);
			this.panel1.Controls.Add(this.btnLineUp);
			this.panel1.Controls.Add(this.btnPageDown);
			this.panel1.Controls.Add(this.btnPageUp);
			this.panel1.Controls.Add(this.chkRLE);
			this.panel1.Controls.Add(this.cboTileSize);
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.cboTiles);
			this.panel1.Controls.Add(this.nudOffset);
			this.panel1.Controls.Add(this.cboMapType);
			this.panel1.Controls.Add(this.nudWidth);
			this.panel1.Controls.Add(this.cboIntSize);
			this.panel1.Controls.Add(this.nudHeight);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.tilePicker);
			this.panel1.Controls.Add(this.label7);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Location = new System.Drawing.Point(278, 28);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(283, 429);
			this.panel1.TabIndex = 18;
			// 
			// btnLineDown
			// 
			this.btnLineDown.Location = new System.Drawing.Point(184, 9);
			this.btnLineDown.Name = "btnLineDown";
			this.btnLineDown.Size = new System.Drawing.Size(75, 23);
			this.btnLineDown.TabIndex = 24;
			this.btnLineDown.Text = "Line Down";
			this.btnLineDown.UseVisualStyleBackColor = true;
			this.btnLineDown.Click += new System.EventHandler(this.btnLineDown_Click);
			// 
			// btnLineUp
			// 
			this.btnLineUp.Location = new System.Drawing.Point(104, 9);
			this.btnLineUp.Name = "btnLineUp";
			this.btnLineUp.Size = new System.Drawing.Size(75, 23);
			this.btnLineUp.TabIndex = 23;
			this.btnLineUp.Text = "Line Up";
			this.btnLineUp.UseVisualStyleBackColor = true;
			this.btnLineUp.Click += new System.EventHandler(this.btnLineUp_Click);
			// 
			// btnPageDown
			// 
			this.btnPageDown.Location = new System.Drawing.Point(184, 34);
			this.btnPageDown.Name = "btnPageDown";
			this.btnPageDown.Size = new System.Drawing.Size(75, 23);
			this.btnPageDown.TabIndex = 22;
			this.btnPageDown.Text = "Page Down";
			this.btnPageDown.UseVisualStyleBackColor = true;
			this.btnPageDown.Click += new System.EventHandler(this.btnPageDown_Click);
			// 
			// btnPageUp
			// 
			this.btnPageUp.Location = new System.Drawing.Point(104, 34);
			this.btnPageUp.Name = "btnPageUp";
			this.btnPageUp.Size = new System.Drawing.Size(75, 23);
			this.btnPageUp.TabIndex = 21;
			this.btnPageUp.Text = "Page Up";
			this.btnPageUp.UseVisualStyleBackColor = true;
			this.btnPageUp.Click += new System.EventHandler(this.btnPageUp_Click);
			// 
			// chkRLE
			// 
			this.chkRLE.AutoSize = true;
			this.chkRLE.Location = new System.Drawing.Point(9, 146);
			this.chkRLE.Name = "chkRLE";
			this.chkRLE.Size = new System.Drawing.Size(47, 17);
			this.chkRLE.TabIndex = 20;
			this.chkRLE.Text = "RLE";
			this.chkRLE.UseVisualStyleBackColor = true;
			this.chkRLE.CheckedChanged += new System.EventHandler(this.chkRLE_CheckedChanged);
			// 
			// cboTileSize
			// 
			this.cboTileSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTileSize.FormattingEnabled = true;
			this.cboTileSize.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
			this.cboTileSize.Location = new System.Drawing.Point(184, 159);
			this.cboTileSize.Name = "cboTileSize";
			this.cboTileSize.Size = new System.Drawing.Size(60, 21);
			this.cboTileSize.TabIndex = 19;
			this.cboTileSize.SelectedIndexChanged += new System.EventHandler(this.cboTileSize_SelectedIndexChanged);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(181, 143);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(47, 13);
			this.label8.TabIndex = 18;
			this.label8.Text = "Tile Size";
			// 
			// tilePicker
			// 
			this.tilePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tilePicker.Location = new System.Drawing.Point(9, 224);
			this.tilePicker.Name = "tilePicker";
			this.tilePicker.SelectedTileIndex = 0;
			this.tilePicker.Size = new System.Drawing.Size(263, 202);
			this.tilePicker.State = null;
			this.tilePicker.TabIndex = 1;
			this.tilePicker.TilePick += new System.EventHandler<XleMapEditor.TilePickEventArgs>(this.tilePicker_TilePick);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoadMapping,
            this.btnSaveMapping,
            this.toolStripSeparator1,
            this.btnSaveMap,
            this.btnSaveToMap,
            this.toolStripSeparator2,
            this.zoomIn,
            this.zoomOut});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(556, 25);
			this.toolStrip1.TabIndex = 19;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// btnLoadMapping
			// 
			this.btnLoadMapping.Image = global::XleMapEditor.Properties.Resources.openHS;
			this.btnLoadMapping.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnLoadMapping.Name = "btnLoadMapping";
			this.btnLoadMapping.Size = new System.Drawing.Size(104, 22);
			this.btnLoadMapping.Text = "Load Mapping";
			this.btnLoadMapping.Click += new System.EventHandler(this.btnLoadMapping_Click);
			// 
			// btnSaveMapping
			// 
			this.btnSaveMapping.Image = global::XleMapEditor.Properties.Resources.saveHS;
			this.btnSaveMapping.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSaveMapping.Name = "btnSaveMapping";
			this.btnSaveMapping.Size = new System.Drawing.Size(102, 22);
			this.btnSaveMapping.Text = "Save Mapping";
			this.btnSaveMapping.Click += new System.EventHandler(this.btnSaveMapping_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnSaveMap
			// 
			this.btnSaveMap.Image = global::XleMapEditor.Properties.Resources.saveHS;
			this.btnSaveMap.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSaveMap.Name = "btnSaveMap";
			this.btnSaveMap.Size = new System.Drawing.Size(105, 22);
			this.btnSaveMap.Text = "Save XMF map";
			this.btnSaveMap.Click += new System.EventHandler(this.btnSaveMap_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// zoomIn
			// 
			this.zoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.zoomIn.Image = global::XleMapEditor.Properties.Resources.ZoomIn;
			this.zoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.zoomIn.Name = "zoomIn";
			this.zoomIn.Size = new System.Drawing.Size(23, 22);
			this.zoomIn.Text = "Zoom In";
			this.zoomIn.Click += new System.EventHandler(this.zoomIn_Click);
			// 
			// zoomOut
			// 
			this.zoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.zoomOut.Image = global::XleMapEditor.Properties.Resources.ZoomOut;
			this.zoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.zoomOut.Name = "zoomOut";
			this.zoomOut.Size = new System.Drawing.Size(23, 20);
			this.zoomOut.Text = "Zoom Out";
			this.zoomOut.Click += new System.EventHandler(this.zoomOut_Click);
			// 
			// openFile
			// 
			this.openFile.Filter = "Mapping file|*.mpp";
			// 
			// saveFile
			// 
			this.saveFile.Filter = "Mapping file|*.mpp";
			// 
			// xmfSave
			// 
			this.xmfSave.Filter = "XMF Map (*.xmf)|*.xmf";
			// 
			// mapView
			// 
			this.mapView.AllowBoxSelect = false;
			this.mapView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.mapView.Location = new System.Drawing.Point(12, 28);
			this.mapView.Name = "mapView";
			this.mapView.SelRect = new System.Drawing.Rectangle(0, 0, 40, 40);
			this.mapView.Size = new System.Drawing.Size(260, 429);
			this.mapView.State = null;
			this.mapView.TabIndex = 0;
			this.mapView.TileMouseDown += new System.EventHandler<XleMapEditor.TileMouseEventArgs>(this.mapView_TileMouseDown);
			this.mapView.TileMouseMove += new System.EventHandler<XleMapEditor.TileMouseEventArgs>(this.mapView_TileMouseMove);
			this.mapView.Load += new System.EventHandler(this.mapView_Load);
			// 
			// btnSaveToMap
			// 
			this.btnSaveToMap.Image = global::XleMapEditor.Properties.Resources.saveHS;
			this.btnSaveToMap.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSaveToMap.Name = "btnSaveToMap";
			this.btnSaveToMap.Size = new System.Drawing.Size(162, 22);
			this.btnSaveToMap.Text = "Save to existing XMF map";
			this.btnSaveToMap.Click += new System.EventHandler(this.btnSaveToMap_Click);
			// 
			// frmImport
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(556, 469);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.mapView);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "frmImport";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Import Map";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmImport_KeyDown);
			((System.ComponentModel.ISupportInitialize)(this.nudOffset)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private XleMapView mapView;
		private TilePicker tilePicker;
		private System.Windows.Forms.NumericUpDown nudOffset;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown nudWidth;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cboIntSize;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown nudHeight;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox cboMapType;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cboTiles;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton zoomIn;
		private System.Windows.Forms.ToolStripButton zoomOut;
		private System.Windows.Forms.ComboBox cboTileSize;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnLoadMapping;
		private System.Windows.Forms.ToolStripButton btnSaveMapping;
		private System.Windows.Forms.OpenFileDialog openFile;
		private System.Windows.Forms.SaveFileDialog saveFile;
		private System.Windows.Forms.ToolStripButton btnSaveMap;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.SaveFileDialog xmfSave;
		private System.Windows.Forms.CheckBox chkRLE;
		private System.Windows.Forms.Button btnPageDown;
		private System.Windows.Forms.Button btnPageUp;
		private System.Windows.Forms.Button btnLineDown;
		private System.Windows.Forms.Button btnLineUp;
		private System.Windows.Forms.ToolStripButton btnSaveToMap;
	}
}