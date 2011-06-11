using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ERY.Xle;
using AgateLib;
using AgateLib.DisplayLib;
using Agate = AgateLib.Geometry;

namespace XleMapEditor
{
	public partial class XleMapView : UserControl
	{
		DisplayWindow mainWindow;

		Point p1;
		Point p2;

		EditorState mState;

		bool mDrawGuards = true;
		bool mDrawRoofs = false;

		public XleMapView()
		{
			InitializeComponent();
		}

		[Browsable(false)]
		public EditorState State
		{
			get
			{
				return mState;
			}
			set
			{
				mState = value;
				if (value == null)
					return;

				sbRight.Maximum = TheMap.Width;
				sbRight.Minimum = 0;
				sbRight.Value = 0;

				sbDown.Maximum = TheMap.Height;
				sbDown.Minimum = 0;
				sbDown.Value = 0;
			}
		}
		[Browsable(false)]
		public XleMap TheMap { get { return State.TheMap; } }

		[Browsable(false)]
		public Size TilesDrawn { get; private set; }
		[Browsable(false)]
		public Point TopLeft { get; private set; }

		[DefaultValue(true)]
		public bool AllowBoxSelect { get; set; }

		[DefaultValue(true)]
		public bool DrawGuards
		{
			get { return mDrawGuards; }
			set
			{
				mDrawGuards = value;
				Redraw();
			}
		}
		[DefaultValue(false)]
		public bool DrawRoofs
		{
			get { return mDrawRoofs; }
			set
			{
				mDrawRoofs = value;
				Redraw();
			}
		}

		[Browsable(false)]
		public Rectangle SelRect
		{
			get
			{
				Rectangle retval = new Rectangle();
				Size sz = new Size(1, 1);

				retval = Rectangle.Union(new Rectangle(p1, sz), new Rectangle(p2, sz));

				if (retval.Width == 0) retval.Width++;
				if (retval.Height == 0) retval.Height++;

				return retval;
			}
			set
			{
				p1 = value.Location;
				p2 = new Point(value.Right, value.Bottom);
			}
		}
		public void CreateDisplayWindow()
		{
			mainWindow = DisplayWindow.CreateFromControl(target);
		}

		public void Redraw()
		{
			if (State == null) return;
			if (TheMap == null) return;

			int tiley;
			int a;
			int tilex;
			int t;
			int xx;
			int yy;
			Agate.Rectangle srcRect = new AgateLib.Geometry.Rectangle();
			Agate.Rectangle destRect = new AgateLib.Geometry.Rectangle();

			Display.RenderTarget = mainWindow;

			Display.BeginFrame();
			Display.Clear(Agate.Color.FromArgb(0x55, 0x55, 0x55));

			TilesDrawn = new Size(
				(int)Math.Ceiling(target.ClientRectangle.Width / (double)State.DisplaySize),
				(int)Math.Ceiling(target.ClientRectangle.Height / (double)State.DisplaySize));


			xx = 0;
			yy = 0;

			sbRight.Maximum = TheMap.Width;
			sbRight.Minimum = 0;
			sbRight.LargeChange = (int)Math.Max(TilesDrawn.Width - 2, 1);
			sbDown.LargeChange = (int)Math.Max(TilesDrawn.Height - 2, 1);
			sbDown.Maximum = TheMap.Height;
			sbDown.Minimum = 0;

			TopLeft = new Point(
				sbRight.Value - 1,
				sbDown.Value - 1);


			for (int j = TopLeft.Y; j <= TopLeft.Y + TilesDrawn.Height + 1; j++)
			{
				for (int i = TopLeft.X; i <= TopLeft.X + TilesDrawn.Width + 1; i++)
				{
					if (i >= 0 && i < TheMap.Width && j >= 0 && j < TheMap.Height)
					{
						a = TheMap[i, j];

						if (DrawRoofs && TheMap is IHasRoofs)
						{
							IHasRoofs hasroofs = (IHasRoofs)TheMap;

							foreach (Roof r in hasroofs.Roofs)
							{
								if (r.PointInRoof(i, j) == false)
									continue;

								t = r[i - r.X, j - r.Y];

								if (t != 127)
									a = t;
							}

						}

						tilex = (a % 16) * 16;
						tiley = (a / 16) * 16;

						srcRect.X = tilex;
						srcRect.Y = tiley;
						srcRect.Width = State.TileSize;
						srcRect.Height = State.TileSize;

						destRect.X = xx * State.DisplaySize;
						destRect.Y = yy * State.DisplaySize;
						destRect.Width = State.DisplaySize;
						destRect.Height = State.DisplaySize;

						State.TileSurface.Draw(srcRect, destRect);
					}

					xx++;
				}

				yy++;
				xx = 0;

			}

			if (DrawGuards && TheMap is IHasGuards)
			{
				IHasGuards g = TheMap as IHasGuards;

				for (int i = 0; i < g.Guards.Count; i++)
				{
					if (g.Guards[i].X > TopLeft.X && g.Guards[i].X < TopLeft.X + TilesDrawn.Width &&
						g.Guards[i].Y > TopLeft.Y && g.Guards[i].Y < TopLeft.Y + TilesDrawn.Height)
					{
						srcRect.Y = 5 * 32;
						srcRect.X = 0 * 32;
						srcRect.Width = 32;
						srcRect.Height = 32;

						destRect.X = (g.Guards[i].X - TopLeft.X) * State.DisplaySize;
						destRect.Y = (g.Guards[i].Y - TopLeft.Y) * State.DisplaySize;
						destRect.Width = 32;
						destRect.Height = 32;

						MainModule.CharSurface.Draw(srcRect, destRect);

					}
				}
			}


			for (int i = 0; i < TheMap.Events.Count; i++)
			{
				XleEvent evt = TheMap.Events[i];

				if (evt.Rectangle.Right < TopLeft.X) continue;
				if (evt.Rectangle.Left > TopLeft.X + TilesDrawn.Width) continue;
				if (evt.Rectangle.Bottom < TopLeft.Y) continue;
				if (evt.Rectangle.Top > TopLeft.Y + TilesDrawn.Height) continue;

				srcRect.X = (evt.X - TopLeft.X) * State.DisplaySize;
				srcRect.Y = (evt.Y - TopLeft.Y) * State.DisplaySize;
				srcRect.Width = evt.Width * State.DisplaySize;
				srcRect.Height = evt.Height * State.DisplaySize;

				Display.DrawRect(srcRect, Agate.Color.Yellow);

				srcRect.Width = State.DisplaySize;
				srcRect.Height = State.DisplaySize;

				Display.DrawRect(srcRect, Agate.Color.Yellow);
			}


			if (TheMap is IHasRoofs)
			{
				///'''''''''''''''''''''''
				//'  Draw Roofs rectangles
				IHasRoofs r = TheMap as IHasRoofs;

				for (int i = 0; i < r.Roofs.Count; i++)
				{
					Roof current = r.Roofs[i];

					if (TopLeft.X + TilesDrawn.Width < current.X) continue;
					if (TopLeft.Y + TilesDrawn.Height < current.Y) continue;
					if (TopLeft.X > current.X + current.Width) continue;
					if (TopLeft.Y > current.Y + current.Height) continue;


					//srcRect.X = (current.X - TopLeft.X) * MainModule.TileSize;
					//srcRect.Width = MainModule.TileSize;
					//srcRect.Y = (MainModule.Roofs[i].anchorTarget.Y - MainModule.topy) * MainModule.TileSize;
					//srcRect.Height = MainModule.TileSize;

					//Display.DrawRect(srcRect, Agate.Color.Yellow);

					srcRect.X = (current.X - TopLeft.X) * State.DisplaySize;
					srcRect.Width = current.Width * State.DisplaySize;
					srcRect.Y = (current.Y - TopLeft.Y) * State.DisplaySize;
					srcRect.Height = current.Height * State.DisplaySize;

					Display.DrawRect(srcRect, Agate.Color.FromArgb(255, 60, 255, 180));

				}
			}

			Display.DrawRect(new Agate.Rectangle(
				(SelRect.X - TopLeft.X) * State.DisplaySize,
				(SelRect.Y - TopLeft.Y) * State.DisplaySize,
				SelRect.Width * State.DisplaySize,
				SelRect.Height * State.DisplaySize), Agate.Color.White);

			Display.EndFrame();
		}

		Point PixelToTile(Point p)
		{
			if (State == null) return Point.Empty;

			Point retval = new Point(
			  (p.X / State.DisplaySize) + TopLeft.X,
			  (p.Y / State.DisplaySize) + TopLeft.Y);

			return retval;
		}
		private void target_MouseDown(object sender, MouseEventArgs e)
		{
			if (State == null) return;

			Point tile = PixelToTile(e.Location);

			p1 = tile;
			p2 = tile;

			OnTileMouseDown(tile, e);

			Redraw();

		}

		Point lastMouseMove;
		private void target_MouseMove(object sender, MouseEventArgs e)
		{
			if (State == null) return;

			Point tile = PixelToTile(e.Location);

			if (lastMouseMove == tile)
				return;

			if (e.Button == MouseButtons.Left)
			{
				p2 = tile;
			}
			lastMouseMove = tile;

			OnTileMouseMove(tile, e);

			Redraw();
		}
		private void target_MouseUp(object sender, MouseEventArgs e)
		{
			if (State == null) return;

			Point tile = PixelToTile(e.Location);

			OnTileMouseUp(tile, e);
		}

		protected void OnTileMouseDown(Point tile, MouseEventArgs e)
		{
			if (TileMouseDown != null)
				TileMouseDown(this, new TileMouseEventArgs(tile.X, tile.Y, e));
		}
		protected void OnTileMouseMove(Point tile, MouseEventArgs e)
		{
			if (TileMouseMove != null)
				TileMouseMove(this, new TileMouseEventArgs(tile.X, tile.Y, e));
		}
		protected void OnTileMouseUp(Point tile, MouseEventArgs e)
		{
			if (TileMouseUp != null)
				TileMouseUp(this, new TileMouseEventArgs(tile.X, tile.Y, e));
		}

		public event EventHandler<TileMouseEventArgs> TileMouseDown;
		public event EventHandler<TileMouseEventArgs> TileMouseMove;
		public event EventHandler<TileMouseEventArgs> TileMouseUp;


		internal void CenterOnSel()
		{
			int centerx = SelRect.X + SelRect.Width / 2;
			int centery = SelRect.Y + SelRect.Height / 2;

			int rightValue = centerx - TilesDrawn.Width / 2;
			int downValue = centery - TilesDrawn.Height / 2;

			rightValue = Math.Max(rightValue, 0);
			downValue = Math.Max(downValue, 0);

			sbRight.Value = rightValue;
			sbDown.Value = downValue;
		}

		private void sbRight_Scroll(object sender, ScrollEventArgs e)
		{
			Redraw();
		}
		private void sbDown_Scroll(object sender, ScrollEventArgs e)
		{
			Redraw();
		}

		private void target_Paint(object sender, PaintEventArgs e)
		{
			Redraw();
		}

		private void XleMapView_Resize(object sender, EventArgs e)
		{
			Redraw();
		}
	}

	public class TileMouseEventArgs : MouseEventArgs
	{
		public TileMouseEventArgs(int tilex, int tiley, MouseEventArgs e)
			: base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
		{
			Tile = new Point(tilex, tiley);
		}
		public Point Tile { get; private set; }
	}
}