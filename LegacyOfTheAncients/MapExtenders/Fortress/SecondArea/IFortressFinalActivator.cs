using System;
using ERY.Xle.Services;
using ERY.Xle.Maps;

namespace ERY.Xle.LotA.MapExtenders.Fortress.SecondArea
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