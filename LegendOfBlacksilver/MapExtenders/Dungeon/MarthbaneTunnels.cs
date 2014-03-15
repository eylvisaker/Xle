using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Dungeon
{
	class MarthbaneTunnels : LobDungeonBase
	{
		DungeonMonster king;

		public override void OnLoad(GameState state)
		{
			if (Lob.Story.RescuedKing)
			{
				OpenEscapeRoute(state);
			}
			else
			{
				king = new DungeonMonster(XleCore.Data.DungeonMonsters[18])
					{
						DungeonLevel = 7,
						Location = new Point(10, 0),
						HP = 400,
					};

				TheMap.Monsters.Add(king);
			}
		}
		protected override int MonsterGroup(int dungeonLevel)
		{
			if (dungeonLevel <= 2) return 0;
			if (dungeonLevel <= 6) return 1;

			return 2;
		}

		public override DungeonMonster GetMonsterToSpawn(GameState state)
		{
			if (state.Player.DungeonLevel == 7)
				return null;

			return base.GetMonsterToSpawn(state);
		}

		public override bool SpawnMonsters(GameState state)
		{
			if (state.Player.DungeonLevel == 7)
				return false;
			else
				return true;
		}

		public override void UpdateMonsters(GameState state, ref bool handled)
		{
			// disable normal monster processing if we see the king.
			if (state.Player.DungeonLevel == 7)
				handled = true;
		}
		public override void PrintExamineMonsterMessage(DungeonMonster foundMonster, ref bool handled)
		{
			if (foundMonster.Data.Name == "king")
			{
				XleCore.TextArea.PrintLine("You see a king!", XleColor.White);
				handled = true;
			}
		}

		public override void PlayerSpeak(GameState state, ref bool handled)
		{
			if (state.Player.DungeonLevel != 7) return;
			if (king == null) return;
			if (king.HP <= 0) return;

			handled = true;

			if (Lob.Story.MarthbaneOfferedHelpToKing == false)
			{
				SoundMan.PlaySound(LotaSound.VeryGood);

				XleCore.TextArea.Clear(true);
				XleCore.TextArea.PrintLineSlow("I am king durek!!", XleColor.White);
				XleCore.TextArea.PrintLineSlow("Do you come to help me?", XleColor.White);
				XleCore.TextArea.PrintLineSlow();

				if (XleCore.QuickMenuYesNo() == 1)
				{
					DoomedMessage();
					return;
				}

				Lob.Story.MarthbaneOfferedHelpToKing = true;

				XleCore.TextArea.Clear(true);
				XleCore.TextArea.PrintLineSlow("I fear you have been caught in the", XleColor.White);
				XleCore.TextArea.PrintLineSlow("same trap that imprisons me...", XleColor.White);
				XleCore.TextArea.PrintLineSlow();
				XleCore.TextArea.PrintLineSlow("unless...", XleColor.White);

				XleCore.Wait(2000);
			}

			XleCore.TextArea.Clear(true);
			XleCore.TextArea.PrintLineSlow("Do you carry my signet ring?", XleColor.White);
			XleCore.TextArea.PrintLineSlow();

			if (XleCore.QuickMenuYesNo() == 1)
			{
				DoomedMessage();
				return;
			}

			state.Player.Items[LobItem.SignetRing] = 0;

			XleCore.TextArea.Clear(true);
			XleCore.TextArea.PrintLineSlow("In times of distress, the ring will\nreturn me to the castle!!  I fear it\ncan do nothing more than give you a\nroute of escape.", XleColor.White);

			XleCore.Wait(3000);

			XleCore.TextArea.Clear(true);
			XleCore.TextArea.PrintLineSlow("\n\n\nNoble adventurer, i am in your debt.\nMay we meet in better times.", XleColor.White);

			XleCore.Wait(3000);

			SoundMan.PlaySound(LotaSound.EnemyMiss);

			Lob.Story.RescuedKing = true;
			TheMap.Monsters.Remove(king);

			OpenEscapeRoute(state);
		}

		private void OpenEscapeRoute(GameState state)
		{
			// 11, 0, 7 change to 17
			// 13, 0, 4 change to 18

			TheMap[11, 0, 7] = 17;
			TheMap[13, 0, 4] = 18;
		}

		private static void DoomedMessage()
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Then I fear we are both doomed.", XleColor.White);
			XleCore.TextArea.PrintLine();
		}
	}
}
