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
	[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]public partial class frmStartup
	{
		#region "Windows Form Designer generated code "
		[System.Diagnostics.DebuggerNonUserCode()]public frmStartup()
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
		public System.Windows.Forms.Button cmdImport;
		public System.Windows.Forms.OpenFileDialog cmdDialogOpen;
		public System.Windows.Forms.Button cmdExit;
		public System.Windows.Forms.Button cmdOpenMap;
		public System.Windows.Forms.Button cmdCreateMap;
		public System.Windows.Forms.PictureBox Image1;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmStartup));
			this.components = new System.ComponentModel.Container();
			this.ToolTip1 = new System.Windows.Forms.ToolTip(components);
			this.cmdImport = new System.Windows.Forms.Button();
			this.cmdImport.Click += new System.EventHandler(cmdImport_Click);
			this.cmdDialogOpen = new System.Windows.Forms.OpenFileDialog();
			this.cmdExit = new System.Windows.Forms.Button();
			this.cmdExit.Click += new System.EventHandler(cmdExit_Click);
			this.cmdOpenMap = new System.Windows.Forms.Button();
			this.cmdOpenMap.Click += new System.EventHandler(cmdOpenMap_Click);
			this.cmdCreateMap = new System.Windows.Forms.Button();
			this.cmdCreateMap.Click += new System.EventHandler(cmdCreateMap_Click);
			this.Image1 = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			this.ToolTip1.Active = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Text = "Legacy of the Ancients Map Editor";
			this.ClientSize = new System.Drawing.Size(351, 143);
			this.Location = new System.Drawing.Point(3, 22);
			this.Icon = (System.Drawing.Icon) (resources.GetObject("frmStartup.Icon"));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
			this.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ControlBox = true;
			this.Enabled = true;
			this.KeyPreview = false;
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ShowInTaskbar = true;
			this.HelpButton = false;
			this.WindowState = System.Windows.Forms.FormWindowState.Normal;
			this.Name = "frmStartup";
			this.cmdImport.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cmdImport.Text = "&Import Existing Map";
			this.cmdImport.Size = new System.Drawing.Size(81, 33);
			this.cmdImport.Location = new System.Drawing.Point(224, 64);
			this.cmdImport.TabIndex = 3;
			this.cmdImport.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdImport.BackColor = System.Drawing.SystemColors.Control;
			this.cmdImport.CausesValidation = true;
			this.cmdImport.Enabled = true;
			this.cmdImport.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdImport.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdImport.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdImport.TabStop = true;
			this.cmdImport.Name = "cmdImport";
			this.cmdExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.CancelButton = this.cmdExit;
			this.cmdExit.Text = "E&xit";
			this.cmdExit.Size = new System.Drawing.Size(57, 17);
			this.cmdExit.Location = new System.Drawing.Point(144, 112);
			this.cmdExit.TabIndex = 2;
			this.cmdExit.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdExit.BackColor = System.Drawing.SystemColors.Control;
			this.cmdExit.CausesValidation = true;
			this.cmdExit.Enabled = true;
			this.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdExit.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdExit.TabStop = true;
			this.cmdExit.Name = "cmdExit";
			this.cmdOpenMap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cmdOpenMap.Text = "&Open Existing Map";
			this.cmdOpenMap.Size = new System.Drawing.Size(89, 33);
			this.cmdOpenMap.Location = new System.Drawing.Point(128, 64);
			this.cmdOpenMap.TabIndex = 1;
			this.cmdOpenMap.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdOpenMap.BackColor = System.Drawing.SystemColors.Control;
			this.cmdOpenMap.CausesValidation = true;
			this.cmdOpenMap.Enabled = true;
			this.cmdOpenMap.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdOpenMap.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdOpenMap.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdOpenMap.TabStop = true;
			this.cmdOpenMap.Name = "cmdOpenMap";
			this.cmdCreateMap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cmdCreateMap.Text = "Create &New Map";
			this.cmdCreateMap.Size = new System.Drawing.Size(81, 33);
			this.cmdCreateMap.Location = new System.Drawing.Point(40, 64);
			this.cmdCreateMap.TabIndex = 0;
			this.cmdCreateMap.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.cmdCreateMap.BackColor = System.Drawing.SystemColors.Control;
			this.cmdCreateMap.CausesValidation = true;
			this.cmdCreateMap.Enabled = true;
			this.cmdCreateMap.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdCreateMap.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdCreateMap.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdCreateMap.TabStop = true;
			this.cmdCreateMap.Name = "cmdCreateMap";
			this.Image1.Size = new System.Drawing.Size(192, 32);
			this.Image1.Location = new System.Drawing.Point(80, 16);
			this.Image1.Image = (System.Drawing.Image) (resources.GetObject("Image1.Image"));
			this.Image1.Enabled = true;
			this.Image1.Cursor = System.Windows.Forms.Cursors.Default;
			this.Image1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
			this.Image1.Visible = true;
			this.Image1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.Image1.Name = "Image1";
			this.Controls.Add(cmdImport);
			this.Controls.Add(cmdExit);
			this.Controls.Add(cmdOpenMap);
			this.Controls.Add(cmdCreateMap);
			this.Controls.Add(Image1);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		#endregion
	}
}
