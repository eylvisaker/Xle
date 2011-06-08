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

namespace ERY.Xle.XleMapTypes
{
	public class Museum : Map3D
	{
		int[] mData;
		int mHeight;
		int mWidth;
		Dictionary<int, MuseumDisplays.Exhibit> mExhibits = new Dictionary<int, MuseumDisplays.Exhibit>();

		public Museum()
		{
			mExhibits.Add(0x50, new MuseumDisplays.Information());
			mExhibits.Add(0x51, new MuseumDisplays.Welcome());
			mExhibits.Add(0x52, new MuseumDisplays.Weaponry());
			mExhibits.Add(0x53, new MuseumDisplays.Thornberry());
			mExhibits.Add(0x54, new MuseumDisplays.Fountain());
			mExhibits.Add(0x55, new MuseumDisplays.PirateTreasure());
			mExhibits.Add(0x56, new MuseumDisplays.HerbOfLife());
			mExhibits.Add(0x57, new MuseumDisplays.NativeCurrency());
			mExhibits.Add(0x58, new MuseumDisplays.StonesWisdom());
			mExhibits.Add(0x59, new MuseumDisplays.Tapestry());
			mExhibits.Add(0x5A, new MuseumDisplays.LostDisplays());
			mExhibits.Add(0x5B, new MuseumDisplays.KnightsTest());
			mExhibits.Add(0x5C, new MuseumDisplays.FourJewels());
			mExhibits.Add(0x5D, new MuseumDisplays.Guardian());
			mExhibits.Add(0x5E, new MuseumDisplays.Pegasus());
			mExhibits.Add(0x5F, new MuseumDisplays.AncientArtifact());
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
			info.Write("Data", mData);
		}
		public override string[] MapMenu()
		{
			List<string> retval = new List<string>();

			retval.Add("Armor");
			retval.Add("Fight");
			retval.Add("Gamespeed");
			retval.Add("Hold");
			retval.Add("Inventory");
			retval.Add("Pass");
			retval.Add("Rob");
			retval.Add("Speak");
			retval.Add("Take");
			retval.Add("Use");
			retval.Add("Weapon");
			retval.Add("Xamine");

			return retval.ToArray();
		}
		public override IEnumerable<string> AvailableTilesets
		{
			get { yield return "tiles3d.png"; }
		}
		public override void InitializeMap(int width, int height)
		{
			mData = new int[height * width];
			mHeight = height;
			mWidth = width;
		}

		#region --- Drawing ---

		protected override Surface Backdrop
		{
			get { return g.MuseumBackdrop; }
		}
		protected override Surface Wall
		{
			get { return g.MuseumWall; }
		}
		protected override Surface SidePassages
		{
			get { return g.MuseumSidePassage; }
		}

		VertexBuffer wall_vb;
		IndexBuffer wall_ib;

		protected internal override void ConstructRenderTimeData()
		{
			List<Vertex> vertices = new List<Vertex>();
			List<int> indices = new List<int>();

			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					// museum tiles
					if (0x40 <= this[x, y] && this[x, y] <= 0x5F)
						AddWalls(vertices, indices, x - 0.5f, y - 0.5f);
				}
			}

			wall_vb = new VertexBuffer(Vertex.VertexLayout, vertices.Count);
			wall_vb.WriteVertexData(vertices.ToArray());
			wall_vb.PrimitiveType = PrimitiveType.TriangleList;

			wall_ib = new IndexBuffer(IndexBufferType.Int16, indices.Count);
			wall_ib.WriteIndices(indices.ToArray());
		}

		private void AddWalls(List<Vertex> vertices, List<int> indices, float x, float y)
		{
			AddWall(vertices, indices, x, y, new Vector3(1, 0, 0), new Vector3(0, -1, 0));
			AddWall(vertices, indices, x, y + 1, new Vector3(0, -1, 0), new Vector3(-1, 0, 0));
			AddWall(vertices, indices, x + 1, y, new Vector3(0, 1, 0), new Vector3(1, 0, 0));
			AddWall(vertices, indices, x + 1, y + 1, new Vector3(-1, 0, 0), new Vector3(0, 1, 0));
		}

		private void AddWall(List<PositionTextureNormalTangent> vertices, List<int> indices, float x, float y, Vector3 dir, Vector3 normal)
		{
			const int zscale = 1;

			Vertex[] verts = new Vertex[4];

			verts[0].Position = new Vector3(x, y, zscale);
			verts[1].Position = new Vector3(x + dir.X, y + dir.Y, zscale);
			verts[2].Position = new Vector3(x + dir.X, y + dir.Y, 0);
			verts[3].Position = new Vector3(x, y, 0);

			verts[0].Texture = new Vector2(0, 0);
			verts[1].Texture = new Vector2(1, 0);
			verts[2].Texture = new Vector2(1, 1);
			verts[3].Texture = new Vector2(0, 1);

			for (int i = 0; i < 4; i++)
			{
				verts[i].Normal = normal;
				verts[i].Tangent = dir;
			}

			int index = vertices.Count;
			vertices.AddRange(verts);

			indices.Add(index);
			indices.Add(index + 1);
			indices.Add(index + 2);

			indices.Add(index);
			indices.Add(index + 2);
			indices.Add(index + 3);

		}

		void Render(object obj)
		{
			wall_vb.DrawIndexed(wall_ib);
		}

		#endregion

		public override bool AutoDrawPlayer
		{
			get { return false; }
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
		public override void PlayerCursorMovement(Player player, Direction dir)
		{
			string command;
			Point stepDirection;

			_MoveDungeon(player, dir, true, out command, out stepDirection);

			if (stepDirection.IsEmpty == false)
			{
				if (CanPlayerStepInto(player, player.X + stepDirection.X, player.Y + stepDirection.Y) == false)
				{
					command = "Bump into wall";
					SoundMan.PlaySound(LotaSound.Bump);
				}
				//else
				//    SoundMan.PlaySound(LotaSound.MuseumWalk);
			}
			Commands.UpdateCommand(command);

			if (stepDirection.IsEmpty == false)
			{
				player.Move(stepDirection.X, stepDirection.Y);
			}

			Commands.UpdateCommand(command);
		}

		
		protected override bool CheckMovementImpl(Player player, int dx, int dy)
		{
			return CanPlayerStepInto(player, player.X + dx, player.Y + dy);
		}

		public override void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine)
		{
			fontColor = XleColor.White;


			boxColor = XleColor.Gray;
			innerColor = XleColor.Yellow;
			vertLine = 15 * 16;
		}

		public override bool CanPlayerStepInto(Player player, int xx, int yy)
		{
			if (this[xx, yy] >= 0x40)
				return false;
			else if ((this[xx, yy] & 0xf0) == 0x00)
				return false;
			else
				return true;
		}

		public override bool PlayerXamine(Player player)
		{
			g.AddBottom();

			if (InteractWithDisplay(player))
				return true;

			g.AddBottom("You are in an ancient museum.");

			return true;
		}

		#region --- Museum Exhibits ---

		private bool InteractWithDisplay(Player player)
		{
			Point stepDir = StepDirection(player.FaceDirection);

			int tileAt = this[player.X + stepDir.X, player.Y + stepDir.Y];

			if (mExhibits.ContainsKey(tileAt) == false)
				return false;

			MuseumDisplays.Exhibit ex = mExhibits[tileAt];

			g.AddBottom("You see a plaque.  It Reads...");
			g.AddBottom();
			g.AddBottomCentered(ex.LongName, ex.ExhibitColor);

			if (ex.IsClosed(player))
			{
				g.AddBottomCentered("- Exhibit closed -", ex.ExhibitColor);
			}
			else
			{
				g.AddBottomCentered(ex.CoinString, ex.ExhibitColor);
			}

			g.AddBottom();
			XleCore.WaitForKey();
			

			return true;
		}

		private void Information(Player player)
		{
			throw new NotImplementedException();
		}

		#endregion

		public override bool PlayerFight(Player player)
		{
			g.AddBottom();
			g.AddBottom("There is nothing to fight.");

			return true;
		}

		public override bool PlayerRob(Player player)
		{
			g.AddBottom();
			g.AddBottom("There is nothing to rob.");

			return true;
		}

		protected override bool PlayerSpeakImpl(Player player)
		{
			g.AddBottom();
			g.AddBottom("There is no reply.");

			return true;
		}

		public override bool PlayerTake(Player player)
		{
			g.AddBottom();
			g.AddBottom("There is nothing to take.");

			return true;
		}
	}
}