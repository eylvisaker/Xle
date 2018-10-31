using AgateLib;
using AgateLib.Display;
using AgateLib.Display.BitmapFont;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Xle.Data;
using Xle.Blacksilver.MapExtenders.Castle;
using Xle.Blacksilver.MapExtenders.Citadel;
using Xle.Blacksilver.MapExtenders.Labyrinth;
using Xle.Maps.XleMapTypes;
using Xle.Serialization;

namespace Xle.Blacksilver
{
    public class LobFactory : XleGameFactory
    {
        private readonly IContentProvider content;
        private readonly XleData data;

        public LobFactory(
            IContentProvider content,
            XleData data)
        {
            this.content = content;
            this.data = data;

            LoadSurfaces();
        }

        public XleData Data { get; set; }

        public override IXleSerializable CreateStoryData()
        {
            return new LobStory();
        }

        public override string GameTitle
        {
            get
            {
                return "Legend of Blacksilver";
            }
        }

        public override void LoadSurfaces()
        {
            var fontSurface = content.Load<Texture2D>("Images/font");
            FontMetrics fontMetrics = BuildFontMetrics();

            var bitmapFont = new BitmapFontTexture(fontSurface,
                fontMetrics,
                "LotaFont");

            Font = new FontBuilder("LotaFont")
                .AddFontTexture(new FontSettings(16, FontStyles.None), bitmapFont)
                .Build();
            Font.Size = 16;

            Character = content.Load<Texture2D>("Images/character");
            Monsters = content.Load<Texture2D>("Images/OverworldMonsters");

            Lob3DSurfaces.LoadSurfaces(content);

            foreach (var exinfo in Data.ExhibitInfo.Values)
            {
                try
                {
                    exinfo.LoadImage(content);
                }
                catch (System.IO.FileNotFoundException)
                {
                    System.Diagnostics.Debug.Print("Image " + exinfo.ImageFile + " not found.");
                }
            }
        }

        private static FontMetrics BuildFontMetrics()
        {
            var fontMetrics = new FontMetrics();
            for (int i = 0; i < 128; i++)
            {
                int col = i % 16;
                int row = i / 16;
                int x = col * 16;
                int y = row * 16;

                fontMetrics.Add(i, new GlyphMetrics(new Rectangle(x, y, 16, 16)));
            }

            return fontMetrics;
        }

        public override Maps.Map3DSurfaces GetMap3DSurfaces(Map3D map3D)
        {
            if (map3D is Museum)
                return Lob3DSurfaces.Archives;
            else
                return Lob3DSurfaces.IslandCaverns;
        }

        public override void SetGameSpeed(GameState state, int Gamespeed)
        {
            base.SetGameSpeed(state, Gamespeed);

            state.GameSpeed.CastleOpenChestSoundTime = 300;
        }

        public override int MailItemID
        {
            get { return (int)LobItem.Package; }
        }
        public override int HealingItemID
        {
            get { return (int)LobItem.LifeElixir; }
        }
        public override int ClimbingGearItemID
        {
            get { return (int)LobItem.ClimbingGear; }
        }
    }
}
