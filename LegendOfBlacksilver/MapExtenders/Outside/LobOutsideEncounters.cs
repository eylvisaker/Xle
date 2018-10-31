using Xle.Maps.Outdoors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Services.Rendering.Maps;
using AgateLib;

namespace Xle.Blacksilver.MapExtenders.Outside
{
    [Transient]
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

        public Task AfterPlayerAction()
        {
            return Task.CompletedTask;
        }

        public bool AttemptMovement(int dx, int dy)
        {
            return true;
        }

        public void CancelEncounter()
        {
        }

        public Task HitMonster(int damage)
        {
            return Task.CompletedTask;
        }

        public void OnLoad()
        {
        }

        public Task Step()
        {
            return Task.CompletedTask;
        }
    }
}
