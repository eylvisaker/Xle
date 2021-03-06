﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Rendering.Maps;

namespace Xle.Maps.Outdoors
{
    public interface IOutsideEncounters
    {
        string MonsterName { get; }
        bool InEncounter { get; }
        bool IsMonsterFriendly { get; set; }
        IReadOnlyList<Monster> CurrentMonsters { get; }
        EncounterState EncounterState { get; }
        OutsideRenderState RenderState { get; set; }

        Task Step();
        Task HitMonster(int damage);
        void OnLoad();
        Task AfterPlayerAction();
        void CancelEncounter();
        bool AttemptMovement(int dx, int dy);
    }
}