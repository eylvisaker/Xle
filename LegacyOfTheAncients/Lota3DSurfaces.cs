using AgateLib;
using ERY.Xle.Maps;
using Microsoft.Xna.Framework.Graphics;

namespace ERY.Xle.LotA
{
    public static class Lota3DSurfaces
    {
        public static Map3DSurfaces Museum = new Map3DSurfaces();
        public static Map3DSurfaces MuseumDark = new Map3DSurfaces();
        public static Map3DSurfaces DungeonBlue = new Map3DSurfaces();
        public static Map3DSurfaces DungeonBrown = new Map3DSurfaces();

        internal static void LoadSurfaces(IContentProvider content)
        {
            Museum.Walls = content.Load<Texture2D>("Images/Museum/walls.png");
            Museum.Torches = content.Load<Texture2D>("Images/Museum/torches.png");
            Museum.ExhibitOpen = content.Load<Texture2D>("Images/Museum/Exhibits/exopen.png");
            Museum.ExhibitClosed = content.Load<Texture2D>("Images/Museum/Exhibits/exopen.png");

            MuseumDark.Walls = content.Load<Texture2D>("Images/MuseumDark/walls.png");

            DungeonBlue.Traps = content.Load<Texture2D>("Images/Dungeon/PiratesLair/traps.png");
            DungeonBlue.Walls = content.Load<Texture2D>("Images/Dungeon/PiratesLair/walls.png");

            DungeonBrown.Traps = content.Load<Texture2D>("Images/Dungeon/Armak/traps.png");
            DungeonBrown.Walls = content.Load<Texture2D>("Images/Dungeon/Armak/walls.png");
        }
    }
}
