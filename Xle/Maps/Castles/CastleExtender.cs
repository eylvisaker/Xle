using System.Threading.Tasks;
using Xle.Maps.Towns;
using Xle.Maps.XleMapTypes;
using Xle.Services.Rendering;
using Xle.Services.Rendering.Maps;

namespace Xle.Maps.Castles
{
    public class CastleExtender : TownExtender
    {
        public new CastleMap TheMap
        {
            get { return (CastleMap)base.TheMap; }
            set { base.TheMap = value; }
        }

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

        public override Task PlayOpenRoofSound(Roof roof)
        {
            // do nothing here
            return Task.CompletedTask;
        }

        public override Task PlayCloseRoofSound(Roof roof)
        {
            // do nothing here
            return Task.CompletedTask;
        }
    }
}
