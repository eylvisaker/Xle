using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Data;
using Xle.Services;
using Xle.Services.Commands.Implementation;
using Xle.Services.Rendering.Maps;

namespace Xle.Maps.Outdoors.Commands
{
    [ServiceName("OutsideFight")]
    public class OutsideFight : Fight
    {
        public IOutsideEncounters Encounters { get; set; }

        OutsideExtender map { get { return (OutsideExtender)GameState.MapExtender; } }
        OutsideRenderer MapRenderer
        {
            get { return map.MapRenderer; }
        }

        EncounterState EncounterState
        {
            get { return Encounters.EncounterState; }
        }

        bool IsMonsterFriendly
        {
            get { return Encounters.IsMonsterFriendly; }
            set { Encounters.IsMonsterFriendly = value; }
        }

        string MonstName
        {
            get { return Encounters.MonsterName; }
        }

        IReadOnlyList<Monster> currentMonst
        {
            get { return Encounters.CurrentMonsters; }
        }

        int monstCount
        {
            get { return Encounters.CurrentMonsters.Count; }
        }

        public override void Execute()
        {
            string weaponName = Player.CurrentWeapon.BaseName(Data);

            TextArea.PrintLine("\n");

            if (EncounterState == EncounterState.MonsterReady)
            {
                int dam = attack();

                TextArea.Print("Attack ", XleColor.White);
                TextArea.Print(MonstName, XleColor.Cyan);
                TextArea.PrintLine();

                TextArea.Print("with ", XleColor.White);
                TextArea.Print(weaponName, XleColor.Cyan);
                TextArea.PrintLine();

                if (dam <= 0)
                {
                    SoundMan.PlaySound(LotaSound.PlayerMiss);

                    TextArea.PrintLine("Your Attack missed.", XleColor.Yellow);

                    return;
                }

                SoundMan.PlaySound(LotaSound.PlayerHit);

                Encounters.HitMonster(dam);
            }
            else if (EncounterState > 0)
            {
                TextArea.PrintLine("The unknown creature is not ");
                TextArea.PrintLine("within range.");

                GameControl.Wait(300 + 100 * Player.Gamespeed);
            }
            else
            {
                return;
            }

            return;
        }

        int attack()
        {
            int damage = PlayerHit(currentMonst[monstCount - 1].Defense);

            if (currentMonst[monstCount - 1].Vulnerability > 0)
            {
                if (Player.CurrentWeapon.ID == currentMonst[monstCount - 1].Vulnerability)
                {
                    damage += Random.Next(11) + 20;
                }
                else
                {
                    damage = 1 + Random.Next((damage < 10) ? damage : 10);
                }
            }
            IsMonsterFriendly = false;

            return damage;
        }

        /// <summary>
        /// Player damages a creature. Returns the amount of damage the player did,
        /// or zero if the player missed.
        /// </summary>
        /// <param name="defense"></param>
        /// <returns></returns>
        private int PlayerHit(int defense)
        {
            int wt = Player.CurrentWeapon.ID;
            int qt = Player.CurrentWeapon.Quality;

            int dam = Player.Attribute[Attributes.strength] - 12;
            dam += (int)(wt * (qt + 2)) / 2;

            dam = (int)(dam * Random.Next(30, 150) / 100.0 + 0.5);
            dam += Random.Next(-2, 3);

            if (dam < 3)
                dam = 1 + Random.Next(3);

            int hit = Player.Attribute[Attributes.dexterity] * 8 + 15 * qt;

            System.Diagnostics.Debug.WriteLine("Hit: " + hit.ToString() + " Dam: " + dam.ToString());

            hit -= Random.Next(400);

            if (hit < 0)
                dam = 0;

            //return 100;
            return dam;
        }

    }
}
