using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Ancients.MapExtenders.Outside;
using Xle.Maps.Outdoors;
using Xle.Services.Rendering.Maps;
using Xunit;
using Moq;

namespace Xle.LegacyOfTheAncients.Outside
{
	public class OutsideEncountersTest : LotaTest
	{
		OutsideEncounters oe = new OutsideEncounters();
		private Mock<ITerrainMeasurement> terrainMeasurement = new Mock<ITerrainMeasurement>();
		private Mock<IOutsideEncounterRenderer> encounterRenderer = new Mock<IOutsideEncounterRenderer>();

		public OutsideEncountersTest()
		{
			oe.GameState = GameState;
			oe.TextArea = Services.TextArea.Object;
			oe.GameControl = Services.GameControl.Object;
			oe.TerrainMeasurement = terrainMeasurement.Object;
			oe.Data = Services.Data;
			oe.Random = new Random(1);
			oe.MapRenderer = encounterRenderer.Object;
			oe.SoundMan = Services.SoundMan.Object;

			oe.Data.MonsterInfoList.Add(new MonsterInfo());
		}

		[Fact]
		public void OutsideAttack()
		{
			
			oe.InitiateEncounter();
			oe.HitMonster(4);
		}
	}
}
