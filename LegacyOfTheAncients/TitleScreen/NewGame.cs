using AgateLib.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.TitleScreen
{
	class NewGame : TitleState
	{
		string tempName;
		Player player;
		string[] wnd;

		public override void KeyDown(KeyCode keyCode, string keyString)
		{
			if ((keyCode >= KeyCode.A && keyCode <= KeyCode.Z) || keyCode == KeyCode.Space ||
				(keyCode >= KeyCode.D0 && keyCode <= KeyCode.D9))
			{

				if (tempName.Length < 14)
				{
					tempName += keyString;
				}

			}
			else if (keyCode == KeyCode.BackSpace || keyCode == KeyCode.Delete)
			{
				if (tempName.Length > 0)
					tempName = tempName.Substring(0, tempName.Length - 1);
			}
			else if (keyCode == KeyCode.Escape || keyCode == KeyCode.F1)
			{
				NewState = new SecondMainMenu();
			}
			else if (keyCode == KeyCode.Enter && tempName.Length > 0)
			{
				if (System.IO.File.Exists(@"Saved\" + tempName + ".chr"))
				{
					SoundMan.PlaySound(LotaSound.Medium);

					for (int i = 13; i < 25; i++)
					{
						wnd[i] = "";
					}

					wnd[16] = "    " + tempName + " has already begun.";

					Wait(2000);
				}
				else
				{
					for (int i = 13; i < 25; i++)
					{
						wnd[i] = "";
					}

					wnd[16] = "    " + tempName + "'s adventures begin";

					SoundMan.PlaySoundSync(LotaSound.VeryGood);

					player = new Player(tempName);
					player.MapID = 5;
					player.Location = new Point(3, 1);
					player.FaceDirection = Direction.West;

					player.Items[LotaItem.GoldArmband] = 1;
					player.Items[LotaItem.Compendium] = 1;
					player.Items[LotaItem.JadeCoin] = 2;

					player.AddArmor(1, 0);
					player.CurrentArmorIndex = 1;

					player.StoryData = new LotaStory();
					player.SavePlayer();
				}
			}
		}


		protected override void DrawFrame()
		{
			throw new NotImplementedException();
		}
	}
}
