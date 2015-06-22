using ERY.Xle.Maps.Renderers;
using ERY.Xle.Maps.XleMapTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.Maps.Extenders
{
    public class TempleExtender : Map2DExtender
    {
        public new Temple TheMap { get { return (Temple)base.TheMap; } }

        protected override Renderers.XleMapRenderer CreateMapRenderer()
        {
            return new Map2DRenderer();
        }

        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.Orange;
            scheme.FrameHighlightColor = XleColor.Yellow;
        }

        protected override void PlayerFight(GameState state, Direction fightDir)
        {
            TextArea.PrintLine();
            TextArea.PrintLine("Nothing much hit.");

            SoundMan.PlaySound(LotaSound.Bump);
        }

        public override bool UseFancyMagicPrompt
        {
            get
            {
                return false;
            }
        }
    }
}
