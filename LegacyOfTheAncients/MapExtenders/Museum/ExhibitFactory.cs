using Xle.Ancients.MapExtenders.Museum.MuseumDisplays;
using Xle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xle.Bootstrap;
using AgateLib;

namespace Xle.Ancients.MapExtenders.Museum
{
    public interface IExhibitFactory
    {
        Information Information();
        Welcome Welcome();
        Weaponry Weaponry();
        Thornberry Thornberry();
        Fountain Fountain();
        PirateTreasure PirateTreasure();
        HerbOfLife HerbOfLife();
        NativeCurrency NativeCurrency();
        StonesWisdom StonesWisdom();
        Tapestry Tapestry();
        LostDisplays LostDisplays();
        KnightsTest KnightsTest();
        FourJewels FourJewels();
        Guardian Guardian();
        Pegasus Pegasus();
        AncientArtifact AncientArtifact();
    }

    [Singleton]
    public class ExhibitFactory : IExhibitFactory
    {
        private readonly IAgateServiceLocator serviceLocator;

        public ExhibitFactory(IAgateServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public AncientArtifact AncientArtifact() => serviceLocator.Resolve<AncientArtifact>();
        public Fountain Fountain() => serviceLocator.Resolve<Fountain>();
        public FourJewels FourJewels() => serviceLocator.Resolve<FourJewels>();
        public Guardian Guardian() => serviceLocator.Resolve<Guardian>();
        public HerbOfLife HerbOfLife() => serviceLocator.Resolve<HerbOfLife>();
        public Information Information() => serviceLocator.Resolve<Information>();
        public KnightsTest KnightsTest() => serviceLocator.Resolve<KnightsTest>();
        public LostDisplays LostDisplays() => serviceLocator.Resolve<LostDisplays>();
        public NativeCurrency NativeCurrency() => serviceLocator.Resolve<NativeCurrency>();
        public Pegasus Pegasus() => serviceLocator.Resolve<Pegasus>();
        public PirateTreasure PirateTreasure() => serviceLocator.Resolve<PirateTreasure>();
        public StonesWisdom StonesWisdom() => serviceLocator.Resolve<StonesWisdom>();
        public Tapestry Tapestry() => serviceLocator.Resolve<Tapestry>();
        public Thornberry Thornberry() => serviceLocator.Resolve<Thornberry>();
        public Weaponry Weaponry() => serviceLocator.Resolve<Weaponry>();
        public Welcome Welcome() => serviceLocator.Resolve<Welcome>();
    }
}
