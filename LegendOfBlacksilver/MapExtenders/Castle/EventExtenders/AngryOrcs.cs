﻿using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders.Castle.EventExtenders
{
    public class AngryOrcs : CastleEvent
    {
        public override Task<bool> StepOn()
        {
            if (DurekCastleObject.InOrcArea == false)
            {
                DurekCastleObject.StoredAngryFlag = DurekCastleObject.IsAngry;

                DurekCastleObject.IsAngry = true;
                DurekCastleObject.InOrcArea = true;
            }

            return Task.FromResult(true);
        }
    }
}
