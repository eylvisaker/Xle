using AgateLib.Geometry;
using ERY.Xle.Maps.Renderers;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.Maps.Extenders
{
	public class MuseumExtender : Map3DExtender
	{
		int doorVal = 2;

		public new Museum TheMap { get { return (Museum)base.TheMap;  } }
		public new MuseumRenderer MapRenderer { get { return (MuseumRenderer)base.MapRenderer; } }

		protected override Renderers.XleMapRenderer CreateMapRenderer()
		{
			return new MuseumRenderer();
		}
		protected override void PlayPlayerMoveSound()
		{
			SoundMan.PlaySound(LotaSound.WalkMuseum);
		}
		protected override void OnBeforePlayerMove(GameState state, Direction dir)
		{
			MapRenderer.DrawCloseup = false;
		}
		protected override void CommandTextForInvalidMovement(ref string command)
		{
			command = "Bump into wall";
		}

		public override Map3DSurfaces Surfaces(GameState state)
		{
			return null;
		}

		protected bool IsFacingDoor(GameState state)
		{
			Point faceDir = state.Player.FaceDirection.StepDirection();
			Point test = new Point(state.Player.X + faceDir.X, state.Player.Y + faceDir.Y);
			bool facingDoor = TheMap[test.X, test.Y] == 0x02;

			return facingDoor;
		}


		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			throw new NotImplementedException();
		}

		public override void SetColorScheme(ColorScheme scheme)
		{
			scheme.TextColor = XleColor.White;

			scheme.FrameColor = XleColor.DarkGray;
			scheme.FrameHighlightColor = XleColor.Yellow;

			scheme.MapAreaWidth = 23;
		}
		
		public MuseumExtender()
		{
		}

		public override void OnLoad(GameState state)
		{
			base.OnLoad(state);

			CheckExhibitStatus(state);
		}
		public virtual Exhibit GetExhibitByTile(int tile)
		{
			throw new NotImplementedException();
		}

		public virtual void CheckExhibitStatus(GameState state)
		{
		}
		public virtual void NeedsCoinMessage(Player player, Exhibit ex)
		{
		}
		public virtual void PrintUseCoinMessage(Player player, Exhibit ex)
		{
		}

		public override bool PlayerXamine(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			if (InteractWithDisplay(state))
				return true;

			XleCore.TextArea.PrintLine("You are in an ancient museum.");

			return true;
		}
		public override bool PlayerFight(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			
			Point lookingAt = state.Player.Location;
			lookingAt.X += state.Player.FaceDirection.StepDirection().X;
			lookingAt.Y += state.Player.FaceDirection.StepDirection().Y;

			if (ExhibitAt(state.Player.Location) != null)
			{
				PrintExhibitStopsActionMessage();
			}
			else if (TheMap[lookingAt] == doorVal)
			{
				SoundMan.PlaySound(LotaSound.PlayerHit);

				XleCore.TextArea.PrintLine("The door does not budge.");
			}
			else
				XleCore.TextArea.PrintLine("There is nothing to fight.");

			return true;
		}

		private static void PrintExhibitStopsActionMessage()
		{
			XleCore.TextArea.PrintLine("The display case");
			XleCore.TextArea.PrintLine("force field stops you.");
		}
		public override bool PlayerRob(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			if (ExhibitAt(state.Player.Location) != null)
			{
				PrintExhibitStopsActionMessage();
			}
			else
			{
				XleCore.TextArea.PrintLine("There is nothing to rob.");
			}

			return true;
		}
		protected override bool PlayerSpeakImpl(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("There is no reply.");

			return true;
		}
		public override bool PlayerTake(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("There is nothing to take.");

			return true;
		}

		public Exhibit ExhibitAt(Point location)
		{
			return ExhibitAt(location.X, location.Y);
		}
		public Exhibit ExhibitAt(int x, int y)
		{
			int tileAt = TheMap[x, y];

			return GetExhibitByTile(tileAt);
		}

		protected bool InteractWithDisplay(GameState state)
		{
			var player = state.Player;
			Point stepDir = player.FaceDirection.StepDirection();

			Exhibit ex = ExhibitAt(player.X + stepDir.X, player.Y + stepDir.Y);

			if (ex == null)
				return false;

			MapRenderer.DrawCloseup = true;
			MapRenderer.mCloseup = ex;
			MapRenderer.mDrawStatic = ex.StaticBeforeCoin;

			XleCore.TextArea.PrintLine(ex.IntroductionText);
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLineCentered(ex.LongName + " ", ex.TitleColor);

			XleCore.PromptToContinueOnWait = true;

			if (ex.IsClosed(player))
			{
				XleCore.TextArea.PrintLineCentered(" - Exhibit closed - ", ex.TitleColor);
				XleCore.TextArea.PrintLine();
				XleCore.WaitForKey();

				return true;
			}

			XleCore.TextArea.PrintLineCentered(ex.InsertCoinText + " ", ex.TitleColor);
			XleCore.TextArea.PrintLine();
			XleCore.WaitForKey();

			if (ex.RequiresCoin(player) == false)
			{
				MapRenderer.mDrawStatic = false;
				RunExhibit(state, ex);
			}
			else
			{
				if (ex.HasBeenVisited(player) == false)
					XleCore.TextArea.PrintLine("You haven't used this exhibit.");
				else
					XleCore.TextArea.PrintLine();

				if (XleCore.Options.DisableExhibitsRequireCoins == false && ex.PlayerHasCoin(player) == false)
				{
					NeedsCoinMessage(player, ex);
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

					if (XleCore.Options.DisableExhibitsRequireCoins == false)
						ex.UseCoin(player);

					MapRenderer.mDrawStatic = false;
					RunExhibit(state, ex);
				}
			}

			return true;
		}

		private void RunExhibit(GameState state, Exhibit ex)
		{
			ex.RunExhibit(state.Player);

			CheckExhibitStatus(state);
		}


	}
}
