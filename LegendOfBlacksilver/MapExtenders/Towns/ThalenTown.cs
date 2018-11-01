using AgateLib;

namespace Xle.Blacksilver.MapExtenders.Towns
{
    [Transient("ThalenTown")]
    public class ThalenTown : LobTown
    {
        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.Gray;
            scheme.FrameHighlightColor = XleColor.Yellow;
        }
    }
}
