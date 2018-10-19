using Xle.Maps.XleMapTypes;
using Xle.Services.Rendering;
using Xle.Services.Rendering.Maps;

namespace Xle.Maps.Temples
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
    }
}
