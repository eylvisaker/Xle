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

namespace ERY.Xle.XleMapTypes
{
	using Extenders;

	public class Dungeon : Map3D
	{
		int mWidth;
		int mHeight;
		int mLevels = 1;

		int[] mData;
		int[] mTreasures = new int[16];
		int mCurrentLevel;

		public int Treasure1 { get { return mTreasures[1]; } set { mTreasures[1] = value; } }
		public int Treasure2 { get { return mTreasures[2]; } set { mTreasures[2] = value; } }
		public int Treasure3 { get { return mTreasures[3]; } set { mTreasures[3] = value; } }
		public int Treasure4 { get { return mTreasures[4]; } set { mTreasures[4] = value; } }
		public int Treasure5 { get { return mTreasures[5]; } set { mTreasures[5] = value; } }
		public int Treasure6 { get { return mTreasures[6]; } set { mTreasures[6] = value; } }
		public int Treasure7 { get { return mTreasures[7]; } set { mTreasures[7] = value; } }
		public int Treasure8 { get { return mTreasures[8]; } set { mTreasures[8] = value; } }

		public string ScriptClassName { get; set; }

		IDungeonExtender Extender;

		public override IEnumerable<string> AvailableTilesets
		{
			get { yield return "DungeonTiles.png"; }
		}

		protected override void ReadData(XleSerializationInfo info)
		{
			mWidth = info.ReadInt32("Width");
			mHeight = info.ReadInt32("Height");
			mLevels = info.ReadInt32("Levels");
			mData = info.ReadInt32Array("Data");
			ScriptClassName = info.ReadString("ScriptClass", "");

			if (info.ContainsKey("Treasures"))
				mTreasures = info.ReadInt32Array("Treasures");

			Extender = CreateExtender<IDungeonExtender>(ScriptClassName);
		}

		protected override void WriteData(XleSerializationInfo info)
		{
			info.Write("Width", mWidth, true);
			info.Write("Height", mHeight, true);
			info.Write("Levels", mLevels, true);
			info.Write("ScriptClass", ScriptClassName);
			info.Write("Data", mData);
			info.Write("Treasures", mTreasures);
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

		public int CurrentLevel
		{
			get { return mCurrentLevel; }
			set { mCurrentLevel = value; }
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
			get
			{
				if (yy < 0 || yy >= Height || xx < 0 || xx >= Width)
				{
					return 0;
				}
				else
				{
					return mData[mCurrentLevel * Height * Width + yy * Width + xx];
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
					mData[mCurrentLevel * Height * Width + yy * Width + xx] = value;
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
						g.AddBottom("");
						g.AddBottom("You climb out of the dungeon.");

						if (Extender != null)
							Extender.OnPlayerExitDungeon(player);

						XleCore.wait(1000);

						// TODO: fix this
						player.ReturnToOutside();

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

			string tempstring = "You are now at level " + (player.DungeonLevel + 1).ToString() + ".";

			g.AddBottom("");
			g.AddBottom(tempstring, XleColor.White);
		}

		protected override bool ShowDirections(Player player)
		{
			// check for compass.
			return player.Item(11) > 0;
		}
		public override void OnLoad(Player player)
		{
			base.OnLoad(player);

			player.beenInDungeon = true;
			CurrentLevel = player.DungeonLevel;
		}

		public override string[] MapMenu()
		{
			List<string> retval = new List<string>();

			retval.Add("Armor");
			retval.Add("Climb");
			retval.Add("End");
			retval.Add("Fight");
			retval.Add("Gamespeed");
			retval.Add("Hold");
			retval.Add("Inventory");
			retval.Add("Magic");
			retval.Add("Open");
			retval.Add("Pass");
			retval.Add("Use");
			retval.Add("Weapon");
			retval.Add("Xamine");

			return retval.ToArray();
		}

		#region --- Drawing ---

		protected override Surface Backdrop
		{
			get { return g.DungeonBlueBackdrop; }
		}
		protected override Surface Wall
		{
			get { return g.DungeonBlueWall; }
		}
		protected override Surface SidePassages
		{
			get { return g.DungeonBlueSidePassage; }
		}
		protected override Surface Door
		{
			get { return g.MuseumDoor; }
		}

		#endregion

		public override void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine)
		{
			fontColor = XleColor.White;

			boxColor = XleColor.Gray;
			innerColor = XleColor.LightGreen;
			fontColor = XleColor.Cyan;
			vertLine = 15 * 16;

		}

		public override bool PlayerFight(Player player)
		{
			return false;
		}
		public override bool PlayerXamine(Player player)
		{
			SoundMan.PlaySound(LotaSound.Xamine);
			XleCore.wait(500);

			Point faceDir = new Point();

			switch (player.FaceDirection)
			{
				case Direction.East: faceDir = new Point(1, 0); break;
				case Direction.West: faceDir = new Point(-1, 0); break;
				case Direction.North: faceDir = new Point(0, -1); break;
				case Direction.South: faceDir = new Point(0, 1); break;
				default: break;
			}

			g.AddBottom();

			bool revealHidden = false;

			for (int i = 0; i < 5; i++)
			{
				Point loc = new Point(player.X + faceDir.X * i, player.Y + faceDir.Y * i);

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
				g.AddBottom("Hidden objects detected!!!", XleColor.White);
				SoundMan.PlaySound(LotaSound.XamineDetected);
			}

			string extraText = string.Empty;
			bool monster = false;
			int distance = 0;

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
				// check for monsters
			}

			if (monster)
			{
				g.AddBottom("A " + extraText + " is stalking you!!!");
			}
			else if (extraText != string.Empty)
			{
				if (distance > 0)
				{
					g.AddBottom("A " + extraText + " is in sight.");
				}
				else
				{
					g.AddBottom("You are standing next ");
					g.AddBottom("to a " + extraText + ".");
				}
			}
			else
			{
				g.AddBottom("Nothing unusual in sight.");
			}

			return true;
		}
		public override bool PlayerOpen(Player player)
		{
			int val = this[player.X, player.Y];

			if (val == 0x1e)
			{
				OpenBox(player);
				this[player.X, player.Y] = 0x10;
			}
			else if (val >= 0x30 && val <= 0x3f)
			{
				OpenChest(player, val);
				this[player.X, player.Y] = 0x10;
			}
			else
			{
				g.AddBottom("Nothing to open.");
				XleCore.wait(1000);
			}


			return true;
		}

		private void OpenBox(Player player)
		{
			int amount = 40 + mCurrentLevel * 20;

			Commands.UpdateCommand("Open Box");
			g.AddBottom();
			XleCore.wait(500);

			if (amount + player.HP > player.MaxHP)
			{
				amount = player.MaxHP - player.HP;
				if (amount < 0)
					amount = 0;
			}

			bool handled = false;

			if (Extender != null)
			{
				Extender.OnBeforeOpenBox(player, ref handled);
			}

			if (handled == false)
			{
				if (amount == 0)
					g.AddBottom("You find nothing.");
				else
				{
					g.AddBottom("Hit points:  + " + amount.ToString(), XleColor.Yellow);
					player.HP += amount;
				}
			}

			XleCore.wait(1000);
			while (SoundMan.IsAnyPlaying())
				XleCore.wait(10);
		}
		private void OpenChest(Player player, int val)
		{
			val -= 0x30;

			Commands.UpdateCommand("Open Chest");
			SoundMan.PlaySound(LotaSound.OpenChest);
			g.AddBottom();
			XleCore.wait(500);

			if (val == 0)
			{
				int amount = 30 + mCurrentLevel * 20;

				g.AddBottom("You find " + amount.ToString() + " gold.", XleColor.Yellow);

				player.Gold += amount;

				XleCore.wait(1000);
				SoundMan.FinishSounds();
			}
			else
			{
				int treasure = mTreasures[val];

				if (Extender != null)
					Extender.OnBeforeGiveItem(player, ref treasure);

				if (treasure > 0)
				{
					string text = "You find a " + XleCore.ItemList[treasure].LongName + "!";
					g.ClearBottom();
					g.AddBottom(text);

					player.ItemCount(treasure, 1);

					SoundMan.PlaySound(LotaSound.VeryGood);

					while (SoundMan.IsPlaying(LotaSound.VeryGood))
					{
						g.UpdateBottom(text, XleColor.Yellow);
						XleCore.wait(50);
						g.UpdateBottom(text, XleColor.White);
						XleCore.wait(50);
					}
				}
				else
				{
					g.AddBottom("You find nothing.");
				}
			}
		}

		protected override void OnPlayerEnterPosition(Player player, int x, int y)
		{
			int val = this[x, y];

			if (val >= 0x21 && val <= 0x2a)
			{
				OnPlayerTriggerTrap(player, x, y);
			}
			else if (val >= 0x11 && val <= 0x1a)
			{
				OnPlayerAvoidTrap(player, x, y);
			}
		}

		private void OnPlayerAvoidTrap(Player player, int x, int y)
		{
			// don't print a message for ceiling holes
			if (this[x, y] == 0x21) return;

			//string name = TrapName(this[x, y]);

			//g.AddBottom();
			//g.AddBottom("You avoid the " + name + ".");
			//XleCore.wait(150);
		}
		private void OnPlayerTriggerTrap(Player player, int x, int y)
		{
			// don't trigger ceiling holes
			if (this[x, y] == 0x21) return;

			this[x, y] -= 0x10;
			int damage = 31;
			g.AddBottom();

			if (this[x, y] == 0x12)
			{
				g.AddBottom("You fall through a hidden hole.", XleColor.White);
			}
			else
			{
				g.AddBottom("You're ambushed by a " + TrapName(this[x, y]) + ".", XleColor.White);
				XleCore.wait(100);
			}

			g.AddBottom("   H.P. - " + damage.ToString());
			player.HP -= damage;

			SoundMan.PlaySound(LotaSound.EnemyHit);
			XleCore.wait(500);

			if (this[x, y] == 0x12)
			{
				player.DungeonLevel++;
				DungeonLevelText(player);
			}
		}

		string TrapName(int val)
		{
			switch (val)
			{
				case 0x11: return "ceiling hole";
				case 0x12: return "floor hole";
				case 0x13: return "poison gas vent";
				case 0x14: return "slime splotch";
				case 0x15: return "trip wire";
				default: throw new ArgumentException();
			}
		}
	}
}
