using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AgateLib;


namespace XleMapEditor
{
	sealed class MainModule
	{
		public struct CustomObject
		{
			public string name;
			public int width;
			public int height;
			public int[,] Matrix;

			public CustomObject(string name, int width, int height)
			{
				this.name = name;
				this.width = width;
				this.height = height;
				Matrix = new int[width, height];
			}
		}


		public static AgateLib.DisplayLib.Surface CharSurface;

		public static string fileName;

		public static string LotaPath;
		public const int TileSize = 16;

		public static CustomObject[] PreDefObjects = new CustomObject[21];


		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();

			using (AgateSetup setup = new AgateSetup())
			{
				setup.Initialize(true, false, false);
				if (setup.WasCanceled)
				{
					return;
				}

				frmMapEdit frm = new frmMapEdit();
				Application.Run(frm);
			}

		}

		static Random rnd;
		public static Random Random
		{
			get
			{
				if (rnd == null)
					rnd = new Random();

				return rnd;
			}
		}

		static public void CreateSurfaces()
		{
			AgateLib.AgateFileProvider.Images.Clear();
			AgateLib.AgateFileProvider.Images.AddPath(Directory.GetCurrentDirectory() + "/game/images");

			LotaPath = Directory.GetCurrentDirectory() + "/game";

			CharSurface = new AgateLib.DisplayLib.Surface("character.png");

		}

	}
}
