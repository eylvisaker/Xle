using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;
using AgateLib.Serialization.Xle;
using AgateLib.DisplayLib;

using Vertex = AgateLib.Geometry.VertexTypes.PositionTextureNormalTangent;
using ERY.Xle.Maps.XleMapTypes.Extenders;

namespace ERY.Xle.Maps.XleMapTypes
{
	public class Museum : Map3D
	{
		int[] mData;
		int mHeight;
		int mWidth;
		
		IMuseumExtender Extender { get; set; }

		public Museum()
		{
		}
		
		protected override void ReadData(XleSerializationInfo info)
		{
			mWidth = info.ReadInt32("Width");
			mHeight = info.ReadInt32("Height");
			mData = info.ReadInt32Array("Data");
		}
		protected override void WriteData(XleSerializationInfo info)
		{
			info.Write("Width", mWidth, true);
			info.Write("Height", mHeight, true);
			info.Write("Data", mData, NumericEncoding.Csv);
		}
		public override IEnumerable<string> AvailableTileImages
		{
			get { yield return "DungeonTiles.png"; }
		}
		public override void InitializeMap(int width, int height)
		{
			mData = new int[height * width];
			mHeight = height;
			mWidth = width;
		}
		
		public override int Height
		{
			get { return mHeight; }
		}
		public override int Width
		{
			get { return mWidth; }
		}

		public override int this[int xx, int yy]
		{
			get
			{

				if (yy < 0 || yy >= Height || xx < 0 || xx >= Width)
				{
					return 0;
				}
				else
				{
					return mData[yy * mWidth + xx];
				}
			}
			set
			{
				if (yy < 0 || yy >= Height ||
					xx < 0 || xx >= Width)
				{
					return;
				}
				else
				{
					mData[yy * mWidth + xx] = value;
				}

			}
		}

		protected override double StepQuality
		{
			get
			{
				return 0.5;
			}
		}

		protected override Extenders.IMapExtender CreateExtenderImpl()
		{
			if (XleCore.Factory == null)
				Extender = new NullMuseumExtender();
			else
				Extender = XleCore.Factory.CreateMapExtender(this);

			return Extender;
		}

		public override void OnLoad(Player player)
		{
			base.OnLoad(player);

			CheckExhibitStatus(player);
			
		}

		protected override Color ExtraColor(Point location)
		{
			var ex = ExhibitAt(location.X, location.Y);

			if (ex == null)
				return base.ExtraColor(location);

			return ex.ExhibitColor;
		}

		protected override Map3D.ExtraType GetExtraType(int val, int side)
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
		private bool IsExhibitAt(Point location)
		{
			return IsExhibit(this[location.X, location.Y]);
		}
		private bool IsExhibit(int value)
		{
			if ((value & 0xf0) == 0x50)
				return true;
			else
				return false;
		}

		#region --- Museum Exhibits ---

		MuseumDisplays.Exhibit mCloseup;
		bool mDrawStatic;

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

		private MuseumDisplays.Exhibit ExhibitAt(int x, int y)
		{
			int tileAt = this[x, y];

			return Extender.GetExhibitByTile(tileAt);
		}
		
		private bool InteractWithDisplay(Player player)
		{
			Point stepDir = StepDirection(player.FaceDirection);

			MuseumDisplays.Exhibit ex = ExhibitAt(player.X + stepDir.X, player.Y + stepDir.Y);

			if (ex == null)
				return false;

			DrawCloseup = true;
			mCloseup = ex;
			mDrawStatic = ex.StaticBeforeCoin;

			XleCore.TextArea.PrintLine(ex.IntroductionText);
			XleCore.TextArea.PrintLine();
			g.AddBottomCentered(ex.LongName, ex.TitleColor);

			XleCore.PromptToContinueOnWait = true;

			if (ex.IsClosed(player))
			{
				g.AddBottomCentered("- Exhibit closed -", ex.TitleColor);
				XleCore.TextArea.PrintLine();
				XleCore.WaitForKey();

				return true;
			}

			g.AddBottomCentered(ex.InsertCoinText, ex.TitleColor);
			XleCore.TextArea.PrintLine();
			XleCore.WaitForKey();

			if (ex.RequiresCoin(player) == false)
			{
				mDrawStatic = false;
				RunExhibit(player, ex);
			}
			else
			{
				if (ex.HasBeenVisited(player) == false)
					XleCore.TextArea.PrintLine("You haven't used this exhibit.");
				else
					XleCore.TextArea.PrintLine();

				if (Extender.PlayerHasCoin(player, ex) == false)
				{
					Extender.NeedsCoinMessage(player, ex);
					XleCore.Wait(500);

					return true;
				}
				else
				{
					XleCore.TextArea.PrintLine(ex.UseCoinMessage);
					XleCore.TextArea.PrintLine();

					int choice = XleCore.QuickMenu(new MenuItemList("Yes", "no"), 3);

					if (choice == 1)
						return true;

					Extender.UseCoin(player, ex);

					mDrawStatic = false;
					RunExhibit(player, ex);
				}
			}

			return true;
		}

		private void RunExhibit(Player player, MuseumDisplays.Exhibit ex)
		{
			ex.RunExhibit(player);

			CheckExhibitStatus(player);
		}

		private void CheckExhibitStatus(Player player)
		{
			Extender.CheckExhibitStatus(GameState);
		}


		#endregion

		public override bool PlayerXamine(Player player)
		{
			XleCore.TextArea.PrintLine();

			if (InteractWithDisplay(player))
				return true;

			XleCore.TextArea.PrintLine("You are in an ancient museum.");

			return true;
		}
		public override bool PlayerFight(Player player)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("There is nothing to fight.");

			return true;
		}
		public override bool PlayerRob(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("There is nothing to rob.");

			return true;
		}
		protected override bool PlayerSpeakImpl(Player player)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("There is no reply.");

			return true;
		}
		public override bool PlayerTake(Player player)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("There is nothing to take.");

			return true;
		}

		protected override void DrawMuseumExhibit(int distance, Rectangle destRect, int val)
		{
			var exhibit = Extender.GetExhibitByTile(val);
			Color clr = exhibit.ExhibitColor;

			DrawExhibitStatic(destRect, clr, distance);

			if (distance == 1)
			{
				DrawExhibitText(destRect, exhibit);
			}
		}

		private static void DrawExhibitText(Rectangle destRect, MuseumDisplays.Exhibit exhibit)
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

			if (1 == 1)
			{
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
			}
			else
			{

				destRect.X += destOffset.X;
				destRect.Y += destOffset.Y;
				destRect.Width = srcRect.Width;
				destRect.Height = srcRect.Height;

				Surfaces.MuseumExhibitStatic.Color = clr;
				Surfaces.MuseumExhibitStatic.Draw(srcRect, destRect);
			}

			anim++;
			if (anim % freq == 0)
				offset = XleCore.random.Next((destOffset.Width - 16) / 4) * 4;
		}

		protected override void PlayPlayerMoveSound()
		{
			SoundMan.PlaySound(LotaSound.WalkMuseum);
		}
	}
}