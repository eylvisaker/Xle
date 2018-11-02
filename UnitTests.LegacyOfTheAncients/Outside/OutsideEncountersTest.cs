using Moq;
using System;
using System.Threading.Tasks;
using Xle.Ancients.MapExtenders.Outside;
using Xle.Data;
using Xle.Maps.Outdoors;
using Xunit;

namespace Xle.Ancients.Outside
{
    public class OutsideEncountersTest : LotaTest
    {
        private OutsideEncounters oe = new OutsideEncounters();
        private Mock<ITerrainMeasurement> terrainMeasurement = new Mock<ITerrainMeasurement>();

        public OutsideEncountersTest()
        {
            oe.GameState = GameState;
            oe.TextArea = Services.TextArea.Object;
            oe.GameControl = Services.GameControl.Object;
            oe.TerrainMeasurement = terrainMeasurement.Object;
            oe.Data = Services.Data;
            oe.Random = new Random(1);
            oe.RenderState = new OutsideRenderState();
            oe.SoundMan = Services.SoundMan.Object;

            oe.Data.MonsterInfo.Add(new MonsterInfo());
        }

        [Fact]
        public async Task OutsideAttack()
        {
            oe.InitiateEncounter();
            await oe.HitMonster(4);
        }
    }
}
