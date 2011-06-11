

namespace XleMapEditor
{
	public partial class frmMapEdit
	{
		#region "Windows Form Designer generated code "
		[System.Diagnostics.DebuggerNonUserCode()]
		public frmMapEdit()
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();
		}
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
		public System.Windows.Forms.ToolTip ToolTip1;
		public System.Windows.Forms.ToolStripStatusLabel _StatusBar1_Panel1;
		public System.Windows.Forms.StatusStrip StatusBar1;
		public System.Windows.Forms.Button cmdFill;
		public System.Windows.Forms.CheckBox chkRandom;
		public System.Windows.Forms.Button cmdObject;
		public System.Windows.Forms.Button cmdGuard;
		public System.Windows.Forms.Button cmdModifySpecial;
		public System.Windows.Forms.Button cmdDeleteSpecial;
		public System.Windows.Forms.ListBox lstPreDef;
		public System.Windows.Forms.Button cmdRoof;
		public System.Windows.Forms.CheckBox chkDrawRoof;
		public System.Windows.Forms.CheckBox chkRestrict;
		public System.Windows.Forms.CheckBox chkDrawGuards;
		public System.Windows.Forms.Label lblDim;
		public System.Windows.Forms.Label lblX;
		public System.Windows.Forms.Label lblY;
		public System.Windows.Forms.Label lblTile;
		public System.Windows.Forms.Label lblCurrentTile;
		public System.Windows.Forms.Label Label3;
		public System.Windows.Forms.Panel frmRight;
		public System.Windows.Forms.OpenFileDialog cmdDialogOpen;
		public System.Windows.Forms.SaveFileDialog cmdDialogSave;
		public System.Windows.Forms.ToolStripMenuItem mnuNew;
		public System.Windows.Forms.ToolStripMenuItem mnuOpen;
		public System.Windows.Forms.ToolStripMenuItem mnuImport;
		public System.Windows.Forms.ToolStripSeparator mnuFileSep0;
		public System.Windows.Forms.ToolStripMenuItem mnuSave;
		public System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
		public System.Windows.Forms.ToolStripSeparator mnuSep0;
		public System.Windows.Forms.ToolStripMenuItem mnuQuit;
		public System.Windows.Forms.ToolStripMenuItem mnuFile;
		public System.Windows.Forms.ToolStripMenuItem mnuProperties;
		public System.Windows.Forms.ToolStripSeparator mnuSep2;
		public System.Windows.Forms.ToolStripMenuItem mnuPlaceSpecial;
		public System.Windows.Forms.ToolStripMenuItem mnuModifySpecial;
		public System.Windows.Forms.ToolStripMenuItem mnuDeleteSpecial;
		public System.Windows.Forms.ToolStripSeparator mnuSep1;
		public System.Windows.Forms.ToolStripMenuItem mnuRefreshTiles;
		public System.Windows.Forms.ToolStripMenuItem mnuOptions;
		public System.Windows.Forms.MenuStrip MainMenu1;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMapEdit));
			this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.chkRestrict = new System.Windows.Forms.CheckBox();
			this.StatusBar1 = new System.Windows.Forms.StatusStrip();
			this._StatusBar1_Panel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnFindEvent = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cmdDeleteSpecial = new System.Windows.Forms.Button();
			this.cmdPlaceEvent = new System.Windows.Forms.Button();
			this.cmdModifySpecial = new System.Windows.Forms.Button();
			this.nudEvent = new System.Windows.Forms.NumericUpDown();
			this.lblEventCount = new System.Windows.Forms.Label();
			this.cmdFill = new System.Windows.Forms.Button();
			this.chkRandom = new System.Windows.Forms.CheckBox();
			this.frmRight = new System.Windows.Forms.Panel();
			this.tilePicker = new XleMapEditor.TilePicker();
			this.cmdObject = new System.Windows.Forms.Button();
			this.cmdGuard = new System.Windows.Forms.Button();
			this.lstPreDef = new System.Windows.Forms.ListBox();
			this.cmdRoof = new System.Windows.Forms.Button();
			this.chkDrawRoof = new System.Windows.Forms.CheckBox();
			this.chkDrawGuards = new System.Windows.Forms.CheckBox();
			this.lblDim = new System.Windows.Forms.Label();
			this.lblX = new System.Windows.Forms.Label();
			this.lblY = new System.Windows.Forms.Label();
			this.lblTile = new System.Windows.Forms.Label();
			this.lblCurrentTile = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.cmdDialogOpen = new System.Windows.Forms.OpenFileDialog();
			this.cmdDialogSave = new System.Windows.Forms.SaveFileDialog();
			this.MainMenu1 = new System.Windows.Forms.MenuStrip();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuImport = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFileSep0 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSep0 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuQuit = new System.Windows.Forms.ToolStripMenuItem();
			this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSep2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuPlaceSpecial = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuModifySpecial = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDeleteSpecial = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSep1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRefreshTiles = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnNew = new System.Windows.Forms.ToolStripButton();
			this.btnOpen = new System.Windows.Forms.ToolStripButton();
			this.btnSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnCopy = new System.Windows.Forms.ToolStripButton();
			this.btnPaste = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
			this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.btnProperties = new System.Windows.Forms.ToolStripButton();
			this.mapView = new XleMapEditor.XleMapView();
			this.StatusBar1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudEvent)).BeginInit();
			this.frmRight.SuspendLayout();
			this.MainMenu1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// chkRestrict
			// 
			this.chkRestrict.AutoSize = true;
			this.chkRestrict.BackColor = System.Drawing.SystemColors.Control;
			this.chkRestrict.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkRestrict.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkRestrict.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkRestrict.Location = new System.Drawing.Point(11, 197);
			this.chkRestrict.Margin = new System.Windows.Forms.Padding(2);
			this.chkRestrict.Name = "chkRestrict";
			this.chkRestrict.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkRestrict.Size = new System.Drawing.Size(107, 18);
			this.chkRestrict.TabIndex = 5;
			this.chkRestrict.Text = "Restrict Drawing";
			this.ToolTip1.SetToolTip(this.chkRestrict, "Restricts tiles drawn to selection");
			this.chkRestrict.UseVisualStyleBackColor = false;
			// 
			// StatusBar1
			// 
			this.StatusBar1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StatusBar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._StatusBar1_Panel1});
			this.StatusBar1.Location = new System.Drawing.Point(0, 596);
			this.StatusBar1.Name = "StatusBar1";
			this.StatusBar1.Padding = new System.Windows.Forms.Padding(1, 0, 9, 0);
			this.StatusBar1.Size = new System.Drawing.Size(661, 25);
			this.StatusBar1.TabIndex = 27;
			// 
			// _StatusBar1_Panel1
			// 
			this._StatusBar1_Panel1.AutoSize = false;
			this._StatusBar1_Panel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this._StatusBar1_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this._StatusBar1_Panel1.Margin = new System.Windows.Forms.Padding(0);
			this._StatusBar1_Panel1.Name = "_StatusBar1_Panel1";
			this._StatusBar1_Panel1.Size = new System.Drawing.Size(96, 25);
			this._StatusBar1_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnFindEvent);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.cmdDeleteSpecial);
			this.panel1.Controls.Add(this.cmdPlaceEvent);
			this.panel1.Controls.Add(this.cmdModifySpecial);
			this.panel1.Controls.Add(this.nudEvent);
			this.panel1.Controls.Add(this.lblEventCount);
			this.panel1.Location = new System.Drawing.Point(11, 437);
			this.panel1.Margin = new System.Windows.Forms.Padding(2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(194, 111);
			this.panel1.TabIndex = 33;
			// 
			// btnFindEvent
			// 
			this.btnFindEvent.Location = new System.Drawing.Point(97, 68);
			this.btnFindEvent.Name = "btnFindEvent";
			this.btnFindEvent.Size = new System.Drawing.Size(81, 24);
			this.btnFindEvent.TabIndex = 35;
			this.btnFindEvent.Text = "Find Event";
			this.btnFindEvent.UseVisualStyleBackColor = true;
			this.btnFindEvent.Click += new System.EventHandler(this.btnFindEvent_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(1, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(37, 14);
			this.label1.TabIndex = 33;
			this.label1.Text = "Event:";
			// 
			// cmdDeleteSpecial
			// 
			this.cmdDeleteSpecial.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdDeleteSpecial.Enabled = false;
			this.cmdDeleteSpecial.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdDeleteSpecial.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdDeleteSpecial.Location = new System.Drawing.Point(96, 42);
			this.cmdDeleteSpecial.Margin = new System.Windows.Forms.Padding(2);
			this.cmdDeleteSpecial.Name = "cmdDeleteSpecial";
			this.cmdDeleteSpecial.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdDeleteSpecial.Size = new System.Drawing.Size(80, 23);
			this.cmdDeleteSpecial.TabIndex = 9;
			this.cmdDeleteSpecial.Text = "&Delete Event";
			this.cmdDeleteSpecial.UseVisualStyleBackColor = true;
			this.cmdDeleteSpecial.Click += new System.EventHandler(this.cmdDeleteSpecial_Click);
			// 
			// cmdPlaceEvent
			// 
			this.cmdPlaceEvent.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdPlaceEvent.Location = new System.Drawing.Point(11, 69);
			this.cmdPlaceEvent.Margin = new System.Windows.Forms.Padding(2);
			this.cmdPlaceEvent.Name = "cmdPlaceEvent";
			this.cmdPlaceEvent.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdPlaceEvent.Size = new System.Drawing.Size(78, 23);
			this.cmdPlaceEvent.TabIndex = 13;
			this.cmdPlaceEvent.Text = "Place &Event";
			this.cmdPlaceEvent.UseVisualStyleBackColor = true;
			this.cmdPlaceEvent.Click += new System.EventHandler(this.cmdPlaceSpecial_Click);
			// 
			// cmdModifySpecial
			// 
			this.cmdModifySpecial.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdModifySpecial.Enabled = false;
			this.cmdModifySpecial.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdModifySpecial.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdModifySpecial.Location = new System.Drawing.Point(11, 42);
			this.cmdModifySpecial.Margin = new System.Windows.Forms.Padding(2);
			this.cmdModifySpecial.Name = "cmdModifySpecial";
			this.cmdModifySpecial.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdModifySpecial.Size = new System.Drawing.Size(81, 23);
			this.cmdModifySpecial.TabIndex = 10;
			this.cmdModifySpecial.Text = "Modify &Event";
			this.cmdModifySpecial.UseVisualStyleBackColor = true;
			this.cmdModifySpecial.Click += new System.EventHandler(this.cmdModifySpecial_Click);
			// 
			// nudEvent
			// 
			this.nudEvent.Location = new System.Drawing.Point(43, 18);
			this.nudEvent.Margin = new System.Windows.Forms.Padding(2);
			this.nudEvent.Name = "nudEvent";
			this.nudEvent.Size = new System.Drawing.Size(135, 20);
			this.nudEvent.TabIndex = 32;
			// 
			// lblEventCount
			// 
			this.lblEventCount.AutoSize = true;
			this.lblEventCount.Location = new System.Drawing.Point(3, 2);
			this.lblEventCount.Name = "lblEventCount";
			this.lblEventCount.Size = new System.Drawing.Size(38, 14);
			this.lblEventCount.TabIndex = 34;
			this.lblEventCount.Text = "Count:";
			// 
			// cmdFill
			// 
			this.cmdFill.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdFill.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdFill.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdFill.Location = new System.Drawing.Point(159, 395);
			this.cmdFill.Margin = new System.Windows.Forms.Padding(2);
			this.cmdFill.Name = "cmdFill";
			this.cmdFill.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdFill.Size = new System.Drawing.Size(81, 26);
			this.cmdFill.TabIndex = 28;
			this.cmdFill.Text = "Fill Selection";
			this.cmdFill.UseVisualStyleBackColor = true;
			this.cmdFill.Click += new System.EventHandler(this.cmdFill_Click);
			// 
			// chkRandom
			// 
			this.chkRandom.AutoSize = true;
			this.chkRandom.BackColor = System.Drawing.SystemColors.Control;
			this.chkRandom.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkRandom.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkRandom.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkRandom.Location = new System.Drawing.Point(11, 131);
			this.chkRandom.Margin = new System.Windows.Forms.Padding(2);
			this.chkRandom.Name = "chkRandom";
			this.chkRandom.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkRandom.Size = new System.Drawing.Size(104, 18);
			this.chkRandom.TabIndex = 21;
			this.chkRandom.Text = "Randomize Tiles";
			this.chkRandom.UseVisualStyleBackColor = false;
			// 
			// frmRight
			// 
			this.frmRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.frmRight.BackColor = System.Drawing.SystemColors.Control;
			this.frmRight.Controls.Add(this.tilePicker);
			this.frmRight.Controls.Add(this.cmdFill);
			this.frmRight.Controls.Add(this.panel1);
			this.frmRight.Controls.Add(this.chkRandom);
			this.frmRight.Controls.Add(this.cmdObject);
			this.frmRight.Controls.Add(this.cmdGuard);
			this.frmRight.Controls.Add(this.lstPreDef);
			this.frmRight.Controls.Add(this.cmdRoof);
			this.frmRight.Controls.Add(this.chkDrawRoof);
			this.frmRight.Controls.Add(this.chkRestrict);
			this.frmRight.Controls.Add(this.chkDrawGuards);
			this.frmRight.Controls.Add(this.lblDim);
			this.frmRight.Controls.Add(this.lblX);
			this.frmRight.Controls.Add(this.lblY);
			this.frmRight.Controls.Add(this.lblTile);
			this.frmRight.Controls.Add(this.lblCurrentTile);
			this.frmRight.Controls.Add(this.Label3);
			this.frmRight.Cursor = System.Windows.Forms.Cursors.Default;
			this.frmRight.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.frmRight.ForeColor = System.Drawing.SystemColors.ControlText;
			this.frmRight.Location = new System.Drawing.Point(411, 26);
			this.frmRight.Margin = new System.Windows.Forms.Padding(2);
			this.frmRight.Name = "frmRight";
			this.frmRight.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.frmRight.Size = new System.Drawing.Size(250, 568);
			this.frmRight.TabIndex = 3;
			this.frmRight.Text = "Frame1";
			// 
			// tilePicker
			// 
			this.tilePicker.Location = new System.Drawing.Point(132, 26);
			this.tilePicker.Name = "tilePicker";
			this.tilePicker.SelectedTileIndex = 0;
			this.tilePicker.Size = new System.Drawing.Size(108, 364);
			this.tilePicker.State = null;
			this.tilePicker.TabIndex = 34;
			this.tilePicker.TilePick += new System.EventHandler<XleMapEditor.TilePickEventArgs>(this.tilePicker_TilePick);
			// 
			// cmdObject
			// 
			this.cmdObject.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdObject.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdObject.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdObject.Location = new System.Drawing.Point(11, 394);
			this.cmdObject.Margin = new System.Windows.Forms.Padding(2);
			this.cmdObject.Name = "cmdObject";
			this.cmdObject.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdObject.Size = new System.Drawing.Size(70, 39);
			this.cmdObject.TabIndex = 12;
			this.cmdObject.Text = "Place &Object";
			this.cmdObject.UseVisualStyleBackColor = true;
			this.cmdObject.Click += new System.EventHandler(this.cmdObject_Click);
			// 
			// cmdGuard
			// 
			this.cmdGuard.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdGuard.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdGuard.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdGuard.Location = new System.Drawing.Point(11, 250);
			this.cmdGuard.Margin = new System.Windows.Forms.Padding(2);
			this.cmdGuard.Name = "cmdGuard";
			this.cmdGuard.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdGuard.Size = new System.Drawing.Size(70, 39);
			this.cmdGuard.TabIndex = 11;
			this.cmdGuard.Text = "Place &Guard";
			this.cmdGuard.UseVisualStyleBackColor = true;
			this.cmdGuard.Click += new System.EventHandler(this.cmdGuard_Click);
			// 
			// lstPreDef
			// 
			this.lstPreDef.BackColor = System.Drawing.SystemColors.Window;
			this.lstPreDef.Cursor = System.Windows.Forms.Cursors.Default;
			this.lstPreDef.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstPreDef.ForeColor = System.Drawing.SystemColors.WindowText;
			this.lstPreDef.ItemHeight = 14;
			this.lstPreDef.Location = new System.Drawing.Point(11, 316);
			this.lstPreDef.Margin = new System.Windows.Forms.Padding(2);
			this.lstPreDef.Name = "lstPreDef";
			this.lstPreDef.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lstPreDef.Size = new System.Drawing.Size(83, 74);
			this.lstPreDef.TabIndex = 8;
			// 
			// cmdRoof
			// 
			this.cmdRoof.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdRoof.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdRoof.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdRoof.Location = new System.Drawing.Point(11, 219);
			this.cmdRoof.Margin = new System.Windows.Forms.Padding(2);
			this.cmdRoof.Name = "cmdRoof";
			this.cmdRoof.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdRoof.Size = new System.Drawing.Size(70, 27);
			this.cmdRoof.TabIndex = 7;
			this.cmdRoof.Text = "&Roof";
			this.cmdRoof.UseVisualStyleBackColor = true;
			this.cmdRoof.Click += new System.EventHandler(this.cmdRoof_Click);
			// 
			// chkDrawRoof
			// 
			this.chkDrawRoof.AutoSize = true;
			this.chkDrawRoof.BackColor = System.Drawing.SystemColors.Control;
			this.chkDrawRoof.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkDrawRoof.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkDrawRoof.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkDrawRoof.Location = new System.Drawing.Point(11, 153);
			this.chkDrawRoof.Margin = new System.Windows.Forms.Padding(2);
			this.chkDrawRoof.Name = "chkDrawRoof";
			this.chkDrawRoof.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkDrawRoof.Size = new System.Drawing.Size(81, 18);
			this.chkDrawRoof.TabIndex = 6;
			this.chkDrawRoof.Text = "Show Roof";
			this.chkDrawRoof.UseVisualStyleBackColor = false;
			this.chkDrawRoof.CheckedChanged += new System.EventHandler(this.chkDrawRoof_CheckedChanged);
			this.chkDrawRoof.CheckStateChanged += new System.EventHandler(this.chkDrawRoof_CheckStateChanged);
			// 
			// chkDrawGuards
			// 
			this.chkDrawGuards.AutoSize = true;
			this.chkDrawGuards.BackColor = System.Drawing.SystemColors.Control;
			this.chkDrawGuards.Checked = true;
			this.chkDrawGuards.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDrawGuards.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkDrawGuards.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkDrawGuards.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkDrawGuards.Location = new System.Drawing.Point(11, 175);
			this.chkDrawGuards.Margin = new System.Windows.Forms.Padding(2);
			this.chkDrawGuards.Name = "chkDrawGuards";
			this.chkDrawGuards.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkDrawGuards.Size = new System.Drawing.Size(94, 18);
			this.chkDrawGuards.TabIndex = 4;
			this.chkDrawGuards.Text = "Show Guards";
			this.chkDrawGuards.UseVisualStyleBackColor = false;
			this.chkDrawGuards.CheckedChanged += new System.EventHandler(this.chkDrawGuards_CheckedChanged);
			this.chkDrawGuards.CheckStateChanged += new System.EventHandler(this.chkDrawGuards_CheckStateChanged);
			// 
			// lblDim
			// 
			this.lblDim.BackColor = System.Drawing.SystemColors.Control;
			this.lblDim.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblDim.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDim.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblDim.Location = new System.Drawing.Point(8, 26);
			this.lblDim.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblDim.Name = "lblDim";
			this.lblDim.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblDim.Size = new System.Drawing.Size(92, 30);
			this.lblDim.TabIndex = 19;
			this.lblDim.Text = "Map Dimensions";
			// 
			// lblX
			// 
			this.lblX.BackColor = System.Drawing.SystemColors.Control;
			this.lblX.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblX.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblX.Location = new System.Drawing.Point(8, 56);
			this.lblX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblX.Name = "lblX";
			this.lblX.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblX.Size = new System.Drawing.Size(181, 12);
			this.lblX.TabIndex = 18;
			this.lblX.Text = "X:";
			// 
			// lblY
			// 
			this.lblY.BackColor = System.Drawing.SystemColors.Control;
			this.lblY.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblY.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblY.Location = new System.Drawing.Point(8, 67);
			this.lblY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblY.Name = "lblY";
			this.lblY.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblY.Size = new System.Drawing.Size(171, 12);
			this.lblY.TabIndex = 17;
			this.lblY.Text = "Y:";
			// 
			// lblTile
			// 
			this.lblTile.BackColor = System.Drawing.SystemColors.Control;
			this.lblTile.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblTile.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblTile.Location = new System.Drawing.Point(8, 78);
			this.lblTile.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblTile.Name = "lblTile";
			this.lblTile.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblTile.Size = new System.Drawing.Size(165, 12);
			this.lblTile.TabIndex = 16;
			this.lblTile.Text = "Tile:";
			// 
			// lblCurrentTile
			// 
			this.lblCurrentTile.BackColor = System.Drawing.SystemColors.Control;
			this.lblCurrentTile.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblCurrentTile.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblCurrentTile.Location = new System.Drawing.Point(8, 100);
			this.lblCurrentTile.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblCurrentTile.Name = "lblCurrentTile";
			this.lblCurrentTile.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblCurrentTile.Size = new System.Drawing.Size(171, 12);
			this.lblCurrentTile.TabIndex = 15;
			this.lblCurrentTile.Text = "current tile";
			// 
			// Label3
			// 
			this.Label3.AutoSize = true;
			this.Label3.BackColor = System.Drawing.SystemColors.Control;
			this.Label3.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label3.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label3.Location = new System.Drawing.Point(14, 300);
			this.Label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.Label3.Name = "Label3";
			this.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label3.Size = new System.Drawing.Size(104, 14);
			this.Label3.TabIndex = 14;
			this.Label3.Text = "Pre-defined objects:";
			// 
			// MainMenu1
			// 
			this.MainMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.menuEdit,
            this.mnuOptions});
			this.MainMenu1.Location = new System.Drawing.Point(0, 0);
			this.MainMenu1.Name = "MainMenu1";
			this.MainMenu1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
			this.MainMenu1.Size = new System.Drawing.Size(661, 24);
			this.MainMenu1.TabIndex = 28;
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuOpen,
            this.mnuImport,
            this.mnuFileSep0,
            this.mnuSave,
            this.mnuSaveAs,
            this.mnuSep0,
            this.mnuQuit});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new System.Drawing.Size(37, 22);
			this.mnuFile.Text = "&File";
			// 
			// mnuNew
			// 
			this.mnuNew.Image = global::XleMapEditor.Properties.Resources.NewDocumentHS;
			this.mnuNew.Name = "mnuNew";
			this.mnuNew.Size = new System.Drawing.Size(155, 22);
			this.mnuNew.Text = "&New...";
			this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
			// 
			// mnuOpen
			// 
			this.mnuOpen.Image = global::XleMapEditor.Properties.Resources.openHS;
			this.mnuOpen.Name = "mnuOpen";
			this.mnuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mnuOpen.Size = new System.Drawing.Size(155, 22);
			this.mnuOpen.Text = "&Open...";
			this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
			// 
			// mnuImport
			// 
			this.mnuImport.Name = "mnuImport";
			this.mnuImport.Size = new System.Drawing.Size(155, 22);
			this.mnuImport.Text = "&Import...";
			this.mnuImport.Click += new System.EventHandler(this.mnuImport_Click);
			// 
			// mnuFileSep0
			// 
			this.mnuFileSep0.Name = "mnuFileSep0";
			this.mnuFileSep0.Size = new System.Drawing.Size(152, 6);
			// 
			// mnuSave
			// 
			this.mnuSave.Image = global::XleMapEditor.Properties.Resources.saveHS;
			this.mnuSave.Name = "mnuSave";
			this.mnuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.mnuSave.Size = new System.Drawing.Size(155, 22);
			this.mnuSave.Text = "&Save";
			this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
			// 
			// mnuSaveAs
			// 
			this.mnuSaveAs.Name = "mnuSaveAs";
			this.mnuSaveAs.Size = new System.Drawing.Size(155, 22);
			this.mnuSaveAs.Text = "S&ave as...";
			this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
			// 
			// mnuSep0
			// 
			this.mnuSep0.Name = "mnuSep0";
			this.mnuSep0.Size = new System.Drawing.Size(152, 6);
			// 
			// mnuQuit
			// 
			this.mnuQuit.Name = "mnuQuit";
			this.mnuQuit.Size = new System.Drawing.Size(155, 22);
			this.mnuQuit.Text = "&Quit";
			this.mnuQuit.Click += new System.EventHandler(this.mnuQuit_Click);
			// 
			// menuEdit
			// 
			this.menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
			this.menuEdit.Name = "menuEdit";
			this.menuEdit.Size = new System.Drawing.Size(39, 22);
			this.menuEdit.Text = "Edit";
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Image = global::XleMapEditor.Properties.Resources.CopyHS;
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.copyToolStripMenuItem.Text = "Copy Tiles";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Image = global::XleMapEditor.Properties.Resources.PasteHS;
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.pasteToolStripMenuItem.Text = "Paste Tiles";
			this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
			// 
			// mnuOptions
			// 
			this.mnuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuProperties,
            this.mnuSep2,
            this.mnuPlaceSpecial,
            this.mnuModifySpecial,
            this.mnuDeleteSpecial,
            this.mnuSep1,
            this.mnuRefreshTiles});
			this.mnuOptions.Name = "mnuOptions";
			this.mnuOptions.Size = new System.Drawing.Size(43, 22);
			this.mnuOptions.Text = "&Map";
			// 
			// mnuProperties
			// 
			this.mnuProperties.Image = global::XleMapEditor.Properties.Resources.PropertiesHS;
			this.mnuProperties.Name = "mnuProperties";
			this.mnuProperties.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.mnuProperties.Size = new System.Drawing.Size(191, 22);
			this.mnuProperties.Text = "&Properties...";
			this.mnuProperties.Click += new System.EventHandler(this.mnuProperties_Click);
			// 
			// mnuSep2
			// 
			this.mnuSep2.Name = "mnuSep2";
			this.mnuSep2.Size = new System.Drawing.Size(188, 6);
			// 
			// mnuPlaceSpecial
			// 
			this.mnuPlaceSpecial.Name = "mnuPlaceSpecial";
			this.mnuPlaceSpecial.Size = new System.Drawing.Size(191, 22);
			this.mnuPlaceSpecial.Text = "Place &Special";
			// 
			// mnuModifySpecial
			// 
			this.mnuModifySpecial.Name = "mnuModifySpecial";
			this.mnuModifySpecial.Size = new System.Drawing.Size(191, 22);
			this.mnuModifySpecial.Text = "Modify &Special";
			// 
			// mnuDeleteSpecial
			// 
			this.mnuDeleteSpecial.Name = "mnuDeleteSpecial";
			this.mnuDeleteSpecial.Size = new System.Drawing.Size(191, 22);
			this.mnuDeleteSpecial.Text = "&Delete Special";
			// 
			// mnuSep1
			// 
			this.mnuSep1.Name = "mnuSep1";
			this.mnuSep1.Size = new System.Drawing.Size(188, 6);
			// 
			// mnuRefreshTiles
			// 
			this.mnuRefreshTiles.Name = "mnuRefreshTiles";
			this.mnuRefreshTiles.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
			this.mnuRefreshTiles.Size = new System.Drawing.Size(191, 22);
			this.mnuRefreshTiles.Text = "&Refresh Tiles";
			this.mnuRefreshTiles.Click += new System.EventHandler(this.mnuRefreshTiles_Click);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave,
            this.toolStripSeparator1,
            this.btnCopy,
            this.btnPaste,
            this.toolStripSeparator2,
            this.btnZoomOut,
            this.btnZoomIn,
            this.toolStripSeparator3,
            this.btnProperties});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(661, 25);
			this.toolStrip1.TabIndex = 31;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// btnNew
			// 
			this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnNew.Image = global::XleMapEditor.Properties.Resources.NewDocumentHS;
			this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(23, 22);
			this.btnNew.Text = "New Map";
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// btnOpen
			// 
			this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnOpen.Image = global::XleMapEditor.Properties.Resources.openHS;
			this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(23, 22);
			this.btnOpen.Text = "Open Map";
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// btnSave
			// 
			this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSave.Image = global::XleMapEditor.Properties.Resources.saveHS;
			this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(23, 22);
			this.btnSave.Text = "Save Map";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnCopy
			// 
			this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnCopy.Image = global::XleMapEditor.Properties.Resources.CopyHS;
			this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(23, 22);
			this.btnCopy.Text = "Copy Tiles";
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// btnPaste
			// 
			this.btnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnPaste.Image = global::XleMapEditor.Properties.Resources.PasteHS;
			this.btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnPaste.Name = "btnPaste";
			this.btnPaste.Size = new System.Drawing.Size(23, 22);
			this.btnPaste.Text = "Paste Tiles";
			this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// btnZoomOut
			// 
			this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomOut.Image = global::XleMapEditor.Properties.Resources.ZoomOut;
			this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomOut.Name = "btnZoomOut";
			this.btnZoomOut.Size = new System.Drawing.Size(23, 22);
			this.btnZoomOut.Text = "Zoom Out";
			this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
			// 
			// btnZoomIn
			// 
			this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomIn.Image = global::XleMapEditor.Properties.Resources.ZoomIn;
			this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomIn.Name = "btnZoomIn";
			this.btnZoomIn.Size = new System.Drawing.Size(23, 22);
			this.btnZoomIn.Text = "Zoom In";
			this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// btnProperties
			// 
			this.btnProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnProperties.Image = global::XleMapEditor.Properties.Resources.PropertiesHS;
			this.btnProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnProperties.Name = "btnProperties";
			this.btnProperties.Size = new System.Drawing.Size(23, 22);
			this.btnProperties.Text = "toolStripButton1";
			this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
			// 
			// mapView
			// 
			this.mapView.AllowBoxSelect = false;
			this.mapView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.mapView.Location = new System.Drawing.Point(0, 52);
			this.mapView.Name = "mapView";
			this.mapView.SelRect = new System.Drawing.Rectangle(0, 0, 19, 19);
			this.mapView.Size = new System.Drawing.Size(406, 541);
			this.mapView.State = null;
			this.mapView.TabIndex = 32;
			this.mapView.TileMouseDown += new System.EventHandler<XleMapEditor.TileMouseEventArgs>(this.mapView_TileMouseDown);
			this.mapView.TileMouseMove += new System.EventHandler<XleMapEditor.TileMouseEventArgs>(this.mapView_TileMouseMove);
			// 
			// frmMapEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(661, 621);
			this.Controls.Add(this.mapView);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.StatusBar1);
			this.Controls.Add(this.frmRight);
			this.Controls.Add(this.MainMenu1);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Location = new System.Drawing.Point(6, 44);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "frmMapEdit";
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Xle Map Editor";
			this.Activated += new System.EventHandler(this.frmMEdit_Activated);
			this.Deactivate += new System.EventHandler(this.frmMEdit_Deactivate);
			this.Load += new System.EventHandler(this.frmMEdit_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMEdit_Paint);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMEdit_KeyDown);
			this.StatusBar1.ResumeLayout(false);
			this.StatusBar1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudEvent)).EndInit();
			this.frmRight.ResumeLayout(false);
			this.frmRight.PerformLayout();
			this.MainMenu1.ResumeLayout(false);
			this.MainMenu1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button cmdPlaceEvent;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.NumericUpDown nudEvent;
		private System.Windows.Forms.Label lblEventCount;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnFindEvent;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton btnNew;
		private System.Windows.Forms.ToolStripButton btnOpen;
		private System.Windows.Forms.ToolStripButton btnSave;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnCopy;
		private System.Windows.Forms.ToolStripButton btnPaste;
		private System.Windows.Forms.ToolStripMenuItem menuEdit;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton btnProperties;
		private XleMapView mapView;
		private TilePicker tilePicker;
		private System.Windows.Forms.ToolStripButton btnZoomIn;
		private System.Windows.Forms.ToolStripButton btnZoomOut;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
	}
}
