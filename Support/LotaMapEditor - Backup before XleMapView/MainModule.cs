using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ERY.AgateLib;


namespace XleMapEditor
{
	sealed class MainModule
	{
        [Obsolete]
		public const int maxMapSize = 3000;
        [Obsolete]
		public const int maxSpecial = 120;
        [Obsolete]
		public const int maxRoofs = 40;
		
		[Obsolete]
		public struct CustomObject
		{
			public string name;
			public int width;
			public int height;
			public int[,] Matrix;
			
			public void Initialize()
			{
				Matrix = new int[11, 11];
			}
		}
		
        [Obsolete]
		public struct EditorRoof
		{
			public Point anchor;
			public Point anchorTarget;
			public int width;
			public int height;
			public int[,] Matrix;
			
			public void Initialize()
			{
				Matrix = new int[101, 101];
			}
		}
		
		public struct MyArea
		{
			public int[,] Matrix;
			
			public void Initialize()
			{
				Matrix = new int[7, 7];
			}
		}
		
		
		public static ERY.AgateLib.Surface TileSurface;
		public static ERY.AgateLib.Surface CharSurface;
				
		[Obsolete] public static int x1;
        [Obsolete]        public static int x2;
        [Obsolete]        public static int y1;
        [Obsolete]        public static int y2;
				
		[Obsolete()]public static int[,] mMap = new int[maxMapSize + 1, maxMapSize + 1];
		[Obsolete()]public static int mapWidth;
		[Obsolete()]public static int mapHeight;
		
		[Obsolete()]public static int picTilesX;
		[Obsolete]public static int picTilesY;
        [Obsolete]
        public static int leftX;
        [Obsolete]
		public static int topy;
		public static string fileName;
		public static int defaultTile;

        [Obsolete]
        public static int[] special = new int[maxSpecial + 1];
        [Obsolete]
        public static int[] specialx = new int[maxSpecial + 1];
        [Obsolete]
        public static int[] specialy = new int[maxSpecial + 1];
        [Obsolete]
        public static string[] specialdata = new string[maxSpecial + 1];
        [Obsolete]
        public static int[] specialwidth = new int[maxSpecial + 1];
        [Obsolete]
        public static int[] specialheight = new int[maxSpecial + 1];
        [Obsolete]
        public static int specialCount;
        [Obsolete]
        public static int[] mail = new int[4];
		
		public static int currentTile;
        [Obsolete]
        public static int fileOffset;
        [Obsolete]
        public static int BuyRaftMap;
        [Obsolete]
        public static int BuyRaftX;
        [Obsolete]
        public static int BuyRaftY;

        [Obsolete]
        public static Point[] guard = new Point[101];
        [Obsolete]
        public static int guardAttack;
        [Obsolete]
        public static int guardDefense;
        [Obsolete]
        public static int guardColor;
        [Obsolete]
        public static int guardHP;
		
		public static string LotaPath;
		public const int TileSize = 16;
		
        [Obsolete]
		public static bool SelectedOK;
		public static bool UpdateScreen;
		public static string TileSet;
		public static bool StartNewMap;
		public static bool ImportMap;
		public static CustomObject[] PreDefObjects = new CustomObject[21];
        [Obsolete]
        public static int NumRoofs;
        [Obsolete]
		public static EditorRoof[] Roofs = new EditorRoof[maxRoofs + 1];
		
		public static byte[] ImportedData;
		public static bool AutoHeightWidth;
		public static int ImportOffset;
		public static bool TrimCrLf;
		
		public static int ImportHeight;
		public static int ImportWidth;
		public static int AreaWidth;
		public static int AreaHeight;
        public static MyArea[] Areas = new MyArea[256];

        [Obsolete]
		public enum EnumMapType
		{
			mapMuseum = 1,
			mapOutside,
			maptown,
			mapDungeon,
			mapCastle,
			mapTemple
		}
		
		public static void SaveMapping(string path)
		{
            //int ff;
            //int j;
            //int i;
            //int k;
			
            //ff = FileSystem.FreeFile();
			
            //FileSystem.FileOpen(ff, path, OpenMode.Output, (OpenAccess) (-1), (OpenShare) (-1), -1);
			
            //FileSystem.PrintLine(ff, AreaWidth, AreaHeight);
			
            //for (i = 0; i <= 255; i++)
            //{
            //    for (j = 0; j <= AreaHeight - 1; j++)
            //    {
            //        for (k = 0; k <= AreaWidth - 1; k++)
            //        {
            //            FileSystem.Print(ff, Areas[i].Matrix[j, k], FileSystem.TAB());
            //        }
            //        FileSystem.PrintLine(ff, "");
            //    }
            //}
			
            //FileSystem.FileClose(ff);
		}
		public static void LoadMapping(string path)
		{
			int j;
			int i;
			int k;
			StreamReader ff = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read));
			
			AreaWidth = int.Parse(ff.ReadLine());
			AreaHeight = int.Parse(ff.ReadLine());
			
			
			for (i = 0; i <= 255; i++)
			{
				for (j = 0; j <= AreaHeight - 1; j++)
				{
					for (k = 0; k <= AreaWidth - 1; k++)
					{
						Areas[i].Matrix[j, k] = int.Parse(ff.ReadLine());
					}
				}
			}
			
			ff.Close();
			
			RecalibrateImport();
		}
		
        [Obsolete]
		public static int Map(int x, int y)
		{
			return mMap[x, y];
		}
		
		public static void RecalibrateImport()
		{
			int ii;
			int i;
			int j;
			int jj;
			//UPGRADE_NOTE: loc was upgraded to loc_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
			int src;
			int loc_Renamed;
			
			if (AutoHeightWidth)
			{
				for (i = ImportOffset; i <= (ImportedData.Length - 1); i++)
				{
					if (ImportedData[i] == 13 && ImportedData[i + 1] == 10)
					{
						ImportWidth = i;
						break;
					}
				}
				
				ImportWidth = ImportWidth + 2;
				ImportHeight = ((ImportedData.Length - 1) - ImportOffset) / ImportWidth;
				
				mapWidth = AreaWidth * (ImportWidth - 2);
				mapHeight = AreaHeight * ImportHeight;
				
				
				TrimCrLf = true;
			}
			else
			{
				if (TrimCrLf)
				{
					mapWidth = AreaWidth * (ImportWidth - 2);
				}
				else
				{
					mapWidth = AreaWidth * ImportWidth;
				}
				
				if (ImportHeight > ((ImportedData.Length - 1) - ImportOffset) / ImportWidth)
				{
					ImportHeight = ((ImportedData.Length - 1) - ImportOffset) / ImportWidth;
				}
				
				mapHeight = AreaHeight * ImportHeight;
				
				
			}
			
			for (j = 0; j <= mapHeight - 1; j += AreaHeight)
			{
				for (i = 0; i <= mapWidth - 1; i += AreaWidth)
				{
					loc_Renamed = ImportLocation(i, j);
					src = ImportedData[loc_Renamed];
					
					for (jj = 0; jj <= AreaHeight - 1; jj++)
					{
						for (ii = 0; ii <= AreaWidth - 1; ii++)
						{
							mMap[i + ii, j + jj] = Areas[src].Matrix[ii, jj];
						}
					}
					
				}
			}
			
			AutoHeightWidth = false;
			
		}
		
		public static int ImportLocation(int x, int y)
		{
			return (y / AreaHeight) * ImportWidth + x / AreaWidth + ImportOffset;
		}
		
		public static void ResetImportDefinitions()
		{
			AreaWidth = 1;
			AreaHeight = 1;
			
			int j;
			int i;
			int k;
			
			for (i = 0; i <= 255; i++)
			{
				for (j = 0; j <= 6; j++)
				{
					for (k = 0; k <= 6; k++)
					{
						Areas[i].Matrix[j, k] = i;
					}
				}
			}
			
		}
		
		public static void PaintArea(int x, int y, int value)
		{
			int sourceByte;
			int ax;
			int ay;
			
			sourceByte = ImportLocation(x, y);
			
			ax = x % AreaWidth;
			ay = y % AreaHeight;
			
			Areas[ImportedData[sourceByte]].Matrix[ax, ay] = value;
			
			RecalibrateImport();
		}
		
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			
			int i;
			int j;
			int k;
			
			for (i = 1; i <= maxRoofs; i++)
			{
				
				Roofs[i].Initialize();
				
				for (j = 0; j <= 100; j++)
				{
					for (k = 0; k <= 100; k++)
					{
						
						Roofs[i].Matrix[j, k] = 127;
					}
				}
			}
			
			for (i = 1; i <= maxSpecial; i++)
			{
				special[i] = 0;
				specialx[i] = 0;
				specialy[i] = 0;
				
				specialwidth[i] = 1;
				specialheight[i] = 1;
			}
			
			using (AgateSetup setup = new AgateSetup())
			{
				setup.Initialize(true, false, false);
				if (setup.Cancel)
				{
					return;
				}

                frmMEdit frm = new frmMEdit();
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
        
		static public void CreateSurfaces(int width, int height)
		{
			
			FileManager.ImagePath = new SearchPath(Directory.GetCurrentDirectory() + "/game/images");
			
			LotaPath = Directory.GetCurrentDirectory() + "/game";
			
			
			CharSurface = new ERY.AgateLib.Surface("character.png");
			
		}
		
		static public void LoadTiles(string theFile)
		{
			int j;
			int i;
			int o = 0;
			// 			int i;
			// 			int o;
			TextReader ff;
			string name = "";
			
			TileSurface = new ERY.AgateLib.Surface(LotaPath + "\\images\\" + theFile);
			
			
			for (i = 0; i <= 20; i++)
			{
				CustomObject with_1 = PreDefObjects[i];
				with_1.name = "";
				with_1.height = 0;
				with_1.width = 0;
				
			}

            using (FileStream myfile = File.Open(Path.GetDirectoryName(Application.ExecutablePath) + "\\predef.txt", FileMode.Open, FileAccess.Read))
            {
                ff = new StreamReader(myfile);

                do
                {
                    name = ff.ReadLine();
                } while (!(name == null || name.ToLower() == "[" + theFile.ToLower() + "]"));


                if (name != null)
                {
                    do
                    {
                        name = ff.ReadLine();

                        if (name != "" && name.Substring(0, 1) != "[")
                        {
                            CustomObject with_2 = PreDefObjects[o];

                            with_2.name = name;
                            with_2.width = int.Parse(ff.ReadLine());
                            with_2.height = int.Parse(ff.ReadLine());

                            for (i = 0; i <= with_2.height - 1; i++)
                            {
                                for (j = 0; j <= with_2.width - 1; j++)
                                {
                                    with_2.Matrix[j, i] = int.Parse(ff.ReadLine());
                                }
                            }


                            o++;
                        }


                    } while (!(name == null || name == "" || name.Substring(0, 1) == "["));
                }
            }
			
			frmMEdit.Default.lstPreDef.Items.Clear();
			
			for (i = 0; i <= 20; i++)
			{
				
				if (PreDefObjects[i].name != "")
				{
					frmMEdit.Default.lstPreDef.Items.Insert(i, PreDefObjects[i].name);
				}
				
			}
		}
		
	}
}
