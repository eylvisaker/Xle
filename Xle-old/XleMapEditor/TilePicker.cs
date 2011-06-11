using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib;
using AgateLib.DisplayLib;
using Agate = AgateLib.Geometry;

namespace XleMapEditor
{
	[DefaultEvent("TilePick")]
	public partial class TilePicker : UserControl
	{
		public TilePicker()
		{
			InitializeComponent();
		}

		EditorState mState;

		[Browsable(false)]
		public EditorState State
		{
			get { return mState; }
			set
			{
				mState = value;

				Redraw();
			}
		}
		DisplayWindow wind;

		Size tilesDrawn;
		int tileStart;

		public int SelectedTileIndex { get; set; }
		int TileCount
		{
			get
			{
				return TileSquare.Width * TileSquare.Height;
			}
		}
		Size TileSquare
		{
			get
			{
				Size retval = new Size(TileSurface.SurfaceWidth / State.TileSize,
					TileSurface.SurfaceHeight / State.TileSize);

				return retval;
			}
		}
		public void CreateDisplayWindow()
		{
			wind = DisplayWindow.CreateFromControl(target);
		}

		Surface TileSurface
		{
			get { return mState.TileSurface; }
		}

		public Agate.Rectangle SourceRectangle(int tileIndex)
		{
			int span = TileSquare.Width;

			return new AgateLib.Geometry.Rectangle(
				(tileIndex % span) * State.TileSize,
				(tileIndex / span) * State.TileSize,
				State.TileSize,
				State.TileSize);
		}

		public void Redraw()
		{
			if (State == null) return;
			if (TileSurface == null) return;

			tilesDrawn.Width = target.ClientRectangle.Width / State.TileSize;
			tilesDrawn.Height = target.ClientRectangle.Height / State.TileSize + 1;

			sbDown.Maximum = TileCount / tilesDrawn.Width + 1;
			sbDown.LargeChange = tilesDrawn.Height - 2;

			tileStart = sbDown.Value * tilesDrawn.Width;

			Display.RenderTarget = wind;
			Display.BeginFrame();
			Display.Clear();

			Agate.Rectangle srcRect = new AgateLib.Geometry.Rectangle();
			Agate.Rectangle destRect = new AgateLib.Geometry.Rectangle();
			Agate.Rectangle selRect = new AgateLib.Geometry.Rectangle();

			int tileIndex = tileStart;
			destRect.Size = new AgateLib.Geometry.Size(State.TileSize, State.TileSize);

			for (int j = 0; j < tilesDrawn.Height; j++)
			{
				for (int i = 0; i < tilesDrawn.Width; i++)
				{
					srcRect = SourceRectangle(tileIndex);
					destRect.X = i * State.TileSize;
					destRect.Y = j * State.TileSize;

					State.TileSurface.Draw(srcRect, destRect);

					if (tileIndex == SelectedTileIndex)
					{
						selRect = destRect;
					}

					tileIndex++;

					if (tileIndex > TileCount)
						break;
				}

				if (tileIndex > TileCount)
					break;

			}

			if (selRect.IsEmpty == false)
			{
				Display.DrawRect(selRect, Agate.Color.Yellow);

				selRect.X--; selRect.Y--; selRect.Width += 2; selRect.Height += 2;
				Display.DrawRect(selRect, Agate.Color.Yellow);
			}

			Display.EndFrame();
		}

		int TileAt(Point e)
		{
			Point tilePt = new Point(e.X / State.TileSize, e.Y / State.TileSize);

			return tilePt.Y * tilesDrawn.Width + tilePt.X + tileStart;
		}
		private void target_MouseDown(object sender, MouseEventArgs e)
		{
			int tile = TileAt(e.Location);

			SelectedTileIndex = tile;
			Redraw();

			OnTilePick(tile, e.Button);
		}

		void OnTilePick(int tile, MouseButtons button)
		{
			if (TilePick != null)
				TilePick(this, new TilePickEventArgs(tile, button));
		}

		public event EventHandler<TilePickEventArgs> TilePick;

		private void sbDown_Scroll(object sender, ScrollEventArgs e)
		{
			Redraw();
		}

		private void target_Paint(object sender, PaintEventArgs e)
		{
			Redraw();
		}
	}

	public class TilePickEventArgs : EventArgs
	{
		internal TilePickEventArgs(int tile, MouseButtons button)
		{
			Tile = tile;
			Button = button;
		}

		public int Tile { get; private set; }
		public MouseButtons Button { get; private set; }
	}
}
