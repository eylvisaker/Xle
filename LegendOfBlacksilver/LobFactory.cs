﻿using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using System;
using System.Collections.Generic;
using Xle.Data;
using Xle.LoB.MapExtenders.Castle;
using Xle.LoB.MapExtenders.Citadel;
using Xle.LoB.MapExtenders.Labyrinth;
using Xle.Maps.XleMapTypes;
using Xle.Serialization;

namespace Xle.LoB
{
    public class LobFactory : XleGameFactory
    {
        private Dictionary<string, Type> mExtenders = new Dictionary<string, Type>();

        public LobFactory()
        {
            FillExtenderDictionaries();
        }

        public XleData Data { get; set; }

        public override IXleSerializable CreateStoryData()
        {
            return new LobStory();
        }

        private void FillExtenderDictionaries()
        {
            mExtenders["castle"] = typeof(DurekCastle);
            mExtenders["citadel1"] = typeof(CitadelGround);
            mExtenders["citadel2"] = typeof(CitadelUpper);
            mExtenders["labyrinth1"] = typeof(LabyrinthBase);
            mExtenders["labyrinth2"] = typeof(LabyrinthUpper);
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
            var fontSurface = FontSurface.BitmapMonospace("Images/font.png", new Size(16, 16));

            Font = new FontBuilder("LotaFont")
                .AddFontSurface(new FontSettings(16, FontStyles.None), fontSurface)
                .Build();
            Font.Size = 16;

            Character = new Surface("Images/character.png");
            Monsters = new Surface("Images/OverworldMonsters.png");

            Lob3DSurfaces.LoadSurfaces();

            foreach (var exinfo in Data.ExhibitInfo.Values)
            {
                try
                {
                    exinfo.LoadImage();
                }
                catch (System.IO.FileNotFoundException)
                {
                    System.Diagnostics.Debug.Print("Image " + exinfo.ImageFile + " not found.");
                }
            }
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
