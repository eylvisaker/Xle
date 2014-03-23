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

		public Dungeon()
		{
			Monsters = new List<DungeonMonster>();
		}

		DungeonExtender Extender;

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

		protected override MapExtender CreateExtenderImpl()
		{
			if (XleCore.Factory == null)
			{
				Extender = new DungeonExtender();
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

		public override void CheckSounds(Player player)
		{
			Extender.CheckSounds(XleCore.GameState);
		}
		public override bool CanPlayerStepIntoImpl(Player player, int xx, int yy)
		{
			return Extender.CanPlayerStepIntoImpl(player, xx, yy);
		}

		public int CurrentLevel { get; set; }

		public override Color DefaultColor
		{
			get
			{
				return XleColor.Cyan;
			}
		}
		public override int this[int xx, int yy]
		{
			get { return this[xx, yy, CurrentLevel]; }
			set { this[xx, yy, CurrentLevel] = value; }
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
			return Extender.PlayerClimb(XleCore.GameState);

		}
		protected override bool ShowDirections(Player player)
		{
			return Extender.ShowDirection(player);
		}
		public override void OnLoad(Player player)
		{
			base.OnLoad(player);

			Extender.OnLoad(player);
		}


		public override bool PlayerFight(Player player)
		{
			return Extender.PlayerFight(XleCore.GameState);

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

		private DungeonMonster MonsterAt(int dungeonLevel, Point loc)
		{
			return Monsters.FirstOrDefault(m => m.DungeonLevel == dungeonLevel && m.Location == loc);
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
			Extender.PlayerMagicImpl(state, magic);
		}
		

		public override bool PlayerXamine(Player player)
		{
			return Extender.PlayerXamine(XleCore.GameState);
			
		}
		public override bool PlayerOpen(Player player)
		{
			return Extender.PlayerOpen(XleCore.GameState);
		}


		protected override bool PlayerSpeakImpl(Player player)
		{
			return Extender.PlayerSpeak(XleCore.GameState);
		}
		protected override void AfterExecuteCommandImpl(Player player, KeyCode cmd)
		{
			Extender.AfterExecuteCommandImpl(XleCore.GameState);
		}
		protected override void AfterStepImpl(Player player, bool didEvent)
		{
			Extender.AfterPlayerStep(XleCore.GameState);
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

		protected override void PlayPlayerMoveSound()
		{
			Extender.PlayPlayerMoveSound();
		}

	}
}
