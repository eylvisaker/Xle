using ERY.Xle;
using ERY.Xle.LotA;
using ERY.Xle.Services.Commands.Implementation;
using ERY.Xle.XleEventTypes.Extenders;
using ERY.XleTests.LegacyOfTheAncients;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.XleTests.LegacyOfTheAncients.Commands
{
    [TestClass]
    public class LotaUseTest : LotaTest
    {
        LotaUse use;
        Mock<IItemChooser> itemChooser;
        Mock<IEventInteractor> eventInteractor;

        public LotaUseTest()
        {
            itemChooser = new Mock<IItemChooser>();
            eventInteractor = new Mock<IEventInteractor>();

            use = new LotaUse();

            InitializeCommand(use);

            use.GameControl = Services.GameControl.Object;
            use.StatsDisplay = Services.StatsDisplay.Object;
            use.SoundMan = Services.SoundMan.Object;
            use.Data = Services.Data;
            use.SubMenu = Services.SubMenu.Object;
            use.ItemChooser = itemChooser.Object;
            use.EventInteractor = eventInteractor.Object;

        }

        private void SetupItemForUse(LotaItem item)
        {
            Services.Data.ItemList.Add((int)item,
                item.ToString(), item.ToString(), "");

            use.ShowItemMenu = true;

            Player.Items[item] = 4;

            itemChooser.Setup(x => x.ChooseItem()).Returns((int)item);
        }

        [TestMethod]
        public void UseHealingItem()
        {
            SetupItemForUse(LotaItem.HealingHerb);

            Player.HP = 1;
            use.Execute();

            Assert.AreEqual(Player.MaxHP / 2 + 1, Player.HP);
        }

        [TestMethod]
        public void UseWithEvent()
        {
            SetupItemForUse(LotaItem.MagicIce);

            eventInteractor
                .Setup(x => x.InteractWithFirstEvent(It.IsAny<Func<EventExtender, bool>>()))
                .Returns(true)
                .Verifiable();

            use.Execute();

            eventInteractor.Verify(x => x.InteractWithFirstEvent(
                It.IsAny<Func<EventExtender, bool>>()), Times.Once);
        }
    }
}
