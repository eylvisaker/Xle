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
	partial class frmSpecial : System.Windows.Forms.Form
	{
		
		#region Default Instance
		
		private static frmSpecial defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
		public static frmSpecial Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new frmSpecial();
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
		
		public int sType;
		public bool setProperties;
		public Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString sData = new Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(100);
		public bool Changing;
		
		//UPGRADE_WARNING: Event cboChestItem.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		//UPGRADE_WARNING: ComboBox event cboChestItem.Change was upgraded to cboChestItem.TextChanged which has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
		private void cboChestItem_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		//UPGRADE_WARNING: Event cboChestItem.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void cboChestItem_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		private void cboChestItem_Leave(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		//UPGRADE_WARNING: Event cboDoorKey.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		//UPGRADE_WARNING: ComboBox event cboDoorKey.Change was upgraded to cboDoorKey.TextChanged which has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
		private void cboDoorKey_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		//UPGRADE_WARNING: Event cboDoorKey.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void cboDoorKey_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		private void cboDoorKey_Leave(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		//UPGRADE_WARNING: Event chkAutoChange.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void chkAutoChange_CheckStateChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		private void cmdCancel_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			MainModule.SelectedOK = false;
			
			this.Hide();
			
		}
		
		private void frmSpecial_Load(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
			
		}
		private void frmSpecial_Paint(System.Object eventSender, System.Windows.Forms.PaintEventArgs eventArgs)
		{
			
			int a;
			int i;
			string b = "";
			string c;
			string tSdata;
			
			tSdata = sData.Value;
			
			if (setProperties)
			{
				
				Changing = true;
				
				if (sType == 1 || sType == 20)
				{
					optType[(short) sType].Checked = true;
					
					a = Strings.Asc(tSdata.Substring(0)) * 256;
					a = a + Strings.Asc(tSdata.Substring(1));
					
					txtNewMap.Text = Conversion.Str(a);
					
					a = Strings.Asc(tSdata.Substring(2)) * 256;
					a = a + Strings.Asc(tSdata.Substring(3));
					
					txtNewX.Text = Conversion.Str(a);
					
					a = Strings.Asc(tSdata.Substring(4)) * 256;
					a = a + Strings.Asc(tSdata.Substring(5));
					
					txtNewY.Text = Conversion.Str(a);
					
					if (Strings.Asc(tSdata.Substring(6)) == 11)
					{
						chkAutoChange.CheckState = System.Windows.Forms.CheckState.Checked;
					}
					else
					{
						chkAutoChange.CheckState = System.Windows.Forms.CheckState.Unchecked;
					}
					
				}
				else if (sType == 21)
				{
					optType[(short) sType].Checked = true;
					optSpeakWith[(short) (Strings.Asc(sData.Value))].Checked = true;
					
				}
				else if (sType == 22)
				{
					optType[(short) sType].Checked = true;
					// do nothing else
					
				}
				else if (sType == 23 || sType == 25)
				{
					optType[(short) sType].Checked = true;
					if (Strings.Asc(tSdata.Substring(0, 1)) == 0)
					{
						optTreasure[0].Checked = true;
						cboChestItem.SelectedIndex = Strings.Asc(tSdata.Substring(1, 1)) - 1;
						
					}
					else
					{
						optTreasure[1].Checked = true;
						txtGold.Text = System.Convert.ToString(Strings.Asc(tSdata.Substring(1, 1)) * 256 + Strings.Asc(tSdata.Substring(2, 1)));
					}
					
				}
				else if (sType == 24)
				{
					
					optType[(short) sType].Checked = true;
					cboDoorKey.SelectedIndex = Strings.Asc(tSdata.Substring(0, 1)) - 4;
					
				}
				else if (sType >= 2 && sType <= optStoreType.UBound() + 2)
				{
					
					optType[2].Checked = true;
					optStoreType[(short) (sType - 2)].Checked = true;
					
					for (i = 1; i <= tSdata.Length; i++)
					{
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						//UPGRADE_WARNING: Couldn't resolve default property of object c. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						c = tSdata.Substring(i - 1, 1);
						
						//UPGRADE_WARNING: Couldn't resolve default property of object c. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						if (c != "\\")
						{
							//UPGRADE_WARNING: Couldn't resolve default property of object c. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							b = b + c;
						}
						else
						{
							break;
						}
						
					}
					
					txtName.Text = b;
					
					//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					i = i + 2;
					
					//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					txtPrice.Text = System.Convert.ToString(Strings.Asc(tSdata.Substring(i - 1, 1)) * 256 + Strings.Asc(tSdata.Substring(i + 1 - 1, 1)));
					
					
				}
				
				setProperties = false;
				
				Changing = false;
				
				UpdateControls();
			}
			
			
		}
		
		private void UpdateControls()
		{
			int a = 0;
			bool storeEnable = false;
			bool mapChangeEnable = false;
			bool okEnable;
			mapChangeEnable = false;
			// 			bool okEnable;
			// 			bool mapChangeEnable;
			// 			bool okEnable;
			int i;
			//			char c;
			
			
			if (! Changing)
			{
				//On Error Resume Next
				
				okEnable = true;
				
				for (i = optType.LBound(); i <= optType.UBound(); i++)
				{
					if (optType[(short) i].Checked == true)
					{
						a = 1;
					}
				}
				
				if (a == 0)
				{
					okEnable = false;
				}
				
				sData = new Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(100);
				
				if (! Information.IsNumeric(txtSpcWidth.Text) && ! Information.IsNumeric(txtSpcHeight.Text))
				{
					okEnable = false;
				}
				
				if (optType[2].Checked == true)
				{
					storeEnable = true;
					
					if (txtName.Text == "" || ! Information.IsNumeric(txtPrice.Text))
					{
						okEnable = false;
					}
					
					
					//UPGRADE_WARNING: Mod has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					sData.Value = sData.Value + txtName.Text + "\\0" + Strings.Chr(int.Parse(txtPrice.Text) / 256) + Strings.Chr(int.Parse(txtPrice.Text) % 256);
					
					a = 0;
					
					for (i = 0; i <= optStoreType.UBound(); i++)
					{
						if (optStoreType[(short) i].Checked == true)
						{
							a = 1;
						}
					}
					
					if (a == 0)
					{
						okEnable = false;
					}
					
				}
				else if (optType[1].Checked == true || optType[20].Checked == true)
				{
					mapChangeEnable = true;
					
					if (txtNewMap.Text == "")
					{
						okEnable = false;
					}
					if (txtNewX.Text == "")
					{
						okEnable = false;
					}
					if (txtNewY.Text == "")
					{
						okEnable = false;
					}
					
					a = int.Parse(txtNewMap.Text) / 256;
					sData.Value = sData.Value + Strings.Chr(a);
					
					//UPGRADE_WARNING: Mod has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					a = int.Parse(txtNewMap.Text) % 256;
					sData.Value = sData.Value + Strings.Chr(a);
					
					a = int.Parse(txtNewX.Text) / 256;
					sData.Value = sData.Value + Strings.Chr(a);
					
					//UPGRADE_WARNING: Mod has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					a = int.Parse(txtNewX.Text) % 256;
					sData.Value = sData.Value + Strings.Chr(a);
					
					a = int.Parse(txtNewY.Text) / 256;
					sData.Value = sData.Value + Strings.Chr(a);
					
					//UPGRADE_WARNING: Mod has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					a = int.Parse(txtNewY.Text) % 256;
					sData.Value = sData.Value + Strings.Chr(a);
					
					if (System.Convert.ToInt32(chkAutoChange.CheckState) == 1)
					{
						a = 11;
					}
					else
					{
						a = 10;
					}
					sData.Value = sData.Value + Strings.Chr(a);
					
				}
				else if (optType[21].Checked == true)
				{
					
					a = optSpeakWith.LBound() - 1;
					for (i = optSpeakWith.LBound(); i <= optSpeakWith.UBound(); i++)
					{
						if (optSpeakWith[(short) i].Checked == true)
						{
							a = i;
						}
						
					}
					
					if (a < optSpeakWith.LBound())
					{
						okEnable = false;
					}
					else
					{
						sData.Value = sData.Value + Strings.Chr(a);
					}
					
				}
				else if (optType[23].Checked == true || optType[25].Checked == true)
				{
					
					if (optTreasure[0].Checked == true)
					{
						sData.Value = sData.Value + '\0';
						
						sData.Value = sData.Value + Strings.Chr(cboChestItem.SelectedIndex + 1);
						
					}
					else if (optTreasure[1].Checked == true)
					{
						sData.Value = sData.Value + '\u0001';
						
						if (Information.IsNumeric(txtGold.Text))
						{
							sData.Value = sData.Value + Strings.Chr(int.Parse(txtGold.Text) / 256);
							sData.Value = sData.Value + Strings.Chr(int.Parse(txtGold.Text) % 256);
						}
						else
						{
							
							okEnable = false;
						}
						
					}
					
				}
				else if (optType[24].Checked == true)
				{
					
					if (cboDoorKey.SelectedIndex > - 1)
					{
						sData.Value = sData.Value + Strings.Chr(cboDoorKey.SelectedIndex + 4);
					}
					else
					{
						okEnable = false;
					}
					
				}
				
				lblSpecialData.Text = "";
				for (i = 1; i <= Strings.Len(sData); i++)
				{
					lblSpecialData.Text = lblSpecialData.Text + Conversion.Hex(Strings.Asc(sData.Value.Substring(i - 1, 1))) + " ";
				}
				
				frmStore.Visible = storeEnable;
				frmMapChange.Visible = mapChangeEnable;
				cmdOK.Enabled = okEnable;
				
			}
			
		}
		
		
		private void cmdOK_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
			
			int i;
			
			MainModule.SelectedOK = true;
			
			sType = 0;
			
			if (optType[2].Checked)
			{
				for (i = 0; i <= optStoreType.Count() - 1; i++)
				{
					if (optStoreType[(short) i].Checked == true)
					{
						sType = i + 2;
					}
				}
			}
			else if (optType[1].Checked)
			{
				sType = 1;
			}
			else
			{
				for (i = 20; i <= optType.UBound(); i++)
				{
					if (optType[(short) i].Checked == true)
					{
						sType = i;
					}
				}
				
				
			}
			
			//sData = lblSpecialData
			
			this.Hide();
			
			
		}
		
		//UPGRADE_WARNING: Event optSpeakWith.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void optSpeakWith_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			//If eventSender.Checked Then
			//    Dim Index As Integer = optSpeakWith.GetIndex(eventSender)
			//    UpdateControls()
			//End If
		}
		
		//UPGRADE_WARNING: Event optTreasure.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void optTreasure_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			//If eventSender.Checked Then
			//    Dim Index As Integer = optTreasure.GetIndex(eventSender)
			//    cboChestItem.Enabled = False
			//    txtGold.Enabled = False
			
			//    If Index = 0 Then cboChestItem.Enabled = True
			//    If Index = 1 Then txtGold.Enabled = True
			
			//    UpdateControls()
			//End If
		}
		
		//UPGRADE_WARNING: Event optType.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void optType_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			//If eventSender.Checked Then
			//    Dim Index As Integer = optType.GetIndex(eventSender)
			//    frmSpeakWith.Visible = False
			//    frmStore.Visible = False
			//    frmMapChange.Visible = False
			//    frmChest.Visible = False
			//    frmDoor.Visible = False
			
			
			//    Select Case Index
			//        Case 2
			//            frmStore.Visible = True
			//        Case 1, 20
			//            frmMapChange.Visible = True
			//        Case 21
			//            frmSpeakWith.Visible = True
			//        Case 23, 25
			//            frmChest.Visible = True
			//        Case 24
			//            frmDoor.Visible = True
			//    End Select
			
			//    UpdateControls()
			//End If
		}
		
		public void SetDefaults()
		{
			int i;
			
			optType[1].Checked = false;
			optType[2].Checked = false;
			optType[20].Checked = false;
			optType[21].Checked = false;
			
			for (i = 0; i <= optStoreType.Count() - 1; i++)
			{
				optStoreType[(short) i].Checked = false;
			}
			
			txtLocX.Text = MainModule.x1.ToString();
			txtLocY.Text = MainModule.y1.ToString();
			
			txtName.Text = "";
			txtNewX.Text = "";
			txtNewY.Text = "";
			
			txtGold.Text = "";
			cboDoorKey.SelectedIndex = - 1;
			cboChestItem.SelectedIndex = - 1;
			
			optTreasure[0].Checked = false;
			optTreasure[0].Checked = false;
			
			txtSpcWidth.Text = "1";
			txtSpcHeight.Text = "1";
			
			txtPrice.Text = "100";
			
			UpdateControls();
			
		}
		
		//UPGRADE_WARNING: Event txtGold.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtGold_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		//UPGRADE_WARNING: Event txtName.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtName_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		private void txtName_Leave(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		private void txtNewX_Leave(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		private void txtNewY_Leave(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		private void txtNewMap_Leave(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		//UPGRADE_WARNING: Event txtPrice.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtPrice_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
	}
}
