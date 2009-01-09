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
	[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]public partial class frmRoof
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
		private System.ComponentModel.Container components = null;
		public System.Windows.Forms.ToolTip ToolTip1;
		public System.Windows.Forms.Button cmdCopy;
		public System.Windows.Forms.CheckBox chkDrawGround;
		public System.Windows.Forms.TextBox txtTargetY;
		public System.Windows.Forms.TextBox txtTargetX;
		public System.Windows.Forms.TextBox txtAnchorY;
		public System.Windows.Forms.TextBox txtAnchorX;
		public System.Windows.Forms.HScrollBar hsbRoofIndex;
		public System.Windows.Forms.TextBox txtRoofY;
		public System.Windows.Forms.TextBox txtRoofX;
		public System.Windows.Forms.Button cmdDone;
		public System.Windows.Forms.PictureBox Picture2;
		public System.Windows.Forms.PictureBox Picture1;
		public System.Windows.Forms.Label Label6;
		public System.Windows.Forms.Label lblCurrentTile;
		public System.Windows.Forms.Label lblTile;
		public System.Windows.Forms.Label lblY;
		public System.Windows.Forms.Label lblx;
		public System.Windows.Forms.Label Label2;
		public System.Windows.Forms.Label Label3;
		public System.Windows.Forms.Label lblEditing;
		public System.Windows.Forms.Label Label1;
		public System.Windows.Forms.Label Label5;
		public System.Windows.Forms.Label Label4;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			base.Load += new System.EventHandler(frmRoof_Load);
			base.Paint += new System.Windows.Forms.PaintEventHandler(frmRoof_Paint);
			this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.cmdCopy = new System.Windows.Forms.Button();
			this.cmdCopy.Click += new System.EventHandler(cmdCopy_Click);
			this.chkDrawGround = new System.Windows.Forms.CheckBox();
			this.chkDrawGround.CheckStateChanged += new System.EventHandler(chkDrawGround_CheckStateChanged);
			this.txtTargetY = new System.Windows.Forms.TextBox();
			this.txtTargetY.TextChanged += new System.EventHandler(txtTargetY_TextChanged);
			this.txtTargetX = new System.Windows.Forms.TextBox();
			this.txtTargetX.TextChanged += new System.EventHandler(txtTargetX_TextChanged);
			this.txtAnchorY = new System.Windows.Forms.TextBox();
			this.txtAnchorY.TextChanged += new System.EventHandler(txtAnchorY_TextChanged);
			this.txtAnchorX = new System.Windows.Forms.TextBox();
			this.txtAnchorX.TextChanged += new System.EventHandler(txtAnchorX_TextChanged);
			this.hsbRoofIndex = new System.Windows.Forms.HScrollBar();
			this.hsbRoofIndex.ValueChanged += new System.EventHandler(hsbRoofIndex_Change);
			this.hsbRoofIndex.Scroll += new System.Windows.Forms.ScrollEventHandler(hsbRoofIndex_Scroll);
			this.txtRoofY = new System.Windows.Forms.TextBox();
			this.txtRoofY.TextChanged += new System.EventHandler(txtRoofY_TextChanged);
			this.txtRoofX = new System.Windows.Forms.TextBox();
			this.txtRoofX.TextChanged += new System.EventHandler(txtRoofX_TextChanged);
			this.cmdDone = new System.Windows.Forms.Button();
			this.cmdDone.Click += new System.EventHandler(cmdDone_Click);
			this.Picture2 = new System.Windows.Forms.PictureBox();
			this.Picture2.Paint += new System.Windows.Forms.PaintEventHandler(Picture2_Paint);
			this.Picture2.MouseDown += new System.Windows.Forms.MouseEventHandler(Picture2_MouseDown);
			this.Picture1 = new System.Windows.Forms.PictureBox();
			this.Picture1.Paint += new System.Windows.Forms.PaintEventHandler(Picture1_Paint);
			this.Picture1.MouseDown += new System.Windows.Forms.MouseEventHandler(Picture1_MouseDown);
			this.Picture1.MouseMove += new System.Windows.Forms.MouseEventHandler(Picture1_MouseMove);
			this.Label6 = new System.Windows.Forms.Label();
			this.lblCurrentTile = new System.Windows.Forms.Label();
			this.lblTile = new System.Windows.Forms.Label();
			this.lblY = new System.Windows.Forms.Label();
			this.lblx = new System.Windows.Forms.Label();
			this.Label2 = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.lblEditing = new System.Windows.Forms.Label();
			this.Label1 = new System.Windows.Forms.Label();
			this.Label5 = new System.Windows.Forms.Label();
			this.Label4 = new System.Windows.Forms.Label();
			this.sbDown = new System.Windows.Forms.VScrollBar();
			this.sbRight = new System.Windows.Forms.HScrollBar();
			((System.ComponentModel.ISupportInitialize) this.Picture2).BeginInit();
			((System.ComponentModel.ISupportInitialize) this.Picture1).BeginInit();
			this.SuspendLayout();
			//
			//cmdCopy
			//
			this.cmdCopy.BackColor = System.Drawing.SystemColors.Control;
			this.cmdCopy.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdCopy.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdCopy.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdCopy.Location = new System.Drawing.Point(296, 448);
			this.cmdCopy.Name = "cmdCopy";
			this.cmdCopy.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdCopy.Size = new System.Drawing.Size(89, 57);
			this.cmdCopy.TabIndex = 22;
			this.cmdCopy.Text = "Copy From Ground";
			this.cmdCopy.UseVisualStyleBackColor = false;
			//
			//chkDrawGround
			//
			this.chkDrawGround.BackColor = System.Drawing.SystemColors.Control;
			this.chkDrawGround.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkDrawGround.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.chkDrawGround.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkDrawGround.Location = new System.Drawing.Point(296, 400);
			this.chkDrawGround.Name = "chkDrawGround";
			this.chkDrawGround.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkDrawGround.Size = new System.Drawing.Size(209, 33);
			this.chkDrawGround.TabIndex = 21;
			this.chkDrawGround.Text = "Draw Ground";
			this.chkDrawGround.UseVisualStyleBackColor = false;
			//
			//txtTargetY
			//
			this.txtTargetY.AcceptsReturn = true;
			this.txtTargetY.BackColor = System.Drawing.SystemColors.Window;
			this.txtTargetY.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtTargetY.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtTargetY.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtTargetY.Location = new System.Drawing.Point(440, 208);
			this.txtTargetY.MaxLength = 0;
			this.txtTargetY.Name = "txtTargetY";
			this.txtTargetY.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtTargetY.Size = new System.Drawing.Size(89, 27);
			this.txtTargetY.TabIndex = 20;
			this.txtTargetY.Text = "Text2";
			//
			//txtTargetX
			//
			this.txtTargetX.AcceptsReturn = true;
			this.txtTargetX.BackColor = System.Drawing.SystemColors.Window;
			this.txtTargetX.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtTargetX.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtTargetX.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtTargetX.Location = new System.Drawing.Point(336, 208);
			this.txtTargetX.MaxLength = 0;
			this.txtTargetX.Name = "txtTargetX";
			this.txtTargetX.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtTargetX.Size = new System.Drawing.Size(89, 27);
			this.txtTargetX.TabIndex = 19;
			this.txtTargetX.Text = "Text1";
			//
			//txtAnchorY
			//
			this.txtAnchorY.AcceptsReturn = true;
			this.txtAnchorY.BackColor = System.Drawing.SystemColors.Window;
			this.txtAnchorY.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtAnchorY.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtAnchorY.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtAnchorY.Location = new System.Drawing.Point(440, 152);
			this.txtAnchorY.MaxLength = 0;
			this.txtAnchorY.Name = "txtAnchorY";
			this.txtAnchorY.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtAnchorY.Size = new System.Drawing.Size(89, 27);
			this.txtAnchorY.TabIndex = 12;
			this.txtAnchorY.Text = "Text2";
			//
			//txtAnchorX
			//
			this.txtAnchorX.AcceptsReturn = true;
			this.txtAnchorX.BackColor = System.Drawing.SystemColors.Window;
			this.txtAnchorX.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtAnchorX.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtAnchorX.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtAnchorX.Location = new System.Drawing.Point(336, 152);
			this.txtAnchorX.MaxLength = 0;
			this.txtAnchorX.Name = "txtAnchorX";
			this.txtAnchorX.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtAnchorX.Size = new System.Drawing.Size(81, 27);
			this.txtAnchorX.TabIndex = 11;
			this.txtAnchorX.Text = "Text1";
			//
			//hsbRoofIndex
			//
			this.hsbRoofIndex.Cursor = System.Windows.Forms.Cursors.Default;
			this.hsbRoofIndex.LargeChange = 5;
			this.hsbRoofIndex.Location = new System.Drawing.Point(336, 32);
			this.hsbRoofIndex.Maximum = 44;
			this.hsbRoofIndex.Minimum = 1;
			this.hsbRoofIndex.Name = "hsbRoofIndex";
			this.hsbRoofIndex.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.hsbRoofIndex.Size = new System.Drawing.Size(193, 17);
			this.hsbRoofIndex.TabIndex = 9;
			this.hsbRoofIndex.TabStop = true;
			this.hsbRoofIndex.Value = 1;
			//
			//txtRoofY
			//
			this.txtRoofY.AcceptsReturn = true;
			this.txtRoofY.BackColor = System.Drawing.SystemColors.Window;
			this.txtRoofY.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtRoofY.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtRoofY.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtRoofY.Location = new System.Drawing.Point(440, 88);
			this.txtRoofY.MaxLength = 0;
			this.txtRoofY.Name = "txtRoofY";
			this.txtRoofY.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtRoofY.Size = new System.Drawing.Size(81, 33);
			this.txtRoofY.TabIndex = 6;
			this.txtRoofY.Text = "Text2";
			//
			//txtRoofX
			//
			this.txtRoofX.AcceptsReturn = true;
			this.txtRoofX.BackColor = System.Drawing.SystemColors.Window;
			this.txtRoofX.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtRoofX.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.txtRoofX.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtRoofX.Location = new System.Drawing.Point(336, 88);
			this.txtRoofX.MaxLength = 0;
			this.txtRoofX.Name = "txtRoofX";
			this.txtRoofX.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtRoofX.Size = new System.Drawing.Size(81, 33);
			this.txtRoofX.TabIndex = 5;
			this.txtRoofX.Text = "txtRoofX";
			//
			//cmdDone
			//
			this.cmdDone.BackColor = System.Drawing.SystemColors.Control;
			this.cmdDone.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdDone.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdDone.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdDone.Location = new System.Drawing.Point(448, 568);
			this.cmdDone.Name = "cmdDone";
			this.cmdDone.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdDone.Size = new System.Drawing.Size(81, 33);
			this.cmdDone.TabIndex = 4;
			this.cmdDone.Text = "Done";
			this.cmdDone.UseVisualStyleBackColor = false;
			//
			//Picture2
			//
			this.Picture2.BackColor = System.Drawing.SystemColors.Control;
			this.Picture2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Picture2.Cursor = System.Windows.Forms.Cursors.Default;
			this.Picture2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Picture2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Picture2.Location = new System.Drawing.Point(16, 352);
			this.Picture2.Name = "Picture2";
			this.Picture2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Picture2.Size = new System.Drawing.Size(256, 256);
			this.Picture2.TabIndex = 1;
			//
			//Picture1
			//
			this.Picture1.BackColor = System.Drawing.SystemColors.Control;
			this.Picture1.Cursor = System.Windows.Forms.Cursors.Default;
			this.Picture1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Picture1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Picture1.Location = new System.Drawing.Point(0, 0);
			this.Picture1.Name = "Picture1";
			this.Picture1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Picture1.Size = new System.Drawing.Size(303, 311);
			this.Picture1.TabIndex = 0;
			//
			//Label6
			//
			this.Label6.BackColor = System.Drawing.SystemColors.Control;
			this.Label6.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label6.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label6.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label6.Location = new System.Drawing.Point(336, 184);
			this.Label6.Name = "Label6";
			this.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label6.Size = new System.Drawing.Size(121, 25);
			this.Label6.TabIndex = 18;
			this.Label6.Text = "Anchor Target";
			//
			//lblCurrentTile
			//
			this.lblCurrentTile.BackColor = System.Drawing.SystemColors.Control;
			this.lblCurrentTile.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblCurrentTile.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.lblCurrentTile.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblCurrentTile.Location = new System.Drawing.Point(344, 352);
			this.lblCurrentTile.Name = "lblCurrentTile";
			this.lblCurrentTile.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblCurrentTile.Size = new System.Drawing.Size(200, 25);
			this.lblCurrentTile.TabIndex = 17;
			this.lblCurrentTile.Text = "Label6";
			//
			//lblTile
			//
			this.lblTile.BackColor = System.Drawing.SystemColors.Control;
			this.lblTile.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblTile.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.lblTile.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblTile.Location = new System.Drawing.Point(344, 328);
			this.lblTile.Name = "lblTile";
			this.lblTile.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblTile.Size = new System.Drawing.Size(200, 25);
			this.lblTile.TabIndex = 16;
			this.lblTile.Text = "Label6";
			//
			//lblY
			//
			this.lblY.BackColor = System.Drawing.SystemColors.Control;
			this.lblY.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblY.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.lblY.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblY.Location = new System.Drawing.Point(344, 304);
			this.lblY.Name = "lblY";
			this.lblY.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblY.Size = new System.Drawing.Size(200, 25);
			this.lblY.TabIndex = 15;
			this.lblY.Text = "Label6";
			//
			//lblx
			//
			this.lblx.BackColor = System.Drawing.SystemColors.Control;
			this.lblx.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblx.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.lblx.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblx.Location = new System.Drawing.Point(344, 280);
			this.lblx.Name = "lblx";
			this.lblx.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblx.Size = new System.Drawing.Size(200, 17);
			this.lblx.TabIndex = 14;
			this.lblx.Text = "X:";
			//
			//Label2
			//
			this.Label2.BackColor = System.Drawing.SystemColors.Control;
			this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label2.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label2.Location = new System.Drawing.Point(336, 248);
			this.Label2.Name = "Label2";
			this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label2.Size = new System.Drawing.Size(137, 25);
			this.Label2.TabIndex = 13;
			this.Label2.Text = "Current Position:";
			//
			//Label3
			//
			this.Label3.BackColor = System.Drawing.SystemColors.Control;
			this.Label3.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label3.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label3.Location = new System.Drawing.Point(336, 128);
			this.Label3.Name = "Label3";
			this.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label3.Size = new System.Drawing.Size(89, 17);
			this.Label3.TabIndex = 10;
			this.Label3.Text = "Anchor Point";
			//
			//lblEditing
			//
			this.lblEditing.BackColor = System.Drawing.SystemColors.Control;
			this.lblEditing.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblEditing.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.lblEditing.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblEditing.Location = new System.Drawing.Point(336, 8);
			this.lblEditing.Name = "lblEditing";
			this.lblEditing.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblEditing.Size = new System.Drawing.Size(201, 25);
			this.lblEditing.TabIndex = 8;
			this.lblEditing.Text = "Editing Roof";
			//
			//Label1
			//
			this.Label1.BackColor = System.Drawing.SystemColors.Control;
			this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label1.Font = new System.Drawing.Font("Arial", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label1.Location = new System.Drawing.Point(336, 64);
			this.Label1.Name = "Label1";
			this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label1.Size = new System.Drawing.Size(145, 17);
			this.Label1.TabIndex = 7;
			this.Label1.Text = "Roof Dimensions";
			//
			//Label5
			//
			this.Label5.BackColor = System.Drawing.SystemColors.Control;
			this.Label5.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label5.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label5.Location = new System.Drawing.Point(0, 352);
			this.Label5.Name = "Label5";
			this.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label5.Size = new System.Drawing.Size(17, 257);
			this.Label5.TabIndex = 3;
			//
			//Label4
			//
			this.Label4.BackColor = System.Drawing.SystemColors.Control;
			this.Label4.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label4.Location = new System.Drawing.Point(16, 336);
			this.Label4.Name = "Label4";
			this.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label4.Size = new System.Drawing.Size(281, 17);
			this.Label4.TabIndex = 2;
			this.Label4.Text = " 0    1   2   3    4   5   6    7   8   9   A    B   C   D   E   F";
			//
			//sbDown
			//
			this.sbDown.Location = new System.Drawing.Point(304, 0);
			this.sbDown.Name = "sbDown";
			this.sbDown.Size = new System.Drawing.Size(17, 311);
			this.sbDown.TabIndex = 25;
			//
			//sbRight
			//
			this.sbRight.Location = new System.Drawing.Point(0, 312);
			this.sbRight.Name = "sbRight";
			this.sbRight.Size = new System.Drawing.Size(303, 17);
			this.sbRight.TabIndex = 26;
			//
			//frmRoof
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) 9.0, (float) 19.0);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(544, 614);
			this.Controls.Add(this.sbRight);
			this.Controls.Add(this.sbDown);
			this.Controls.Add(this.cmdCopy);
			this.Controls.Add(this.chkDrawGround);
			this.Controls.Add(this.txtTargetY);
			this.Controls.Add(this.txtTargetX);
			this.Controls.Add(this.txtAnchorY);
			this.Controls.Add(this.txtAnchorX);
			this.Controls.Add(this.hsbRoofIndex);
			this.Controls.Add(this.txtRoofY);
			this.Controls.Add(this.txtRoofX);
			this.Controls.Add(this.cmdDone);
			this.Controls.Add(this.Picture2);
			this.Controls.Add(this.Picture1);
			this.Controls.Add(this.Label6);
			this.Controls.Add(this.lblCurrentTile);
			this.Controls.Add(this.lblTile);
			this.Controls.Add(this.lblY);
			this.Controls.Add(this.lblx);
			this.Controls.Add(this.Label2);
			this.Controls.Add(this.Label3);
			this.Controls.Add(this.lblEditing);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.Label5);
			this.Controls.Add(this.Label4);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Font = new System.Drawing.Font("Times New Roman", 12.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Location = new System.Drawing.Point(4, 23);
			this.Name = "frmRoof";
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Text = "Edit Roof";
			((System.ComponentModel.ISupportInitialize) this.Picture2).EndInit();
			((System.ComponentModel.ISupportInitialize) this.Picture1).EndInit();
			this.ResumeLayout(false);
			
		}
		internal System.Windows.Forms.VScrollBar sbDown;
		internal System.Windows.Forms.HScrollBar sbRight;
		#endregion
	}
}
