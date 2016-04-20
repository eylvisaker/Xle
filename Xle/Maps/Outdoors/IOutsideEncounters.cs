using System.Collections.Generic;
using ERY.Xle.Data;
using ERY.Xle.Services;
using ERY.Xle.Services.Rendering.Maps;

namespace ERY.Xle.Maps.Outdoors
{
    public interface IOutsideEncounters : IXleService
    {
        string MonsterName { get; }
        bool InEncounter { get; }
        bool IsMonsterFriendly { get; set; }
        IReadOnlyList<Monster> CurrentMonsters { get; }
        EncounterState EncounterState { get; }
		IOutsideEncounterRenderer MapRenderer { get; set; }

        void Step();
        void HitMonster(int damage);
        void OnLoad();
        void AfterPlayerAction();
        void CancelEncounter();
        bool AttemptMovement(int dx, int dy);
    }
}