using AgateLib.Geometry;
using ERY.Xle.Maps.Extenders;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.Renderers
{
	public class MuseumRenderer : Map3DRenderer
	{
		protected override ExtraType GetExtraType(int val, int side)
		{
			if (val >= 0x50 && val <= 0x5f)
			{
				if (side == -1) return ExtraType.DisplayCaseLeft;
				if (side == 1) return ExtraType.DisplayCaseRight;
			}
			if (val == 1)
			{
				if (side == -1) return ExtraType.TorchLeft;
				if (side == 1) return ExtraType.TorchRight;
			}

			return ExtraType.None;
		}
		protected override bool ExtraScale
		{
			get
			{
				return true;
			}
		}

		#region --- Museum Exhibits ---

		public Exhibit mCloseup;
		public bool mDrawStatic;

		protected override void DrawCloseupImpl(Rectangle inRect)
		{
			Rectangle displayRect = new Rectangle(inRect.X + 64, inRect.Y + 64, 240, 128);

			Surfaces.MuseumExhibitStatic.Draw(displayRect);
			Surfaces.MuseumExhibitCloseup.Draw(inRect);

			DrawExhibitText(inRect, mCloseup);

			if (mDrawStatic == false)
			{
				mCloseup.Draw(displayRect);
			}
		}

		#endregion

		protected override void DrawMuseumExhibit(int distance, Rectangle destRect, int val)
		{
			var exhibit = ((MuseumExtender)TheMap.Extender).GetExhibitByTile(val);
			Color clr = exhibit.ExhibitColor;

			DrawExhibitStatic(destRect, clr, distance);

			if (distance == 1)
			{
				DrawExhibitText(destRect, exhibit);
			}
		}

		private static void DrawExhibitText(Rectangle destRect, Exhibit exhibit)
		{
			int px = 176;
			int py = 208;

			int textLength = exhibit.Name.Length;

			px -= (textLength / 2) * 16;

			px += destRect.X;
			py += destRect.Y;

			AgateLib.DisplayLib.Display.FillRect(px, py, textLength * 16, 16, Color.Black);

			Color clr = exhibit.TitleColor;
			XleCore.Renderer.WriteText(px, py, exhibit.Name, clr);
		}

		int anim;
		int offset = 0;

		private void DrawExhibitStatic(Rectangle destRect, Color clr, int distance)
		{
			Rectangle destOffset = new Rectangle(96, 96, 160, 96);

			if (distance == 2)
			{
				destOffset.X = 128;
				destOffset.Y = 112;
				destOffset.Width = 112;
				destOffset.Height = 64;
			}

			Rectangle srcRect = new Rectangle(0, 0, destOffset.Width, destOffset.Height);
			Rectangle oldDest = destRect;

			oldDest.X += destOffset.X;
			oldDest.Y += destOffset.Y;
			oldDest.Width = srcRect.Width;
			oldDest.Height = srcRect.Height;

			int freq = 5;

			srcRect.X = offset;
			srcRect.Width -= srcRect.X;

			destRect.X += destOffset.X;
			destRect.Y += destOffset.Y;
			destRect.Width = srcRect.Width;
			destRect.Height = srcRect.Height;

			Surfaces.MuseumExhibitStatic.Color = clr;
			Surfaces.MuseumExhibitStatic.Draw(srcRect, destRect);

			destRect = Rectangle.FromLTRB(destRect.Right, destRect.Top, oldDest.Right, destRect.Bottom);
			srcRect.X = 0;
			srcRect.Width = destRect.Width;

			Surfaces.MuseumExhibitStatic.Draw(srcRect, destRect);


			anim++;
			if (anim % freq == 0)
				offset = XleCore.random.Next((destOffset.Width - 16) / 4) * 4;
		}

	}
}
