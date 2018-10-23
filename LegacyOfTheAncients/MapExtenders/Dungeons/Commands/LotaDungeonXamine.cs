using AgateLib;

using Xle.Maps.Dungeons.Commands;

namespace Xle.Ancients.MapExtenders.Dungeons.Commands
{
    [Transient("LotaDungeonXamine")]
    public class LotaDungeonXamine : DungeonXamine
    {
        private bool printLevelDuringXamine;

        public XleOptions Options { get; set; }

        protected override bool PrintLevelDuringXamine
        {
            get { return Options.EnhancedGameplay; }
        }
    }
}
