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
using Agate = ERY.AgateLib.Geometry;
using Microsoft.VisualBasic.CompilerServices;


// TODO: fix all mouse down events to use integers instead of singles!
//
namespace XleMapEditor
{
	partial class frmMEdit : System.Windows.Forms.Form
	{
		
		#region Default Instance
		
		private static frmMEdit defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
		public static frmMEdit Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new frmMEdit();
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
		private int oldw;
		private int oldh;
		private int offset;
		private bool validMap;
		
		private ERY.AgateLib.DisplayWindow mainWindow;
		private ERY.AgateLib.DisplayWindow tilesWindow;
		
		//UPGRADE_WARNING: Event chkDrawGuards.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void chkDrawGuards_CheckStateChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			blit();
		}
		
		//UPGRADE_WARNING: Event chkDrawRoof.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void chkDrawRoof_CheckStateChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			blit();
		}
		
		private void cmdDeleteSpecial_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			int i;
			
			for (i = 1; i <= MainModule.maxSpecial; i++)
			{
				if (MainModule.specialx[i] == MainModule.x1 && MainModule.specialy[i] == MainModule.y1)
				{
					MainModule.specialx[i] = 0;
					MainModule.specialy[i] = 0;
					MainModule.specialwidth[i] = 0;
					MainModule.specialheight[i] = 0;
					MainModule.special[i] = 0;
					
				}
			}
			
			FillSpecial();
			blit();
			
			SetPos(MainModule.x1, MainModule.y1);
			
			
		}
		
		private void cmdFill_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			int xdif;
			int ydif;
			int tile;
			int r;
			int i;
			int j;
			
			//UPGRADE_WARNING: Couldn't resolve default property of object xdif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			xdif = MainModule.x2 - MainModule.x1;
			//UPGRADE_WARNING: Couldn't resolve default property of object ydif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			ydif = MainModule.x2 - MainModule.x1;
			
			//UPGRADE_WARNING: Couldn't resolve default property of object xdif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			if (xdif == 0)
			{
				//UPGRADE_WARNING: Couldn't resolve default property of object xdif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				xdif++;
			}
			
			//UPGRADE_WARNING: Couldn't resolve default property of object ydif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			if (ydif == 0)
			{
				//UPGRADE_WARNING: Couldn't resolve default property of object ydif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				ydif++;
			}
			
			//UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			tile = MainModule.currentTile;
			if (System.Convert.ToInt32(chkRandom.CheckState) == 1)
			{
				if (MainModule.currentTile == 7)
				{
					//UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					r = System.Convert.ToInt32(VBMath.Rnd(1) * 4);
					
					//UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					//UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					if (r < 2)
					{
						tile = MainModule.currentTile + r;
					}
					//UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					//UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					if (r > 1)
					{
						tile = MainModule.currentTile + r + 14;
					}
					
				}
				else if (MainModule.currentTile == 2 || MainModule.currentTile == 129 || MainModule.currentTile == 182)
				{
					//UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					r = System.Convert.ToInt32(VBMath.Rnd(1) * 2);
					
					//UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					//UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					tile = MainModule.currentTile + r;
					
				}
			}
			
			//UPGRADE_WARNING: Couldn't resolve default property of object xdif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			for (i = MainModule.x1; i <= MainModule.x2; i += System.Math.Sign(xdif))
			{
				//UPGRADE_WARNING: Couldn't resolve default property of object ydif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				for (j = MainModule.y1; j <= MainModule.y2; j += System.Math.Sign(ydif))
				{
					if (System.Convert.ToInt32(chkRandom.CheckState) == 1)
					{
						if (MainModule.currentTile == 7)
						{
							//UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							r = System.Convert.ToInt32(VBMath.Rnd(1) * 4);
							
							//UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							if (r < 2)
							{
								tile = MainModule.currentTile + r;
							}
							//UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							if (r > 1)
							{
								tile = MainModule.currentTile + r + 14;
							}
							
						}
						else if (MainModule.currentTile == 2 || MainModule.currentTile == 129 || MainModule.currentTile == 182)
						{
							//UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							r = System.Convert.ToInt32(VBMath.Rnd(1) * 2);
							
							//UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							tile = MainModule.currentTile + r;
							
						}
					}
					
					//UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					//UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					PaintLoc(i, j, tile);
					
				}
			}
			
			blit();
			
		}
		
		private void cmdGuard_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			int i;
			bool found = false;
			
			for (i = 0; i <= 100; i++)
			{
				if (MainModule.guard[i].X == MainModule.x1 && MainModule.guard[i].Y == MainModule.y1)
				{
					MainModule.guard[i].X = 0;
					MainModule.guard[i].Y = 0;
					found = true;
				}
			}
			
			if (found != true)
			{
				for (i = 0; i <= 100; i++)
				{
					if (MainModule.guard[i].X == 0 && MainModule.guard[i].Y == 0)
					{
						MainModule.guard[i].X = MainModule.x1;
						MainModule.guard[i].Y = MainModule.y1;
						
						blit();
						return;
						
					}
				}
			}
			
			blit();
		}
		
		private void cmdModifySpecial_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			//			int j;
			int i;
			int s = 0;
			// 			int i;
			// 			int s;
			
			
			for (i = 1; i <= MainModule.maxSpecial; i++)
			{
				if (MainModule.specialx[i] == MainModule.x1 && MainModule.specialy[i] == MainModule.y1)
				{
					frmSpecial.Default.Changing = true;
					
					frmSpecial.Default.sData = MainModule.specialdata[i];
					frmSpecial.Default.txtLocX.Text = MainModule.specialx[i].ToString();
					frmSpecial.Default.txtLocY.Text = MainModule.specialy[i].ToString();
					frmSpecial.Default.sType = MainModule.special[i];
					frmSpecial.Default.setProperties = true;
					frmSpecial.Default.txtSpcWidth.Text = MainModule.specialwidth[i].ToString();
					frmSpecial.Default.txtSpcHeight.Text = MainModule.specialheight[i].ToString();
					
					frmSpecial.Default.Changing = false;
					
					s = i;
				}
			}
			
			frmSpecial.Default.ShowDialog();
			
			if (MainModule.SelectedOK)
			{
				MainModule.special[s] = frmSpecial.Default.sType;
				MainModule.specialx[s] = int.Parse(frmSpecial.Default.txtLocX.Text);
				MainModule.specialy[s] = int.Parse(frmSpecial.Default.txtLocY.Text);
				MainModule.specialdata[s] = frmSpecial.Default.sData;
				MainModule.specialwidth[s] = int.Parse(frmSpecial.Default.txtSpcWidth.Text);
				MainModule.specialheight[s] = int.Parse(frmSpecial.Default.txtSpcHeight.Text);
				
			}
			
			SetPos(MainModule.x1, MainModule.y1);
			
			FillSpecial();
			
		}
		
		private void cmdObject_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			
			int Index;
			int i;
			int j;
			
			Index = lstPreDef.SelectedIndex;
			
			for (j = 0; j <= MainModule.PreDefObjects[Index].height - 1; j++)
			{
				for (i = 0; i <= MainModule.PreDefObjects[Index].width - 1; i++)
				{
					//UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					if (MainModule.PreDefObjects[Index].Matrix[i, j] > - 1)
					{
						//UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						PaintLoc(MainModule.x1 + i, MainModule.y1 + j, MainModule.PreDefObjects[Index].Matrix[i, j]);
					}
				}
			}
			
			blit();
		}
		
		private void cmdPlaceSpecial_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			
			int i;
			//			int sx;
			//			int sy;
			//			int j;
			
			frmSpecial.Default.SetDefaults();
			
			frmSpecial.Default.txtLocX.Text = MainModule.x1.ToString();
			frmSpecial.Default.txtLocY.Text = MainModule.y1.ToString();
			frmSpecial.Default.txtSpcWidth.Text = System.Convert.ToString(MainModule.x2 - MainModule.x1 + 1);
			frmSpecial.Default.txtSpcHeight.Text = System.Convert.ToString(MainModule.y2 - MainModule.y1 + 1);
			
			frmSpecial.Default.ShowDialog();
			
			if (MainModule.SelectedOK)
			{
				for (i = 1; i <= MainModule.maxSpecial; i++)
				{
					if (MainModule.special[i] == 0)
					{
						
						MainModule.special[i] = frmSpecial.Default.sType;
						
						MainModule.specialx[i] = int.Parse(frmSpecial.Default.txtLocX.Text);
						MainModule.specialy[i] = int.Parse(frmSpecial.Default.txtLocY.Text);
						
						MainModule.specialwidth[i] = int.Parse(frmSpecial.Default.txtSpcWidth.Text);
						MainModule.specialheight[i] = int.Parse(frmSpecial.Default.txtSpcHeight.Text);
						
						MainModule.specialdata[i] = new Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(100, frmSpecial.Default.sData.Value);
						
						break;
					}
				}
				
				SetPos(MainModule.x1, MainModule.y1);
				
				FillSpecial();
			}
			
			blit();
			
		}
		
		private void cmdRoof_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			int i;
			//			int j;
			bool found = false;
			
			//UPGRADE_WARNING: Couldn't resolve default property of object frmRoof.RoofIndex. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			frmRoof.Default.RoofIndex = 1;
			
			for (i = 1; i <= MainModule.maxRoofs; i++)
			{
				//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				if (MainModule.x1 >= MainModule.Roofs[i].anchorTarget.X - MainModule.Roofs[i].anchor.X && MainModule.y1 >= MainModule.Roofs[i].anchorTarget.Y - MainModule.Roofs[i].anchor.Y && MainModule.x1 < MainModule.Roofs[i].anchorTarget.X - MainModule.Roofs[i].anchor.X + MainModule.Roofs[i].width && MainModule.y1 < MainModule.Roofs[i].anchorTarget.Y - MainModule.Roofs[i].anchor.Y + MainModule.Roofs[i].height)
				{
					found = true;
					//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					//UPGRADE_WARNING: Couldn't resolve default property of object frmRoof.RoofIndex. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					frmRoof.Default.RoofIndex = i;
					//frmRoof.txtAnchorX = Roofs(i).anchor.X
					//frmRoof.txtAnchorY = Roofs(i).anchor.Y
					//frmRoof.txtTargetX = Roofs(i).anchorTarget.X
					//frmRoof.txtTargetY = Roofs(i).anchorTarget.Y
					//frmRoof.txtRoofX = Roofs(i).width
					//frmRoof.txtRoofY = Roofs(i).height
					frmRoof.Default.SetControls();
					
					break;
				}
			}
			
			if (found == false)
			{
				for (i = 1; i <= MainModule.maxRoofs; i++)
				{
					//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					if (MainModule.Roofs[i].anchorTarget.X == 0 && MainModule.Roofs[i].anchorTarget.Y == 0)
					{
						
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						//UPGRADE_WARNING: Couldn't resolve default property of object frmRoof.RoofIndex. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						frmRoof.Default.RoofIndex = i;
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						MainModule.Roofs[i].width = MainModule.x2 - MainModule.x1 + 1;
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						MainModule.Roofs[i].height = MainModule.y2 - MainModule.y1 + 1;
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						MainModule.Roofs[i].anchorTarget.X = MainModule.x1;
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						MainModule.Roofs[i].anchorTarget.Y = MainModule.y1;
						frmRoof.Default.chkDrawGround.CheckState = System.Windows.Forms.CheckState.Checked;
						frmRoof.Default.SetControls();
						
						break;
					}
				}
			}
			
			
			frmRoof.Default.ShowDialog();
			
			
			
		}
		
		private void cmdSetCorner_Click()
		{
			//			object optCorner;
			//			object txtCY;
			//			object txtCX;
			//			object i;
			// TODO: I have no idea what this function is supposed to do.
			
			//For i = 0 To 1
			//    'UPGRADE_WARNING: Couldn't resolve default property of object optCorner(i).value. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			//    If optCorner(i).value = True Then
			//        'UPGRADE_WARNING: Couldn't resolve default property of object txtCX(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			//        txtCX(i) = x1
			//        'UPGRADE_WARNING: Couldn't resolve default property of object txtCY(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			//        txtCY(i) = y1
			//    End If
			//Next
			
		}
		
		
		//UPGRADE_WARNING: Form event frmMEdit.Activate has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
		private void frmMEdit_Activated(System.Object eventSender, System.EventArgs eventArgs)
		{
			MainModule.UpdateScreen = true;
			
		}
		
		//UPGRADE_WARNING: Form event frmMEdit.Deactivate has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
		private void frmMEdit_Deactivate(System.Object eventSender, System.EventArgs eventArgs)
		{
			MainModule.UpdateScreen = false;
		}
		
		private void frmMEdit_Load(System.Object eventSender, System.EventArgs eventArgs)
		{
			int i;
			
			for (i = 0; i <= 15; i++)
			{
				Label5.Text = Label5.Text + Conversion.Hex(i) + System.Environment.NewLine;
			}
			
			mainWindow = new ERY.AgateLib.DisplayWindow(Picture1);
			tilesWindow = new ERY.AgateLib.DisplayWindow(Picture2);
			
			MainModule.CreateSurfaces((Picture1.Width), (Picture1.Height));
			
			cmdDialogOpen.InitialDirectory = MainModule.LotaPath;
			cmdDialogSave.InitialDirectory = MainModule.LotaPath;
			
			while (! validMap)
			{
				
				frmStartup.Default.ShowDialog();
				
				if (MainModule.StartNewMap)
				{
					AssignProperties();
					NewMap(false);
					
				}
				else if (MainModule.ImportMap)
				{
					ImportNewMap();
					
				}
				else
				{
					OpenMap();
					
				}
				
			}
			
			MainModule.x1 = 0;
			MainModule.x2 = 0;
			MainModule.y1 = 0;
			MainModule.y2 = 0;
			
			blit();
		}
		
		private void frmMEdit_Paint(System.Object eventSender, System.Windows.Forms.PaintEventArgs eventArgs)
		{
			
		}
		
		public void blit()
		{
			if (MainModule.UpdateScreen == true)
			{
				
				draw();
				
			}
			
			
		}
		
		private void draw()
		{
			int tiley;
			int a;
			int j;
			int i;
			int tilex;
			int k;
			int centery;
			int centerx;
			int t;
			int xx;
			int yy;
			ERY.AgateLib.Geometry.Rectangle srcRect = new ERY.AgateLib.Geometry.Rectangle();
			ERY.AgateLib.Geometry.Rectangle destRect = new 			ERY.AgateLib.Geometry.Rectangle();
			// 			ERY.AgateLib.Geometry.Rectangle destRect;
			// 			ERY.AgateLib.Geometry.Rectangle destRect;
			
			Display.RenderTarget = mainWindow;
			
			Display.BeginFrame();
			Display.Clear(Agate.Color.FromArgb(0x55, 0x55, 0x55));
			
			
			lblDim.Text = "Map Dimensions: " + MainModule.mapWidth + " x " + MainModule.mapHeight;
			if (MainModule.fileName != "")
			{
				this.Text = "LotA Town Editor - " + MainModule.fileName;
			}
			
			MainModule.picTilesX = Picture1.ClientRectangle.Width / MainModule.TileSize / 2;
			MainModule.picTilesY = Picture1.ClientRectangle.Height / MainModule.TileSize / 2;
			
			
			xx = 0;
			yy = 0;
			
			sbRight1.Maximum = MainModule.mapWidth;
			sbRight1.Minimum = 0;
			sbRight1.LargeChange = MainModule.picTilesX * 2 - 2;
			sbDown.LargeChange = MainModule.picTilesY * 2 - 2;
			sbDown.Maximum = MainModule.mapHeight;
			sbDown.Minimum = 0;
			
			
			centerx = sbRight1.Value;
			centery = sbDown.Value;
			
			MainModule.leftX = centerx - MainModule.picTilesX;
			MainModule.topy = centery - MainModule.picTilesY;
			
			sbSpecial.Minimum = 0;
			sbSpecial.Maximum = NumSpecials();
			sbSpecial.LargeChange = 5;
			sbSpecial.SmallChange = 1;
			
			lblSpcCount.Text = "Specials: " + sbSpecial.Maximum + " count.";
			
			for (j = MainModule.dispY - MainModule.picTilesY; j <= MainModule.dispY + MainModule.picTilesY + 1; j++)
			{
				for (i = MainModule.dispX - MainModule.picTilesX; i <= MainModule.dispX + MainModule.picTilesX + 1; i++)
				{
					if (i >= 0 && i < MainModule.mapWidth && j >= 0 && j < MainModule.mapHeight)
					{
						
						a = MainModule.Map(i, j);
						
						if (chkDrawRoof.CheckState != 0)
						{
							for (k = 1; k <= MainModule.maxRoofs; k++)
							{
								if ((i >= MainModule.Roofs[k].anchorTarget.X - MainModule.Roofs[k].anchor.X && j >= MainModule.Roofs[k].anchorTarget.Y - MainModule.Roofs[k].anchor.Y) && (i < MainModule.Roofs[k].anchorTarget.X - MainModule.Roofs[k].anchor.X + MainModule.Roofs[k].width && j < MainModule.Roofs[k].anchorTarget.Y - MainModule.Roofs[k].anchor.Y + MainModule.Roofs[k].height))
								{
									
									t = MainModule.Roofs[k].Matrix[i - MainModule.Roofs[k].anchorTarget.X + MainModule.Roofs[k].anchor.X, j - MainModule.Roofs[k].anchorTarget.Y + MainModule.Roofs[k].anchor.Y];
									
									if (t != 127)
									{
										a = t;
									}
									
								}
							}
							
						}
						
						tilex = (a % 16) * 16;
						tiley = (a / 16)* 16;
						
						srcRect.X = tilex;
						srcRect.Y = tiley;
						srcRect.Width = MainModule.TileSize;
						srcRect.Height = MainModule.TileSize;
						
						destRect.X = xx * MainModule.TileSize;
						destRect.Y = yy * MainModule.TileSize;
						destRect.Width = MainModule.TileSize;
						destRect.Height = MainModule.TileSize;
						
						MainModule.TileSurface.Draw(srcRect, destRect);
						
						
					}
					else
					{
						//Picture1.Line (xx * tilesize, yy * tilesize)-((xx + 1) * tilesize, (yy + 1) * tilesize), vbBlack, BF
						
					}
					
					xx++;
				}
				
				yy++;
				xx = 0;
				
			}
			
			// TODO: Fix this
			//If chkDrawGuards.CheckState And (MapType = MainModule.EnumMapType.maptown Or MapType = MainModule.EnumMapType.mapCastle) Then
			if (chkDrawGuards.Checked)
			{
				for (i = 0; i <= 100; i++)
				{
					//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					if (MainModule.guard[i].X >= MainModule.leftX && MainModule.guard[i].X < MainModule.leftX + 2 * MainModule.picTilesX + 1 && MainModule.guard[i].Y >= MainModule.topy && MainModule.guard[i].Y < MainModule.topy + 2 * MainModule.picTilesY + 1 && MainModule.guard[i].X > 0 && MainModule.guard[i].Y > 0)
					{
						
						srcRect.Y = 5 * 32;
						srcRect.X = 0 * 32;
						srcRect.Width = 32;
						srcRect.Height = 32;
						
						destRect.X = (MainModule.guard[i].X - MainModule.leftX) * MainModule.TileSize;
						destRect.Y = (MainModule.guard[i].Y - MainModule.topy) * MainModule.TileSize;
						destRect.Width = 32;
						destRect.Height = 32;
						
						
						MainModule.CharSurface.Draw(srcRect, destRect);
						
					}
				}
			}
			
			
			for (i = 1; i <= MainModule.maxSpecial; i++)
			{
				if (MainModule.specialx[i] >= MainModule.leftX || MainModule.specialx[i] + MainModule.specialwidth[i] < MainModule.leftX + 2 * MainModule.picTilesX + 1 || MainModule.specialy[i] >= MainModule.topy || MainModule.specialy[i] + MainModule.specialheight[i] < MainModule.topy + 2 * MainModule.picTilesY + 1 || MainModule.special[i] > 0)
				{
					
					srcRect.X = (MainModule.specialx[i] - MainModule.leftX) * MainModule.TileSize;
					srcRect.Width = MainModule.specialwidth[i] * MainModule.TileSize;
					srcRect.Y = (MainModule.specialy[i] - MainModule.topy) * MainModule.TileSize;
					srcRect.Height = MainModule.specialheight[i] * MainModule.TileSize;
					
					//DDSBack.DrawBox(r1.Left, r1.Top, r1.Right, r1.Bottom)
					Display.DrawRect(srcRect, Agate.Color.Cyan);
				}
			}
			
			//DDSBack.SetForeColor(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow))
			
			if (MainModule.MapType != System.Convert.ToInt32(MainModule.EnumMapType.mapOutside))
			{
				///'''''''''''''''''''''''
				//'  Draw Roofs
				for (i = 1; i <= MainModule.maxRoofs; i++)
				{
					if ((MainModule.leftX < MainModule.Roofs[i].anchorTarget.X - MainModule.Roofs[i].anchor.X || MainModule.topy < MainModule.Roofs[i].anchorTarget.Y - MainModule.Roofs[i].anchor.Y) || (MainModule.leftX + MainModule.picTilesX * 2 > MainModule.Roofs[i].anchorTarget.X - MainModule.Roofs[i].anchor.X + MainModule.Roofs[i].width || MainModule.topy + MainModule.picTilesY * 2 > MainModule.Roofs[i].anchorTarget.Y - MainModule.Roofs[i].anchor.Y + MainModule.Roofs[i].height))
					{
						
						
						srcRect.X = (MainModule.Roofs[i].anchorTarget.X - MainModule.leftX) * MainModule.TileSize;
						srcRect.Width = MainModule.TileSize;
						srcRect.Y = (MainModule.Roofs[i].anchorTarget.Y - MainModule.topy) * MainModule.TileSize;
						srcRect.Height = MainModule.TileSize;
						
						Display.DrawRect(srcRect, Agate.Color.Yellow);
						
						srcRect.X = (MainModule.Roofs[i].anchorTarget.X - MainModule.Roofs[i].anchor.X - MainModule.leftX) * MainModule.TileSize;
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						srcRect.Width = MainModule.Roofs[i].width * MainModule.TileSize;
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						srcRect.Y = (MainModule.Roofs[i].anchorTarget.Y - MainModule.Roofs[i].anchor.Y - MainModule.topy) * MainModule.TileSize;
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						srcRect.Height = MainModule.Roofs[i].height * MainModule.TileSize;
						
						Display.DrawRect(srcRect, Agate.Color.Yellow);
					}
				}
			}
			
			Display.DrawRect(Agate.Rectangle.FromLTRB((MainModule.x1 - MainModule.leftX) * MainModule.TileSize, (MainModule.y1 - MainModule.topy) * MainModule.TileSize, (MainModule.x2 - MainModule.leftX + 1) * MainModule.TileSize, (MainModule.y2 - MainModule.topy + 1) * MainModule.TileSize), Agate.Color.White);
			
			Display.EndFrame();
			
			lblX.Text = "x1:   " + MainModule.x1 + "  0x" + Conversion.Hex(MainModule.x1) + "    x2: " + MainModule.x2;
			lblY.Text = "y1:   " + MainModule.y1 + "  0x" + Conversion.Hex(MainModule.y1) + "    y2: " + MainModule.y2;
			
			if (MainModule.x1 < 0 || MainModule.x1 > MainModule.mapWidth || MainModule.y1 < 0 || MainModule.y1 > MainModule.mapHeight)
			{
				lblTile.Text = "Tile: Out of range";
			}
			else
			{
				lblTile.Text = "Tile: " + MainModule.Map(MainModule.x1, MainModule.y1) + "   0x" + Conversion.Hex(MainModule.Map(MainModule.x1, MainModule.y1));
			}
			
			if (MainModule.ImportMap)
			{
				if (MainModule.ImportLocation(MainModule.x1, MainModule.y1) >= 0 && MainModule.ImportLocation(MainModule.x1, MainModule.y1) <= (MainModule.ImportedData.Length - 1))
				{
					lblImport.Text = "Import: " + MainModule.ImportedData[MainModule.ImportLocation(MainModule.x1, MainModule.y1)];
				}
				else
				{
					lblImport.Text = "Import: Out of Range";
				}
			}
			else
			{
				lblImport.Text = "";
			}
			
			lblCurrentTile.Text = "Current Tile: " + MainModule.currentTile + "   0x" + Conversion.Hex(MainModule.currentTile);
			
			// Label7(0).Caption = ""
			// Label7(1).Caption = ""
			// For i = leftX - 1 To leftX + picTilesX * 2 + 2
			//     Label7(i Mod 2) = Label7(i Mod 2) & i
			//     Label7((i + 1) Mod 2) = Label7((i + 1) Mod 2) & "  "
			// Next
			//
			// Label8.Caption = ""
			// For i = topY - 1 To topY + picTilesY * 2 + 2
			//     Label8 = Label8 & i & vbCrLf
			// Next
			
			switch (MainModule.MapType)
			{
				case  (int) (MainModule.EnumMapType.mapOutside):
					cmdGuard.Visible = false;
					break;
				case  (int) (MainModule.EnumMapType.mapDungeon):
					cmdGuard.Visible = false;
					break;
				case  (int) (MainModule.EnumMapType.mapMuseum):
					cmdGuard.Visible = false;
					break;
				case  (int) (MainModule.EnumMapType.maptown):
					cmdGuard.Visible = true;
					break;
				case  (int) (MainModule.EnumMapType.mapCastle):
					cmdGuard.Visible = true;
					break;
			}
			
			
			Display.RenderTarget = tilesWindow;
			Display.BeginFrame();
			
			MainModule.TileSurface.Draw();
			
			Display.EndFrame();
			
		}
		
		private void FillSpecial()
		{
			int i;
			int j;
			
			Text7.Text = "";
			for (i = 1; i <= MainModule.maxSpecial; i++)
			{
				if (MainModule.special[i] > 0)
				{
					if (MainModule.specialx[i] == 0 && MainModule.specialy[i] == 0)
					{
						MainModule.specialheight[i] = 0;
						MainModule.specialwidth[i] = 0;
					}
					
					Text7.Text = Text7.Text + "Store #" + i + ": Type " + MainModule.special[i];
					Text7.Text = Text7.Text + System.Environment.NewLine + "    At Point: (" + MainModule.specialx[i] + ", " + MainModule.specialy[i] + ")";
					Text7.Text = Text7.Text + System.Environment.NewLine + "    Data: " + MainModule.specialdata[i].Value;
					Text7.Text = Text7.Text + System.Environment.NewLine + "          ";
					
					for (j = 1; j <= Strings.RTrim(MainModule.specialdata[i].Value).Length; j++)
					{
						Text7.Text = Text7.Text + Conversion.Hex(Strings.Asc(Strings.Mid(MainModule.specialdata[i].Value, j, 1))) + "  ";
					}
					
					
					Text7.Text = Text7.Text + System.Environment.NewLine + System.Environment.NewLine;
				}
				
			}
		}
		
		//UPGRADE_WARNING: Event frmMEdit.Resize may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void frmMEdit_Resize(System.Object eventSender, System.EventArgs eventArgs)
		{
			frmRight.Left = this.ClientRectangle.Width - frmRight.Width;
			frmBottom.Top = this.ClientRectangle.Height - frmBottom.Height - StatusBar1.Height;
			
			Picture1.Width = frmRight.Left - sbDown.Width - Picture1.Left;
			Picture1.Height = frmBottom.Top - sbRight1.Height - Picture1.Top;
			
			sbDown.Left = Picture1.Left + Picture1.Width;
			sbRight1.Top = Picture1.Top + Picture1.Height;
			
			sbDown.Height = Picture1.Height;
			sbRight1.Width = Picture1.Width;
			
			
		}
		
		private void frmMEdit_FormClosed(System.Object eventSender, System.Windows.Forms.FormClosedEventArgs eventArgs)
		{
			Display.Dispose();
			
			ProjectData.EndApp();
		}
		
		private void lblX_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			int xx;
			xx = int.Parse(Interaction.InputBox("Enter X:", "", "", -1, -1));
			
			SetPos(xx, MainModule.y1);
			
			blit();
		}
		
		private void lblY_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			int yy;
			yy = int.Parse(Interaction.InputBox("Enter Y:", "", "", -1, -1));
			
			SetPos(MainModule.x1, yy);
			
			blit();
		}
		
		
		public void mnuFinalize_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			
			if (Interaction.MsgBox("Are you sure you wish to finalize this imported map" + System.Environment.NewLine + System.Environment.NewLine + "You will have to edit it as a standard map from now on!", MsgBoxStyle.Question | MsgBoxStyle.YesNo, "Finalize") == MsgBoxResult.Yes)
			{
				SetPropertiesForm();
				
				//TODO: Fix this
				//frmProperties.optType(MapType).Checked = True
				frmProperties.Default.txtName.Text = "";
				
				
				frmProperties.Default.ShowDialog();
				
				if (MainModule.SelectedOK)
				{
					AssignProperties();
					
					mnuSaveAs_Click(mnuSaveAs, new System.EventArgs());
					MainModule.ImportMap = false;
					
					SetMenus();
					
					MainModule.UpdateScreen = true;
					blit();
				}
				
			}
			
		}
		
		public void mnuImport_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			
			cmdDialogOpen.Title = "Import Map";
			cmdDialogSave.Title = "Import Map";
			
			cmdDialogOpen.FileName = "E:\\Legacy\\.";
			cmdDialogSave.FileName = "E:\\Legacy\\.";
			cmdDialogOpen.Filter = "Export Files|*.export|All Files (*.*)|*.*";
			cmdDialogSave.Filter = "Export Files|*.export|All Files (*.*)|*.*";
			cmdDialogOpen.FilterIndex = 1;
			cmdDialogSave.FilterIndex = 1;
			
			cmdDialogOpen.InitialDirectory = (new global::Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\..\\Included Maps";
			cmdDialogSave.InitialDirectory = (new global::Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\..\\Included Maps";
			
			cmdDialogOpen.DefaultExt = "map";
			cmdDialogSave.DefaultExt = "map";
			
			cmdDialogOpen.ShowDialog();
			cmdDialogSave.FileName = cmdDialogOpen.FileName;
			
			MainModule.fileName = cmdDialogOpen.FileName;
			
			MainModule.StartNewMap = false;
			MainModule.ImportMap = true;
			
			ImportNewMap();
			
		}
		
		public void mnuImportRefresh_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			MainModule.RecalibrateImport();
			
		}
		
		public void mnuLoadMapping_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			
			
			cmdDialogOpen.Title = "Load Mapping";
			cmdDialogSave.Title = "Load Mapping";
			
			
			cmdDialogOpen.Filter = "Mapping File (*.mpn)|*.mpn";
			cmdDialogSave.Filter = "Mapping File (*.mpn)|*.mpn";
			cmdDialogOpen.FilterIndex = 1;
			cmdDialogSave.FilterIndex = 1;
			cmdDialogOpen.InitialDirectory = (new global::Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\.";
			cmdDialogSave.InitialDirectory = (new global::Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\.";
			cmdDialogOpen.FileName = "";
			cmdDialogSave.FileName = "";
			
			cmdDialogOpen.DefaultExt = "mpn";
			cmdDialogSave.DefaultExt = "mpn";
			
			cmdDialogOpen.ShowDialog();
			cmdDialogSave.FileName = cmdDialogOpen.FileName;
			
			MainModule.LoadMapping(cmdDialogOpen.FileName);
			
		}
		
		public void mnuNew_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			
			NewMap(true);
			
		}
		
		private void ImportNewMap()
		{
			// TODO: Fix or delete this
			//Dim fso As New Scripting.FileSystemObject
			//Dim theFile As Scripting.File
			//Dim fileNum As Integer
			//Dim i As Integer
			
			//fileNum = FreeFile()
			
			//theFile = fso.GetFile(fileName)
			//ReDim ImportedData(theFile.Size)
			
			//FileOpen(fileNum, fileName, OpenMode.Binary)
			
			//For i = 0 To theFile.Size - 1
			//    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			//    FileGet(fileNum, ImportedData(i))
			//Next
			
			//ResetImportDefinitions()
			
			//frmImport.SetDefaults()
			//frmImport.ShowDialog()
			
			//If Not frmImport.ClickedOK Then Exit Sub
			
			//validMap = True
			
			//For i = 1 To maxRoofs
			//    Roofs(i).anchor.X = 0
			//    Roofs(i).anchor.Y = 0
			//    Roofs(i).anchorTarget.X = 0
			//    Roofs(i).anchorTarget.Y = 0
			//    Roofs(i).height = 0
			//    Roofs(i).width = 0
			
			//Next
			
			//For i = 0 To 100
			//    guard(i).X = 0
			//    guard(i).Y = 0
			//Next
			
			//For i = 1 To maxSpecial
			//    special(i) = 0
			//    specialx(i) = 0
			//    specialy(i) = 0
			//    specialdata(i) = New VB6.FixedLengthString(100)
			//    specialwidth(i) = 0
			//    specialheight(i) = 0
			
			//Next
			
			//SetMenus()
			//LoadTiles(TileSet)
			//blit()
		}
		
		private void NewMap(bool show_Renamed)
		{
			int j;
			int i;
			int k;
			
			if (show_Renamed == true)
			{
				frmProperties.Default.SetDefaults();
				frmProperties.Default.Text = "New Map";
				
				frmProperties.Default.ShowDialog();
			}
			else
			{
				frmProperties.Default.SetDefaults();
				
			}
			
			if (MainModule.SelectedOK || ! show_Renamed)
			{
				
				for (i = 0; i <= MainModule.mapWidth; i++)
				{
					for (j = 0; j <= MainModule.mapHeight; j++)
					{
						PaintLoc(i, j, 0);
					}
				}
				
				sbRight1.Maximum = MainModule.mapWidth;
				sbRight1.Minimum = 0;
				sbRight1.Value = 0;
				
				sbDown.Maximum = MainModule.mapHeight;
				sbDown.Minimum = 0;
				sbDown.Value = 0;
				
				for (i = 0; i <= 100; i++)
				{
					MainModule.guard[i].X = 0;
					MainModule.guard[i].Y = 0;
				}
				
				for (i = 0; i <= MainModule.maxSpecial; i++)
				{
					MainModule.special[i] = 0;
					MainModule.specialx[i] = 0;
					MainModule.specialy[i] = 0;
					MainModule.specialdata[i] = new Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(100);
					MainModule.specialwidth[i] = 0;
				}
				
				MainModule.NumRoofs = 0;
				for (i = 1; i <= MainModule.maxRoofs; i++)
				{
					MainModule.Roofs[i].anchor.X = 0;
					MainModule.Roofs[i].anchor.Y = 0;
					MainModule.Roofs[i].anchorTarget.X = 0;
					MainModule.Roofs[i].anchorTarget.Y = 0;
					MainModule.Roofs[i].height = 0;
					MainModule.Roofs[i].width = 0;
					
					for (j = 0; j <= 100; j++)
					{
						for (k = 0; k <= 100; k++)
						{
							MainModule.Roofs[i].Matrix[j, k] = 127;
						}
					}
				}
				
				MainModule.LoadTiles(MainModule.TileSet);
				
				validMap = true;
			}
			
			SetMenus();
			
		}
		
		public void mnuParameters_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			// TODO: Figure out what this is
			//frmImport.ShowDialog()
			
			//LoadTiles(TileSet)
			//blit()
		}
		
		public void mnuProperties_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			SetPropertiesForm();
			
			frmProperties.Default.ShowDialog();
			
			if (MainModule.SelectedOK)
			{
				AssignProperties();
			}
			
		}
		
		public void mnuQuit_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			Display.Dispose();
			ProjectData.EndApp();
		}
		
		
		public void mnuRefreshTiles_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			MainModule.LoadTiles(MainModule.TileSet);
		}
		
		public void mnuSaveAs_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			
			cmdDialogOpen.Title = "Save Map";
			cmdDialogSave.Title = "Save Map";
			
			cmdDialogOpen.Filter = "Binary Map Files (*.bmf)|*.bmf|Outside Map Files (*.map)|*.map|Town Map Files (*.twn)|*.twn|All Files (*.*)|*.*";
			cmdDialogSave.Filter = "Binary Map Files (*.bmf)|*.bmf|Outside Map Files (*.map)|*.map|Town Map Files (*.twn)|*.twn|All Files (*.*)|*.*";
			cmdDialogOpen.FilterIndex = 2;
			cmdDialogSave.FilterIndex = 0;
			cmdDialogOpen.InitialDirectory = MainModule.LotaPath + "\\included maps\\.";
			cmdDialogSave.InitialDirectory = MainModule.LotaPath + "\\included maps\\.";
			cmdDialogOpen.FileName = "";
			cmdDialogSave.FileName = "";
			
			cmdDialogSave.ShowDialog();
			cmdDialogOpen.FileName = cmdDialogSave.FileName;
			
			MainModule.fileName = cmdDialogOpen.FileName;
			
			mnuSave_Click(mnuSave, new System.EventArgs());
			
			//ErrorHandler:
			1.GetHashCode() ; //nop
		}
		
		
		
		public void mnuSaveMapping_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			
			cmdDialogOpen.Title = "Save Mapping";
			cmdDialogSave.Title = "Save Mapping";
			
			cmdDialogOpen.Filter = "Mapping File (*.mpn)|*.mpn";
			cmdDialogSave.Filter = "Mapping File (*.mpn)|*.mpn";
			cmdDialogOpen.FilterIndex = 1;
			cmdDialogSave.FilterIndex = 1;
			cmdDialogOpen.InitialDirectory = (new global::Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\.";
			cmdDialogSave.InitialDirectory = (new global::Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\.";
			cmdDialogOpen.FileName = "";
			cmdDialogSave.FileName = "";
			
			cmdDialogSave.ShowDialog();
			cmdDialogOpen.FileName = cmdDialogSave.FileName;
			
			MainModule.SaveMapping(cmdDialogOpen.FileName);
			
			//ErrorHandler:
			1.GetHashCode() ; //nop
			
		}
		
		private void Picture1_MouseDown(System.Object eventSender, System.Windows.Forms.MouseEventArgs eventArgs)
		{
			int Button = System.Convert.ToInt32(eventArgs.Button) / 0x100000;
			int Shift = System.Convert.ToInt32(System.Windows.Forms.Control.ModifierKeys) / 0x10000;
			float x = eventArgs.X;
			float y = eventArgs.Y;
			int yy;
			int xx;
			
			xx = (int) x;
			yy = (int) y;
			
			xx = (xx / 16)+ MainModule.leftX;
			yy = (yy / 16)+ MainModule.topy;
			
			if (MainModule.UpdateScreen == false)
			{
				return;
			}
			
			if (Button == 1)
			{
				
				SetPos(xx, yy);
				
			}
			else if (Button == 2 && xx >= 0 && yy >= 0)
			{
				// TODO: fix this hack
				Picture1_MouseMove(Picture1, new System.Windows.Forms.MouseEventArgs(((System.Windows.Forms.MouseButtons) (Button * 0x100000)), 0, xx, yy, 0));
				
			}
			
			
			blit();
		}
		
		private void Picture1_MouseMove(System.Object eventSender, System.Windows.Forms.MouseEventArgs eventArgs)
		{
			int Button = System.Convert.ToInt32(eventArgs.Button) / 0x100000;
			int Shift = System.Convert.ToInt32(System.Windows.Forms.Control.ModifierKeys) / 0x10000;
			float x = eventArgs.X;
			float y = eventArgs.Y;
			int r;
			int xx;
			int yy;
			int tile = 0;
			// 			int xx;
			// 			int yy;
			// 			int tile;
			
			xx = (int) x;
			yy = (int) y;
			
			xx = (xx / 16)+ MainModule.leftX;
			yy = (yy / 16)+ MainModule.topy;
			
			if (MainModule.UpdateScreen == false)
			{
				return;
			}
			
			if (Button == 1)
			{
				SetRightPos(xx, yy);
				blit();
				
			}
			else if (Button == 2 && xx >= 0 && yy >= 0)
			{
				
				if (MainModule.ImportMap)
				{
					PaintLoc(xx, yy, MainModule.currentTile);
				}
				else
				{
					if (chkRestrict.CheckState != 0)
					{
						if (xx < MainModule.x1 || xx > MainModule.x2 || yy < MainModule.y1 || yy > MainModule.y2)
						{
							return;
						}
					}
					
					if (chkRandom.CheckState == 0)
					{
						PaintLoc(xx, yy, MainModule.currentTile);
					}
					else
					{
						if (MainModule.currentTile == 7)
						{
							r = System.Convert.ToInt32(VBMath.Rnd(1) * 4);
							
							if (r < 2)
							{
								tile = MainModule.currentTile + r;
							}
							if (r > 1)
							{
								tile = MainModule.currentTile + r + 14;
							}
							
							PaintLoc(xx, yy, tile);
						}
						else if (MainModule.currentTile == 2 || MainModule.currentTile == 129 || MainModule.currentTile == 182)
						{
							r = System.Convert.ToInt32(VBMath.Rnd(1) * 2);
							
							tile = MainModule.currentTile + r;
							
							PaintLoc(xx, yy, tile);
							
						}
						else
						{
							PaintLoc(xx, yy, MainModule.currentTile);
						}
					}
				}
				
				blit();
			}
			
		}
		
		private void Picture1_Paint(System.Object eventSender, System.Windows.Forms.PaintEventArgs eventArgs)
		{
			blit();
		}
		
		private void Picture2_MouseDown(System.Object eventSender, System.Windows.Forms.MouseEventArgs eventArgs)
		{
			int Button = System.Convert.ToInt32(eventArgs.Button) / 0x100000;
			int Shift = System.Convert.ToInt32(System.Windows.Forms.Control.ModifierKeys) / 0x10000;
			int x = eventArgs.X;
			int y = eventArgs.Y;
			int yy;
			int xx;
			int tile;
			
			xx = x;
			yy = y;
			
			xx = xx / 16;
			yy = yy / 16;
			
			tile = yy * 16 + xx;
			
			if (MainModule.UpdateScreen == false)
			{
				return;
			}
			
			if (Button == 2)
			{
				PaintLoc(MainModule.x1, MainModule.y1, tile);
			}
			
			MainModule.currentTile = tile;
			
			blit();
		}
		
		private void Picture2_Paint(System.Object eventSender, System.Windows.Forms.PaintEventArgs eventArgs)
		{
			blit();
		}
		
		private void Text4_Change()
		{
			
		}
		public void mnuSave_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			
			
			if (MainModule.fileName == "")
			{
				mnuSaveAs_Click(mnuSaveAs, new System.EventArgs());
				
				if (MainModule.fileName == "")
				{
					return;
				}
				
			}
			
			SaveMap();
			
		}
		
		private void SaveMap()
		{
			if (Path.GetExtension(MainModule.fileName) != ".bmf")
			{
				old_SaveMap();
				return;
			}
			
			
			XleMap saveMap = null;
			int i;
			int j;
			
			switch (MainModule.MapType)
			{
				case 1:
					saveMap = new ERY.Xle.XleMapTypes.Museum();
					break;
					
				case 2:
					saveMap = new ERY.Xle.XleMapTypes.Outside();
					break;
					
				case 3:
					saveMap = new ERY.Xle.XleMapTypes.Town();
					break;
					
				case 4:
					saveMap = new ERY.Xle.XleMapTypes.Dungeon();
					break;
					
				case 5:
					saveMap = new ERY.Xle.XleMapTypes.Castle();
					break;
					
			}
			
			saveMap.InitializeMap(MainModule.mapWidth, MainModule.mapHeight);
			saveMap.MapName = MainModule.mapName.Value.Trim();
			
			saveMap.TileSet = MainModule.TileSet;
			
			for (j = 0; j <= MainModule.mapHeight - 1; j++)
			{
				for (i = 0; i <= MainModule.mapWidth - 1; i++)
				{
					saveMap[i, j] = MainModule.mMap[i, j];
				}
			}
			
			if (typeof(ERY.Xle.XleMapTypes.Town).IsAssignableFrom(saveMap.GetType()))
			{
				ERY.Xle.XleMapTypes.Town t = (ERY.Xle.XleMapTypes.Town) saveMap;
				
				t.OutsideTile = MainModule.defaultTile;
				
				t.BuyRaftMap = MainModule.BuyRaftMap;
				t.BuyRaftPt = new Agate.Point(MainModule.BuyRaftX, MainModule.BuyRaftY);
				
				for (i = 0; i <= MainModule.mail.GetUpperBound(0); i++)
				{
					if (MainModule.mail[i] != 0)
					{
						t.Mail.Add(MainModule.mail[i]);
					}
				}
				
			}
			
			if (typeof(IHasGuards).IsAssignableFrom(saveMap.GetType()))
			{
				IHasGuards h = (IHasGuards) saveMap;
				
				for (i = 1; i <= MainModule.guard.GetUpperBound(0); i++)
				{
					Guard g = new Guard();
					
					g.Location = Interop.Convert(MainModule.guard[i]);
					g.HP = MainModule.guardHP;
					g.Facing = Direction.South;
					g.Attack = MainModule.guardAttack;
					g.Defense = MainModule.guardDefense;
					
					if (g.Location.IsEmpty == false)
					{
						h.Guards.Add(g);
					}
					
				}
			}
			
			if (typeof(IHasRoofs).IsAssignableFrom(saveMap.GetType()))
			{
				for (i = 1; i <= MainModule.maxRoofs; i++)
				{
					Roof r = new Roof();
					
					if (MainModule.Roofs[i].width == 0 && MainModule.Roofs[i].height == 0)
					{
						continue;
					}
					
					r.SetSize(MainModule.Roofs[i].width, MainModule.Roofs[i].height);
					r.Location = new Agate.Point(MainModule.Roofs[i].anchorTarget.X - MainModule.Roofs[i].anchor.X, MainModule.Roofs[i].anchorTarget.Y - MainModule.Roofs[i].anchor.Y);
					
					for (int k = 0; k <= MainModule.Roofs[i].width - 1; k++)
					{
						for (j = 0; j <= MainModule.Roofs[i].height - 1; j++)
						{
							r[k, j] = MainModule.Roofs[i].Matrix[k, j];
						}
					}
					
					((IHasRoofs) saveMap).Roofs.Add(r);
				}
			}
			
			for (i = 1; i <= NumSpecials(); i++)
			{
				XleEvent evt = null;
				char[] c = MainModule.specialdata[i].Value.ToCharArray();
				
				switch (MainModule.special[i])
				{
					case 1:
						ERY.Xle.XleEventTypes.ChangeMapEvent e = new ERY.Xle.XleEventTypes.ChangeMapEvent();
						
						e.MapID = Strings.Asc(c[0]) * 256 + Strings.Asc(c[1]);
						e.Location = new Agate.Point(Strings.Asc(c[2]) * 256 + Strings.Asc(c[3]), Strings.Asc(c[4]) * 256 + Strings.Asc(c[5]));
						
						if (Strings.Asc(c[6]) < 11 || Strings.Asc(c[6]) == 32)
						{
							e.Ask = true;
						}
						else
						{
							e.Ask = false;
						}
						
						evt = e;
						break;
						
					case 2:
						ERY.Xle.XleEventTypes.StoreBank e_1 = new ERY.Xle.XleEventTypes.StoreBank();
						evt = e_1;
						break;
						
					case 3:
						ERY.Xle.XleEventTypes.StoreWeapon e_2 = new ERY.Xle.XleEventTypes.StoreWeapon();
						evt = e_2;
						break;
						
					case 4:
						ERY.Xle.XleEventTypes.StoreArmor e_3 = new ERY.Xle.XleEventTypes.StoreArmor();
						evt = e_3;
						break;
						
					case 5:
						ERY.Xle.XleEventTypes.StoreWeaponTraining e_4 = new ERY.Xle.XleEventTypes.StoreWeaponTraining();
						evt = e_4;
						break;
						
					case 6:
						ERY.Xle.XleEventTypes.StoreArmorTraining e_5 = new ERY.Xle.XleEventTypes.StoreArmorTraining();
						evt = e_5;
						break;
						
					case 7:
						ERY.Xle.XleEventTypes.StoreBlackjack e_6 = new ERY.Xle.XleEventTypes.StoreBlackjack();
						evt = e_6;
						break;
						
					case 8:
						ERY.Xle.XleEventTypes.StoreLending e_7 = new ERY.Xle.XleEventTypes.StoreLending();
						evt = e_7;
						break;
						
					case 9:
						ERY.Xle.XleEventTypes.StoreRaft e_8 = new ERY.Xle.XleEventTypes.StoreRaft();
						evt = e_8;
						break;
						
					case 10:
						ERY.Xle.XleEventTypes.StoreHealer e_9 = new ERY.Xle.XleEventTypes.StoreHealer();
						evt = e_9;
						break;
						
					case 11:
						ERY.Xle.XleEventTypes.StoreJail e_10 = new ERY.Xle.XleEventTypes.StoreJail();
						evt = e_10;
						break;
						
					case 12:
						ERY.Xle.XleEventTypes.StoreFortune e_11 = new ERY.Xle.XleEventTypes.StoreFortune();
						evt = e_11;
						break;
						
					case 13:
						ERY.Xle.XleEventTypes.StoreFlipFlop e_12 = new ERY.Xle.XleEventTypes.StoreFlipFlop();
						evt = e_12;
						break;
						
					case 14:
						ERY.Xle.XleEventTypes.StoreBuyback e_13 = new ERY.Xle.XleEventTypes.StoreBuyback();
						evt = e_13;
						break;
						
					case 15:
						ERY.Xle.XleEventTypes.StoreFood e_14 = new ERY.Xle.XleEventTypes.StoreFood();
						evt = e_14;
						break;
						
					case 16:
						ERY.Xle.XleEventTypes.StoreVault e_15 = new ERY.Xle.XleEventTypes.StoreVault();
						evt = e_15;
						break;
						
					case 17:
						ERY.Xle.XleEventTypes.StoreMagic e_16 = new ERY.Xle.XleEventTypes.StoreMagic();
						evt = e_16;
						break;
						
					case 23:
					case 25:
						ERY.Xle.XleEventTypes.ItemAvailableEvent e_17;
						
						if (MainModule.special[i] == 23)
						{
							e_17 = new ERY.Xle.XleEventTypes.TreasureChestEvent();
						}
						else
						{
							e_17 = new ERY.Xle.XleEventTypes.TakeEvent();
						}
						
						evt = e_17;
						
						if (Strings.Asc(MainModule.specialdata[i].Value[0]) == 0)
						{
							e_17.ContainsItem = true;
							e_17.Contents = Strings.Asc(MainModule.specialdata[i].Value[1]);
							
						}
						else
						{
							e_17.ContainsItem = false;
							e_17.Contents = Strings.Asc(MainModule.specialdata[i].Value[1]) * 256 + Strings.Asc(MainModule.specialdata[i].Value[2]);
						}
						break;
						
					case 24:
						ERY.Xle.XleEventTypes.Door e_18 = new ERY.Xle.XleEventTypes.Door();
						evt = e_18;
						
						e_18.RequiredItem = Strings.Asc(MainModule.specialdata[i].Value[0]);
						break;
						
				}
				
				if ((evt) is ERY.Xle.XleEventTypes.Store)
				{
					ERY.Xle.XleEventTypes.Store st = (ERY.Xle.XleEventTypes.Store) evt;
					
					st.ShopName = MainModule.specialdata[i].Value;
					
					// TODO: Fix this:
					if (st.ShopName.Contains("\\\\"))
					{
						//st.ShopName = st.ShopName.Split("\\", )(0)
					}
					else if (st.ShopName.Contains("\\0"))
					{
						//st.ShopName = st.ShopName.Split(Chr(0))(0)
					}
					else
					{
					}
					
				}
				//enum StoreType
				//{
					//	storeBank = 2,					// 2
					//	storeWeapon,					// 3
					//	storeArmor,						// 4
					//	storeWeaponTraining,			// 5
					//	storeArmorTraining,				// 6
					//	storeBlackjack,					// 7
					//	storeLending,					// 8
					//	storeRaft,						// 9
					//	storeHealer,					// 10
					//	storeJail,						// 11
					//	storeFortune,					// 12
					//	storeFlipFlop,					// 13
					//	storeBuyback,					// 14
					//	storeFood,						// 15
					//	storeVault,						// 16
					//	storeMagic						// 17
					//};
					
					if (evt != null)
					{
						
						evt.X = MainModule.specialx[i];
						evt.Y = MainModule.specialy[i];
						evt.Width = MainModule.specialwidth[i];
						evt.Height = MainModule.specialheight[i];
						
						saveMap.Events.Add(evt);
					}
					
				}
				
				//' Now serialize it.
				System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				using (Stream ff = File.OpenWrite(MainModule.fileName))
				{
					formatter.Serialize(ff, saveMap);
					
					ff.Flush();
				}
				
				
				
			}
			private void old_SaveMap()
			{
				string path;
				int offset;
				int file;
				Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString mn = new Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(16);
				int j;
				int i;
				int k;
				Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString test = new Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(1);
				
				mn.Value = MainModule.mapName.Value;
				
				
				
				Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString a = new Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(1);
				byte b;
				if (MainModule.fileName != "")
				{
					path = MainModule.fileName;
					
					file = FileSystem.FreeFile();
					
					
					FileSystem.Kill(path);
					FileSystem.FileOpen(file, path, OpenMode.Binary, (OpenAccess) (-1), (OpenShare) (-1), -1);
					
					offset = 1;
					
					
					a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.mapWidth / 256))); //0
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, offset, false);
					
					offset++;
					
					a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.mapWidth % 256))); //1
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, offset, false);
					
					offset++;
					
					a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.mapHeight / 256))); //2
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, offset, false);
					
					offset++;
					
					a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.mapHeight % 256))); //3
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, offset, false);
					
					offset++;
					
					a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.fileOffset / 256))); //4
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, offset, false);
					
					offset++;
					
					a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.fileOffset % 256))); //5
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, offset, false);
					
					offset++;
					
					a.Value = System.Convert.ToString(Strings.Chr(MainModule.MapType)); //6
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, offset, false);
					
					offset++;
					
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, mn.Value, offset, false);
					
					offset = offset + mn.Value.Length; //7
					
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, "@", offset, false);
					
					offset++; //23
					
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, MainModule.defaultTile, offset);
					
					offset++; //24
					
					a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardHP / 256)));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, offset, false);
					
					offset++; //25
					
					a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardHP % 256)));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, offset, false);
					
					offset++; //26
					
					a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardAttack / 256)));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, offset, false);
					
					offset++; //27
					
					a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardAttack % 256)));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, offset, false);
					
					offset++; //28
					
					a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardDefense / 256)));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, offset, false);
					
					offset++; //29
					
					a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardDefense % 256)));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, offset, false);
					
					offset++; //30
					
					a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardColor / 256)));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, offset, false);
					
					offset++; //31
					
					a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardColor % 256)));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, offset, false);
					
					offset++; //32
					
					if (MainModule.TileSet == "Tiles.bmp")
					{
						a.Value = System.Convert.ToString('\0');
					}
					else if (MainModule.TileSet == "TownTiles.bmp")
					{
						a.Value = System.Convert.ToString('\u0001');
					}
					else if (MainModule.TileSet == "CastleTiles.bmp")
					{
						a.Value = System.Convert.ToString('\u0002');
					}
					else if (MainModule.TileSet == "LOB Tiles.bmp")
					{
						a.Value = System.Convert.ToString('\u0003');
					}
					else if (MainModule.TileSet == "LOB TownTiles.bmp")
					{
						a.Value = System.Convert.ToString('\u0004');
					}
					
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, 34, false);
					
					offset++; //33
					
					a.Value = System.Convert.ToString(Strings.Chr(MainModule.BuyRaftMap)); //36  Buy Raft Map
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, 37, false);
					
					a.Value = System.Convert.ToString(Strings.Chr(MainModule.BuyRaftX / 256));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, 38, false); //37  Buy Raft X
					a.Value = System.Convert.ToString(Strings.Chr(MainModule.BuyRaftX % 256));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, 39, false); //38  Buy Raft X
					
					a.Value = System.Convert.ToString(Strings.Chr(MainModule.BuyRaftY / 256));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, 40, false); //39  Buy Raft y
					a.Value = System.Convert.ToString(Strings.Chr(MainModule.BuyRaftY % 256));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, 41, false); //40  Buy Raft Y
					
					a.Value = System.Convert.ToString('\u0078');
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, 42, false); //41 special count
					
					a.Value = System.Convert.ToString(Strings.Chr(MainModule.mail[0]));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, 43, false); // 42 mail 0
					
					a.Value = System.Convert.ToString(Strings.Chr(MainModule.mail[1]));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, 44, false); // 43 mail 1
					
					a.Value = System.Convert.ToString(Strings.Chr(MainModule.mail[2]));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, 45, false); // 44 mail 2
					
					a.Value = System.Convert.ToString(Strings.Chr(MainModule.mail[3]));
					//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					FileSystem.FilePut(file, a.Value, 46, false); // 45 mail 3
					
					offset = MainModule.fileOffset + 1;
					
					for (j = 0; j <= MainModule.mapHeight - 1; j++)
					{
						for (i = 0; i <= MainModule.mapWidth - 1; i++)
						{
							
							//UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							b = (byte) (MainModule.Map(i, j));
							
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, b, offset);
							offset++;
							
						}
					}
					
					//offset = (mapHeight + 1) * mapWidth + 1
					
					for (i = 1; i <= MainModule.maxSpecial; i++)
					{
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
						FileSystem.FilePut(file, MainModule.special[i], offset);
						
						offset++;
						
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialx[i] / 256)));
						//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
						FileSystem.FilePut(file, a.Value, offset, false);
						
						offset++;
						
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialx[i] % 256)));
						//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
						FileSystem.FilePut(file, a.Value, offset, false);
						
						offset++;
						
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialy[i] / 256)));
						//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
						FileSystem.FilePut(file, a.Value, offset, false);
						
						offset++;
						
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialy[i] % 256)));
						//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
						FileSystem.FilePut(file, a.Value, offset, false);
						
						offset++;
						
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialwidth[i] / 256)));
						//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
						FileSystem.FilePut(file, a.Value, offset, false);
						
						offset++;
						
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialwidth[i] % 256)));
						//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
						FileSystem.FilePut(file, a.Value, offset, false);
						
						offset++;
						
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialheight[i] / 256)));
						//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
						FileSystem.FilePut(file, a.Value, offset, false);
						
						offset++;
						
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialheight[i] % 256)));
						//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
						FileSystem.FilePut(file, a.Value, offset, false);
						
						offset++;
						
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
						FileSystem.FilePut(file, MainModule.specialdata[i], offset);
						//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
						offset = offset + Strings.Len(MainModule.specialdata[i]);
					}
					
					if (MainModule.MapType == System.Convert.ToInt32(MainModule.EnumMapType.maptown)|| MainModule.MapType == System.Convert.ToInt32(MainModule.EnumMapType.mapCastle))
					{
						
						//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
						FileSystem.FilePut(file, "5555557", offset, false);
						
						offset = offset + "5555557".Length;
						
						for (i = 0; i <= 100; i++)
						{
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guard[i].X / 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guard[i].X % 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guard[i].Y / 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guard[i].Y % 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
						}
						
						a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int((offset - 1) / 256)));
						//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
						FileSystem.FilePut(file, a.Value, 35, false); // 34 (roof offset)
						
						a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int((offset - 1) % 256)));
						//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
						FileSystem.FilePut(file, a.Value, 36, false); // 35 (roof offset)
						
						for (i = 1; i <= MainModule.maxRoofs; i++)
						{
							// anchor
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchor.X / 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchor.X % 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchor.Y / 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchor.Y % 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
							
							// anchortarget
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchorTarget.X / 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchorTarget.X % 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchorTarget.Y / 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchorTarget.Y % 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
							
							//  Dimensions
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].width / 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].width % 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].height / 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].height % 256)));
							//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							FileSystem.FilePut(file, a.Value, offset, false);
							
							offset++;
							
							//  Data
							//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							for (j = 0; j <= MainModule.Roofs[i].height - 1; j++)
							{
								//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								for (k = 0; k <= MainModule.Roofs[i].width - 1; k++)
								{
									//UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									//UPGRADE_WARNING: Couldn't resolve default property of object k. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									//UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									a.Value = System.Convert.ToString(Strings.Chr(MainModule.Roofs[i].Matrix[k, j]));
									//UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
									FileSystem.FilePut(file, a.Value, offset, false);
									
									offset++;
								}
							}
						}
						
					}
					
					FileSystem.FileClose(file);
					
					//UPGRADE_WARNING: Lower bound of collection StatusBar1.Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
					StatusBar1.Items[1].Text = "Saved successfully: " + DateAndTime.TimeOfDay;
					
				}
				
				blit();
				
			}
			public void mnuOpen_Click(System.Object eventSender, System.EventArgs eventArgs)
			{
				
				try
				{
					
					cmdDialogOpen.Title = "Open Map";
					cmdDialogSave.Title = "Open Map";
					
					cmdDialogOpen.Filter = "All Map Files|*.bmf;*.map;*.twn|Outside Map Files (*.map)|*.map|Town Map Files (*.twn)|*.twn|All Files (*.*)|*.*";
					cmdDialogSave.Filter = "All Map Files|*.bmf;*.map;*.twn|Outside Map Files (*.map)|*.map|Town Map Files (*.twn)|*.twn|All Files (*.*)|*.*";
					cmdDialogOpen.FilterIndex = 1;
					cmdDialogSave.FilterIndex = 1;
					
					cmdDialogOpen.InitialDirectory = MainModule.LotaPath + "\\Included Maps";
					cmdDialogSave.InitialDirectory = MainModule.LotaPath + "\\Included Maps";
					cmdDialogOpen.FileName = "";
					cmdDialogSave.FileName = "";
					
					cmdDialogOpen.DefaultExt = "map";
					cmdDialogSave.DefaultExt = "map";
					
					cmdDialogOpen.ShowDialog();
					cmdDialogSave.FileName = cmdDialogOpen.FileName;
					
					MainModule.fileName = cmdDialogOpen.FileName;
					
					
					OpenMap();
					
				}
				catch (Exception)
				{
					
				}
				
			}
			private void OpenMap()
			{
				old_OpenMap();
			}
			
			private void old_OpenMap()
			{
				string path;
				int file;
				int newOffset;
				string tempName;
				int j;
				int i;
				int k;
				string b;
				int ro;
				
				path = MainModule.fileName;
				
				// TODO: discard this method.
				
				Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString a = new Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(1);
				if (path != "")
				{
					file = FileSystem.FreeFile();
					FileSystem.FileOpen(file, path, OpenMode.Binary, (OpenAccess) (-1), (OpenShare) (-1), -1);
					
					
					for (i = 1; i <= MainModule.maxRoofs; i++)
					{
						MainModule.Roofs[i].anchor.X = 0;
						MainModule.Roofs[i].anchor.Y = 0;
						MainModule.Roofs[i].anchorTarget.X = 0;
						MainModule.Roofs[i].anchorTarget.Y = 0;
						MainModule.Roofs[i].height = 0;
						MainModule.Roofs[i].width = 0;
						
					}
					
					for (i = 0; i <= 100; i++)
					{
						MainModule.guard[i].X = 0;
						MainModule.guard[i].Y = 0;
					}
					
					for (i = 1; i <= MainModule.maxSpecial; i++)
					{
						MainModule.special[i] = 0;
						MainModule.specialx[i] = 0;
						MainModule.specialy[i] = 0;
						MainModule.specialdata[i] = new Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(100);
						MainModule.specialwidth[i] = 0;
						MainModule.specialheight[i] = 0;
						
					}
					
					
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType, 1);
					MainModule.mapWidth = Strings.Asc(a.Value) * 256;
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType2 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType2, 2);
					MainModule.mapWidth = MainModule.mapWidth + Strings.Asc(a.Value);
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType3 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType3, 3);
					MainModule.mapHeight = Strings.Asc(a.Value) * 256;
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType4 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType4, 4);
					MainModule.mapHeight = MainModule.mapHeight + Strings.Asc(a.Value);
					
					if (MainModule.mapWidth > 3000 || MainModule.mapHeight > 3000)
					{
						MessageBox.Show("Bad File: " + path + System.Environment.NewLine + System.Environment.NewLine + "The data is invalid.  Please try a different file or replace it.");
						validMap = false;
						
						return;
					}
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType5 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType5, 5);
					newOffset = Strings.Asc(a.Value) * 256;
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType6 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType6, 6);
					newOffset = newOffset + Strings.Asc(a.Value);
					MainModule.fileOffset = newOffset;
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType7 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType7, 7);
					MainModule.MapType = Strings.Asc(a.Value);
					
					//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					offset = 8;
					
					tempName = "";
					do
					{
						//FileGet(file, a.Value, offset)
						tempName = tempName + a.Value;
						offset++;
					} while (!(a.Value == "@" || offset > 25));
					
					MainModule.mapName.Value = Strings.RTrim(Microsoft.VisualBasic.Strings.Left(tempName, tempName.Length - 1));
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType8 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType8, 25);
					MainModule.defaultTile = Strings.Asc(a.Value);
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType9 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType9, 26);
					//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					offset++; //25
					MainModule.guardHP = Strings.Asc(a.Value) * 256;
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType10 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType10, 27);
					//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					offset++; //26
					MainModule.guardHP = MainModule.guardHP + Strings.Asc(a.Value);
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType11 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType11, 28);
					//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					offset++; //27
					MainModule.guardAttack = Strings.Asc(a.Value) * 256;
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType12 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType12, 29);
					//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					offset++; //28
					MainModule.guardAttack = MainModule.guardAttack + Strings.Asc(a.Value);
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType13 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType13, 30);
					//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					offset++; //29
					MainModule.guardDefense = Strings.Asc(a.Value) * 256;
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType14 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType14, 31);
					//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					offset++; //30
					MainModule.guardDefense = MainModule.guardDefense + Strings.Asc(a.Value);
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType15 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType15, 32);
					//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					offset++; //31
					MainModule.guardColor = Strings.Asc(a.Value) * 256;
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType16 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType16, 33);
					//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					offset++; //32
					MainModule.guardColor = MainModule.guardColor + Strings.Asc(a.Value);
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType17 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType17, 34);
					//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					offset++; //33
					if (a.Value == System.Convert.ToString('\0'))
					{
						MainModule.TileSet = "Tiles.png";
					}
					else if (a.Value == System.Convert.ToString('\u0001'))
					{
						MainModule.TileSet = "TownTiles.png";
					}
					else if (a.Value == System.Convert.ToString('\u0002'))
					{
						MainModule.TileSet = "CastleTiles.png";
					}
					else if (a.Value == System.Convert.ToString('\u0003'))
					{
						MainModule.TileSet = "LOB Tiles.png";
					}
					else if (a.Value == System.Convert.ToString('\u0004'))
					{
						MainModule.TileSet = "LOB TownTiles.png";
					}
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType18 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType18, 37);
					MainModule.BuyRaftMap = Strings.Asc(a.Value); //36  Buy Raft Map
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType19 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType19, 38); //37  Buy Raft X
					MainModule.BuyRaftX = Strings.Asc(a.Value) * 256;
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType20 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType20, 39); //38  Buy Raft X
					MainModule.BuyRaftX = MainModule.BuyRaftX + Strings.Asc(a.Value);
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType21 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType21, 40); //39  Buy Raft y
					MainModule.BuyRaftY = Strings.Asc(a.Value) * 256;
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType22 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType22, 41); //40  Buy Raft Y
					MainModule.BuyRaftY = MainModule.BuyRaftY + Strings.Asc(a.Value);
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType23 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType23, 42); //41  special count
					if (Strings.Asc(a.Value) == 0)
					{
						a.Value = System.Convert.ToString('\u0014');
					}
					MainModule.specialCount = Strings.Asc(a.Value);
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType24 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType24, 35);
					//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					offset++; // 34 (Roof offset)
					//UPGRADE_WARNING: Couldn't resolve default property of object ro. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					ro = Strings.Asc(a.Value) * 256;
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType25 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType25, 36);
					//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					offset++; // 35 (roof offset)
					//UPGRADE_WARNING: Couldn't resolve default property of object ro. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					ro = ro + Strings.Asc(a.Value) + 1;
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType26 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType26, 43); // 42 mail 0
					MainModule.mail[0] = Strings.Asc(a.Value);
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType27 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType27, 44); // 43 mail 1
					MainModule.mail[1] = Strings.Asc(a.Value);
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType28 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType28, 45); // 44 mail 2
					MainModule.mail[2] = Strings.Asc(a.Value);
					
					//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
					System.ValueType temp_SystemValueType29 = (System.ValueType) a.Value;
					FileSystem.FileGet(file, ref temp_SystemValueType29, 46); // 45 mail 3
					MainModule.mail[3] = Strings.Asc(a.Value);
					
					//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					offset = newOffset + 1;
					
					for (j = 0; j <= MainModule.mapHeight - 1; j++)
					{
						for (i = 0; i <= MainModule.mapWidth - 1; i++)
						{
							
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType30 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType30, offset);
							
							MainModule.mMap[i, j] = Strings.Asc(a.Value);
							
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							
						}
					}
					
					for (i = 1; i <= MainModule.specialCount; i++)
					{
						System.ValueType temp_SystemValueType31 = (System.ValueType) a.Value;
						FileSystem.FileGet(file, ref temp_SystemValueType31, offset);
						offset++;
						MainModule.special[i] = Strings.Asc(a.Value);
						
						System.ValueType temp_SystemValueType32 = (System.ValueType) a.Value;
						FileSystem.FileGet(file, ref temp_SystemValueType32, offset);
						offset++;
						MainModule.specialx[i] = Strings.Asc(a.Value) * 256;
						
						System.ValueType temp_SystemValueType33 = (System.ValueType) a.Value;
						FileSystem.FileGet(file, ref temp_SystemValueType33, offset);
						offset++;
						MainModule.specialx[i] = MainModule.specialx[i] + Strings.Asc(a.Value);
						
						System.ValueType temp_SystemValueType34 = (System.ValueType) a.Value;
						FileSystem.FileGet(file, ref temp_SystemValueType34, offset);
						offset++;
						MainModule.specialy[i] = Strings.Asc(a.Value);
						
						System.ValueType temp_SystemValueType35 = (System.ValueType) a.Value;
						FileSystem.FileGet(file, ref temp_SystemValueType35, offset);
						offset++;
						MainModule.specialy[i] = MainModule.specialy[i] + Strings.Asc(a.Value);
						
						System.ValueType temp_SystemValueType36 = (System.ValueType) a.Value;
						FileSystem.FileGet(file, ref temp_SystemValueType36, offset);
						offset++;
						MainModule.specialwidth[i] = Strings.Asc(a.Value) * 256;
						
						System.ValueType temp_SystemValueType37 = (System.ValueType) a.Value;
						FileSystem.FileGet(file, ref temp_SystemValueType37, offset);
						offset++;
						MainModule.specialwidth[i] = MainModule.specialwidth[i] + Strings.Asc(a.Value);
						
						System.ValueType temp_SystemValueType38 = (System.ValueType) a.Value;
						FileSystem.FileGet(file, ref temp_SystemValueType38, offset);
						offset++;
						MainModule.specialheight[i] = Strings.Asc(a.Value);
						
						System.ValueType temp_SystemValueType39 = (System.ValueType) a.Value;
						FileSystem.FileGet(file, ref temp_SystemValueType39, offset);
						offset++;
						MainModule.specialheight[i] = MainModule.specialheight[i] + Strings.Asc(a.Value);
						
						System.ValueType temp_SystemValueType40 = (System.ValueType) MainModule.specialdata[i].Value;
						FileSystem.FileGet(file, ref temp_SystemValueType40, offset);
						offset = offset + 100;
					}
					
					for (i = 1; i <= MainModule.maxRoofs; i++)
					{
						MainModule.Roofs[i].anchor.X = 0;
						MainModule.Roofs[i].anchor.Y = 0;
						MainModule.Roofs[i].anchorTarget.X = 0;
						MainModule.Roofs[i].anchorTarget.Y = 0;
						MainModule.Roofs[i].height = 0;
						MainModule.Roofs[i].width = 0;
						
					}
					
					if (MainModule.MapType == System.Convert.ToInt32(MainModule.EnumMapType.maptown)|| MainModule.MapType == System.Convert.ToInt32(MainModule.EnumMapType.mapCastle))
					{
						while (!(Strings.Right(b, 7) == "5555557" || FileSystem.EOF(file)))
						{
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType41 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType41, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							
							b = b + a.Value;
						}
						
						
						for (i = 0; i <= 100; i++)
						{
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType42 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType42, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.guard[i].X = Strings.Asc(a.Value) * 256;
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType43 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType43, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.guard[i].X = MainModule.guard[i].X + Strings.Asc(a.Value);
							
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType44 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType44, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.guard[i].Y = Strings.Asc(a.Value) * 256;
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType45 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType45, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.guard[i].Y = MainModule.guard[i].Y + Strings.Asc(a.Value);
						}
						
						
						
						for (i = 1; i <= MainModule.maxRoofs; i++)
						{
							// anchor
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType46 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType46, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.Roofs[i].anchor.X = Strings.Asc(a.Value) * 256;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType47 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType47, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.Roofs[i].anchor.X = MainModule.Roofs[i].anchor.X + Strings.Asc(a.Value);
							
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType48 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType48, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.Roofs[i].anchor.Y = Strings.Asc(a.Value);
							
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType49 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType49, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.Roofs[i].anchor.Y = MainModule.Roofs[i].anchor.Y + Strings.Asc(a.Value);
							
							// anchortarget
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType50 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType50, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.Roofs[i].anchorTarget.X = Strings.Asc(a.Value) * 256;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType51 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType51, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.Roofs[i].anchorTarget.X = MainModule.Roofs[i].anchorTarget.X + Strings.Asc(a.Value);
							
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType52 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType52, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.Roofs[i].anchorTarget.Y = Strings.Asc(a.Value) * 256;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType53 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType53, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.Roofs[i].anchorTarget.Y = MainModule.Roofs[i].anchorTarget.Y + Strings.Asc(a.Value);
							
							//  Dimensions
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType54 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType54, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.Roofs[i].width = Strings.Asc(a.Value) * 256;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType55 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType55, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.Roofs[i].width = MainModule.Roofs[i].width + Strings.Asc(a.Value);
							
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType56 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType56, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.Roofs[i].height = Strings.Asc(a.Value) * 256;
							
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
							System.ValueType temp_SystemValueType57 = (System.ValueType) a.Value;
							FileSystem.FileGet(file, ref temp_SystemValueType57, offset);
							//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							offset++;
							MainModule.Roofs[i].height = MainModule.Roofs[i].height + Strings.Asc(a.Value);
							
							//  Data
							for (j = 0; j <= MainModule.Roofs[i].height - 1; j++)
							{
								for (k = 0; k <= MainModule.Roofs[i].width - 1; k++)
								{
									//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									//UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
									System.ValueType temp_SystemValueType58 = (System.ValueType) a.Value;
									FileSystem.FileGet(file, ref temp_SystemValueType58, offset);
									//UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									offset++;
									MainModule.Roofs[i].Matrix[k, j] = Strings.Asc(a.Value);
								}
							}
							
						}
						
					}
					
					FileSystem.FileClose(file);
					
					MainModule.fileName = path;
					
					MainModule.x1 = 0;
					MainModule.x2 = 0;
					MainModule.y1 = 0;
					MainModule.y2 = 0;
					
					
					sbRight1.Maximum = MainModule.mapWidth;
					sbDown.Maximum = MainModule.mapHeight;
					
					MainModule.LoadTiles(MainModule.TileSet);
					validMap = true;
					MainModule.ImportMap = false;
					
				}
				
				SetMenus();
				FillSpecial();
				blit();
			}
			
			public void SetPropertiesForm()
			{
				int i;
				
				object with_1 = frmProperties;
				with_1.txtName.Text = MainModule.mapName.Value;
				with_1.txtDefaultTile.Text = MainModule.defaultTile.ToString();
				with_1.mType = MainModule.MapType;
				with_1.txtAttack.Text = MainModule.guardAttack.ToString();
				with_1.txtHP.Text = MainModule.guardHP.ToString();
				with_1.txtDefense.Text = MainModule.guardDefense.ToString();
				with_1.txtColor.Text = MainModule.guardColor.ToString();
				with_1.txtDefaultTile.Text = MainModule.defaultTile.ToString();
				with_1.Text = MainModule.mapName.Value.TrimStart() + " Properties";
				with_1.txtWidth.Text = MainModule.mapWidth.ToString();
				with_1.txtHeight.Text = MainModule.mapHeight.ToString();
				with_1.theTiles = MainModule.TileSet;
				with_1.txtBuyRaftMap.Text = MainModule.BuyRaftMap.ToString();
				with_1.txtBuyRaftX.Text = MainModule.BuyRaftX.ToString();
				with_1.txtBuyRaftY.Text = MainModule.BuyRaftY.ToString();
				
				with_1.setProperties = true;
				
				// TODO: remove control array.
				for (i = 0; i <= 3; i++)
				{
					with_1.txtMail((short) i).Text = MainModule.mail[i].ToString();
				}
				
				
				oldw = (int) (Microsoft.VisualBasic.Compatibility.VB6.Support.PixelsToTwipsX(Width));
				oldh = (int) (Microsoft.VisualBasic.Compatibility.VB6.Support.PixelsToTwipsY(Height));
			}
			
			public void AssignProperties()
			{
				int i;
				
				MainModule.mapName.Value = frmProperties.Default.txtName.Text;
				
				if (frmProperties.Default.txtDefaultTile.Text != "")
				{
					MainModule.defaultTile = int.Parse(frmProperties.Default.txtDefaultTile.Text);
				}
				if (frmProperties.Default.txtAttack.Text != "")
				{
					MainModule.guardAttack = int.Parse(frmProperties.Default.txtAttack.Text);
				}
				if (frmProperties.Default.txtHP.Text != "")
				{
					MainModule.guardHP = int.Parse(frmProperties.Default.txtHP.Text);
				}
				if (frmProperties.Default.txtDefense.Text != "")
				{
					MainModule.guardDefense = int.Parse(frmProperties.Default.txtDefense.Text);
				}
				if (frmProperties.Default.txtColor.Text != "")
				{
					MainModule.guardColor = int.Parse(frmProperties.Default.txtColor.Text);
				}
				if (frmProperties.Default.theTiles != "")
				{
					MainModule.TileSet = frmProperties.Default.theTiles;
				}
				
				Size newSize = new Size(int.Parse(frmProperties.Default.txtWidth.Text), int.Parse(frmProperties.Default.txtHeight.Text));
				
				if (newSize.Width != MainModule.TheMap.Width || newSize.Height != MainModule.TheMap.Height)
				{
					//TheMap.Resize(newSize.Width, newSize.Height)
				}
				
				
				//BuyRaftMap = frmProperties.mbuyraftmap
				//BuyRaftX = Integer.Parse(frmProperties.mBuyRaftX)
				//BuyRaftY = Integer.Parse(frmProperties.mBuyRaftY)
				
				for (i = 0; i <= 3; i++)
				{
					MainModule.mail[i] = int.Parse(frmProperties.Default.txtMail[(short) i].Text);
				}
				
				
				if (oldw > 0 && oldh > 0)
				{
					if (oldw != MainModule.mapWidth && oldh != MainModule.mapHeight)
					{
						
					}
				}
				
				
				SetPos(MainModule.x1, MainModule.y1);
				MainModule.LoadTiles(MainModule.TileSet);
				
			}
			
			public void SetPos(int xx, int yy)
			{
				bool stopChecking = false;
				int i;
				MainModule.x1 = xx;
				MainModule.y1 = yy;
				
				MainModule.x2 = xx;
				MainModule.y2 = yy;
				
				for (i = 1; i <= MainModule.maxSpecial; i++)
				{
					if (MainModule.specialx[i] == MainModule.x1 && MainModule.specialy[i] == MainModule.y1 && stopChecking == false)
					{
						cmdPlaceSpecial.Enabled = false;
						cmdModifySpecial.Enabled = true;
						cmdDeleteSpecial.Enabled = true;
						stopChecking = true;
					}
					else if (stopChecking == false)
					{
						cmdPlaceSpecial.Enabled = true;
						cmdModifySpecial.Enabled = false;
						cmdDeleteSpecial.Enabled = false;
					}
				}
				
			}
			
			public void SetRightPos(int xx, int yy)
			{
				MainModule.x2 = xx;
				MainModule.y2 = yy;
			}
			
			private void sbDown_Change(System.Object eventSender, System.EventArgs eventArgs)
			{
				MainModule.dispY = sbDown.Value;
				blit();
			}
			
			private void sbRight_Change(System.Object eventSender, System.EventArgs eventArgs)
			{
				MainModule.dispX = sbRight1.Value;
				blit();
			}
			
			private void PaintLoc(int x, int y, int value)
			{
				if (MainModule.ImportMap)
				{
					MainModule.PaintArea(x, y, value);
				}
				else
				{
					MainModule.mMap[x, y] = value;
				}
				
			}
			
			private void SetMenus()
			{
				
				mnuSave.Enabled = ! MainModule.ImportMap;
				mnuSaveAs.Enabled = ! MainModule.ImportMap;
				mnuProperties.Enabled = ! MainModule.ImportMap;
				mnuPlaceSpecial.Enabled = ! MainModule.ImportMap;
				mnuModifySpecial.Enabled = ! MainModule.ImportMap;
				mnuDeleteSpecial.Enabled = ! MainModule.ImportMap;
				
				mnuFinalize.Enabled = MainModule.ImportMap;
				mnuTitleImport.Enabled = MainModule.ImportMap;
				
			}
			
			private void sbSpecial_Change(System.Object eventSender, System.EventArgs eventArgs)
			{
				lblFindSpecial.Text = "Find Special: " + sbSpecial.Value;
				
				if (sbSpecial.Value == 0)
				{
					return;
				}
				
				MainModule.x1 = MainModule.specialx[sbSpecial.Value];
				MainModule.x2 = MainModule.x1;
				
				MainModule.y1 = MainModule.specialy[sbSpecial.Value];
				MainModule.y2 = MainModule.y1;
				
				sbRight1.Value = Math.Max(MainModule.x1 - MainModule.picTilesX / 5, 0);
				sbDown.Value = Math.Max(MainModule.y1 - MainModule.picTilesY / 5, 0);
				
				blit();
				
			}
			
			private void SortSpecials()
			{
				int i;
				int j;
				int max;
				int min;
				
				max = 0;
				min = 120;
				
				for (i = 120; i >= 1; i--)
				{
					if (MainModule.special[i] == 0)
					{
						if (i < max)
						{
							for (j = i; j <= max; j++)
							{
								MainModule.special[i] = MainModule.special[i + 1];
								MainModule.specialx[i] = MainModule.specialx[i + 1];
								MainModule.specialy[i] = MainModule.specialy[i + 1];
								MainModule.specialdata[i] = MainModule.specialdata[i + 1];
								MainModule.specialwidth[i] = MainModule.specialwidth[i + 1];
								MainModule.specialheight[i] = MainModule.specialheight[i + 1];
							}
							
						}
						
					}
					else if (MainModule.special[i] > 0)
					{
						if (i > max)
						{
							max = i;
						}
						
					}
					
				}
				
			}
			
			private int NumSpecials()
			{
				int i;
				
				SortSpecials();
				
				for (i = 1; i <= 120; i++)
				{
					if (MainModule.special[i] == 0)
					{
						return i - 1;
					}
				}
				
				return 0;
			}
		}
	}
