using AgateLib;

namespace Xle.Blacksilver.MapExtenders.Towns
{
    [Transient("MaelbaneTown")]
    public class MaelbaneTown : LobTown
    {
        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.Orange;
            scheme.FrameHighlightColor = XleColor.Yellow;
        }
    }
}
