using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using AgateLib.DisplayLib;

namespace ERY.Xle.Maps.XleMapTypes
{
	using Extenders;

	public class Dungeon : Map3D
	{
		int mWidth;
		int mHeight;
		int mLevels = 1;

		int[] mData;
		int mCurrentLevel;

		public Dungeon()
		{
			Monsters = new List<DungeonMonster>();
		}

		IDungeonExtender Extender;

		public List<DungeonMonster> Monsters { get; set; }

		public override IEnumerable<string> AvailableTileImages
		{
			get { yield return "DungeonTiles.png"; }
		}

		protected override void ReadData(XleSerializationInfo info)
		{
			mWidth = info.ReadInt32("Width");
			mHeight = info.ReadInt32("Height");
			mLevels = info.ReadInt32("Levels");
			mData = info.ReadInt32Array("Data");
			MaxMonsters = info.ReadInt32("MaxMonsters");
			MonsterHealthScale = info.ReadInt32("MonsterHealthScale");
			MonsterDamageScale = info.ReadInt32("MonsterDamageScale");
		}
		protected override void WriteData(XleSerializationInfo info)
		{
			info.Write("Width", mWidth, true);
			info.Write("Height", mHeight, true);
			info.Write("Levels", mLevels, true);
			info.Write("Data", mData, NumericEncoding.Csv);
			info.Write("MaxMonsters", MaxMonsters);
			info.Write("MonsterHealthScale", MonsterHealthScale);
			info.Write("MonsterDamageScale", MonsterDamageScale);
		}

		protected override IMapExtender CreateExtenderImpl()
		{
			if (XleCore.Factory == null)
			{
				Extender = new NullDungeonExtender();
			}
			else
			{
				Extender = XleCore.Factory.CreateMapExtender(this);
			}

			return Extender;
		}

		public override bool IsMultiLevelMap
		{
			get { return true; }
		}
		public override void InitializeMap(int width, int height)
		{
			mWidth = width;
			mHeight = height;

			mData = new int[mLevels * mHeight * mWidth];
		}
		public override void SetLevels(int count)
		{
			int[] newData = new int[count * Height * Width];
			int levelSize = Height * Width;

			for (int i = 0; i < Levels; i++)
			{
				for (int x = 0; x < Width; x++)
				{
					for (int y = 0; y < Height; y++)
					{
						int newIndex = i * levelSize + y * Width + x;
						int oldIndex = i * levelSize + y * Width + x;

						if (newIndex >= newData.Length ||
							oldIndex >= mData.Length)
						{
							goto done;
						}
						newData[newIndex] = mData[oldIndex];
					}
				}
			}

		done:
			mData = newData;
			mLevels = count;
		}
		public override int Height
		{
			get { return mHeight; }
		}
		public override int Width
		{
			get { return mWidth; }
		}
		public override int Levels
		{
			get
			{
				return mLevels;
			}
		}

		public int MaxMonsters { get; set; }
		public int MonsterHealthScale { get; set; }
		public int MonsterDamageScale { get; set; }

		public int CurrentLevel
		{
			get { return mCurrentLevel; }
			set { mCurrentLevel = value; }
		}


		public override void CheckSounds(Player player)
		{
			Extender.CheckSounds(XleCore.GameState);
		}

		public override Color DefaultColor
		{
			get
			{
				return XleColor.Cyan;
			}
		}
		public override int this[int xx, int yy]
		{
			get { return this[xx, yy, mCurrentLevel]; }
			set { this[xx, yy, mCurrentLevel] = value; }
		}
		public int this[int xx, int yy, int dungeonLevel]
		{
			get
			{
				if (yy < 0 || yy >= Height || xx < 0 || xx >= Width)
				{
					return 0;
				}
				else
				{
					return mData[dungeonLevel * Height * Width + yy * Width + xx];
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
					mData[dungeonLevel * Height * Width + yy * Width + xx] = value;
				}
			}
		}
		public override bool PlayerClimb(Player player)
		{
			switch (this[player.X, player.Y])
			{
				case 0x11:
					if (player.DungeonLevel == 0)
					{
						XleCore.TextArea.PrintLine("\n\nYou climb out of the dungeon.");

						Extender.OnPlayerExitDungeon(player);

						XleCore.Wait(1000);

						// TODO: fix this
						player.ReturnToPreviousMap();

						return true;
					}
					else
					{
						player.DungeonLevel--;
					}
					break;

				case 0x12:
					player.DungeonLevel++;
					break;

				default:
					return false;

			}

			DungeonLevelText(player);

			return true;
		}

		private void DungeonLevelText(Player player)
		{
			mCurrentLevel = player.DungeonLevel;

			if (this[player.X, player.Y] == 0x21) this[player.X, player.Y] = 0x11;
			if (this[player.X, player.Y] == 0x22) this[player.X, player.Y] = 0x12;

			XleCore.TextArea.PrintLine("\n\nYou are now at level " + (player.DungeonLevel + 1).ToString() + ".", XleColor.White);
		}

		protected override bool ShowDirections(Player player)
		{
			return Extender.ShowDirection(player);
		}
		public override void OnLoad(Player player)
		{
			base.OnLoad(player);

			Extender.OnLoad(player);

			CurrentLevel = player.DungeonLevel;
		}


		public override bool PlayerFight(Player player)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			int distance = 0;
			int maxDistance = 1;
			if (XleCore.Data.WeaponList[player.CurrentWeaponType].Ranged)
				maxDistance = 5;

			DungeonMonster monst = MonsterInFrontOfPlayer(player, ref distance);

			if (monst == null)
			{
				XleCore.TextArea.PrintLine("Nothing to fight.");
				return true;
			}
			else if (distance > maxDistance)
			{
				XleCore.TextArea.PrintLine("The " + monst.Name + " is out-of-range");
				XleCore.TextArea.PrintLine("of your " + player.CurrentWeaponTypeName + ".");
				return true;
			}

			bool hit = Extender.RollToHitMonster(XleCore.GameState, monst);

			XleCore.TextArea.Print("Hit ");
			XleCore.TextArea.Print(monst.Name, XleColor.White);
			XleCore.TextArea.PrintLine(" with " + player.CurrentWeaponTypeName);

			if (hit)
			{
				int damage = Extender.RollDamageToMonster(XleCore.GameState, monst);

				SoundMan.PlaySound(LotaSound.PlayerHit);

				HitMonster(monst, damage, XleColor.Cyan);
			}
			else
			{
				SoundMan.PlaySound(LotaSound.PlayerMiss);
				XleCore.TextArea.PrintLine("Your attack misses.");
				XleCore.Wait(500);
			}

			return true;
		}

		private void HitMonster(DungeonMonster monst, int damage, Color clr)
		{
			XleCore.TextArea.Print("Enemy hit by blow of ", clr);
			XleCore.TextArea.Print(damage.ToString(), XleColor.White);
			XleCore.TextArea.PrintLine("!");

			monst.HP -= damage;
			XleCore.Wait(1000);

			if (monst.HP <= 0)
			{
				Monsters.Remove(monst);
				XleCore.TextArea.PrintLine(monst.Name + " dies!!");

				SoundMan.PlaySound(LotaSound.EnemyDie);

				XleCore.Wait(500);
			}
		}

		public DungeonMonster MonsterInFrontOfPlayer(Player player)
		{
			int distance = 0;
			return MonsterInFrontOfPlayer(player, ref distance);
		}
		public DungeonMonster MonsterInFrontOfPlayer(Player player, ref int distance)
		{
			Point fightDir = StepDirection(player.FaceDirection);
			DungeonMonster monst = null;

			for (int i = 1; i <= 5; i++)
			{
				Point loc = new Point(player.X + fightDir.X * i, player.Y + fightDir.Y * i);

				distance = i;
				monst = MonsterAt(player.DungeonLevel, loc);

				if (monst != null)
					break;
				if (CanPlayerStepIntoImpl(player, loc.X, loc.Y) == false)
					break;
			}

			return monst;
		}
		protected override void PlayerMagicImpl(GameState state, MagicSpell magic)
		{
			switch (magic.ID)
			{
				case 1:
				case 2:
					UseAttackMagic(state, magic);
					break;

				default:
					Extender.CastSpell(state, magic);
					break;
			}
		}
		private void UseAttackMagic(GameState state, MagicSpell magic)
		{
			int distance = 0;
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Shoot " + magic.Name + ".", XleColor.White);

			DungeonMonster monst = MonsterInFrontOfPlayer(state.Player, ref distance);
			var magicSound = magic.ID == 1 ? LotaSound.MagicFlame : LotaSound.MagicBolt;
			var hitSound = magic.ID == 1 ? LotaSound.MagicFlameHit : LotaSound.MagicBoltHit;

			if (monst == null)
			{
				SoundMan.PlayMagicSound(magicSound, hitSound, distance);
				XleCore.TextArea.PrintLine("There is no effect.", XleColor.White);
			}
			else
			{
				if (Extender.RollSpellFizzle(state, magic))
				{
					SoundMan.PlayMagicSound(magicSound, LotaSound.MagicFizzle, distance);
					XleCore.TextArea.PrintLine("Attack fizzles.", XleColor.White);
					XleCore.Wait(500);
				}
				else
				{
					SoundMan.PlayMagicSound(magicSound, hitSound, distance);
					int damage = Extender.RollSpellDamage(state, magic, distance);

					HitMonster(monst, damage, XleColor.White);
				}
			}
		}

		private DungeonMonster MonsterAt(int dungeonLevel, Point loc)
		{
			return Monsters.FirstOrDefault(m => m.DungeonLevel == dungeonLevel && m.Location == loc);
		}

		public override bool PlayerXamine(Player player)
		{
			SoundMan.PlaySound(LotaSound.Xamine);
			XleCore.Wait(500);

			Point faceDir = new Point();

			switch (player.FaceDirection)
			{
				case Direction.East: faceDir = new Point(1, 0); break;
				case Direction.West: faceDir = new Point(-1, 0); break;
				case Direction.North: faceDir = new Point(0, -1); break;
				case Direction.South: faceDir = new Point(0, 1); break;
				default: break;
			}

			XleCore.TextArea.PrintLine("\n");

			bool revealHidden = false;
			DungeonMonster foundMonster = null;

			for (int i = 0; i < 5; i++)
			{
				Point loc = new Point(player.X + faceDir.X * i, player.Y + faceDir.Y * i);

				foundMonster = MonsterAt(player.DungeonLevel, loc);

				if (foundMonster != null)
					break;
				if (this[loc.X, loc.Y] < 0x10)
					break;
				if (this[loc.X, loc.Y] >= 0x21 && this[loc.X, loc.Y] < 0x2a)
				{
					this[loc.X, loc.Y] -= 0x10;
					revealHidden = true;
				}
			}

			if (revealHidden)
			{
				XleCore.TextArea.PrintLine("Hidden objects detected!!!", XleColor.White);
				SoundMan.PlaySound(LotaSound.XamineDetected);
			}

			string extraText = string.Empty;
			int distance = 0;

			if (foundMonster != null)
			{
				bool handled = false;

				Extender.PrintExamineMonsterMessage(foundMonster, ref handled);

				if (false == handled)
				{
					string name = " " + foundMonster.Name;
					if ("aeiou".Contains(foundMonster.Name[0]))
						name = "n" + name;

					XleCore.TextArea.PrintLine("A" + name + " is stalking you!", XleColor.White);
				}
			}
			else
			{
				for (int i = 0; i < 5; i++)
				{
					Point loc = new Point(player.X + faceDir.X * i, player.Y + faceDir.Y * i);
					int val = this[loc.X, loc.Y];

					if (val < 0x10) break;

					if (extraText == string.Empty)
					{
						distance = i;

						if (val > 0x10 && val < 0x1a)
						{
							extraText = TrapName(val);
						}
						if (val >= 0x30 && val <= 0x3f)
						{
							extraText = "treasure chest";
						}
						if (val == 0x1e)
						{
							extraText = "box";
						}
					}
				}

				if (extraText != string.Empty)
				{
					if (distance > 0)
					{
						XleCore.TextArea.PrintLine("A " + extraText + " is in sight.");
					}
					else
					{
						XleCore.TextArea.PrintLine("You are standing next ");
						XleCore.TextArea.PrintLine("to a " + extraText + ".");
					}
				}
				else
				{
					if (Extender.PrintLevelDuringXamine)
					{
						XleCore.TextArea.PrintLine("Level " + (player.DungeonLevel + 1).ToString() + ".");
					}

					XleCore.TextArea.PrintLine("Nothing unusual in sight.");
				}
			}

			return true;
		}
		public override bool PlayerOpen(Player player)
		{
			int val = this[player.X, player.Y];
			bool clearBox = true;

			if (val == 0x1e)
			{
				OpenBox(player, ref clearBox);

				if (clearBox)
					this[player.X, player.Y] = 0x10;
			}
			else if (val >= 0x30 && val <= 0x3f)
			{
				OpenChest(player, val, ref clearBox);

				if (clearBox)
					this[player.X, player.Y] = 0x10;
			}
			else
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Nothing to open.");
				XleCore.Wait(1000);
			}


			return true;
		}


		protected override bool PlayerSpeakImpl(Player player)
		{
			bool handled = false;

			Extender.PlayerSpeak(XleCore.GameState, ref handled);

			return handled;
		}
		private void OpenBox(Player player, ref bool clearBox)
		{
			int amount = XleCore.random.Next(60, 200);

			XleCore.TextArea.PrintLine(" Box");
			XleCore.TextArea.PrintLine();
			SoundMan.PlaySound(LotaSound.OpenChest);
			XleCore.Wait(500);

			if (amount + player.HP > player.MaxHP)
			{
				amount = player.MaxHP - player.HP;
				if (amount < 0)
					amount = 0;
			}

			bool handled = false;

			Extender.OnBeforeOpenBox(player, ref handled);

			if (handled == false)
			{
				if (amount == 0)
					XleCore.TextArea.PrintLine("You find nothing.", Color.Yellow);
				else
				{
					XleCore.TextArea.PrintLine("Hit points:  + " + amount.ToString(), XleColor.Yellow);
					player.HP += amount;
					SoundMan.PlaySound(LotaSound.Good);
					XleCore.FlashHPWhileSound(XleColor.Yellow);
				}
			}

			SoundMan.FinishSounds();
		}
		private void OpenChest(Player player, int val, ref bool clearBox)
		{
			val -= 0x30;

			XleCore.TextArea.PrintLine(" Chest");
			XleCore.TextArea.PrintLine();

			SoundMan.PlaySound(LotaSound.OpenChest);
			XleCore.Wait(XleCore.GameState.GameSpeed.DungeonOpenChestSoundTime);

			// TODO: give weapons
			// TODO: bobby trap chests.

			if (val == 0)
			{
				int amount = XleCore.random.Next(90, 300);

				XleCore.TextArea.PrintLine("You find " + amount.ToString() + " gold.", XleColor.Yellow);

				player.Gold += amount;

				XleCore.FlashHPWhileSound(XleColor.Yellow);
			}
			else
			{
				int treasure = Extender.GetTreasure(XleCore.GameState, CurrentLevel + 1, val);

				bool handled = false;

				Extender.OnBeforeGiveItem(player, ref treasure, ref handled, ref clearBox);

				if (handled == false)
				{
					if (treasure > 0)
					{
						string text = "You find a " + XleCore.Data.ItemList[treasure].LongName + "!!";
						XleCore.TextArea.Clear();
						XleCore.TextArea.PrintLine(text);

						player.Items[treasure] += 1;

						SoundMan.PlaySound(LotaSound.VeryGood);

						XleCore.TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood),
							XleColor.White, XleColor.Yellow, 100);
					}
					else
					{
						XleCore.TextArea.PrintLine("You find nothing.");
					}
				}
			}
		}

		protected override void AfterExecuteCommandImpl(Player player, KeyCode cmd)
		{
			XleCore.Wait(100);
			UpdateMonsters(XleCore.GameState);
		}
		protected override void AfterStepImpl(Player player, bool didEvent)
		{
			int val = this[player.X, player.Y];

			CurrentLevel = player.DungeonLevel;

			if (val >= 0x21 && val <= 0x2a)
			{
				OnPlayerTriggerTrap(player, player.X, player.Y);
			}
			else if (val >= 0x11 && val <= 0x1a)
			{
				OnPlayerAvoidTrap(player, player.X, player.Y);
			}

			base.AfterStepImpl(player, didEvent);
		}

		private void UpdateMonsters(GameState state)
		{
			bool handled = false;
			Extender.UpdateMonsters(state, ref handled);
			if (handled)
				return;

			foreach (var monster in Monsters.Where(x => x.DungeonLevel == state.Player.DungeonLevel))
			{
				var delta = new Point(
					state.Player.X - monster.Location.X,
					state.Player.Y - monster.Location.Y);

				if (Math.Abs(delta.X) + Math.Abs(delta.Y) == 1)
				{
					MonsterAttackPlayer(state, monster);
				}
				else
				{
					if (Math.Abs(delta.X) > Math.Abs(delta.Y))
					{
						if (false == TryMonsterStep(monster, new Point(delta.X, 0)))
						{
							TryMonsterStep(monster, new Point(0, delta.Y));
						}
					}
					else
					{
						if (false == TryMonsterStep(monster, new Point(0, delta.Y)))
							TryMonsterStep(monster, new Point(delta.X, 0));
					}
				}
			}

			if (Extender.SpawnMonsters(state) &&
				Monsters.Count(monst => monst.DungeonLevel == state.Player.DungeonLevel) < MaxMonsters)
			{
				SpawnMonster(state);
			}
		}

		private void MonsterAttackPlayer(GameState state, DungeonMonster monster)
		{
			XleCore.TextArea.PrintLine();

			var delta = new Point(
				monster.Location.X - state.Player.X,
				monster.Location.Y - state.Player.Y);

			var forward = StepDirection(state.Player.FaceDirection);
			var right = RightDirection(state.Player.FaceDirection);
			var left = LeftDirection(state.Player.FaceDirection);
			bool allowEffect = false;

			if (delta == forward)
			{
				XleCore.TextArea.Print("Attacked by ");
				XleCore.TextArea.Print(monster.Name, XleColor.Yellow);
				XleCore.TextArea.PrintLine("!");

				allowEffect = true;
			}
			else if (delta == right)
			{
				XleCore.TextArea.Print("Attacked from the ");
				XleCore.TextArea.Print("right", XleColor.Yellow);
				XleCore.TextArea.PrintLine(".");
			}
			else if (delta == left)
			{
				XleCore.TextArea.Print("Attacked from the ");
				XleCore.TextArea.Print("left", XleColor.Yellow);
				XleCore.TextArea.PrintLine(".");
			}
			else
			{
				XleCore.TextArea.Print("Attacked from ");
				XleCore.TextArea.Print("behind", XleColor.Yellow);
				XleCore.TextArea.PrintLine(".");
			}

			if (Extender.RollToHitPlayer(state, monster))
			{
				int damage = Extender.RollDamageToPlayer(state, monster);

				SoundMan.PlaySound(LotaSound.EnemyHit);
				XleCore.TextArea.Print("Hit by blow of ");
				XleCore.TextArea.Print(damage.ToString(), XleColor.Yellow);
				XleCore.TextArea.PrintLine("!");

				state.Player.HP -= damage;
			}
			else
			{
				SoundMan.PlaySound(LotaSound.EnemyMiss);
				XleCore.TextArea.PrintLine("Attack missed.", XleColor.Green);
			}

			XleCore.Wait(250);
		}

		private bool TryMonsterStep(DungeonMonster monster, Point delta)
		{
			if (delta.X != 0 && delta.Y != 0) throw new ArgumentOutOfRangeException();
			if (delta.X == 0 && delta.Y == 0) return false;

			if (delta.X != 0) delta.X /= Math.Abs(delta.X);
			if (delta.Y != 0) delta.Y /= Math.Abs(delta.Y);

			if (CanMonsterStepInto(monster, new Point(monster.Location.X + delta.X, monster.Location.Y + delta.Y)))
			{
				monster.Location = new Point(
					monster.Location.X + delta.X,
					monster.Location.Y + delta.Y);

				return true;
			}

			return false;
		}

		private bool CanMonsterStepInto(DungeonMonster monster, Point newPt)
		{
			if (IsMapSpaceBlocked(newPt.X, newPt.Y))
				return false;

			if (IsSpaceOccupiedByMonster(XleCore.GameState.Player, newPt.X, newPt.Y))
				return false;

			return true;
		}

		private void SpawnMonster(Xle.GameState state)
		{
			DungeonMonster monster = Extender.GetMonsterToSpawn(state);

			if (monster == null)
				return;

			do
			{
				monster.Location = new Point(
					XleCore.random.Next(1, 15),
					XleCore.random.Next(1, 15));

			} while (this.CanPlayerStepIntoImpl(state.Player, monster.Location.X, monster.Location.Y) == false || monster.Location == state.Player.Location);

			monster.DungeonLevel = state.Player.DungeonLevel;

			Monsters.Add(monster);
		}

		protected override void DrawMonsters(int x, int y, Direction faceDirection, Rectangle inRect, int maxDistance)
		{
			Point stepDir = StepDirection(faceDirection);

			for (int distance = 1; distance <= maxDistance; distance++)
			{
				Point loc = new Point(x + stepDir.X * distance, y + stepDir.Y * distance);

				var monster = MonsterAt(XleCore.GameState.Player.DungeonLevel, loc);

				if (monster == null)
					continue;

				var data = XleCore.Data.DungeonMonsters[monster.MonsterID];
				int image = distance - 1;
				var imageInfo = data.Images[image];

				var drawPoint = imageInfo.DrawPoint;
				drawPoint.X += inRect.X;
				drawPoint.Y += inRect.Y;

				var srcRect = imageInfo.SourceRects[0];

				data.Surface.Draw(srcRect, drawPoint);

				break;
			}
		}

		protected override bool IsSpaceOccupiedByMonster(Player player, int xx, int yy)
		{
			return MonsterAt(player.DungeonLevel, new Point(xx, yy)) != null;
		}

		private void OnPlayerAvoidTrap(Player player, int x, int y)
		{
			// don't print a message for ceiling holes
			if (this[x, y] == 0x21) return;

			//string name = TrapName(this[x, y]);

			//XleCore.TextArea.PrintLine();
			//XleCore.TextArea.PrintLine("You avoid the " + name + ".");
			//XleCore.wait(150);
		}
		private void OnPlayerTriggerTrap(Player player, int x, int y)
		{
			// don't trigger ceiling holes
			if (this[x, y] == 0x21) return;

			this[x, y] -= 0x10;
			int damage = 31;
			XleCore.TextArea.PrintLine();

			if (this[x, y] == 0x12)
			{
				XleCore.TextArea.PrintLine("You fall through a hidden hole.", XleColor.White);
			}
			else
			{
				XleCore.TextArea.PrintLine("You're ambushed by a " + TrapName(this[x, y]) + ".", XleColor.White);
				XleCore.Wait(100);
			}

			XleCore.TextArea.PrintLine("   H.P. - " + damage.ToString(), XleColor.White);
			player.HP -= damage;

			SoundMan.PlaySound(LotaSound.EnemyHit);
			XleCore.Wait(500);

			if (this[x, y] == 0x12)
			{
				player.DungeonLevel++;
				DungeonLevelText(player);
			}
		}

		protected override Map3D.ExtraType GetExtraType(int val, int side)
		{
			if (side != 0)
				return ExtraType.None;

			ExtraType extraType = ExtraType.None;

			switch (val)
			{
				case 0x11:
					extraType = ExtraType.GoUp;
					break;
				case 0x12:
					extraType = ExtraType.GoDown;
					break;
				case 0x13:
					extraType = ExtraType.Needle;
					break;
				case 0x14:
					extraType = ExtraType.Slime;
					break;
				case 0x15:
					extraType = ExtraType.TripWire;
					break;
				case 0x1e:
					extraType = ExtraType.Box;
					break;
				case 0x30:
				case 0x31:
				case 0x32:
				case 0x33:
				case 0x34:
				case 0x35:
				case 0x36:
				case 0x37:
				case 0x38:
				case 0x39:
				case 0x3a:
				case 0x3b:
				case 0x3c:
				case 0x3d:
				case 0x3e:
				case 0x3f:
					extraType = ExtraType.Chest;
					break;

			}
			return extraType;
		}
		string TrapName(int val)
		{
			return Extender.TrapName(val);
		}

		protected override void PlayPlayerMoveSound()
		{
			SoundMan.PlaySound(LotaSound.WalkDungeon);
		}


		public void ExecuteKillFlash(Xle.GameState state)
		{
			SoundMan.PlaySoundSync(LotaSound.VeryBad);
			
			Monsters.RemoveAll(monst => monst.KillFlashImmune == false);
		}

		
	}
}
