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

		public new DungeonExtender Extender { get; private set; }

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

		private DungeonMonster MonsterAt(int dungeonLevel, Point loc)
		{
			return Monsters.FirstOrDefault(m => m.DungeonLevel == dungeonLevel && m.Location == loc);
		}

		protected override void PlayerMagicImpl(GameState state, MagicSpell magic)
		{
			throw new NotImplementedException();
			//Extender.PlayerMagicImpl(state, magic);
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
			Extender.DrawMonsters(x, y, faceDirection, inRect, maxDistance);
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
