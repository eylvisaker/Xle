using Xle.Maps.XleMapTypes.MuseumDisplays;

namespace Xle.Maps.Museums
{
    public class MuseumRenderState
    {
        public Exhibit Closeup { get; set; }

        public bool DrawStatic { get; set; }

        public bool DrawCloseup { get; set; }

        public bool AnimateExhibits { get; set; }
    }
}
