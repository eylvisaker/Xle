#define Win32
using System.Diagnostics;
using System;
using System.Windows.Forms;
using AxMSComCtl2;
using ERY.Xle;
using ERY.AgateLib;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Collections;
using Microsoft.VisualBasic.Compatibility;
using System.Linq;
using System.IO;
using ERY.AgateLib.WinForms;

namespace XleMapEditor
{
	[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]public partial class frmMEdit
	{
		#region "Windows Form Designer generated code "
		[System.Diagnostics.DebuggerNonUserCode()]public frmMEdit()
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
		private System.ComponentModel.Container components = null;
		public System.Windows.Forms.ToolTip ToolTip1;
		public System.Windows.Forms.ToolStripStatusLabel _StatusBar1_Panel1;
		public System.Windows.Forms.StatusStrip StatusBar1;
		public System.Windows.Forms.Button cmdFill;
		public System.Windows.Forms.PictureBox Picture2;
		public System.Windows.Forms.TextBox Text7;
		public System.Windows.Forms.CheckBox chkRandom;
		public System.Windows.Forms.Label lblSpcCount;
		public System.Windows.Forms.Label lblFindSpecial;
		public System.Windows.Forms.Label Label5;
		public System.Windows.Forms.Label Label4;
		public System.Windows.Forms.Panel frmBottom;
		public System.Windows.Forms.Button cmdPlaceSpecial;
		public System.Windows.Forms.Button cmdObject;
		public System.Windows.Forms.Button cmdGuard;
		public System.Windows.Forms.Button cmdModifySpecial;
		public System.Windows.Forms.Button cmdDeleteSpecial;
		public System.Windows.Forms.ListBox lstPreDef;
		public System.Windows.Forms.Button cmdRoof;
		public System.Windows.Forms.CheckBox chkDrawRoof;
		public System.Windows.Forms.CheckBox chkRestrict;
		public System.Windows.Forms.CheckBox chkDrawGuards;
		public System.Windows.Forms.Label lblImport;
		public System.Windows.Forms.Label lblDim;
		public System.Windows.Forms.Label lblX;
		public System.Windows.Forms.Label lblY;
		public System.Windows.Forms.Label lblTile;
		public System.Windows.Forms.Label lblCurrentTile;
		public System.Windows.Forms.Label Label3;
		public System.Windows.Forms.Panel frmRight;
		public System.Windows.Forms.OpenFileDialog cmdDialogOpen;
		public System.Windows.Forms.SaveFileDialog cmdDialogSave;
		public System.Windows.Forms.PictureBox Picture1;
		public System.Windows.Forms.ToolStripMenuItem mnuNew;
		public System.Windows.Forms.ToolStripMenuItem mnuOpen;
		public System.Windows.Forms.ToolStripMenuItem mnuImport;
		public System.Windows.Forms.ToolStripSeparator mnuFileSep0;
		public System.Windows.Forms.ToolStripMenuItem mnuSave;
		public System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
		public System.Windows.Forms.ToolStripMenuItem mnuFinalize;
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
		public System.Windows.Forms.ToolStripMenuItem mnuImportRefresh;
		public System.Windows.Forms.ToolStripMenuItem mnuParameters;
		public System.Windows.Forms.ToolStripSeparator mnuImportSep0;
		public System.Windows.Forms.ToolStripMenuItem mnuSaveMapping;
		public System.Windows.Forms.ToolStripMenuItem mnuLoadMapping;
		public System.Windows.Forms.ToolStripMenuItem mnuTitleImport;
		public System.Windows.Forms.MenuStrip MainMenu1;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			base.Activated += new System.EventHandler(frmMEdit_Activated);
			base.Deactivate += new System.EventHandler(frmMEdit_Deactivate);
			base.Load += new System.EventHandler(frmMEdit_Load);
			base.Paint += new System.Windows.Forms.PaintEventHandler(frmMEdit_Paint);
			base.Resize += new System.EventHandler(frmMEdit_Resize);
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMEdit));
			this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.chkRestrict = new System.Windows.Forms.CheckBox();
			this.StatusBar1 = new System.Windows.Forms.StatusStrip();
			this._StatusBar1_Panel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.frmBottom = new System.Windows.Forms.Panel();
			this.cmdFill = new System.Windows.Forms.Button();
			this.cmdFill.Click += new System.EventHandler(cmdFill_Click);
			this.Picture2 = new System.Windows.Forms.PictureBox();
			this.Picture2.MouseDown += new System.Windows.Forms.MouseEventHandler(Picture2_MouseDown);
			this.Picture2.Paint += new System.Windows.Forms.PaintEventHandler(Picture2_Paint);
			this.Text7 = new System.Windows.Forms.TextBox();
			this.chkRandom = new System.Windows.Forms.CheckBox();
			this.lblSpcCount = new System.Windows.Forms.Label();
			this.lblFindSpecial = new System.Windows.Forms.Label();
			this.Label5 = new System.Windows.Forms.Label();
			this.Label4 = new System.Windows.Forms.Label();
			this.frmRight = new System.Windows.Forms.Panel();
			this.cmdPlaceSpecial = new System.Windows.Forms.Button();
			this.cmdPlaceSpecial.Click += new System.EventHandler(cmdPlaceSpecial_Click);
			this.cmdObject = new System.Windows.Forms.Button();
			this.cmdObject.Click += new System.EventHandler(cmdObject_Click);
			this.cmdGuard = new System.Windows.Forms.Button();
			this.cmdGuard.Click += new System.EventHandler(cmdGuard_Click);
			this.cmdModifySpecial = new System.Windows.Forms.Button();
			this.cmdModifySpecial.Click += new System.EventHandler(cmdModifySpecial_Click);
			this.cmdDeleteSpecial = new System.Windows.Forms.Button();
			this.cmdDeleteSpecial.Click += new System.EventHandler(cmdDeleteSpecial_Click);
			this.lstPreDef = new System.Windows.Forms.ListBox();
			this.cmdRoof = new System.Windows.Forms.Button();
			this.cmdRoof.Click += new System.EventHandler(cmdRoof_Click);
			this.chkDrawRoof = new System.Windows.Forms.CheckBox();
			this.chkDrawRoof.CheckStateChanged += new System.EventHandler(chkDrawRoof_CheckStateChanged);
			this.chkDrawGuards = new System.Windows.Forms.CheckBox();
			this.chkDrawGuards.CheckStateChanged += new System.EventHandler(chkDrawGuards_CheckStateChanged);
			this.lblImport = new System.Windows.Forms.Label();
			this.lblDim = new System.Windows.Forms.Label();
			this.lblX = new System.Windows.Forms.Label();
			this.lblX.Click += new System.EventHandler(lblX_Click);
			this.lblY = new System.Windows.Forms.Label();
			this.lblY.Click += new System.EventHandler(lblY_Click);
			this.lblTile = new System.Windows.Forms.Label();
			this.lblCurrentTile = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.cmdDialogOpen = new System.Windows.Forms.OpenFileDialog();
			this.cmdDialogSave = new System.Windows.Forms.SaveFileDialog();
			this.Picture1 = new System.Windows.Forms.PictureBox();
			this.Picture1.MouseDown += new System.Windows.Forms.MouseEventHandler(Picture1_MouseDown);
			this.Picture1.MouseMove += new System.Windows.Forms.MouseEventHandler(Picture1_MouseMove);
			this.Picture1.Paint += new System.Windows.Forms.PaintEventHandler(Picture1_Paint);
			this.MainMenu1 = new System.Windows.Forms.MenuStrip();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNew.Click += new System.EventHandler(mnuNew_Click);
			this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpen.Click += new System.EventHandler(mnuOpen_Click);
			this.mnuImport = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuImport.Click += new System.EventHandler(mnuImport_Click);
			this.mnuFileSep0 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSave.Click += new System.EventHandler(mnuSave_Click);
			this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAs.Click += new System.EventHandler(mnuSaveAs_Click);
			this.mnuFinalize = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFinalize.Click += new System.EventHandler(mnuFinalize_Click);
			this.mnuSep0 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuQuit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuQuit.Click += new System.EventHandler(mnuQuit_Click);
			this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuProperties.Click += new System.EventHandler(mnuProperties_Click);
			this.mnuSep2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuPlaceSpecial = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuModifySpecial = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDeleteSpecial = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSep1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRefreshTiles = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRefreshTiles.Click += new System.EventHandler(mnuRefreshTiles_Click);
			this.mnuTitleImport = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuImportRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuImportRefresh.Click += new System.EventHandler(mnuImportRefresh_Click);
			this.mnuParameters = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuParameters.Click += new System.EventHandler(mnuParameters_Click);
			this.mnuImportSep0 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSaveMapping = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveMapping.Click += new System.EventHandler(mnuSaveMapping_Click);
			this.mnuLoadMapping = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuLoadMapping.Click += new System.EventHandler(mnuLoadMapping_Click);
			this.sbDown = new System.Windows.Forms.VScrollBar();
			this.sbRight1 = new System.Windows.Forms.HScrollBar();
			this.sbSpecial = new System.Windows.Forms.HScrollBar();
			this.StatusBar1.SuspendLayout();
			this.frmBottom.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) this.Picture2).BeginInit();
			this.frmRight.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) this.Picture1).BeginInit();
			this.MainMenu1.SuspendLayout();
			this.SuspendLayout();
			//
			//chkRestrict
			//
			this.chkRestrict.BackColor = System.Drawing.SystemColors.Control;
			this.chkRestrict.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkRestrict.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.chkRestrict.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkRestrict.Location = new System.Drawing.Point(8, 272);
			this.chkRestrict.Name = "chkRestrict";
			this.chkRestrict.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkRestrict.Size = new System.Drawing.Size(97, 35);
			this.chkRestrict.TabIndex = 5;
			this.chkRestrict.Text = "Restrict Drawing";
			this.ToolTip1.SetToolTip(this.chkRestrict, "Restricts tiles drawn to selection");
			this.chkRestrict.UseVisualStyleBackColor = false;
			//
			//StatusBar1
			//
			this.StatusBar1.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.StatusBar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this._StatusBar1_Panel1});
			this.StatusBar1.Location = new System.Drawing.Point(0, 663);
			this.StatusBar1.Name = "StatusBar1";
			this.StatusBar1.Size = new System.Drawing.Size(780, 25);
			this.StatusBar1.TabIndex = 27;
			//
			//_StatusBar1_Panel1
			//
			this._StatusBar1_Panel1.AutoSize = false;
			this._StatusBar1_Panel1.BorderSides = (System.Windows.Forms.ToolStripStatusLabelBorderSides) (((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom);
			this._StatusBar1_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this._StatusBar1_Panel1.Margin = new System.Windows.Forms.Padding(0);
			this._StatusBar1_Panel1.Name = "_StatusBar1_Panel1";
			this._StatusBar1_Panel1.Size = new System.Drawing.Size(96, 25);
			this._StatusBar1_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			//frmBottom
			//
			this.frmBottom.BackColor = System.Drawing.SystemColors.Control;
			this.frmBottom.Controls.Add(this.sbSpecial);
			this.frmBottom.Controls.Add(this.cmdFill);
			this.frmBottom.Controls.Add(this.Picture2);
			this.frmBottom.Controls.Add(this.Text7);
			this.frmBottom.Controls.Add(this.chkRandom);
			this.frmBottom.Controls.Add(this.lblSpcCount);
			this.frmBottom.Controls.Add(this.lblFindSpecial);
			this.frmBottom.Controls.Add(this.Label5);
			this.frmBottom.Controls.Add(this.Label4);
			this.frmBottom.Cursor = System.Windows.Forms.Cursors.Default;
			this.frmBottom.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.frmBottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this.frmBottom.Location = new System.Drawing.Point(10, 371);
			this.frmBottom.Name = "frmBottom";
			this.frmBottom.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.frmBottom.Size = new System.Drawing.Size(753, 281);
			this.frmBottom.TabIndex = 20;
			this.frmBottom.Text = "Frame1";
			//
			//cmdFill
			//
			this.cmdFill.BackColor = System.Drawing.SystemColors.Control;
			this.cmdFill.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdFill.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdFill.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdFill.Location = new System.Drawing.Point(528, 8);
			this.cmdFill.Name = "cmdFill";
			this.cmdFill.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdFill.Size = new System.Drawing.Size(73, 49);
			this.cmdFill.TabIndex = 28;
			this.cmdFill.Text = "Fill Selection";
			this.cmdFill.UseVisualStyleBackColor = false;
			//
			//Picture2
			//
			this.Picture2.BackColor = System.Drawing.SystemColors.Control;
			this.Picture2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Picture2.Cursor = System.Windows.Forms.Cursors.Default;
			this.Picture2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Picture2.Location = new System.Drawing.Point(16, 16);
			this.Picture2.Name = "Picture2";
			this.Picture2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Picture2.Size = new System.Drawing.Size(260, 260);
			this.Picture2.TabIndex = 23;
			this.Picture2.TabStop = false;
			//
			//Text7
			//
			this.Text7.AcceptsReturn = true;
			this.Text7.BackColor = System.Drawing.SystemColors.Window;
			this.Text7.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.Text7.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Text7.ForeColor = System.Drawing.SystemColors.WindowText;
			this.Text7.Location = new System.Drawing.Point(288, 32);
			this.Text7.MaxLength = 0;
			this.Text7.Multiline = true;
			this.Text7.Name = "Text7";
			this.Text7.ReadOnly = true;
			this.Text7.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Text7.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.Text7.Size = new System.Drawing.Size(225, 249);
			this.Text7.TabIndex = 22;
			this.Text7.WordWrap = false;
			//
			//chkRandom
			//
			this.chkRandom.BackColor = System.Drawing.SystemColors.Control;
			this.chkRandom.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkRandom.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.chkRandom.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkRandom.Location = new System.Drawing.Point(288, 0);
			this.chkRandom.Name = "chkRandom";
			this.chkRandom.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkRandom.Size = new System.Drawing.Size(145, 25);
			this.chkRandom.TabIndex = 21;
			this.chkRandom.Text = "Randomize Tiles";
			this.chkRandom.UseVisualStyleBackColor = false;
			//
			//lblSpcCount
			//
			this.lblSpcCount.BackColor = System.Drawing.SystemColors.Control;
			this.lblSpcCount.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblSpcCount.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.lblSpcCount.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblSpcCount.Location = new System.Drawing.Point(536, 128);
			this.lblSpcCount.Name = "lblSpcCount";
			this.lblSpcCount.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblSpcCount.Size = new System.Drawing.Size(185, 17);
			this.lblSpcCount.TabIndex = 31;
			this.lblSpcCount.Text = "Specials: 0";
			//
			//lblFindSpecial
			//
			this.lblFindSpecial.BackColor = System.Drawing.SystemColors.Control;
			this.lblFindSpecial.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblFindSpecial.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.lblFindSpecial.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblFindSpecial.Location = new System.Drawing.Point(536, 104);
			this.lblFindSpecial.Name = "lblFindSpecial";
			this.lblFindSpecial.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblFindSpecial.Size = new System.Drawing.Size(145, 25);
			this.lblFindSpecial.TabIndex = 30;
			this.lblFindSpecial.Text = "Find Special:";
			//
			//Label5
			//
			this.Label5.BackColor = System.Drawing.SystemColors.Control;
			this.Label5.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label5.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label5.Location = new System.Drawing.Point(0, 16);
			this.Label5.Name = "Label5";
			this.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label5.Size = new System.Drawing.Size(17, 257);
			this.Label5.TabIndex = 25;
			//
			//Label4
			//
			this.Label4.BackColor = System.Drawing.SystemColors.Control;
			this.Label4.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label4.Location = new System.Drawing.Point(16, 0);
			this.Label4.Name = "Label4";
			this.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label4.Size = new System.Drawing.Size(281, 17);
			this.Label4.TabIndex = 24;
			this.Label4.Text = " 0    1   2   3    4   5   6    7   8   9   A    B   C   D   E   F";
			//
			//frmRight
			//
			this.frmRight.BackColor = System.Drawing.SystemColors.Control;
			this.frmRight.Controls.Add(this.cmdPlaceSpecial);
			this.frmRight.Controls.Add(this.cmdObject);
			this.frmRight.Controls.Add(this.cmdGuard);
			this.frmRight.Controls.Add(this.cmdModifySpecial);
			this.frmRight.Controls.Add(this.cmdDeleteSpecial);
			this.frmRight.Controls.Add(this.lstPreDef);
			this.frmRight.Controls.Add(this.cmdRoof);
			this.frmRight.Controls.Add(this.chkDrawRoof);
			this.frmRight.Controls.Add(this.chkRestrict);
			this.frmRight.Controls.Add(this.chkDrawGuards);
			this.frmRight.Controls.Add(this.lblImport);
			this.frmRight.Controls.Add(this.lblDim);
			this.frmRight.Controls.Add(this.lblX);
			this.frmRight.Controls.Add(this.lblY);
			this.frmRight.Controls.Add(this.lblTile);
			this.frmRight.Controls.Add(this.lblCurrentTile);
			this.frmRight.Controls.Add(this.Label3);
			this.frmRight.Cursor = System.Windows.Forms.Cursors.Default;
			this.frmRight.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.frmRight.ForeColor = System.Drawing.SystemColors.ControlText;
			this.frmRight.Location = new System.Drawing.Point(370, 27);
			this.frmRight.Name = "frmRight";
			this.frmRight.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.frmRight.Size = new System.Drawing.Size(385, 345);
			this.frmRight.TabIndex = 3;
			this.frmRight.Text = "Frame1";
			//
			//cmdPlaceSpecial
			//
			this.cmdPlaceSpecial.BackColor = System.Drawing.SystemColors.Control;
			this.cmdPlaceSpecial.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdPlaceSpecial.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdPlaceSpecial.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdPlaceSpecial.Location = new System.Drawing.Point(8, 16);
			this.cmdPlaceSpecial.Name = "cmdPlaceSpecial";
			this.cmdPlaceSpecial.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdPlaceSpecial.Size = new System.Drawing.Size(105, 33);
			this.cmdPlaceSpecial.TabIndex = 13;
			this.cmdPlaceSpecial.Text = "Place &Special";
			this.cmdPlaceSpecial.UseVisualStyleBackColor = false;
			//
			//cmdObject
			//
			this.cmdObject.BackColor = System.Drawing.SystemColors.Control;
			this.cmdObject.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdObject.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdObject.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdObject.Location = new System.Drawing.Point(8, 152);
			this.cmdObject.Name = "cmdObject";
			this.cmdObject.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdObject.Size = new System.Drawing.Size(105, 27);
			this.cmdObject.TabIndex = 12;
			this.cmdObject.Text = "Place &Object";
			this.cmdObject.UseVisualStyleBackColor = false;
			//
			//cmdGuard
			//
			this.cmdGuard.BackColor = System.Drawing.SystemColors.Control;
			this.cmdGuard.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdGuard.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdGuard.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdGuard.Location = new System.Drawing.Point(8, 120);
			this.cmdGuard.Name = "cmdGuard";
			this.cmdGuard.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdGuard.Size = new System.Drawing.Size(105, 27);
			this.cmdGuard.TabIndex = 11;
			this.cmdGuard.Text = "Place &Guard";
			this.cmdGuard.UseVisualStyleBackColor = false;
			//
			//cmdModifySpecial
			//
			this.cmdModifySpecial.BackColor = System.Drawing.SystemColors.Control;
			this.cmdModifySpecial.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdModifySpecial.Enabled = false;
			this.cmdModifySpecial.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdModifySpecial.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdModifySpecial.Location = new System.Drawing.Point(8, 48);
			this.cmdModifySpecial.Name = "cmdModifySpecial";
			this.cmdModifySpecial.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdModifySpecial.Size = new System.Drawing.Size(105, 33);
			this.cmdModifySpecial.TabIndex = 10;
			this.cmdModifySpecial.Text = "Modify &Special";
			this.cmdModifySpecial.UseVisualStyleBackColor = false;
			//
			//cmdDeleteSpecial
			//
			this.cmdDeleteSpecial.BackColor = System.Drawing.SystemColors.Control;
			this.cmdDeleteSpecial.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdDeleteSpecial.Enabled = false;
			this.cmdDeleteSpecial.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdDeleteSpecial.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdDeleteSpecial.Location = new System.Drawing.Point(8, 80);
			this.cmdDeleteSpecial.Name = "cmdDeleteSpecial";
			this.cmdDeleteSpecial.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdDeleteSpecial.Size = new System.Drawing.Size(105, 33);
			this.cmdDeleteSpecial.TabIndex = 9;
			this.cmdDeleteSpecial.Text = "&Delete Special";
			this.cmdDeleteSpecial.UseVisualStyleBackColor = false;
			//
			//lstPreDef
			//
			this.lstPreDef.BackColor = System.Drawing.SystemColors.Window;
			this.lstPreDef.Cursor = System.Windows.Forms.Cursors.Default;
			this.lstPreDef.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.lstPreDef.ForeColor = System.Drawing.SystemColors.WindowText;
			this.lstPreDef.ItemHeight = 14;
			this.lstPreDef.Location = new System.Drawing.Point(120, 168);
			this.lstPreDef.Name = "lstPreDef";
			this.lstPreDef.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lstPreDef.Size = new System.Drawing.Size(161, 158);
			this.lstPreDef.TabIndex = 8;
			//
			//cmdRoof
			//
			this.cmdRoof.BackColor = System.Drawing.SystemColors.Control;
			this.cmdRoof.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdRoof.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdRoof.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdRoof.Location = new System.Drawing.Point(8, 184);
			this.cmdRoof.Name = "cmdRoof";
			this.cmdRoof.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdRoof.Size = new System.Drawing.Size(105, 25);
			this.cmdRoof.TabIndex = 7;
			this.cmdRoof.Text = "&Roof";
			this.cmdRoof.UseVisualStyleBackColor = false;
			//
			//chkDrawRoof
			//
			this.chkDrawRoof.BackColor = System.Drawing.SystemColors.Control;
			this.chkDrawRoof.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkDrawRoof.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.chkDrawRoof.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkDrawRoof.Location = new System.Drawing.Point(8, 216);
			this.chkDrawRoof.Name = "chkDrawRoof";
			this.chkDrawRoof.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkDrawRoof.Size = new System.Drawing.Size(105, 25);
			this.chkDrawRoof.TabIndex = 6;
			this.chkDrawRoof.Text = "Show Roof";
			this.chkDrawRoof.UseVisualStyleBackColor = false;
			//
			//chkDrawGuards
			//
			this.chkDrawGuards.BackColor = System.Drawing.SystemColors.Control;
			this.chkDrawGuards.Checked = true;
			this.chkDrawGuards.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDrawGuards.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkDrawGuards.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.chkDrawGuards.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkDrawGuards.Location = new System.Drawing.Point(8, 240);
			this.chkDrawGuards.Name = "chkDrawGuards";
			this.chkDrawGuards.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkDrawGuards.Size = new System.Drawing.Size(105, 19);
			this.chkDrawGuards.TabIndex = 4;
			this.chkDrawGuards.Text = "Show Guards";
			this.chkDrawGuards.UseVisualStyleBackColor = false;
			//
			//lblImport
			//
			this.lblImport.BackColor = System.Drawing.SystemColors.Control;
			this.lblImport.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblImport.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblImport.Location = new System.Drawing.Point(120, 80);
			this.lblImport.Name = "lblImport";
			this.lblImport.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblImport.Size = new System.Drawing.Size(257, 17);
			this.lblImport.TabIndex = 26;
			this.lblImport.Text = "Import:";
			//
			//lblDim
			//
			this.lblDim.BackColor = System.Drawing.SystemColors.Control;
			this.lblDim.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblDim.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.lblDim.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblDim.Location = new System.Drawing.Point(120, 8);
			this.lblDim.Name = "lblDim";
			this.lblDim.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblDim.Size = new System.Drawing.Size(272, 25);
			this.lblDim.TabIndex = 19;
			this.lblDim.Text = "Map Dimensions";
			//
			//lblX
			//
			this.lblX.BackColor = System.Drawing.SystemColors.Control;
			this.lblX.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblX.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblX.Location = new System.Drawing.Point(120, 32);
			this.lblX.Name = "lblX";
			this.lblX.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblX.Size = new System.Drawing.Size(272, 17);
			this.lblX.TabIndex = 18;
			this.lblX.Text = "X:";
			//
			//lblY
			//
			this.lblY.BackColor = System.Drawing.SystemColors.Control;
			this.lblY.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblY.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblY.Location = new System.Drawing.Point(120, 48);
			this.lblY.Name = "lblY";
			this.lblY.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblY.Size = new System.Drawing.Size(256, 17);
			this.lblY.TabIndex = 17;
			this.lblY.Text = "Y:";
			//
			//lblTile
			//
			this.lblTile.BackColor = System.Drawing.SystemColors.Control;
			this.lblTile.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblTile.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblTile.Location = new System.Drawing.Point(120, 64);
			this.lblTile.Name = "lblTile";
			this.lblTile.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblTile.Size = new System.Drawing.Size(248, 17);
			this.lblTile.TabIndex = 16;
			this.lblTile.Text = "Tile:";
			//
			//lblCurrentTile
			//
			this.lblCurrentTile.BackColor = System.Drawing.SystemColors.Control;
			this.lblCurrentTile.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblCurrentTile.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblCurrentTile.Location = new System.Drawing.Point(120, 96);
			this.lblCurrentTile.Name = "lblCurrentTile";
			this.lblCurrentTile.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblCurrentTile.Size = new System.Drawing.Size(256, 17);
			this.lblCurrentTile.TabIndex = 15;
			this.lblCurrentTile.Text = "current tile";
			//
			//Label3
			//
			this.Label3.BackColor = System.Drawing.SystemColors.Control;
			this.Label3.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label3.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label3.Location = new System.Drawing.Point(120, 144);
			this.Label3.Name = "Label3";
			this.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label3.Size = new System.Drawing.Size(129, 17);
			this.Label3.TabIndex = 14;
			this.Label3.Text = "Pre-defined objects:";
			//
			//Picture1
			//
			this.Picture1.BackColor = System.Drawing.SystemColors.Control;
			this.Picture1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Picture1.Cursor = System.Windows.Forms.Cursors.Default;
			this.Picture1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Picture1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Picture1.Location = new System.Drawing.Point(10, 27);
			this.Picture1.Name = "Picture1";
			this.Picture1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Picture1.Size = new System.Drawing.Size(336, 320);
			this.Picture1.TabIndex = 0;
			this.Picture1.TabStop = false;
			//
			//MainMenu1
			//
			this.MainMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mnuFile, this.mnuOptions, this.mnuTitleImport});
			this.MainMenu1.Location = new System.Drawing.Point(0, 0);
			this.MainMenu1.Name = "MainMenu1";
			this.MainMenu1.Size = new System.Drawing.Size(780, 24);
			this.MainMenu1.TabIndex = 28;
			//
			//mnuFile
			//
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mnuNew, this.mnuOpen, this.mnuImport, this.mnuFileSep0, this.mnuSave, this.mnuSaveAs, this.mnuFinalize, this.mnuSep0, this.mnuQuit});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new System.Drawing.Size(37, 20);
			this.mnuFile.Text = "&File";
			//
			//mnuNew
			//
			this.mnuNew.Name = "mnuNew";
			this.mnuNew.Size = new System.Drawing.Size(162, 22);
			this.mnuNew.Text = "&New...";
			//
			//mnuOpen
			//
			this.mnuOpen.Name = "mnuOpen";
			this.mnuOpen.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O);
			this.mnuOpen.Size = new System.Drawing.Size(162, 22);
			this.mnuOpen.Text = "&Open...";
			//
			//mnuImport
			//
			this.mnuImport.Name = "mnuImport";
			this.mnuImport.Size = new System.Drawing.Size(162, 22);
			this.mnuImport.Text = "&Import...";
			//
			//mnuFileSep0
			//
			this.mnuFileSep0.Name = "mnuFileSep0";
			this.mnuFileSep0.Size = new System.Drawing.Size(159, 6);
			//
			//mnuSave
			//
			this.mnuSave.Enabled = false;
			this.mnuSave.Name = "mnuSave";
			this.mnuSave.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S);
			this.mnuSave.Size = new System.Drawing.Size(162, 22);
			this.mnuSave.Text = "&Save";
			//
			//mnuSaveAs
			//
			this.mnuSaveAs.Name = "mnuSaveAs";
			this.mnuSaveAs.Size = new System.Drawing.Size(162, 22);
			this.mnuSaveAs.Text = "S&ave as...";
			//
			//mnuFinalize
			//
			this.mnuFinalize.Name = "mnuFinalize";
			this.mnuFinalize.Size = new System.Drawing.Size(162, 22);
			this.mnuFinalize.Text = "&Finalize && Save...";
			//
			//mnuSep0
			//
			this.mnuSep0.Name = "mnuSep0";
			this.mnuSep0.Size = new System.Drawing.Size(159, 6);
			//
			//mnuQuit
			//
			this.mnuQuit.Name = "mnuQuit";
			this.mnuQuit.Size = new System.Drawing.Size(162, 22);
			this.mnuQuit.Text = "&Quit";
			//
			//mnuOptions
			//
			this.mnuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mnuProperties, this.mnuSep2, this.mnuPlaceSpecial, this.mnuModifySpecial, this.mnuDeleteSpecial, this.mnuSep1, this.mnuRefreshTiles});
			this.mnuOptions.Name = "mnuOptions";
			this.mnuOptions.Size = new System.Drawing.Size(43, 20);
			this.mnuOptions.Text = "&Map";
			//
			//mnuProperties
			//
			this.mnuProperties.Name = "mnuProperties";
			this.mnuProperties.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P);
			this.mnuProperties.Size = new System.Drawing.Size(191, 22);
			this.mnuProperties.Text = "&Properties...";
			//
			//mnuSep2
			//
			this.mnuSep2.Name = "mnuSep2";
			this.mnuSep2.Size = new System.Drawing.Size(188, 6);
			//
			//mnuPlaceSpecial
			//
			this.mnuPlaceSpecial.Name = "mnuPlaceSpecial";
			this.mnuPlaceSpecial.Size = new System.Drawing.Size(191, 22);
			this.mnuPlaceSpecial.Text = "Place &Special";
			//
			//mnuModifySpecial
			//
			this.mnuModifySpecial.Name = "mnuModifySpecial";
			this.mnuModifySpecial.Size = new System.Drawing.Size(191, 22);
			this.mnuModifySpecial.Text = "Modify &Special";
			//
			//mnuDeleteSpecial
			//
			this.mnuDeleteSpecial.Name = "mnuDeleteSpecial";
			this.mnuDeleteSpecial.Size = new System.Drawing.Size(191, 22);
			this.mnuDeleteSpecial.Text = "&Delete Special";
			//
			//mnuSep1
			//
			this.mnuSep1.Name = "mnuSep1";
			this.mnuSep1.Size = new System.Drawing.Size(188, 6);
			//
			//mnuRefreshTiles
			//
			this.mnuRefreshTiles.Name = "mnuRefreshTiles";
			this.mnuRefreshTiles.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5);
			this.mnuRefreshTiles.Size = new System.Drawing.Size(191, 22);
			this.mnuRefreshTiles.Text = "&Refresh Tiles";
			//
			//mnuTitleImport
			//
			this.mnuTitleImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mnuImportRefresh, this.mnuParameters, this.mnuImportSep0, this.mnuSaveMapping, this.mnuLoadMapping});
			this.mnuTitleImport.Name = "mnuTitleImport";
			this.mnuTitleImport.Size = new System.Drawing.Size(55, 20);
			this.mnuTitleImport.Text = "&Import";
			//
			//mnuImportRefresh
			//
			this.mnuImportRefresh.Name = "mnuImportRefresh";
			this.mnuImportRefresh.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R);
			this.mnuImportRefresh.Size = new System.Drawing.Size(165, 22);
			this.mnuImportRefresh.Text = "&Refresh";
			//
			//mnuParameters
			//
			this.mnuParameters.Name = "mnuParameters";
			this.mnuParameters.Size = new System.Drawing.Size(165, 22);
			this.mnuParameters.Text = "&Parameters...";
			//
			//mnuImportSep0
			//
			this.mnuImportSep0.Name = "mnuImportSep0";
			this.mnuImportSep0.Size = new System.Drawing.Size(162, 6);
			//
			//mnuSaveMapping
			//
			this.mnuSaveMapping.Name = "mnuSaveMapping";
			this.mnuSaveMapping.Size = new System.Drawing.Size(165, 22);
			this.mnuSaveMapping.Text = "&Save Mappings...";
			//
			//mnuLoadMapping
			//
			this.mnuLoadMapping.Name = "mnuLoadMapping";
			this.mnuLoadMapping.Size = new System.Drawing.Size(165, 22);
			this.mnuLoadMapping.Text = "&Load Mappings...";
			//
			//sbDown
			//
			this.sbDown.Location = new System.Drawing.Point(346, 27);
			this.sbDown.Maximum = 32767;
			this.sbDown.Name = "sbDown";
			this.sbDown.Size = new System.Drawing.Size(17, 320);
			this.sbDown.TabIndex = 29;
			//
			//sbRight1
			//
			this.sbRight1.Location = new System.Drawing.Point(10, 347);
			this.sbRight1.Maximum = 32767;
			this.sbRight1.Name = "sbRight1";
			this.sbRight1.Size = new System.Drawing.Size(336, 17);
			this.sbRight1.TabIndex = 1;
			//
			//sbSpecial
			//
			this.sbSpecial.Location = new System.Drawing.Point(539, 87);
			this.sbSpecial.Name = "sbSpecial";
			this.sbSpecial.Size = new System.Drawing.Size(189, 17);
			this.sbSpecial.TabIndex = 32;
			//
			//frmMEdit
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) 9.0, (float) 19.0);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(780, 688);
			this.Controls.Add(this.sbRight1);
			this.Controls.Add(this.sbDown);
			this.Controls.Add(this.StatusBar1);
			this.Controls.Add(this.frmBottom);
			this.Controls.Add(this.frmRight);
			this.Controls.Add(this.Picture1);
			this.Controls.Add(this.MainMenu1);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Font = new System.Drawing.Font("Times New Roman", 12.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Icon = (System.Drawing.Icon) (resources.GetObject("$this.Icon"));
			this.Location = new System.Drawing.Point(6, 44);
			this.Name = "frmMEdit";
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Form1";
			this.StatusBar1.ResumeLayout(false);
			this.StatusBar1.PerformLayout();
			this.frmBottom.ResumeLayout(false);
			this.frmBottom.PerformLayout();
			((System.ComponentModel.ISupportInitialize) this.Picture2).EndInit();
			this.frmRight.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize) this.Picture1).EndInit();
			this.MainMenu1.ResumeLayout(false);
			this.MainMenu1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
			
		}
		internal System.Windows.Forms.VScrollBar sbDown;
		internal System.Windows.Forms.HScrollBar sbRight1;
		internal System.Windows.Forms.HScrollBar sbSpecial;
		#endregion
	}
}
