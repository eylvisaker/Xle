﻿using AgateLib;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Menus;
using Xle.ScreenModel;

namespace Xle.Commands.Implementation
{
    [Transient]
    public class Hold : Command
    {
        public IItemChooser ItemChooser { get; set; }

        public override async Task Execute()
        {
            Player.Hold = await ItemChooser.ChooseItem();
        }
    }
}
