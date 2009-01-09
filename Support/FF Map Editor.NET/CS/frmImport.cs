#define Win32
using System.Diagnostics;
using System.Data;
using System;
using System.Windows.Forms;
using AxComCtl3;
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
	partial class frmImport : System.Windows.Forms.Form
	{
		
		#region Default Instance
		
		private static frmImport defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
		public static frmImport Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new frmImport();
					defaultInstance.FormClosed += new FormClosedEventHandler(defaultInstance_FormClosed);
				}
				
				return defaultInstance;
			}
		}
		
		static void defaultInstance_FormClosed(object sender, FormClosedEventArgs e)
		{
			defaultInstance = null;
		}
		
		#endregion
		public object mType;
		public object theTiles;
		public bool ClickedOK;
		
		//UPGRADE_WARNING: Event chkAuto.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void chkAuto_CheckStateChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
			
			
		}
		
		public void SetDefaults()
		{
			short i;
			
			for (i = 1; i <= 6; i++)
			{
				optType[i].Checked = false;
			}
			
			txtFileOffset.Text = "0";
			txtAreaWidth.Text = "1";
			txtAreaHeight.Text = "1";
			
			chkAuto.CheckState = System.Windows.Forms.CheckState.Checked;
			
		}
		
		
		private void cmdCancel_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			
			ClickedOK = false;
			this.Hide();
			
		}
		
		private void cmdOK_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			short i;
			
			if (chkAuto.CheckState)
			{
				MainModule.AutoHeightWidth = true;
			}
			
			MainModule.ImportOffset = short.Parse(txtFileOffset.Text);
			MainModule.AreaWidth = short.Parse(txtAreaWidth.Text);
			MainModule.AreaHeight = short.Parse(txtAreaHeight.Text);
			
			
			for (i = 1; i <= 6; i++)
			{
				if (optType[i].Checked == true)
				{
					MainModule.MapType = i;
				}
				
			}
			
			for (i = 0; i <= 3; i++)
			{
				if (optTiles[i].Checked == true)
				{
					MainModule.TileSet = optTiles[i].Tag;
				}
			}
			
			
			MainModule.RecalibrateImport();
			
			ClickedOK = true;
			this.Hide();
			
		}
		
		//UPGRADE_WARNING: Event optTiles.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void optTiles_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			if (eventSender.Checked)
			{
				short Index = optTiles.GetIndex(eventSender);
				UpdateControls();
			}
		}
		
		//UPGRADE_WARNING: Event optType.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void optType_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			if (eventSender.Checked)
			{
				short Index = optType.GetIndex(eventSender);
				if (Index == MainModule.EnumMapType.mapOutside)
				{
					optTiles[0].Checked = true;
				}
				else if (Index == MainModule.EnumMapType.maptown)
				{
					optTiles[1].Checked = true;
				}
				else if (Index == MainModule.EnumMapType.mapCastle)
				{
					optTiles[2].Checked = true;
				}
				
				UpdateControls();
			}
		}
		
		private void UpdateControls()
		{
			short i;
			short ct;
			bool invalid;
			
			invalid = false;
			
			if (chkAuto.CheckState)
			{
				txtWidth.Enabled = false;
				txtHeight.Enabled = false;
			}
			else
			{
				if (! Information.IsNumeric(txtWidth.Text))
				{
					invalid = true;
				}
				if (! Information.IsNumeric(txtHeight.Text))
				{
					invalid = true;
				}
				
				txtWidth.Enabled = true;
				txtHeight.Enabled = true;
				
			}
			
			
			for (i = 1; i <= 6; i++)
			{
				if (optType[i].Checked == true)
				{
					break;
				}
			}
			
			if (i == 7)
			{
				invalid = true;
			}
			
			for (i = 0; i <= 4; i++)
			{
				if (optTiles[i].Checked == true)
				{
					break;
				}
				
			}
			
			if (i == 5)
			{
				invalid = true;
			}
			
			if (! Information.IsNumeric(txtFileOffset.Text))
			{
				invalid = true;
			}
			if (! Information.IsNumeric(txtAreaWidth.Text))
			{
				invalid = true;
			}
			if (! Information.IsNumeric(txtAreaHeight.Text))
			{
				invalid = true;
			}
			
			if (invalid)
			{
				cmdOK.Enabled = false;
			}
			else
			{
				cmdOK.Enabled = true;
			}
			
		}
		
		//UPGRADE_WARNING: Event txtAreaHeight.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtAreaHeight_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
			
		}
		
		//UPGRADE_WARNING: Event txtAreaWidth.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtAreaWidth_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
			
		}
		
		//UPGRADE_WARNING: Event txtFileOffset.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtFileOffset_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
			
		}
		
		//UPGRADE_WARNING: Event txtHeight.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtHeight_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
			
		}
		
		//UPGRADE_WARNING: Event txtWidth.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtWidth_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
	}
}
