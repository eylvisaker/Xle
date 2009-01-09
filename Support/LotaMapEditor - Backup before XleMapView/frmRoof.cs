
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ERY.AgateLib;
using ERY.Xle;
using Microsoft.VisualBasic;
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
        [Obsolete]
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

        public XleMap TheMap { get; set; }
        public IHasRoofs R
        {
            get { return (IHasRoofs)TheMap; }
        }
        Roof CurrentRoof
        {
            get { return R.Roofs[RoofIndex]; }
        }

        private int mRoofIndex;

        public int RoofIndex
        {
            get { return mRoofIndex; }
            set
            {
                mRoofIndex = value;
                SetControls();
            }
        }


		int RoofTile;
		int roofLeftX;
		int roofTopY;
		int x2;
		int y2;
		bool setting;
		
		ERY.AgateLib.DisplayWindow dispWindow;
		ERY.AgateLib.DisplayWindow tilesWindow;

        Point topLeft;
        Size tilesDrawn;

        public void CreateRoof(Point p1)
        {
            Roof r = new Roof();
            r.X = p1.X;
            r.Y = p1.Y;
            r.SetSize(10, 10);

            R.Roofs.Add(r);

            RoofIndex = R.Roofs.Count - 1;
        }
		
		private void chkDrawGround_CheckStateChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			RoofBlit();
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
		
		private void Picture1_Paint(System.Object eventSender, System.Windows.Forms.PaintEventArgs eventArgs)
		{
			RoofBlit();
		}
		
		public void RoofBlit()
		{
            if (tilesWindow == null)
                return;

			RoofDraw();
			
			Display.RenderTarget = tilesWindow;
			Display.BeginFrame();
			
			MainModule.TileSurface.Draw();
            Agate.Rectangle r = new ERY.AgateLib.Geometry.Rectangle();
            r.X = (RoofTile % 16) * MainModule.TileSize;
            r.Y = (RoofTile / 16) * MainModule.TileSize;
            r.Width = MainModule.TileSize;
            r.Height = MainModule.TileSize;

            Display.DrawRect(r, Agate.Color.Yellow);

            r.X--; r.Y--; r.Width += 2; r.Height += 2;
            Display.DrawRect(r, Agate.Color.Yellow);

			Display.EndFrame();
			
			
		}
		
		private void cmdCopy_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			int i;
			int j;
			Point t = new System.Drawing.Point();

            if (MessageBox.Show(this, "Overwrite current roof?", "Roof Editor",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			
			for (j = 0; j < CurrentRoof.Height; j++)
			{
				for (i = 0; i < CurrentRoof.Width; i++)
				{
                    t.X = CurrentRoof.X + i;
                    t.Y = CurrentRoof.Y + j;

                    if (t.X < TheMap.Width && t.Y < TheMap.Height)
                    {
                        CurrentRoof[i, j] = TheMap[t.X, t.Y];
                    }
                    else
                    {
                        CurrentRoof[i, j] = 127;
                    }
				}
			}
			
			RoofBlit();
			
		}


		private void RoofDraw()
		{
            if (dispWindow == null)
                return;

			//			int ddrval;
			ERY.AgateLib.Geometry.Rectangle r1 = new ERY.AgateLib.Geometry.Rectangle();
			ERY.AgateLib.Geometry.Rectangle r2 = new 			ERY.AgateLib.Geometry.Rectangle();
			int tilex;
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

            tilesDrawn.Width = Picture1.ClientRectangle.Width / MainModule.TileSize + 1;
            tilesDrawn.Height = Picture1.ClientRectangle.Height / MainModule.TileSize + 1;
			
			xx = 0;
			yy = 0;


            sbRight.Maximum = TheMap.Width;
			sbRight.Minimum = 0;
			sbRight.LargeChange = tilesDrawn.Width - 2;
			sbDown.LargeChange = tilesDrawn.Height - 2;
            sbDown.Maximum = TheMap.Height;
			sbDown.Minimum = 0;
			
			roofCenterX = sbRight.Value + 4;
			roofCenterY = sbDown.Value + 4;

            topLeft.X = roofCenterX - tilesDrawn.Width / 2;
            topLeft.Y = roofCenterY - tilesDrawn.Height / 2;

            roofLeftX = topLeft.X;
            roofTopY = topLeft.Y;

            if (chkDrawGround.Checked)
            {
                xx = 0;
                yy = 0;
                for (int j = topLeft.Y; j < topLeft.Y + tilesDrawn.Height; j++)
                {
                    for (int i = topLeft.X; i < topLeft.X + tilesDrawn.Width; i++)
                    {
                        t.X = i + CurrentRoof.X;
                        t.Y = j + CurrentRoof.Y;

                        int a = TheMap[t.X, t.Y];


                        tilex = (a % 16) * 16;
                        tiley = (a / 16) * 16;

                        r1.X = tilex;
                        r1.Width = 16;
                        r1.Y = tiley;
                        r1.Height = 16;

                        r2.X = xx * MainModule.TileSize;
                        r2.Y = yy * MainModule.TileSize;
                        r2.Width = 16;
                        r2.Height = 16;

                        MainModule.TileSurface.Draw(r1, r2);

                        xx++;
                    }
                    yy++;
                    xx = 0;
                }

                Display.FillRect(new Agate.Rectangle(0, 0, Picture1.ClientRectangle.Width, Picture1.ClientRectangle.Height),
                    Agate.Color.FromArgb(160, 0, 0, 0));
            }

            xx = 0;
            yy = 0;
			for (int j = topLeft.Y; j <= topLeft.Y + tilesDrawn.Height; j++)
			{
				for (int i = topLeft.X; i <= topLeft.X + tilesDrawn.Width; i++)
				{
					if (i >= 0 && i < R.Roofs[RoofIndex].Width && j >= 0 && j < R.Roofs[RoofIndex].Height)
					{
						int a = R.Roofs[RoofIndex][i, j];
						
						if (chkDrawGround.CheckState != 0 && a == 127)
						{
							//t = Roofs(k).matrix(i - Roofs(k).anchorTarget.X + Roofs(k).anchor.Y, _
							//'           j - Roofs(k).anchorTarget.Y + Roofs(k).anchor.Y)
							t.X = i + R.Roofs[RoofIndex].X;
                            t.Y = j + R.Roofs[RoofIndex].Y;
							
							if (t.Y < 0 || t.X < 0)
							{
								a = 127;
							}
							else
							{
                                a = TheMap[t.X, t.Y];
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
					
					xx++;
				}
				yy++;
				xx = 0;
				
			}
			
			Display.EndFrame();
			
			r1.X = ( - roofLeftX) * MainModule.TileSize;
			r1.Width = MainModule.TileSize;
			r1.Y = (- roofTopY) * MainModule.TileSize;
			r1.Width = MainModule.TileSize;
			
			Display.DrawRect(r1, Agate.Color.Cyan);
			Display.DrawRect(new Agate.Rectangle((x2 - roofLeftX) * MainModule.TileSize, (y2 - roofTopY) * MainModule.TileSize, (x2 - roofLeftX + 1) * MainModule.TileSize, (y2 - roofTopY) * MainModule.TileSize), Agate.Color.White);
			
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
                R.Roofs[RoofIndex][xx, yy] = RoofTile;
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
			
			xx = (int)(xx / 16) + roofLeftX;
			yy = (int)(yy / 16) + roofTopY;

            SetPos(xx, yy);

			if (Button == 2 && xx >= 0 && yy >=0 && xx < CurrentRoof.Width && yy < CurrentRoof.Height)
			{
                CurrentRoof[xx, yy] = RoofTile;
				
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
			
			RoofTile = tile;

            if (Button == 2)
            {
                CurrentRoof[x2, y2] = RoofTile;
            }

			RoofBlit();
		}
		
		private void SetPos(int xx, int yy)
		{
			x2 = xx;
			y2 = yy;
			
		}
		
		public void UpdateControls()
		{
			lblEditing.Text = "Editing roof #: " + nudRoofIndex.Value.ToString();

            if (setting == true)
                return;

            int width = (int)nudRoofWidth.Value;
            int height = (int)nudRoofHeight.Value;

            if (width != CurrentRoof.Width || height != CurrentRoof.Height)
            {
                CurrentRoof.SetSize(width, height);
            }

            CurrentRoof.X = (int)nudRoofX.Value;
            CurrentRoof.Y = (int)nudRoofY.Value;

    		RoofBlit();

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
            if (setting)
                return;

            try
            {
                setting = true;

                nudRoofIndex.Minimum = 0;
                nudRoofIndex.Maximum = R.Roofs.Count - 1;
                nudRoofX.Maximum = TheMap.Width - 1;
                nudRoofY.Maximum = TheMap.Height - 1;
                nudRoofWidth.Maximum = TheMap.Width;
                nudRoofHeight.Maximum = TheMap.Height;

                nudRoofIndex.Value = RoofIndex;

                nudRoofX.Value = CurrentRoof.X;
                nudRoofY.Value = CurrentRoof.Y;
                nudRoofWidth.Value = CurrentRoof.Width;
                nudRoofHeight.Value = CurrentRoof.Height;
            }
            finally
            {
                setting = false;
            }

			RoofBlit();
		}
		
        private void nudRoofX_ValueChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }
        private void nudRoofY_ValueChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }
        private void nudRoofWidth_ValueChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }
        private void nudRoofHeight_ValueChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }



        private void nudRoofIndex_ValueChanged(object sender, EventArgs e)
        {
            RoofIndex = (int)nudRoofIndex.Value;
        }

        private void sbRight_Scroll(object sender, ScrollEventArgs e)
        {
            RoofBlit();
        }
        private void sbDown_Scroll(object sender, ScrollEventArgs e)
        {
            RoofBlit();
        }

        private void btnCreateRoof_Click(object sender, EventArgs e)
        {
            CreateRoof(new Point(0, 0));
        }
        private void btnDeleteRoof_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this,
                "Really delete roof #" + RoofIndex.ToString() + "?",
                "Delete Roof", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                R.Roofs.RemoveAt(RoofIndex);

                if (R.Roofs.Count == 0)
                {
                    this.Hide();
                    return;
                }
                if (RoofIndex >= R.Roofs.Count)
                {
                    RoofIndex = R.Roofs.Count - 1;
                }

                RoofBlit();
            }

        }



    }
}
