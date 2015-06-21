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

using ERY.Xle.Services.Implementation;

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

		public int CurrentLevel { get; set; }

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

		public override void OnLoad(Player player)
		{
			base.OnLoad(player);
		}

		public DungeonMonster MonsterAt(int dungeonLevel, Point loc)
		{
			return Monsters.FirstOrDefault(m => m.DungeonLevel == dungeonLevel && m.Location == loc);
		}

	}
}
