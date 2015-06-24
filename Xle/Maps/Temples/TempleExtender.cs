using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.Rendering.Maps;

namespace ERY.Xle.Maps.Temples
{
    public class TempleExtender : Map2DExtender
    {
        public new Temple TheMap { get { return (Temple)base.TheMap; } }

        public override XleMapRenderer CreateMapRenderer(IMapRendererFactory factory)
        {
            return factory.TempleRenderer(this);
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
