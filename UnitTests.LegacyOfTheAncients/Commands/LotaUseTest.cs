﻿using Xle;
using Xle.Ancients;
using Xle.Commands.Implementation;
using Xle.XleEventTypes.Extenders;
using Xle.LegacyOfTheAncients;
using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Xle.LegacyOfTheAncients.Commands
{
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

            itemChooser.Setup(x => x.ChooseItem()).ReturnsAsync((int)item);
        }

        [Fact]
        public async Task UseHealingItem()
        {
            SetupItemForUse(LotaItem.HealingHerb);

            Player.HP = 1;
            await use.Execute();

            Player.HP.Should().Be(Player.MaxHP / 2 + 1);
        }

        [Fact]
        public async Task UseWithEvent()
        {
            SetupItemForUse(LotaItem.MagicIce);

            eventInteractor
                .Setup(x => x.InteractWithFirstEvent(It.IsAny<Func<IEventExtender, Task<bool>>>()))
                .ReturnsAsync(true)
                .Verifiable();

            await use.Execute();

            eventInteractor.Verify(x => x.InteractWithFirstEvent(
                It.IsAny<Func<IEventExtender, Task<bool>>>()), Times.Once);
        }
    }
}
