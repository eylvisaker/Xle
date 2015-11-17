using System;
using System.Collections.Generic;
using System.Linq;

using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.InputLib.Legacy;

using ERY.Xle.Data;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services.Menus;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.Rendering.Maps;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.Maps.Outdoors
{
    public class OutsideExtender : Map2DExtender
    {
        Direction monstDir { get; set; }

        public XleSystemState SystemState { get; set; }
        public ITerrainMeasurement TerrainMeasurement { get; set; }
        public IOutsideEncounters OutsideEncounters { get; set; }

        public new Outside TheMap { get { return (Outside)base.TheMap; } }
        public new OutsideRenderer MapRenderer
        {
            get { return (OutsideRenderer)base.MapRenderer; }
        }

        protected override void OnMapRendererSet()
        {
            OutsideEncounters.MapRenderer = MapRenderer;
        }

        public override XleMapRenderer CreateMapRenderer(IMapRendererFactory factory)
        {
            return factory.OutsideRenderer(this);
        }

        public override void CheckSounds()
        {
            if (Player.IsOnRaft)
            {
                if (SoundMan.IsPlaying(LotaSound.Raft1) == false)
                    SoundMan.PlaySound(LotaSound.Raft1);

                SoundMan.StopSound(LotaSound.Ocean1);
                SoundMan.StopSound(LotaSound.Ocean2);
            }
            else
            {
                SoundMan.StopSound(LotaSound.Raft1);
                int ocean = 0;


                for (int i = -1; i <= 2 && ocean == 0; i++)
                {
                    for (int j = -1; j <= 2 && ocean == 0; j++)
                    {
                        if (Math.Sqrt(Math.Pow(i, 2) + Math.Pow(j, 2)) <= 5)
                        {
                            if (TheMap[Player.X + i, Player.Y + j] < 16)
                            {
                                ocean = 1;
                            }
                        }
                    }
                }

                //  If we're not near the ocean, fade the sound out
                if (ocean == 0)
                {
                    /*
                    if (LotaGetSoundStatus(LotaSound.Ocean1) & (DSBSTATUS_LOOPING | DSBSTATUS_PLAYING))
                    {
                        //SoundMan.PlaySound(LotaSound.Ocean1, 0, false);
                        LotaFadeSound(LotaSound.Ocean1, -2);
                    }
                    if (LotaGetSoundStatus(LotaSound.Ocean2) & (DSBSTATUS_LOOPING | DSBSTATUS_PLAYING))
                    {
                        //SoundMan.PlaySound(LotaSound.Ocean2, 0, false);
                        LotaFadeSound(LotaSound.Ocean2, -2);
                    }
                    */
                }
                //  we are near the ocean, so check to see if we need to play the next
                //  sound (at 1 second intervals)
                else
                {
                    /*
                    if (lastOceanSound + 1000 < Timing.TotalMilliseconds )
                    {
                        if (1 + Lota.random.Next(2) == 1)
                        {
                            SoundMan.PlaySound(LotaSound.Ocean1, 0, false);
                        }
                        else
                        {
                            SoundMan.PlaySound(LotaSound.Ocean2, 0, false);
                        }

                        lastOceanSound = clock();
                    }
                     * */
                }
                /*
                //  Play mountain sounds...
                if (player.Terrain() == TerrainType.Mountain)
                {
                    if (!(LotaGetSoundStatus(LotaSound.Mountains) & DSBSTATUS_PLAYING))
                    {
                        SoundMan.PlaySound(LotaSound.Mountains, DSBPLAY_LOOPING, true);
                        //LotaFadeSound(LotaSound.Mountains, 2, DSBPLAY_LOOPING);
                    }
                }
                else if (LotaGetSoundStatus(LotaSound.Mountains) & (DSBSTATUS_LOOPING | DSBSTATUS_PLAYING))
                {
                    //if (LotaGetSoundStatus(LotaSound.Mountains) & DSBSTATUS_PLAYING)
                    {
                        LotaFadeSound(LotaSound.Mountains, -1, 0);
                        //LotaStopSound(LotaSound.Mountains);
                    }

                }
                */
            }
        }

        public override int GetOutsideTile(Point playerPoint, int x, int y)
        {
            return 0;
        }
        
        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.Brown;
            scheme.FrameHighlightColor = XleColor.Yellow;

            scheme.MapAreaWidth = 25;
        }

        public virtual void ModifyTerrainInfo(TerrainInfo info, TerrainType terrain)
        {
        }
        
        public virtual void UpdateEncounterState(ref bool handled)
        {
        }

        public override void PlayerCursorMovement(Direction dir)
        {
            string command;
            Point stepDirection;

            _Move2D(dir, "Move", out command, out stepDirection);

            TextArea.PrintLine(command);

            if (CanPlayerStep(stepDirection) == false)
            {
                TerrainType terrain = TheMap.TerrainAt(Player.X + stepDirection.X, Player.Y + stepDirection.Y);

                if (OutsideEncounters.InEncounter)
                {
                    SoundMan.PlaySound(LotaSound.Bump);

                    TextArea.PrintLine();
                    TextArea.PrintLine("Attempt to disengage");
                    TextArea.PrintLine("is blocked.");

                    GameControl.Wait(500);
                }
                else if (Player.IsOnRaft)
                {
                    SoundMan.PlaySound(LotaSound.Bump);

                    TextArea.PrintLine();
                    TextArea.PrintLine("The raft must stay in the water.", XleColor.Cyan);
                }
                else if (terrain == TerrainType.Water)
                {
                    SoundMan.PlaySound(LotaSound.Bump);

                    TextArea.PrintLine();
                    TextArea.PrintLine("There is too much water for travel.", XleColor.Cyan);
                }
                else if (terrain == TerrainType.Mountain)
                {
                    SoundMan.PlaySound(LotaSound.Bump);

                    TextArea.PrintLine();
                    TextArea.PrintLine("You are not equipped to");
                    TextArea.PrintLine("cross the mountains.");
                }
                else
                {
                    throw new NotImplementedException();
                    //SoundMan.PlaySound(LotaSound.Invalid);
                }
            }
            else
            {
                BeforeStepOn(Player.X + stepDirection.X, Player.Y + stepDirection.Y);

                MovePlayer(stepDirection);

                if (OutsideEncounters.EncounterState == EncounterState.JustDisengaged)
                {
                    TextArea.PrintLine();
                    TextArea.PrintLine("Attempt to disengage");
                    TextArea.PrintLine("is successful.");

                    GameControl.Wait(500);

                    OutsideEncounters.CancelEncounter();
                }

                TerrainInfo info = GetTerrainInfo();

                if (Player.IsOnRaft == false)
                {
                    SoundMan.PlaySound(info.WalkSound);
                }

                Player.TimeDays += info.StepTimeDays;
                Player.TimeQuality += 1;
            }
        }

        public TerrainInfo GetTerrainInfo()
        {
            var terrain = TerrainMeasurement.TerrainAtPlayer();

            return GetTerrainInfo(terrain);
        }

        TerrainInfo GetTerrainInfo(TerrainType terrain)
        {
            TerrainInfo info = new TerrainInfo();

            switch (terrain)
            {
                case TerrainType.Water:
                case TerrainType.Grass:
                case TerrainType.Forest:
                    info.StepTimeDays += .25;
                    break;
                case TerrainType.Swamp:
                    info.StepTimeDays += .5;
                    break;
                case TerrainType.Mountain:
                    info.StepTimeDays += 1;
                    break;
                case TerrainType.Desert:
                    info.StepTimeDays += 1;
                    break;
                case TerrainType.Mixed:
                    info.StepTimeDays += 0.5;
                    break;
            }

            switch (terrain)
            {
                case TerrainType.Swamp:
                    info.WalkSound = (LotaSound.WalkSwamp);
                    break;

                case TerrainType.Desert:
                    info.WalkSound = (LotaSound.WalkDesert);
                    break;

                case TerrainType.Grass:
                case TerrainType.Forest:
                case TerrainType.Mixed:
                default:
                    info.WalkSound = (LotaSound.WalkOutside);
                    break;
            }
            switch (terrain)
            {
                case TerrainType.Grass:
                    info.TerrainName = "grasslands";
                    info.TravelText = "easy";
                    info.FoodUseText = "low";
                    break;
                case TerrainType.Water:
                    info.TerrainName = "water";
                    info.TravelText = "easy";
                    info.FoodUseText = "low";
                    break;
                case TerrainType.Mountain:
                    info.TerrainName = "the mountains";
                    info.TravelText = "slow";
                    info.FoodUseText = "high";
                    break;
                case TerrainType.Forest:
                    info.TerrainName = "a forest";
                    info.TravelText = "easy";
                    info.FoodUseText = "low";
                    break;
                case TerrainType.Desert:
                    info.TerrainName = "a desert";
                    info.TravelText = "slow";
                    info.FoodUseText = "high";
                    break;
                case TerrainType.Swamp:
                    info.TerrainName = "a swamp";
                    info.TravelText = "average";
                    info.FoodUseText = "medium";
                    break;
                case TerrainType.Foothills:
                    info.TerrainName = "mountain foothills";
                    info.TravelText = "average";
                    info.FoodUseText = "medium";
                    break;

                default:
                case TerrainType.Mixed:
                    info.TerrainName = "mixed terrain";
                    info.TravelText = "average";
                    info.FoodUseText = "medium";
                    break;

            }

            ModifyTerrainInfo(info, terrain);

            return info;
        }

        private void UpdateRaftState()
        {
            if (Player.IsOnRaft == false)
            {
                foreach (var raft in Player.Rafts.Where(r => r.MapNumber == TheMap.MapID))
                {
                    if (raft.Location.Equals(Player.Location))
                    {
                        Player.BoardedRaft = raft;
                        break;
                    }
                }
            }

            if (Player.IsOnRaft)
            {
                var raft = Player.BoardedRaft;

                raft.X = Player.X;
                raft.Y = Player.Y;
            }
        }

        public override void AfterExecuteCommand(KeyCode cmd)
        {
            OutsideEncounters.AfterPlayerAction();
        }

        public override bool CanPlayerStepIntoImpl(int xx, int yy)
        {
            int dx = xx - Player.X;
            int dy = yy - Player.Y;

            if (OutsideEncounters.EncounterState != EncounterState.NoEncounter)
            {
                bool result = OutsideEncounters.AttemptMovement(dx, dy);

                if (result == false)
                    return false;
            }

            TerrainType terrain = TerrainMeasurement.TerrainAt(xx, yy);

            if (Player.IsOnRaft)
            {
                return terrain == TerrainType.Water;
            }

            if (terrain == TerrainType.Water)
            {
                for (int i = 0; i < Player.Rafts.Count; i++)
                {
                    if (Math.Abs(Player.Rafts[i].X - xx) < 2 && Math.Abs(Player.Rafts[i].Y - yy) < 2)
                    {
                        return true;
                    }
                }

                return false;
            }

            if (terrain == TerrainType.Mountain && Player.Hold != SystemState.Factory.ClimbingGearItemID)
            {
                return false;
            }

            return true;
        }

        public override void AfterPlayerStep()
        {
            base.AfterPlayerStep();

            // bail out if the player entered another map on this step.
            if (GameState.Map != TheMap)
                return;

            UpdateRaftState();

            OutsideEncounters.Step();
        }

        public override void OnLoad()
        {
            OutsideEncounters.OnLoad();
            base.OnLoad();
        }

        protected void SetMonsterImagePosition()
        {
            monstDir = (Direction)Random.Next((int)Direction.East, (int)Direction.South + 1);
            MapRenderer.MonsterDrawDirection = monstDir;
        }

        public override int WaitTimeAfterStep
        {
            get
            {
                return GameState.GameSpeed.OutsideStepTime;
            }
        }
    }
}
