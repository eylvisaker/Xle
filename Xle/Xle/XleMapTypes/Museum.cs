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
	public class Museum : XleMap
	{
		int[] mData;
		int mHeight;
		int mWidth;

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
					if (0x40 <= this[x, y] && this[x, y] <= 0x4F)
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

		protected override void DrawImpl(int x, int y, Direction faceDirection, Rectangle inRect)
		{
			Vector3 faceVec;

			switch (faceDirection)
			{
				case Direction.East: faceVec = new Vector3(1, 0, 0); break;
				case Direction.West: faceVec = new Vector3(-1, 0, 0); break;
				case Direction.North: faceVec = new Vector3(0, -1, 0); break;
				case Direction.South: faceVec = new Vector3(0, 1, 0); break;
				default:
					throw new Exception("Invalid face direction.");
			}

			Vector3 up = new Vector3(0, 0, -1);
			Vector3 pos = new Vector3(x, y, 0.5);
			Vector3 target = pos + faceVec;
			float aspect = inRect.Width / (float)inRect.Height;
			// real aspect is 1.35294116 but we "fix" it to this value:
			aspect = 1;

			pos = pos - faceVec * 0.4;

			Display.PushClipRect(inRect);
			Color fog = Color.FromArgb(20, 20 ,20);
			Display.Clear(fog, inRect);

			Vector3 lightPos = new Vector3(7, 2.4, 0.8);
			lightPos.X += XleCore.random.Next(-1, 1) * 0.05f;
			lightPos.Y += XleCore.random.Next(-1, 1) * 0.05f;

			Vector4 lightColor = new Vector4(1, 0.7, 0, 1);
			//lightColor *= XleCore.random.Next(7, 9) / 10.0f;

			Matrix4x4 proj = Matrix4x4.Projection(70f, aspect, 0.1f, 5);
			Matrix4x4 view = Matrix4x4.LookAt(pos, target, up);
			Matrix4x4 world = Matrix4x4.Identity;// Matrix4x4.RotateZ((float)Math.PI);

			g.MuseumEffect.SetTexture(AgateLib.DisplayLib.Shaders.EffectTexture.Texture0, "texture0");
			g.MuseumEffect.SetVariable("worldViewProj", proj * world * view);
			g.MuseumEffect.SetVariable("ambientLightColor", Color.FromArgb(64,64,64));
			g.MuseumEffect.SetVariable("lightPos", lightPos);
			g.MuseumEffect.SetVariable("lightColor", lightColor);
			g.MuseumEffect.SetVariable("attenuation", new Vector3(0.3, 0, 0.7));
			g.MuseumEffect.SetVariable("fogColor", fog);

			wall_vb.Textures[0] = g.MuseumWall;

			g.MuseumEffect.Render<object>(Render, null);

			Display.Effect = null;
			Display.PopClipRect();
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

			_MoveDungeon(player, dir, out command, out stepDirection);

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
			else
				return true;
		}

		public override bool PlayerXamine(Player player)
		{
			g.AddBottom();
			g.AddBottom("You are in an ancient museum.");

			return true;
		}
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