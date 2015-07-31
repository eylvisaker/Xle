using ERY.Xle.Maps.Towns;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.Rendering.Maps;

namespace ERY.Xle.Maps.Castles
{
    public class CastleExtender : TownExtender
    {
        public new CastleMap TheMap { get { return (CastleMap)base.TheMap; } }

        public override XleMapRenderer CreateMapRenderer(IMapRendererFactory factory)
        {
            return factory.CastleRenderer(this);
        }
        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.Gray;
            scheme.FrameHighlightColor = XleColor.Yellow;
        }

        public override void PlayOpenRoofSound(Roof roof)
        {
            // do nothing here
        }
        public override void PlayCloseRoofSound(Roof roof)
        {
            // do nothing here
        }
    }
}
