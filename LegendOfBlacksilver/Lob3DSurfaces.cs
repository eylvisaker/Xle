using AgateLib;
using Microsoft.Xna.Framework.Graphics;
using Xle.Maps;

namespace Xle.Blacksilver
{
    public static class Lob3DSurfaces
    {
        public static Map3DSurfaces Archives = new Map3DSurfaces();
        public static Map3DSurfaces IslandCaverns = new Map3DSurfaces();
        public static Map3DSurfaces TaragasMines = new Map3DSurfaces();
        public static Map3DSurfaces MarthbaneTunnels = new Map3DSurfaces();
        public static Map3DSurfaces PitsOfBlackmire = new Map3DSurfaces();
        public static Map3DSurfaces DeathspireChasm = new Map3DSurfaces();


        internal static void LoadSurfaces(IContentProvider content)
        {
            Archives.ExhibitOpen = content.Load<Texture2D>("Images/Museum/Exhibits/exopen");
            Archives.ExhibitClosed = content.Load<Texture2D>("Images/Museum/Exhibits/exclosed");
            Archives.Walls = content.Load<Texture2D>("Images/Museum/walls");
            Archives.Torches = content.Load<Texture2D>("Images/Museum/torches");

            SetDungeon(content, IslandCaverns, "IslandCavern");
            SetDungeon(content, TaragasMines, "TaragasMines");
            SetDungeon(content, MarthbaneTunnels, "Marthbane");
            SetDungeon(content, PitsOfBlackmire, "Blackmire");
            SetDungeon(content, DeathspireChasm, "Deathspire");
        }

        private static void SetDungeon(IContentProvider content, Map3DSurfaces surfs, string name)
        {
            surfs.Walls = content.Load<Texture2D>("Images/Dungeon/" + name + "/walls");
            surfs.Traps = content.Load<Texture2D>("Images/Dungeon/" + name + "/traps");
        }
    }
}
