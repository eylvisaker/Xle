using System.Collections.Generic;
using ERY.Xle.Data;
using ERY.Xle.Services;

namespace ERY.Xle.Maps.Outdoors
{
    public interface IOutsideEncounters : IXleService
    {
        string MonsterName { get; }
        bool InEncounter { get; }
        bool IsMonsterFriendly { get; set; }
        List<Monster> CurrentMonsters { get; }
        EncounterState EncounterState { get; }

        void Step();
        void HitMonster(int damage);
        void OnLoad();
        void AfterPlayerAction();
    }
}