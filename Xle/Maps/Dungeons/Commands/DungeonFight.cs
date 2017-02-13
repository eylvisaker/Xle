using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;

using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;

namespace ERY.Xle.Maps.Dungeons.Commands
{
	[ServiceName("DungeonFight")]
	public class DungeonFight : Fight
	{
		DungeonExtender map
		{
			get { return (DungeonExtender)GameState.MapExtender; }
		}

		DungeonMonster MonsterInFrontOfPlayer(Player player, ref int distance)
		{
			return map.MonsterInFrontOfPlayer(player, ref distance);
		}

		DungeonCombat Combat { get { return map.Combat; } }

		public override void Execute()
		{
			TextArea.PrintLine();
			TextArea.PrintLine();

			int distance = 0;
			int maxDistance = 1;
			if (Player.CurrentWeapon.Info(Data).Ranged)
				maxDistance = 5;

			DungeonMonster monst = MonsterInFrontOfPlayer(Player, ref distance);

			if (monst == null)
			{
				TextArea.PrintLine("Nothing to fight.");
				return;
			}
			else if (distance > maxDistance)
			{
				TextArea.PrintLine("The " + monst.Name + " is out-of-range");
				TextArea.PrintLine("of your " + Player.CurrentWeapon.BaseName(Data) + ".");
				return;
			}

			bool hit = RollToHitMonster(monst);

			TextArea.Print("Hit ");
			TextArea.Print(monst.Name, XleColor.White);
			TextArea.PrintLine(" with " + Player.CurrentWeapon.BaseName(Data));

			if (hit)
			{
				int damage = RollDamageToMonster(monst);

				SoundMan.PlaySound(LotaSound.PlayerHit);

				HitMonster(monst, damage, XleColor.Cyan);
			}
			else
			{
				SoundMan.PlaySound(LotaSound.PlayerMiss);
				TextArea.PrintLine("Your attack misses.");
				GameControl.Wait(500);
			}

			return;
		}

		private void HitMonster(DungeonMonster monst, int damage, Color clr)
		{
			TextArea.Print("Enemy hit by blow of ", clr);
			TextArea.Print(damage.ToString(), XleColor.White);
			TextArea.PrintLine("!");

			monst.HP -= damage;
			GameControl.Wait(1000);

			if (monst.HP <= 0)
			{
				Combat.Monsters.Remove(monst);
				TextArea.PrintLine(monst.Name + " dies!!");

				SoundMan.PlaySound(LotaSound.EnemyDie);

				GameControl.Wait(500);
			}
		}

		protected virtual bool RollToHitMonster(DungeonMonster monster)
		{
			return true;
		}

		protected virtual int RollDamageToMonster(DungeonMonster monster)
		{
			return 9999;
		}
	}
}
