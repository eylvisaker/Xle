using AgateLib;
using Microsoft.Xna.Framework;
using Xle.Services.Rendering.Maps;

namespace Xle.Blacksilver.MapExtenders.Archives
{
    [Transient("ArchiveRenderer")]
    public class ArchiveRenderer : MuseumRenderer
    {
        protected override Rectangle ExhibitCloseupRect
        {
            get
            {
                var result = base.ExhibitCloseupRect;

                result.Y -= 16;

                return result;
            }
        }
    }
}
