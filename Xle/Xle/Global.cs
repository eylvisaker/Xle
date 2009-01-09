using System;
using System.Collections.Generic;
using System.Text;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Geometry;

namespace ERY.Xle
{

    // Directional definitions
    public enum Direction
    {
        None = 0,
        East = 1,
        North,
        West,
        South
    }

    // Animation definitions
    [Obsolete]
    public enum AnimType
    {
        Invalid = -1,
        animInc = -2,
        animDec = -3,
    }
    // Terrain Definitions
    public enum TerrainType
    {
        All = -1,
        Water,
        Mountain,
        Grass,
        Forest,
        Desert,
        Swamp,
        Mixed,
        Foothills,
        Scrubland,
    }


    public static class g
    {
        static int userControl;
        static int mode;
        static int animFrame;
        static bool animating;

        static FontSurface myFont;		// stores the handle to the font
        static Surface myTiles;			// stores the handle to the tiles
        static Surface myCharacter;		// stores the handle to the character sprites
        static Surface pOverlandMonsters;	// stores the handle to the overland monster sprites

        static string[] bottom = new string[5];			// keeps the bottom portion of the screen
        static Color[][] bottomColor = new Color[5][];	// keeps the bottom colors on the screen
        static string[] weaponName = new string[20];		// stores weapon names
        static string[] armorName = new string[20];		// stores armor names
        static string[] qualityName = new string[6];		// stores quality names


        // constructors and destructor:
        static g()
        {
            
            done = false;

            for (int i = 0; i < 5; i++)
            {
                bottomColor[i] = new Color[40];

                for (int j = 0; j < bottomColor[i].Length; j++)
                    bottomColor[i][j] = XleColor.White;

            }
            
            stdDisplay = 0;

            newGraphics = false;

            raftFacing = Direction.East;
            charAnimCount = 0;
            d3dViewport = false;
            ZBufferEnable = true;
            LeftMenuActive = false;
            HPColor = XleColor.White;
            allowEnter = true;

            disableEncounters = false;

            walkTime = 150;

            invisible = false;
            guard = false;

        }

        // DirectX functions and data members
        static public FontSurface Font { get { return myFont; } }					// returns the handle to the font resource
        static public Surface Tiles { get { return myTiles; } }				// returns the handle to the tiles resource
        static public Surface Character { get { return myCharacter; } }		// returns the handle to the character resource
        static public Surface Monsters { get { return pOverlandMonsters; } }				// returns the handle to the monsters resource
        static public Surface wallTexture;			// stores the pointer to the wall texture
        static public Surface floorTexture;			// stores the pointer to the floor texture
        static public Surface ceilingTexture;			// stores the pointer to the ceiling texture
        static public Surface floorHoleTexture;		// stores the pointer to the floorhole texture


        // character functions
        

        static Timing.StopWatch animWatch = new Timing.StopWatch();
        const int frameTime = 150;

        static public int AnimFrame
        {
            get 
            {
                int oldAnim = animFrame;

                if (animWatch.IsPaused == false)
                    animFrame = (((int)animWatch.TotalMilliseconds) / frameTime) % 3;

                if (oldAnim != animFrame)
                {
                    charAnimCount++;

                    if (charAnimCount > 6)
                    {
                        animFrame = 0;
                        charAnimCount = 0;
                        Animating = false;
                    }
                }

                return animFrame;
            }
            set
            {
                animFrame = value;

                while (animFrame < 0)
                    animFrame += 3;

                if (animFrame >= 3)
                    animFrame = value % 3;
            
                animWatch.Reset();
            }
        }
        /// <summary>
        /// sets or returns whether or not the character is animating
        /// </summary>
        /// <returns></returns>
        static public bool Animating
        {
            get
            {
                if (animating == false)
                {
                    animFrame = 0;
                }

                return animWatch.IsPaused == false;
            }
            set
            {
                if (value == false)
                {
                    animFrame = 0;
                    charAnimCount = 0;

                    if (animWatch.IsPaused == false)
                        animWatch.Pause();
                }
                else
                    animWatch.Resume();

            }
        }




        // menu functions
  


        
        static public SubMenu subMenu;					// the submenu
        static public bool LeftMenuActive;				// is the left menu active?
        static public Color HPColor;					// color of left status display

        // action window functions
        /// <summary>
        /// This function adds a line to the bottom of the action window and	
        /// scrolls the remaining lines up one each.
        /// </summary>
        /// <param name="?"></param>
        static public void AddBottom(ColorStringBuilder builder)
        {
            AddBottom(builder.Text, builder.Colors);
        }

        static public void AddBottom()
        {
            AddBottom();
        }
        static public void AddBottom(string line)
        {
            AddBottom(line, null);
        }
        static public void AddBottom(string line, Color color)
        {
            Color[] colors = new Color[40];

            for (int i = 0; i < 40; i++)
            {
                colors[i] = color;
            }

            AddBottom(line, colors);
        }

        static public Color bottomTextColor = XleColor.White;

        // adds a line to the bottom of the action window
        static public void AddBottom(string line, Color[] colors)
        {

            int i;

            bottom[4] = "";

            for (i = 3; i >= 0; i--)
            {
                bottom[i + 1] = bottom[i];
                bottomColor[i + 1] = bottomColor[i];
            }


            bottomColor[0] = new Color[39];

            bottom[0] = line;

            if (colors != null)
            {
                for (i = 0; i < 39; i++)
                {
                    Color clr;

                    if (i >= colors.Length)
                        clr = colors[colors.Length - 1];
                    else
                        clr = colors[i];

                    bottomColor[0][i] = clr;
                }
            }
            else
            {
                for (i = 0; i < 39; i++)
                {
                    bottomColor[0][i] = bottomTextColor;
                }
            }

        }
        static public void UpdateBottom(string line)
        {
            UpdateBottom(line, 0, null);
        }
        static public void UpdateBottom(string line, int loc)
        {
            UpdateBottom(line, loc, null);
        }
        static public void UpdateBottom(ColorStringBuilder builder, int loc)
        {
            UpdateBottom(builder.Text, loc, builder.Colors);
        }

        /// <summary>
        /// This function updates a line in the action window.
        /// </summary>
        static public void UpdateBottom(string line, int loc, Color[] colors)
        {
            bottom[loc] = line;

            if (colors != null)
            {
                for (int i = 0; i < 39; i++)
                {
                    Color clr;

                    if (i < colors.Length)
                        clr = colors[i];
                    else
                        clr = colors[colors.Length - 1];

                    bottomColor[loc][i] = clr;
                }

            }
        }


        static public void UpdateBottom(string line, Color color)
        {
            Color[] colors = new Color[40];

            for (int i = 0; i < 40; i++)
            {
                colors[i] = color;
            }

            UpdateBottom(line, 0, colors);

        }

        static public string Bottom(int line)
        {
            string tempspace;

            if (line >= 0 && line <= 4)
            {
                tempspace = bottom[line];

                return tempspace;
            }
            else
            {
                return null;
            }

        }


        static public Color[] BottomColor(int line)
        {

            Color[] tempspace = new Color[40];

            if (line >= 0 && line <= 4)
            {
                bottomColor[line].CopyTo(tempspace, 0);

                return tempspace;
            }
            else
            {
                return null;
            }

        }


        static public void ClearBottom()
        {
            int i;

            for (i = 0; i < 5; i++)
            {
                AddBottom("");
            }
        }

        static public void WriteSlow(string line, int loc, Color color)
        {


            int i = 0;
            Color[] colors = new Color[40];
            string temp;

            while (i < line.Length && i < 40)
            {
                colors[i++] = color;
                temp = line.Substring(0, i);

                UpdateBottom(temp, loc, colors);

                XleCore.wait(50);
            }

        }


        // others:
        static public bool LoadFont()
        {

            myFont = FontSurface.BitmapMonospace("font.png", new Size(16, 16));
            myFont.StringTransformer = StringTransformer.ToUpper;

            myCharacter = new Surface("character.png");
            pOverlandMonsters = new Surface("OverworldMonsters.png");

            
            return true;

        }
        static public bool LoadTiles(string tileset)
        {
            myTiles = new Surface(tileset);

            return true;
        }
        [Obsolete("Not needed, but loads strings and shit too.")]
        static public void SetHInstance()
        {

            /****************************************************************************
             *  void Global::SetHInstance(HINSTANCE instance)							*
             *																			*
             *  Here the hInstance value is stored for hInstance to return it when		*
             *	called.																	*
             ****************************************************************************/
            //void Global::SetHInstance(HINSTANCE instance, HWND window)
            //{
            //    if (locked == false)
            //    {
            //        char	tempSpace[40];
            //        int		i;

            //        myApp = instance;
            //        hWnd = window;

            //        for (i = 0; i < 8; i++)
            //        {
            //            LoadString (hInstance(), i + 1, tempSpace, 39);
            //            weaponName[i] = tempSpace;
            //        }

            //        for (i = 0; i < 5; i++)
            //        {
            //            LoadString (hInstance(), i + 9, tempSpace, 39);
            //            armorName[i] = tempSpace;

            //        }

            //        for (i = 0; i < 5; i++)
            //        {
            //            LoadString (hInstance(), i + 14, tempSpace, 39);
            //            qualityName[i] = tempSpace;

            //        }
            //    }
            //}
        }
        
        [Obsolete("This releases everything.")]
        static public void ReleaseFont()
        {

            /*
            LPDIRECTDRAWSURFACE7		myFont;				// stores the handle to the font
            LPDIRECTDRAWSURFACE7		myTiles;			// stores the handle to the tiles
            LPDIRECTDRAWSURFACE7		myCharacter;		// stores the handle to the character sprites
            LPDIRECTDRAWSURFACE7		pOverlandMonsters;	// stores the handle to the overland monster sprites

            LPDIRECTDRAWSURFACE7	wallTexture;			// stores the pointer to the wall texture
            LPDIRECTDRAWSURFACE7	floorTexture;			// stores the pointer to the floor texture
            LPDIRECTDRAWSURFACE7	ceilingTexture;			// stores the pointer to the ceiling texture
            LPDIRECTDRAWSURFACE7	floorHoleTexture;		// stores the pointer to the floorhole texture

            LPDIRECTINPUTDEVICE2	pJoystick;				// Joystick

            */

            if (myFont != null)
            {
                myFont.Dispose();
                myFont = null;
            }
            if (myTiles != null)
            {
                myTiles.Dispose();
                myTiles = null;
            }
            if (myCharacter != null)
            {
                myCharacter.Dispose();
                myCharacter = null;
            }
            if (pOverlandMonsters != null)
            {
                pOverlandMonsters.Dispose();
                pOverlandMonsters = null;
            }
            if (wallTexture != null)
            {
                wallTexture.Dispose();
                wallTexture = null;
            }
            if (floorTexture != null)
            {
                floorTexture.Dispose();
                floorTexture = null;
            }
            if (ceilingTexture != null)
            {
                ceilingTexture.Dispose();
                ceilingTexture = null;
            }
            if (floorHoleTexture != null)
            {
                floorHoleTexture.Dispose();
                floorHoleTexture = null;
            }

        }


        
        static public string WeaponName(int a) { return weaponName[a - 1]; }
        static public int WeaponCost(int w, int q)
        {
            return (int)(9.639302862 + 12.709725901 * w + 7.448174718 * Math.Pow(w, 2) +
                5.552075007 * q + 0.731199405 * Math.Pow(q, 2) + 4.290595417 * w * q);

        }
        static public string ArmorName(int a)
        {
            return armorName[a - 1];
        }
        static public int ArmorCost(int a, int q)
        {
            return 120 * a + 12 * q + 8 * q * a;
        }
        static public string QualityName(int a)
        {
            return qualityName[a];
        }


        // map:
        //static public LotaMap map;							// the map class

        // other commonly used variables that don't need accessors
        static public char currentCommand;			// the first letter of the current command being executed
        static public int waitCommand;			// holds the time in msec to wait to give Enter Command
        static public int walkTime;				// time to wait between steps

        static public int screenLeft;				// these two values store the original position
        static public int screenTop;				//		of the window so we can restore it upon exiting 
        //		full screen mode
        static public int stdDisplay;				// heartbeat has control of the display when = 0
        static private bool done;

        public static bool Done
        {
            get
            {
                if (Display.CurrentWindow.IsClosed)
                    return true;
                else
                    return g.done;
            }
            set { g.done = value; }
        }

        static public bool waterReset;				// reset the water dots
        static public int vertLine;				// Vertical line dividing menu and map
        static public bool newGraphics;			// are we displaying the new graphics?
        static public int raftAnim;				// raft animation frame
        static public Direction raftFacing;		// direction the raft is facing (lotaEast or lotaWest)
        static public int charAnimCount;			// animation count for the player
        static public bool ZBufferEnable;			// enables or disables the ZBuffer

        static public bool d3dViewport;			// stores whether or not the d3d viewport has been set
        //ErikCollection<D3DFloor*> d3dFloor;		// first wall of the d3d area
        //static public D3DFloor* d3dFloor;
        static public bool allowEnter;				// used when a player exits a town or dungeon
        static public bool invisible;				// is the player invisible?
        static public bool guard;					// is the player in guard colors?

        static public bool disableEncounters;		// used to disable overworld encounters


        internal static bool HasCommand(KeyCode cmd)
        {
            return true;
        }
    }
}