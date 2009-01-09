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
	partial class frmProperties : System.Windows.Forms.Form
	{
		
		#region Default Instance
		
		private static frmProperties defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
		public static frmProperties Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new frmProperties();
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
		public int mType;
		public string theTiles;
		public int mBuyRaftX;
		public int mbuyraftmap;
		public int mBuyRaftY;
		public bool setProperties;
		
		
		private void cmdCancel_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			MainModule.SelectedOK = false;
			
			this.Hide();
			
		}
		
		private void cmdOK_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			short i;
			
			MainModule.SelectedOK = true;
			
			for (i = 1; i <= 6; i++)
			{
				if (optType[i].Checked == true)
				{
					//UPGRADE_WARNING: Couldn't resolve default property of object mType. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					mType = i;
				}
				
			}
			
			for (i = 0; i <= 3; i++)
			{
				if (optTiles[i].Checked == true)
				{
					//UPGRADE_WARNING: Couldn't resolve default property of object theTiles. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					theTiles = optTiles[i].Tag.ToString();
				}
			}
			
			mBuyRaftX = int.Parse(txtBuyRaftX.Text);
			mBuyRaftY = int.Parse(txtBuyRaftY.Text);
			mbuyraftmap = int.Parse(txtBuyRaftMap.Text);
			
			this.Hide();
			
		}
		
		private void cmdDefaults_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			if (Interaction.MsgBox("Restore all properties to defaults", MsgBoxStyle.YesNo, "Defaults") == MsgBoxResult.Yes)
			{
				SetDefaults();
			}
		}
		
		private void frmProperties_Load(object sender, System.EventArgs e)
		{
			cboTypes.Items.AddRange(XleFactory.MapTypes.ToArray());
		}
		
		private void frmProperties_Paint(System.Object eventSender, System.Windows.Forms.PaintEventArgs eventArgs)
		{
			short i;
			
			//UPGRADE_WARNING: Couldn't resolve default property of object setProperties. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			if (setProperties == true)
			{
				optType[(short) mType].Checked = true;
				
				for (i = 0; i <= 3; i++)
				{
					//UPGRADE_WARNING: Couldn't resolve default property of object theTiles. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					if (optTiles[i].Tag.ToString() == theTiles)
					{
						optTiles[i].Checked = true;
					}
				}
				
				
				//UPGRADE_WARNING: Couldn't resolve default property of object setProperties. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				setProperties = false;
			}
			
			UpdateControls();
		}
		
		//UPGRADE_WARNING: Event optType.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void optType_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			RadioButton sender = (RadioButton) eventSender;
			
			if (sender.Checked)
			{
				int Index = optType.GetIndex(sender);
				if (Index == System.Convert.ToInt32(MainModule.EnumMapType.mapOutside))
				{
					optTiles[0].Checked = true;
				}
				else if (Index == System.Convert.ToInt32(MainModule.EnumMapType.maptown))
				{
					optTiles[1].Checked = true;
				}
				
				UpdateControls();
			}
		}
		
		//UPGRADE_WARNING: Event txtAttack.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtAttack_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		//UPGRADE_WARNING: Event txtBpT.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtBpT_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		//UPGRADE_WARNING: Event txtColor.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtColor_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		//UPGRADE_WARNING: Event txtDefaultTile.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtDefaultTile_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		//UPGRADE_WARNING: Event txtDefense.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtDefense_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
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
		
		//UPGRADE_WARNING: Event txtHP.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtHP_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		//UPGRADE_WARNING: Event txtName.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtName_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		private void UpdateControls()
		{
			bool OKEnabled;
			bool CharEnabled = false;
			bool GuardsEnabled = false;
			bool TilesEnabled;
			
			short i;
			
			OKEnabled = false;
			
			for (i = 1; i <= 6; i++)
			{
				if (optType[i].Checked == true)
				{
					OKEnabled = true;
					
					if (i == System.Convert.ToInt16(MainModule.EnumMapType.maptown)|| i == System.Convert.ToInt16(MainModule.EnumMapType.mapCastle)|| i == System.Convert.ToInt16(MainModule.EnumMapType.mapTemple))
					{
						CharEnabled = true;
					}
				}
			}
			
			TilesEnabled = true;
			
			if (optType[System.Convert.ToInt16(MainModule.EnumMapType.mapDungeon)].Checked == true || optType[System.Convert.ToInt16(MainModule.EnumMapType.mapMuseum)].Checked == true)
			{
				TilesEnabled = false;
			}
			
			if (txtName.Text == "" || txtHeight.Text == "" || txtWidth.Text == "")
			{
				OKEnabled = false;
			}
			
			if (Information.IsNumeric(txtWidth.Text) && Information.IsNumeric(txtHeight.Text))
			{
				if (Conversion.Val(txtWidth.Text) < 1 || Conversion.Val(txtWidth.Text) > MainModule.maxMapSize || Conversion.Val(txtHeight.Text) < 1 || Conversion.Val(txtHeight.Text) > MainModule.maxMapSize)
				{
					OKEnabled = false;
				}
			}
			else
			{
				OKEnabled = false;
			}
			
			// TODO: Fix these:
			//If optType(MainModule.EnumMapType.maptown).Checked = True Or optType(MainModule.EnumMapType.mapCastle).Checked = True Then
			//    GuardsEnabled = True
			//Else
			//    GuardsEnabled = False
			//End If
			
			//If optType(MainModule.EnumMapType.mapDungeon).Checked = True Then
			//    frmDungeon.Visible = True
			//    frmDungeon.Top = frmChar.Top
			
			//Else
			//    frmDungeon.Visible = False
			//End If
			
			cmdOK.Enabled = OKEnabled;
			frmChar.Visible = CharEnabled;
			frmGuards.Visible = GuardsEnabled;
			
		}
		
		public void SetDefaults()
		{
			short i;
			
			for (i = 1; i <= 6; i++)
			{
				optType[i].Checked = false;
			}
			
			txtName.Text = "";
			
		}
		
		//UPGRADE_WARNING: Event txtWidth.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtWidth_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
	}
}
