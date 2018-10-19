using System;
using Xle.Services;
using Xle.Maps;

namespace Xle.Ancients.MapExtenders.Fortress.SecondArea
{
    public interface IFortressFinalActivator : IXleService
    {
        bool CompendiumAttacking { get; set; }

        Guard Warlord { get; }

        event EventHandler WarlordCreated;

        void CreateWarlord();
        void Reset();
    }
}