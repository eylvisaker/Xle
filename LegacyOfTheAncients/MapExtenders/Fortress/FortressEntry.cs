using ERY.Xle.LotA.MapExtenders.Castle;
using ERY.Xle.LotA.MapExtenders.Castle.Events;
using ERY.Xle.LotA.MapExtenders.Fortress.FirstArea;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes;

namespace ERY.Xle.LotA.MapExtenders.Fortress
{
    public class FortressEntry : CastleGround
    {
        ExtenderDictionary extenders = new ExtenderDictionary();

        public FortressEntry()
        {
            WhichCastle = 2;
            CastleLevel = 1;
            GuardAttack = 3.5;

            extenders.Add("MagicIce", new MagicIce());
            extenders.Add("Elevator", new Elevator());
            extenders.Add("GasTrap", new GasTrap());
            extenders.Add("SpeakGuard", new SpeakGuard());
            extenders.Add("Armor", new ArmorBox());
            extenders.Add("GuardWarning", new GuardWarning());
            extenders.Add("SeeCompendium", new SeeCompendium());
        }

        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.Gray;
            scheme.FrameHighlightColor = XleColor.Yellow;
        }

        protected override void OnSetAngry(bool value)
        {
            base.OnSetAngry(value);

            Player.RenderColor = XleColor.White;
        }

        public override void SpeakToGuard(GameState state)
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (IsAngry)
            {
                TextArea.PrintLine("The guard ignores you.");
            }
            else
                TextArea.PrintLine("Greetings soldier.");
        }

        public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
        {
            if (x >= 0)
                return 11;
            else
                return 0;
        }

    }
}
