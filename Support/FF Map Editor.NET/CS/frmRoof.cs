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



namespace XleMapEditor
{
	partial class frmRoof : System.Windows.Forms.Form
	{
		
		#region Default Instance
		
		private static frmRoof defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
		public static frmRoof Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new frmRoof();
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
		
		public int RoofIndex;
		int RoofTile;
		int roofLeftX;
		int roofTopY;
		int x2;
		int y2;
		bool setting;
		
		ERY.AgateLib.DisplayWindow dispWindow;
		ERY.AgateLib.DisplayWindow tilesWindow;
		
		private void Check1_Click()
		{
			
		}
		
		//UPGRADE_WARNING: Event chkDrawGround.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void chkDrawGround_CheckStateChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			RoofBlit();
			
		}
		
		private void cmdDone_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			this.Hide();
			
		}
		
		private void frmRoof_Load(System.Object eventSender, System.EventArgs eventArgs)
		{
			dispWindow = new ERY.AgateLib.DisplayWindow(Picture1);
			tilesWindow = new ERY.AgateLib.DisplayWindow(Picture2);
			
			SetControls();
			
		}
		
		private void frmRoof_Paint(System.Object eventSender, System.Windows.Forms.PaintEventArgs eventArgs)
		{
			UpdateControls();
			RoofBlit();
		}
		
		private void hsbRoofIndex_Change(object sender, EventArgs e)
		{
			
			RoofIndex = hsbRoofIndex.Value;
			
			SetControls();
			
		}
		
		private void Picture1_Paint(System.Object eventSender, System.Windows.Forms.PaintEventArgs eventArgs)
		{
			RoofBlit();
		}
		
		public void RoofBlit()
		{
			
			RoofDraw();
			
			Display.RenderTarget = tilesWindow;
			Display.BeginFrame();
			
			MainModule.TileSurface.Draw();
			
			Display.EndFrame();
			
			
		}
		
		private void cmdCopy_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			int i;
			int j;
			Point t = new System.Drawing.Point();
			
			if (Interaction.MsgBox("Overwrite current roof", MsgBoxStyle.YesNo, "Overwrite") == MsgBoxResult.No)
			{
				return;
			}
			
			for (j = 0; j <= MainModule.Roofs[RoofIndex].height - 1; j++)
			{
				for (i = 0; i <= MainModule.Roofs[RoofIndex].width - 1; i++)
				{
					t.X = MainModule.Roofs[RoofIndex].anchorTarget.X - MainModule.Roofs[RoofIndex].anchor.X + i;
					t.Y = MainModule.Roofs[RoofIndex].anchorTarget.Y - MainModule.Roofs[RoofIndex].anchor.Y + j;
					
					MainModule.Roofs[RoofIndex].Matrix[i, j] = MainModule.Map(t.X, t.Y);
				}
			}
			
			RoofBlit();
			
		}
		
		private void RoofDraw()
		{
			//			int ddrval;
			ERY.AgateLib.Geometry.Rectangle r1 = new ERY.AgateLib.Geometry.Rectangle();
			ERY.AgateLib.Geometry.Rectangle r2 = new 			ERY.AgateLib.Geometry.Rectangle();
			int tilex;
			int i;
			int j;
			int a;
			int tiley;
			int roofCenterX;
			int xx;
			int yy;
			int roofCenterY;
			ERY.AgateLib.Geometry.Point t = new ERY.AgateLib.Geometry.Point();
			//			ERY.AgateLib.Geometry.Point tile;
			
			Display.RenderTarget = dispWindow;
			
			Display.BeginFrame();
			Display.Clear(Agate.Color.FromArgb(0x55, 0x55, 0x55));
			
			
			MainModule.picTilesX = Picture1.ClientRectangle.Width / MainModule.TileSize / 2 + 1;
			MainModule.picTilesY = Picture1.ClientRectangle.Height / MainModule.TileSize / 2;
			
			xx = 0;
			yy = 0;
			
			
			sbRight.Maximum = MainModule.mapWidth;
			sbRight.Minimum = 0;
			sbRight.LargeChange = MainModule.picTilesX * 2 - 2;
			sbDown.LargeChange = MainModule.picTilesY * 2 - 2;
			sbDown.Maximum = MainModule.mapHeight;
			sbDown.Minimum = 0;
			
			roofCenterX = sbRight.Value;
			roofCenterY = sbDown.Value;
			
			roofLeftX = roofCenterX - MainModule.picTilesX;
			roofTopY = roofCenterY - MainModule.picTilesY;
			
			
			for (j = roofCenterY - MainModule.picTilesY; j <= roofCenterY + MainModule.picTilesY + 1; j++)
			{
				for (i = roofCenterX - MainModule.picTilesX; i <= roofCenterX + MainModule.picTilesX + 1; i++)
				{
					if (i >= 0 && i < MainModule.Roofs[RoofIndex].width && j >= 0 && j < MainModule.Roofs[RoofIndex].height)
					{
						
						a = MainModule.Roofs[RoofIndex].Matrix[i, j];
						
						
						if (chkDrawGround.CheckState != 0 && a == 127)
						{
							//t = Roofs(k).matrix(i - Roofs(k).anchorTarget.X + Roofs(k).anchor.Y, _
							//'           j - Roofs(k).anchorTarget.Y + Roofs(k).anchor.Y)
							t.X = MainModule.Roofs[RoofIndex].anchorTarget.X - MainModule.Roofs[RoofIndex].anchor.X + i;
							t.Y = MainModule.Roofs[RoofIndex].anchorTarget.Y - MainModule.Roofs[RoofIndex].anchor.Y + j;
							
							if (t.Y < 0 || t.X < 0)
							{
								//UPGRADE_WARNING: Couldn't resolve default property of object a. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								a = 127;
							}
							else
							{
								//UPGRADE_WARNING: Couldn't resolve default property of object a. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								a = MainModule.Map(t.X, t.Y);
							}
							
						}
						
						tilex = (a % 16) * 16;
						tiley = (a / 16)* 16;
						
						r1.X = tilex;
						r1.Width = 16;
						r1.Y = tiley;
						r1.Height = 16;
						
						r2.X = xx * MainModule.TileSize;
						r2.Y = yy * MainModule.TileSize;
						r2.Width = 16;
						r2.Height = 16;
						
						MainModule.TileSurface.Draw(r1, r2);
						
						
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
			
			Display.EndFrame();
			
			r1.X = (MainModule.Roofs[RoofIndex].anchor.X - roofLeftX) * MainModule.TileSize;
			r1.Width = MainModule.TileSize;
			r1.Y = (MainModule.Roofs[RoofIndex].anchor.Y - roofTopY) * MainModule.TileSize;
			r1.Width = MainModule.TileSize;
			
			Display.DrawRect(r1, Agate.Color.Cyan);
			
			Display.DrawRect(new Agate.Rectangle((x2 - roofLeftX) * MainModule.TileSize, (y2 - roofTopY) * MainModule.TileSize, (x2 - roofLeftX + 1) * MainModule.TileSize, (y2 - roofTopY) * MainModule.TileSize), Agate.Color.White);
			
			lblx.Text = "x2:   " + x2 + "  0x" + Conversion.Hex(x2);
			lblY.Text = "y2:   " + y2 + "  0x" + Conversion.Hex(y2);
			
			if (x2 < 0 || x2 >= MainModule.Roofs[RoofIndex].width || y2 < 0 || y2 >= MainModule.Roofs[RoofIndex].height)
			{
				lblTile.Text = "Tile: Out of range";
			}
			else
			{
				lblTile.Text = "Tile: " + MainModule.Roofs[RoofIndex].Matrix[x2, y2] + "   0x" + Conversion.Hex(MainModule.Roofs[RoofIndex].Matrix[x2, y2]);
			}
			
			lblCurrentTile.Text = "Current Tile: " + RoofTile + "   0x" + Conversion.Hex(RoofTile);
			
			
		}
		
		
		
		private void Picture2_Paint(System.Object eventSender, System.Windows.Forms.PaintEventArgs eventArgs)
		{
			RoofBlit();
		}
		private void Picture1_MouseDown(System.Object eventSender, System.Windows.Forms.MouseEventArgs eventArgs)
		{
			int Button = System.Convert.ToInt32(eventArgs.Button) / 0x100000;
			int Shift = System.Convert.ToInt32(System.Windows.Forms.Control.ModifierKeys) / 0x10000;
			int x = eventArgs.X;
			int y = eventArgs.Y;
			int yy;
			int xx;
			
			
			xx = (x / 16) + roofLeftX;
			yy = (y / 16) + roofTopY;
			
			Debug.Print("X: " + x + "  XX: " + xx + "        Y: " + y + "  YY: " + yy);
			
			
			if (Button == 1)
			{
				
				SetPos(xx, yy);
				
			}
			else if (Button == 2 && xx >= 0 && yy >= 0)
			{
				
				MainModule.Roofs[RoofIndex].Matrix[xx, yy] = RoofTile;
				
			}
			
			
			RoofBlit();
		}
		
		private void Picture1_MouseMove(System.Object eventSender, System.Windows.Forms.MouseEventArgs eventArgs)
		{
			int Button = System.Convert.ToInt32(eventArgs.Button) / 0x100000;
			int Shift = System.Convert.ToInt32(System.Windows.Forms.Control.ModifierKeys) / 0x10000;
			int x = eventArgs.X;
			int y = eventArgs.Y;
			int xx;
			int yy;
			
			xx = x;
			yy = y;
			
			xx = Conversion.Int(xx / 16) + roofLeftX;
			yy = Conversion.Int(yy / 16) + roofTopY;
			
			if (Button == 2 && xx >= 0 && yy >= 0)
			{
				
				MainModule.Roofs[RoofIndex].Matrix[xx, yy] = RoofTile;
				
				RoofBlit();
			}
			
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
			
			if (Button == 2)
			{
				MainModule.Roofs[RoofIndex].Matrix[MainModule.x1, MainModule.y1] = tile;
			}
			
			RoofTile = tile;
			
			RoofBlit();
		}
		
		private void SetPos(int xx, int yy)
		{
			x2 = xx;
			y2 = yy;
			
		}
		
		public void UpdateControls()
		{
			lblEditing.Text = "Editing roof #: " + hsbRoofIndex.Value;
			
			if (setting == false)
			{
				if (Information.IsNumeric(txtRoofX.Text))
				{
					MainModule.Roofs[RoofIndex].width = int.Parse(txtRoofX.Text);
				}
				if (Information.IsNumeric(txtRoofY.Text))
				{
					MainModule.Roofs[RoofIndex].height = int.Parse(txtRoofY.Text);
				}
				if (Information.IsNumeric(txtAnchorX.Text))
				{
					MainModule.Roofs[RoofIndex].anchor.X = int.Parse(txtAnchorX.Text);
				}
				if (Information.IsNumeric(txtAnchorY.Text))
				{
					MainModule.Roofs[RoofIndex].anchor.Y = int.Parse(txtAnchorY.Text);
				}
				if (Information.IsNumeric(txtTargetX.Text))
				{
					MainModule.Roofs[RoofIndex].anchorTarget.X = int.Parse(txtTargetX.Text);
				}
				if (Information.IsNumeric(txtTargetY.Text))
				{
					MainModule.Roofs[RoofIndex].anchorTarget.Y = int.Parse(txtTargetY.Text);
				}
				
				RoofBlit();
			}
			
		}
		
		
		private void sbDown_Change(System.Object eventSender, System.EventArgs eventArgs)
		{
			RoofBlit();
		}
		
		private void sbRight_Change(System.Object eventSender, System.EventArgs eventArgs)
		{
			RoofBlit();
			
		}
		
		//UPGRADE_WARNING: Event txtAnchorX.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtAnchorX_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
			
		}
		
		//UPGRADE_WARNING: Event txtAnchorY.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtAnchorY_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
			
		}
		
		//UPGRADE_WARNING: Event txtRoofX.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
		private void txtRoofX_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
			
		}
		
		public void SetControls()
		{
			setting = true;
			
			txtRoofX.Text = System.Convert.ToString(MainModule.Roofs[RoofIndex].width);
			txtRoofY.Text = System.Convert.ToString(MainModule.Roofs[RoofIndex].height);
			txtAnchorX.Text = System.Convert.ToString(MainModule.Roofs[RoofIndex].anchor.X);
			txtAnchorY.Text = System.Convert.ToString(MainModule.Roofs[RoofIndex].anchor.Y);
			txtTargetX.Text = System.Convert.ToString(MainModule.Roofs[RoofIndex].anchorTarget.X);
			txtTargetY.Text = System.Convert.ToString(MainModule.Roofs[RoofIndex].anchorTarget.Y);
			
			hsbRoofIndex.Value = RoofIndex;
			
			setting = false;
			
			RoofBlit();
		}
		
		private void txtRoofY_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		
		private void txtTargetX_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
			
		}
		
		private void txtTargetY_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
			
		}
		private void hsbRoofIndex_Scroll(System.Object eventSender, System.Windows.Forms.ScrollEventArgs eventArgs)
		{
			switch (eventArgs.Type)
			{
				case System.Windows.Forms.ScrollEventType.EndScroll:
					hsbRoofIndex_Change(eventSender, eventArgs);
					break;
			}
		}
	}
}
