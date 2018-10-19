using Xle.Ancients.MapExtenders.Museum.MuseumDisplays;
using Xle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xle.Ancients.MapExtenders.Museum
{
    public interface IExhibitFactory : IXleFactory
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
}
