using ERY.Xle.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LoB
{
    public class LobStory : IXleSerializable
    {
        public LobStory()
        {
            VisitedArchive = new int[17];

            CastleChests = new int[5][];

            for (int i = 0; i < 5; i++)
                CastleChests[i] = new int[50];
        }

        void IXleSerializable.WriteData(XleSerializationInfo info)
        {
            info.WritePublicProperties(this);
        }
        void IXleSerializable.ReadData(XleSerializationInfo info)
        {
            info.ReadPublicProperties(this);
        }

        public int[] VisitedArchive { get; set; }

        public bool ProcuredSingingCrystal { get; set; }

        public int[][] CastleChests { get; set; }

        public bool MantrekKilled { get; set; }

        public bool DefeatedOrcs { get; set; }

        public bool ClearedRockSlide { get; set; }

        public bool ClosedVaseOfSouls { get; set; }

        public bool DrankEtherium { get; set; }

        public bool RegisteredForTrist { get; set; }

        public bool ClosedMorningStar { get; set; }

        public bool ProcuredSteelHammer { get; set; }

        public bool EatenFlaxton { get; set; }

        public bool MarthbaneOfferedHelpToKing { get; set; }

        public bool RescuedKing { get; set; }

        public bool CitadelPassword { get; set; }

        public bool BoughtStrengthFromWizard { get; set; }

        public bool BoughtOrb { get; set; }

        public bool ElfPaid { get; set; }

        public bool ElfSolvedPuzzle { get; set; }

        public bool RotlungContracted { get; set; }

        public bool Illusion { get; set; }
        public bool ArchivesOpenedOwlTemple { get; internal set; }
        public bool ArchivesOpenedHawkTemmple { get; internal set; }
    }

    public static class LobStoryExtensions
    {
        public static LobStory Story(this GameState state)
        {
            return (LobStory)state.Player.StoryData;
        }

        public static LobStory Story(this Player player)
        {
            return (LobStory)player.StoryData;
        }
    }
}
