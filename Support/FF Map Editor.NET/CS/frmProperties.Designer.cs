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
	[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]public partial class frmProperties
	{
		#region "Windows Form Designer generated code "
		[System.Diagnostics.DebuggerNonUserCode()]public frmProperties()
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
		public System.Windows.Forms.TextBox _txtMail_3;
		public System.Windows.Forms.TextBox _txtMail_2;
		public System.Windows.Forms.TextBox _txtMail_1;
		public System.Windows.Forms.TextBox _txtMail_0;
		public System.Windows.Forms.TextBox txtBuyRaftMap;
		public System.Windows.Forms.TextBox txtBuyRaftY;
		public System.Windows.Forms.TextBox txtBuyRaftX;
		public System.Windows.Forms.TextBox txtDefaultTile;
		public System.Windows.Forms.Label Label15;
		public System.Windows.Forms.Label Label14;
		public System.Windows.Forms.Label Label13;
		public System.Windows.Forms.Label Label12;
		public System.Windows.Forms.Label lblDefaultTile;
		public System.Windows.Forms.GroupBox frmChar;
		public System.Windows.Forms.TextBox txtDungLevels;
		public System.Windows.Forms.TextBox txtDungMonster;
		public System.Windows.Forms.Label Label11;
		public System.Windows.Forms.Label Label10;
		public System.Windows.Forms.GroupBox frmDungeon;
		public System.Windows.Forms.RadioButton _optTiles_2;
		public System.Windows.Forms.RadioButton _optTiles_1;
		public System.Windows.Forms.RadioButton _optTiles_0;
		public System.Windows.Forms.RadioButton _optTiles_4;
		public System.Windows.Forms.RadioButton _optTiles_3;
		public System.Windows.Forms.GroupBox Frame2;
		public System.Windows.Forms.Button cmdDefaults;
		public System.Windows.Forms.TextBox txtHP;
		public System.Windows.Forms.TextBox txtAttack;
		public System.Windows.Forms.TextBox txtDefense;
		public System.Windows.Forms.TextBox txtColor;
		public System.Windows.Forms.Label Label4;
		public System.Windows.Forms.Label Label5;
		public System.Windows.Forms.Label Label6;
		public System.Windows.Forms.Label Label7;
		public System.Windows.Forms.GroupBox frmGuards;
		public System.Windows.Forms.TextBox txtWidth;
		public System.Windows.Forms.TextBox txtHeight;
		public System.Windows.Forms.TextBox txtName;
		public System.Windows.Forms.Label Label8;
		public System.Windows.Forms.Label Label3;
		public System.Windows.Forms.Label Label1;
		public System.Windows.Forms.GroupBox Frame1;
		public System.Windows.Forms.Button cmdCancel;
		public System.Windows.Forms.Button cmdOK;
		public Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray optTiles;
		public Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray optType;
		public Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray txtMail;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			base.Paint += new System.Windows.Forms.PaintEventHandler(frmProperties_Paint);
			this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.frmChar = new System.Windows.Forms.GroupBox();
			this._txtMail_3 = new System.Windows.Forms.TextBox();
			this._txtMail_2 = new System.Windows.Forms.TextBox();
			this._txtMail_1 = new System.Windows.Forms.TextBox();
			this._txtMail_0 = new System.Windows.Forms.TextBox();
			this.txtBuyRaftMap = new System.Windows.Forms.TextBox();
			this.txtBuyRaftY = new System.Windows.Forms.TextBox();
			this.txtBuyRaftX = new System.Windows.Forms.TextBox();
			this.txtDefaultTile = new System.Windows.Forms.TextBox();
			this.txtDefaultTile.TextChanged += new System.EventHandler(txtDefaultTile_TextChanged);
			this.Label15 = new System.Windows.Forms.Label();
			this.Label14 = new System.Windows.Forms.Label();
			this.Label13 = new System.Windows.Forms.Label();
			this.Label12 = new System.Windows.Forms.Label();
			this.lblDefaultTile = new System.Windows.Forms.Label();
			this.frmDungeon = new System.Windows.Forms.GroupBox();
			this.txtDungLevels = new System.Windows.Forms.TextBox();
			this.txtDungMonster = new System.Windows.Forms.TextBox();
			this.Label11 = new System.Windows.Forms.Label();
			this.Label10 = new System.Windows.Forms.Label();
			this.Frame2 = new System.Windows.Forms.GroupBox();
			this._optTiles_2 = new System.Windows.Forms.RadioButton();
			this._optTiles_1 = new System.Windows.Forms.RadioButton();
			this._optTiles_0 = new System.Windows.Forms.RadioButton();
			this._optTiles_4 = new System.Windows.Forms.RadioButton();
			this._optTiles_3 = new System.Windows.Forms.RadioButton();
			this.cmdDefaults = new System.Windows.Forms.Button();
			this.cmdDefaults.Click += new System.EventHandler(cmdDefaults_Click);
			this.frmGuards = new System.Windows.Forms.GroupBox();
			this.txtHP = new System.Windows.Forms.TextBox();
			this.txtHP.TextChanged += new System.EventHandler(txtHP_TextChanged);
			this.txtAttack = new System.Windows.Forms.TextBox();
			this.txtAttack.TextChanged += new System.EventHandler(txtAttack_TextChanged);
			this.txtDefense = new System.Windows.Forms.TextBox();
			this.txtDefense.TextChanged += new System.EventHandler(txtDefense_TextChanged);
			this.txtColor = new System.Windows.Forms.TextBox();
			this.txtColor.TextChanged += new System.EventHandler(txtColor_TextChanged);
			this.Label4 = new System.Windows.Forms.Label();
			this.Label5 = new System.Windows.Forms.Label();
			this.Label6 = new System.Windows.Forms.Label();
			this.Label7 = new System.Windows.Forms.Label();
			this.Frame1 = new System.Windows.Forms.GroupBox();
			this.cboTypes = new System.Windows.Forms.ComboBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.txtWidth = new System.Windows.Forms.TextBox();
			this.txtWidth.TextChanged += new System.EventHandler(txtWidth_TextChanged);
			this.txtHeight = new System.Windows.Forms.TextBox();
			this.txtHeight.TextChanged += new System.EventHandler(txtHeight_TextChanged);
			this.txtName = new System.Windows.Forms.TextBox();
			this.txtName.TextChanged += new System.EventHandler(txtName_TextChanged);
			this.Label8 = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.Label1 = new System.Windows.Forms.Label();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdCancel.Click += new System.EventHandler(cmdCancel_Click);
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdOK.Click += new System.EventHandler(cmdOK_Click);
			this.optTiles = new Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(this.components);
			this.optType = new Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(this.components);
			this.optType.CheckedChanged += new System.EventHandler(optType_CheckedChanged);
			this.txtMail = new Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(this.components);
			this.frmChar.SuspendLayout();
			this.frmDungeon.SuspendLayout();
			this.Frame2.SuspendLayout();
			this.frmGuards.SuspendLayout();
			this.Frame1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) this.optTiles).BeginInit();
			((System.ComponentModel.ISupportInitialize) this.optType).BeginInit();
			((System.ComponentModel.ISupportInitialize) this.txtMail).BeginInit();
			this.SuspendLayout();
			//
			//frmChar
			//
			this.frmChar.BackColor = System.Drawing.SystemColors.Control;
			this.frmChar.Controls.Add(this._txtMail_3);
			this.frmChar.Controls.Add(this._txtMail_2);
			this.frmChar.Controls.Add(this._txtMail_1);
			this.frmChar.Controls.Add(this._txtMail_0);
			this.frmChar.Controls.Add(this.txtBuyRaftMap);
			this.frmChar.Controls.Add(this.txtBuyRaftY);
			this.frmChar.Controls.Add(this.txtBuyRaftX);
			this.frmChar.Controls.Add(this.txtDefaultTile);
			this.frmChar.Controls.Add(this.Label15);
			this.frmChar.Controls.Add(this.Label14);
			this.frmChar.Controls.Add(this.Label13);
			this.frmChar.Controls.Add(this.Label12);
			this.frmChar.Controls.Add(this.lblDefaultTile);
			this.frmChar.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.frmChar.ForeColor = System.Drawing.SystemColors.ControlText;
			this.frmChar.Location = new System.Drawing.Point(8, 264);
			this.frmChar.Name = "frmChar";
			this.frmChar.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.frmChar.Size = new System.Drawing.Size(193, 169);
			this.frmChar.TabIndex = 34;
			this.frmChar.TabStop = false;
			this.frmChar.Text = "Characteristics";
			//
			//_txtMail_3
			//
			this._txtMail_3.AcceptsReturn = true;
			this._txtMail_3.BackColor = System.Drawing.SystemColors.Window;
			this._txtMail_3.Cursor = System.Windows.Forms.Cursors.IBeam;
			this._txtMail_3.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this._txtMail_3.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtMail.SetIndex(this._txtMail_3, 3);
			this._txtMail_3.Location = new System.Drawing.Point(136, 128);
			this._txtMail_3.MaxLength = 0;
			this._txtMail_3.Name = "_txtMail_3";
			this._txtMail_3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._txtMail_3.Size = new System.Drawing.Size(33, 20);
			this._txtMail_3.TabIndex = 54;
			//
			//_txtMail_2
			//
			this._txtMail_2.AcceptsReturn = true;
			this._txtMail_2.BackColor = System.Drawing.SystemColors.Window;
			this._txtMail_2.Cursor = System.Windows.Forms.Cursors.IBeam;
			this._txtMail_2.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this._txtMail_2.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtMail.SetIndex(this._txtMail_2, 2);
			this._txtMail_2.Location = new System.Drawing.Point(96, 128);
			this._txtMail_2.MaxLength = 0;
			this._txtMail_2.Name = "_txtMail_2";
			this._txtMail_2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._txtMail_2.Size = new System.Drawing.Size(33, 20);
			this._txtMail_2.TabIndex = 53;
			//
			//_txtMail_1
			//
			this._txtMail_1.AcceptsReturn = true;
			this._txtMail_1.BackColor = System.Drawing.SystemColors.Window;
			this._txtMail_1.Cursor = System.Windows.Forms.Cursors.IBeam;
			this._txtMail_1.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this._txtMail_1.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtMail.SetIndex(this._txtMail_1, 1);
			this._txtMail_1.Location = new System.Drawing.Point(56, 128);
			this._txtMail_1.MaxLength = 0;
			this._txtMail_1.Name = "_txtMail_1";
			this._txtMail_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._txtMail_1.Size = new System.Drawing.Size(33, 20);
			this._txtMail_1.TabIndex = 52;
			//
			//_txtMail_0
			//
			this._txtMail_0.AcceptsReturn = true;
			this._txtMail_0.BackColor = System.Drawing.SystemColors.Window;
			this._txtMail_0.Cursor = System.Windows.Forms.Cursors.IBeam;
			this._txtMail_0.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this._txtMail_0.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtMail.SetIndex(this._txtMail_0, 0);
			this._txtMail_0.Location = new System.Drawing.Point(16, 128);
			this._txtMail_0.MaxLength = 0;
			this._txtMail_0.Name = "_txtMail_0";
			this._txtMail_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._txtMail_0.Size = new System.Drawing.Size(33, 20);
			this._txtMail_0.TabIndex = 50;
			//
			//txtBuyRaftMap
			//
			this.txtBuyRaftMap.AcceptsReturn = true;
			this.txtBuyRaftMap.BackColor = System.Drawing.SystemColors.Window;
			this.txtBuyRaftMap.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtBuyRaftMap.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtBuyRaftMap.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtBuyRaftMap.Location = new System.Drawing.Point(96, 88);
			this.txtBuyRaftMap.MaxLength = 0;
			this.txtBuyRaftMap.Name = "txtBuyRaftMap";
			this.txtBuyRaftMap.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtBuyRaftMap.Size = new System.Drawing.Size(57, 20);
			this.txtBuyRaftMap.TabIndex = 49;
			this.txtBuyRaftMap.Text = "1";
			//
			//txtBuyRaftY
			//
			this.txtBuyRaftY.AcceptsReturn = true;
			this.txtBuyRaftY.BackColor = System.Drawing.SystemColors.Window;
			this.txtBuyRaftY.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtBuyRaftY.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtBuyRaftY.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtBuyRaftY.Location = new System.Drawing.Point(96, 64);
			this.txtBuyRaftY.MaxLength = 0;
			this.txtBuyRaftY.Name = "txtBuyRaftY";
			this.txtBuyRaftY.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtBuyRaftY.Size = new System.Drawing.Size(57, 20);
			this.txtBuyRaftY.TabIndex = 47;
			this.txtBuyRaftY.Text = "0";
			//
			//txtBuyRaftX
			//
			this.txtBuyRaftX.AcceptsReturn = true;
			this.txtBuyRaftX.BackColor = System.Drawing.SystemColors.Window;
			this.txtBuyRaftX.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtBuyRaftX.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtBuyRaftX.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtBuyRaftX.Location = new System.Drawing.Point(96, 40);
			this.txtBuyRaftX.MaxLength = 0;
			this.txtBuyRaftX.Name = "txtBuyRaftX";
			this.txtBuyRaftX.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtBuyRaftX.Size = new System.Drawing.Size(57, 20);
			this.txtBuyRaftX.TabIndex = 46;
			this.txtBuyRaftX.Text = "0";
			//
			//txtDefaultTile
			//
			this.txtDefaultTile.AcceptsReturn = true;
			this.txtDefaultTile.BackColor = System.Drawing.SystemColors.Window;
			this.txtDefaultTile.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtDefaultTile.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtDefaultTile.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtDefaultTile.Location = new System.Drawing.Point(72, 16);
			this.txtDefaultTile.MaxLength = 0;
			this.txtDefaultTile.Name = "txtDefaultTile";
			this.txtDefaultTile.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtDefaultTile.Size = new System.Drawing.Size(110, 20);
			this.txtDefaultTile.TabIndex = 15;
			this.txtDefaultTile.Text = "0";
			//
			//Label15
			//
			this.Label15.BackColor = System.Drawing.SystemColors.Control;
			this.Label15.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label15.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label15.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label15.Location = new System.Drawing.Point(8, 112);
			this.Label15.Name = "Label15";
			this.Label15.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label15.Size = new System.Drawing.Size(65, 17);
			this.Label15.TabIndex = 51;
			this.Label15.Text = "Mail Route";
			//
			//Label14
			//
			this.Label14.BackColor = System.Drawing.SystemColors.Control;
			this.Label14.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label14.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label14.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label14.Location = new System.Drawing.Point(24, 88);
			this.Label14.Name = "Label14";
			this.Label14.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label14.Size = new System.Drawing.Size(65, 17);
			this.Label14.TabIndex = 48;
			this.Label14.Text = "Buy Raft Map";
			//
			//Label13
			//
			this.Label13.BackColor = System.Drawing.SystemColors.Control;
			this.Label13.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label13.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label13.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label13.Location = new System.Drawing.Point(24, 64);
			this.Label13.Name = "Label13";
			this.Label13.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label13.Size = new System.Drawing.Size(89, 17);
			this.Label13.TabIndex = 45;
			this.Label13.Text = "Buy Raft Y";
			//
			//Label12
			//
			this.Label12.BackColor = System.Drawing.SystemColors.Control;
			this.Label12.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label12.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label12.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label12.Location = new System.Drawing.Point(24, 40);
			this.Label12.Name = "Label12";
			this.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label12.Size = new System.Drawing.Size(97, 17);
			this.Label12.TabIndex = 44;
			this.Label12.Text = "Buy Raft X";
			//
			//lblDefaultTile
			//
			this.lblDefaultTile.BackColor = System.Drawing.SystemColors.Control;
			this.lblDefaultTile.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblDefaultTile.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.lblDefaultTile.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblDefaultTile.Location = new System.Drawing.Point(8, 16);
			this.lblDefaultTile.Name = "lblDefaultTile";
			this.lblDefaultTile.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblDefaultTile.Size = new System.Drawing.Size(65, 17);
			this.lblDefaultTile.TabIndex = 35;
			this.lblDefaultTile.Text = "Default Tile:";
			//
			//frmDungeon
			//
			this.frmDungeon.BackColor = System.Drawing.SystemColors.Control;
			this.frmDungeon.Controls.Add(this.txtDungLevels);
			this.frmDungeon.Controls.Add(this.txtDungMonster);
			this.frmDungeon.Controls.Add(this.Label11);
			this.frmDungeon.Controls.Add(this.Label10);
			this.frmDungeon.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.frmDungeon.ForeColor = System.Drawing.SystemColors.ControlText;
			this.frmDungeon.Location = new System.Drawing.Point(8, 392);
			this.frmDungeon.Name = "frmDungeon";
			this.frmDungeon.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.frmDungeon.Size = new System.Drawing.Size(193, 121);
			this.frmDungeon.TabIndex = 38;
			this.frmDungeon.TabStop = false;
			this.frmDungeon.Text = "Dungeon Level Characteristics";
			//
			//txtDungLevels
			//
			this.txtDungLevels.AcceptsReturn = true;
			this.txtDungLevels.BackColor = System.Drawing.SystemColors.Window;
			this.txtDungLevels.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtDungLevels.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtDungLevels.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtDungLevels.Location = new System.Drawing.Point(64, 48);
			this.txtDungLevels.MaxLength = 0;
			this.txtDungLevels.Name = "txtDungLevels";
			this.txtDungLevels.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtDungLevels.Size = new System.Drawing.Size(121, 20);
			this.txtDungLevels.TabIndex = 42;
			//
			//txtDungMonster
			//
			this.txtDungMonster.AcceptsReturn = true;
			this.txtDungMonster.BackColor = System.Drawing.SystemColors.Window;
			this.txtDungMonster.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtDungMonster.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtDungMonster.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtDungMonster.Location = new System.Drawing.Point(104, 24);
			this.txtDungMonster.MaxLength = 0;
			this.txtDungMonster.Name = "txtDungMonster";
			this.txtDungMonster.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtDungMonster.Size = new System.Drawing.Size(81, 20);
			this.txtDungMonster.TabIndex = 40;
			//
			//Label11
			//
			this.Label11.BackColor = System.Drawing.SystemColors.Control;
			this.Label11.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label11.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label11.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label11.Location = new System.Drawing.Point(8, 48);
			this.Label11.Name = "Label11";
			this.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label11.Size = new System.Drawing.Size(89, 17);
			this.Label11.TabIndex = 41;
			this.Label11.Text = "Levels";
			//
			//Label10
			//
			this.Label10.BackColor = System.Drawing.SystemColors.Control;
			this.Label10.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label10.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label10.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label10.Location = new System.Drawing.Point(8, 24);
			this.Label10.Name = "Label10";
			this.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label10.Size = new System.Drawing.Size(105, 17);
			this.Label10.TabIndex = 39;
			this.Label10.Text = "Monster Str Factor";
			//
			//Frame2
			//
			this.Frame2.BackColor = System.Drawing.SystemColors.Control;
			this.Frame2.Controls.Add(this._optTiles_2);
			this.Frame2.Controls.Add(this._optTiles_1);
			this.Frame2.Controls.Add(this._optTiles_0);
			this.Frame2.Controls.Add(this._optTiles_4);
			this.Frame2.Controls.Add(this._optTiles_3);
			this.Frame2.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Frame2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Frame2.Location = new System.Drawing.Point(8, 168);
			this.Frame2.Name = "Frame2";
			this.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Frame2.Size = new System.Drawing.Size(401, 89);
			this.Frame2.TabIndex = 36;
			this.Frame2.TabStop = false;
			this.Frame2.Text = "Tile Set";
			//
			//_optTiles_2
			//
			this._optTiles_2.BackColor = System.Drawing.SystemColors.Control;
			this._optTiles_2.Cursor = System.Windows.Forms.Cursors.Default;
			this._optTiles_2.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this._optTiles_2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.optTiles.SetIndex(this._optTiles_2, 2);
			this._optTiles_2.Location = new System.Drawing.Point(16, 64);
			this._optTiles_2.Name = "_optTiles_2";
			this._optTiles_2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._optTiles_2.Size = new System.Drawing.Size(161, 17);
			this._optTiles_2.TabIndex = 43;
			this._optTiles_2.TabStop = true;
			this._optTiles_2.Tag = "CastleTiles.bmp";
			this._optTiles_2.Text = "CastleTiles.bmp";
			this._optTiles_2.UseVisualStyleBackColor = false;
			//
			//_optTiles_1
			//
			this._optTiles_1.BackColor = System.Drawing.SystemColors.Control;
			this._optTiles_1.Cursor = System.Windows.Forms.Cursors.Default;
			this._optTiles_1.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this._optTiles_1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.optTiles.SetIndex(this._optTiles_1, 1);
			this._optTiles_1.Location = new System.Drawing.Point(16, 40);
			this._optTiles_1.Name = "_optTiles_1";
			this._optTiles_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._optTiles_1.Size = new System.Drawing.Size(161, 17);
			this._optTiles_1.TabIndex = 12;
			this._optTiles_1.TabStop = true;
			this._optTiles_1.Tag = "TownTiles.bmp";
			this._optTiles_1.Text = "TownTiles.bmp";
			this._optTiles_1.UseVisualStyleBackColor = false;
			//
			//_optTiles_0
			//
			this._optTiles_0.BackColor = System.Drawing.SystemColors.Control;
			this._optTiles_0.Cursor = System.Windows.Forms.Cursors.Default;
			this._optTiles_0.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this._optTiles_0.ForeColor = System.Drawing.SystemColors.ControlText;
			this.optTiles.SetIndex(this._optTiles_0, 0);
			this._optTiles_0.Location = new System.Drawing.Point(16, 16);
			this._optTiles_0.Name = "_optTiles_0";
			this._optTiles_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._optTiles_0.Size = new System.Drawing.Size(113, 17);
			this._optTiles_0.TabIndex = 11;
			this._optTiles_0.TabStop = true;
			this._optTiles_0.Tag = "Tiles.bmp";
			this._optTiles_0.Text = "Tiles.bmp";
			this._optTiles_0.UseVisualStyleBackColor = false;
			//
			//_optTiles_4
			//
			this._optTiles_4.BackColor = System.Drawing.SystemColors.Control;
			this._optTiles_4.Cursor = System.Windows.Forms.Cursors.Default;
			this._optTiles_4.Enabled = false;
			this._optTiles_4.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this._optTiles_4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.optTiles.SetIndex(this._optTiles_4, 4);
			this._optTiles_4.Location = new System.Drawing.Point(192, 40);
			this._optTiles_4.Name = "_optTiles_4";
			this._optTiles_4.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._optTiles_4.Size = new System.Drawing.Size(201, 17);
			this._optTiles_4.TabIndex = 14;
			this._optTiles_4.TabStop = true;
			this._optTiles_4.Tag = "LOB TownTiles.bmp";
			this._optTiles_4.Text = "LOB TownTiles.bmp";
			this._optTiles_4.UseVisualStyleBackColor = false;
			//
			//_optTiles_3
			//
			this._optTiles_3.BackColor = System.Drawing.SystemColors.Control;
			this._optTiles_3.Cursor = System.Windows.Forms.Cursors.Default;
			this._optTiles_3.Enabled = false;
			this._optTiles_3.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this._optTiles_3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.optTiles.SetIndex(this._optTiles_3, 3);
			this._optTiles_3.Location = new System.Drawing.Point(192, 16);
			this._optTiles_3.Name = "_optTiles_3";
			this._optTiles_3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._optTiles_3.Size = new System.Drawing.Size(201, 17);
			this._optTiles_3.TabIndex = 13;
			this._optTiles_3.TabStop = true;
			this._optTiles_3.Tag = "LOB Tiles.bmp";
			this._optTiles_3.Text = "LOB Tiles.bmp";
			this._optTiles_3.UseVisualStyleBackColor = false;
			//
			//cmdDefaults
			//
			this.cmdDefaults.BackColor = System.Drawing.SystemColors.Control;
			this.cmdDefaults.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdDefaults.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdDefaults.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdDefaults.Location = new System.Drawing.Point(208, 400);
			this.cmdDefaults.Name = "cmdDefaults";
			this.cmdDefaults.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdDefaults.Size = new System.Drawing.Size(65, 25);
			this.cmdDefaults.TabIndex = 28;
			this.cmdDefaults.TabStop = false;
			this.cmdDefaults.Text = "Defaults";
			this.cmdDefaults.UseVisualStyleBackColor = false;
			//
			//frmGuards
			//
			this.frmGuards.BackColor = System.Drawing.SystemColors.Control;
			this.frmGuards.Controls.Add(this.txtHP);
			this.frmGuards.Controls.Add(this.txtAttack);
			this.frmGuards.Controls.Add(this.txtDefense);
			this.frmGuards.Controls.Add(this.txtColor);
			this.frmGuards.Controls.Add(this.Label4);
			this.frmGuards.Controls.Add(this.Label5);
			this.frmGuards.Controls.Add(this.Label6);
			this.frmGuards.Controls.Add(this.Label7);
			this.frmGuards.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.frmGuards.ForeColor = System.Drawing.SystemColors.ControlText;
			this.frmGuards.Location = new System.Drawing.Point(208, 264);
			this.frmGuards.Name = "frmGuards";
			this.frmGuards.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.frmGuards.Size = new System.Drawing.Size(201, 121);
			this.frmGuards.TabIndex = 18;
			this.frmGuards.TabStop = false;
			this.frmGuards.Text = "Guards";
			//
			//txtHP
			//
			this.txtHP.AcceptsReturn = true;
			this.txtHP.BackColor = System.Drawing.SystemColors.Window;
			this.txtHP.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtHP.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtHP.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtHP.Location = new System.Drawing.Point(80, 16);
			this.txtHP.MaxLength = 0;
			this.txtHP.Name = "txtHP";
			this.txtHP.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtHP.Size = new System.Drawing.Size(113, 20);
			this.txtHP.TabIndex = 19;
			//
			//txtAttack
			//
			this.txtAttack.AcceptsReturn = true;
			this.txtAttack.BackColor = System.Drawing.SystemColors.Window;
			this.txtAttack.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtAttack.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtAttack.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtAttack.Location = new System.Drawing.Point(80, 40);
			this.txtAttack.MaxLength = 0;
			this.txtAttack.Name = "txtAttack";
			this.txtAttack.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtAttack.Size = new System.Drawing.Size(113, 20);
			this.txtAttack.TabIndex = 20;
			//
			//txtDefense
			//
			this.txtDefense.AcceptsReturn = true;
			this.txtDefense.BackColor = System.Drawing.SystemColors.Window;
			this.txtDefense.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtDefense.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtDefense.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtDefense.Location = new System.Drawing.Point(80, 64);
			this.txtDefense.MaxLength = 0;
			this.txtDefense.Name = "txtDefense";
			this.txtDefense.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtDefense.Size = new System.Drawing.Size(113, 20);
			this.txtDefense.TabIndex = 21;
			//
			//txtColor
			//
			this.txtColor.AcceptsReturn = true;
			this.txtColor.BackColor = System.Drawing.SystemColors.Window;
			this.txtColor.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtColor.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtColor.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtColor.Location = new System.Drawing.Point(80, 88);
			this.txtColor.MaxLength = 0;
			this.txtColor.Name = "txtColor";
			this.txtColor.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtColor.Size = new System.Drawing.Size(113, 20);
			this.txtColor.TabIndex = 23;
			//
			//Label4
			//
			this.Label4.BackColor = System.Drawing.SystemColors.Control;
			this.Label4.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label4.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label4.Location = new System.Drawing.Point(8, 16);
			this.Label4.Name = "Label4";
			this.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label4.Size = new System.Drawing.Size(70, 17);
			this.Label4.TabIndex = 26;
			this.Label4.Text = "HP (+/- 10%)";
			//
			//Label5
			//
			this.Label5.BackColor = System.Drawing.SystemColors.Control;
			this.Label5.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label5.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label5.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label5.Location = new System.Drawing.Point(8, 40);
			this.Label5.Name = "Label5";
			this.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label5.Size = new System.Drawing.Size(71, 17);
			this.Label5.TabIndex = 25;
			this.Label5.Text = "Attack";
			//
			//Label6
			//
			this.Label6.BackColor = System.Drawing.SystemColors.Control;
			this.Label6.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label6.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label6.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label6.Location = new System.Drawing.Point(8, 64);
			this.Label6.Name = "Label6";
			this.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label6.Size = new System.Drawing.Size(71, 17);
			this.Label6.TabIndex = 24;
			this.Label6.Text = "Defense";
			//
			//Label7
			//
			this.Label7.BackColor = System.Drawing.SystemColors.Control;
			this.Label7.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label7.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label7.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label7.Location = new System.Drawing.Point(8, 88);
			this.Label7.Name = "Label7";
			this.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label7.Size = new System.Drawing.Size(71, 17);
			this.Label7.TabIndex = 22;
			this.Label7.Text = "Color (0=deflt)";
			//
			//Frame1
			//
			this.Frame1.BackColor = System.Drawing.SystemColors.Control;
			this.Frame1.Controls.Add(this.cboTypes);
			this.Frame1.Controls.Add(this.Label2);
			this.Frame1.Controls.Add(this.txtWidth);
			this.Frame1.Controls.Add(this.txtHeight);
			this.Frame1.Controls.Add(this.txtName);
			this.Frame1.Controls.Add(this.Label8);
			this.Frame1.Controls.Add(this.Label3);
			this.Frame1.Controls.Add(this.Label1);
			this.Frame1.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Frame1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Frame1.Location = new System.Drawing.Point(8, 8);
			this.Frame1.Name = "Frame1";
			this.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Frame1.Size = new System.Drawing.Size(401, 153);
			this.Frame1.TabIndex = 16;
			this.Frame1.TabStop = false;
			this.Frame1.Text = "Map Type";
			//
			//cboTypes
			//
			this.cboTypes.FormattingEnabled = true;
			this.cboTypes.Location = new System.Drawing.Point(76, 51);
			this.cboTypes.Name = "cboTypes";
			this.cboTypes.Size = new System.Drawing.Size(203, 22);
			this.cboTypes.TabIndex = 33;
			//
			//Label2
			//
			this.Label2.AutoSize = true;
			this.Label2.BackColor = System.Drawing.SystemColors.Control;
			this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label2.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label2.Location = new System.Drawing.Point(16, 54);
			this.Label2.Name = "Label2";
			this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label2.Size = new System.Drawing.Size(54, 14);
			this.Label2.TabIndex = 32;
			this.Label2.Text = "Map Type";
			//
			//txtWidth
			//
			this.txtWidth.AcceptsReturn = true;
			this.txtWidth.BackColor = System.Drawing.SystemColors.Window;
			this.txtWidth.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtWidth.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtWidth.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtWidth.Location = new System.Drawing.Point(76, 94);
			this.txtWidth.MaxLength = 0;
			this.txtWidth.Name = "txtWidth";
			this.txtWidth.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtWidth.Size = new System.Drawing.Size(89, 20);
			this.txtWidth.TabIndex = 8;
			//
			//txtHeight
			//
			this.txtHeight.AcceptsReturn = true;
			this.txtHeight.BackColor = System.Drawing.SystemColors.Window;
			this.txtHeight.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtHeight.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtHeight.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtHeight.Location = new System.Drawing.Point(76, 118);
			this.txtHeight.MaxLength = 0;
			this.txtHeight.Name = "txtHeight";
			this.txtHeight.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtHeight.Size = new System.Drawing.Size(89, 20);
			this.txtHeight.TabIndex = 9;
			//
			//txtName
			//
			this.txtName.AcceptsReturn = true;
			this.txtName.BackColor = System.Drawing.SystemColors.Window;
			this.txtName.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtName.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtName.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtName.Location = new System.Drawing.Point(76, 26);
			this.txtName.MaxLength = 16;
			this.txtName.Name = "txtName";
			this.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtName.Size = new System.Drawing.Size(201, 20);
			this.txtName.TabIndex = 6;
			//
			//Label8
			//
			this.Label8.AutoSize = true;
			this.Label8.BackColor = System.Drawing.SystemColors.Control;
			this.Label8.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label8.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label8.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label8.Location = new System.Drawing.Point(33, 118);
			this.Label8.Name = "Label8";
			this.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label8.Size = new System.Drawing.Size(37, 14);
			this.Label8.TabIndex = 31;
			this.Label8.Text = "Height";
			//
			//Label3
			//
			this.Label3.AutoSize = true;
			this.Label3.BackColor = System.Drawing.SystemColors.Control;
			this.Label3.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label3.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label3.Location = new System.Drawing.Point(36, 97);
			this.Label3.Name = "Label3";
			this.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label3.Size = new System.Drawing.Size(34, 14);
			this.Label3.TabIndex = 29;
			this.Label3.Text = "Width";
			//
			//Label1
			//
			this.Label1.AutoSize = true;
			this.Label1.BackColor = System.Drawing.SystemColors.Control;
			this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label1.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label1.Location = new System.Drawing.Point(13, 29);
			this.Label1.Name = "Label1";
			this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label1.Size = new System.Drawing.Size(57, 14);
			this.Label1.TabIndex = 17;
			this.Label1.Text = "Map Name";
			//
			//cmdCancel
			//
			this.cmdCancel.BackColor = System.Drawing.SystemColors.Control;
			this.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdCancel.Location = new System.Drawing.Point(280, 400);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdCancel.Size = new System.Drawing.Size(65, 25);
			this.cmdCancel.TabIndex = 30;
			this.cmdCancel.TabStop = false;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = false;
			//
			//cmdOK
			//
			this.cmdOK.BackColor = System.Drawing.SystemColors.Control;
			this.cmdOK.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdOK.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdOK.Location = new System.Drawing.Point(352, 400);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdOK.Size = new System.Drawing.Size(65, 25);
			this.cmdOK.TabIndex = 32;
			this.cmdOK.TabStop = false;
			this.cmdOK.Text = "OK";
			this.cmdOK.UseVisualStyleBackColor = false;
			//
			//optType
			//
			//
			//frmProperties
			//
			this.AcceptButton = this.cmdOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) 6.0, (float) 14.0);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(427, 434);
			this.Controls.Add(this.frmChar);
			this.Controls.Add(this.frmDungeon);
			this.Controls.Add(this.Frame2);
			this.Controls.Add(this.cmdDefaults);
			this.Controls.Add(this.frmGuards);
			this.Controls.Add(this.Frame1);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Location = new System.Drawing.Point(4, 23);
			this.Name = "frmProperties";
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Text = "Map Properties";
			this.frmChar.ResumeLayout(false);
			this.frmChar.PerformLayout();
			this.frmDungeon.ResumeLayout(false);
			this.frmDungeon.PerformLayout();
			this.Frame2.ResumeLayout(false);
			this.frmGuards.ResumeLayout(false);
			this.frmGuards.PerformLayout();
			this.Frame1.ResumeLayout(false);
			this.Frame1.PerformLayout();
			((System.ComponentModel.ISupportInitialize) this.optTiles).EndInit();
			((System.ComponentModel.ISupportInitialize) this.optType).EndInit();
			((System.ComponentModel.ISupportInitialize) this.txtMail).EndInit();
			this.ResumeLayout(false);
			
		}
		internal System.Windows.Forms.ComboBox cboTypes;
		public System.Windows.Forms.Label Label2;
		#endregion
	}
}
