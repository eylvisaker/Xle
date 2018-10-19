﻿using Xle.Maps.Outdoors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Services.Rendering.Maps;

namespace Xle.LoB.MapExtenders.Outside
{
    public class LobOutsideEncounters : IOutsideEncounters
    {
        List<Monster> monsters = new List<Monster>();

        public IReadOnlyList<Monster> CurrentMonsters
        {
            get
            {
                return monsters;
            }
        }

        public EncounterState EncounterState { get; set; }

        public bool InEncounter
        {
            get { return monsters.Count > 0; }
        }

        public bool IsMonsterFriendly { get; set; }

        public IOutsideEncounterRenderer MapRenderer { get; set; }

        public string MonsterName
        {
            get
            {
                return monsters.First().Name;
            }
        }

        public void AfterPlayerAction()
        {
        }

        public bool AttemptMovement(int dx, int dy)
        {
            return true;
        }

        public void CancelEncounter()
        {
        }

        public void HitMonster(int damage)
        {
        }

        public void OnLoad()
        {
        }

        public void Step()
        {
        }
    }
}
