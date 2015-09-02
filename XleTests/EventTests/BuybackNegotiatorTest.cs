using ERY.Xle;
using ERY.Xle.XleEventTypes.Stores.Extenders.BuybackImplementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.XleTests.EventTests
{
    [TestClass]
    public class BuybackNegotiatorTest : XleTest
    {
        BuybackNegotiator negotiator;
        Equipment eq;

        public BuybackNegotiatorTest()
        {
            negotiator = new BuybackNegotiator()
            {
                BuybackFormatter = Services.BuybackFormatter.Object,
                OfferWindow = Services.BuybackOfferWindow.Object,
                NumberPicker = Services.NumberPicker.Object,
                QuickMenu = Services.QuickMenu.Object,
                GameControl = Services.GameControl.Object,
                SoundMan = Services.SoundMan.Object,
                GameState = GameState,
                Data = Services.Data,
                Random = new Random(1),
            };

            eq = new WeaponItem { ID = 1, Quality = 3 };
        }

        [TestMethod]
        public void AcceptFirstOffer()
        {
            int offer = 0;
            int playerGold = GameState.Player.Gold;

            Services.BuybackFormatter
                .Setup(x => x.Offer(eq, It.IsAny<int>(), false))
                .Callback<Equipment, int, bool>((e, o, of) => offer = o);

            Services.QuickMenu.Setup(x => x.QuickMenuYesNo(true))
                .Returns(0);

            negotiator.NegotiatePrice(eq);

            Assert.AreEqual(GameState.Player.Gold, playerGold + offer);
        }

        [TestMethod]
        public void AcceptSecondOffer()
        {
            List<int> offers = new List<int>();
            int playerGold = GameState.Player.Gold;
            int count = 0;

            Services.BuybackFormatter
                .Setup(x => x.Offer(eq, It.IsAny<int>(), It.IsAny<bool>()))
                .Callback<Equipment, int, bool>((e, o, of) => offers.Add(o));

            Services.QuickMenu.Setup(x => x.QuickMenuYesNo(true))
                .Returns<bool>(_ =>
                {
                    count++;
                    if (count == 2)
                        return 0;
                    else return 1;
                });

            Services.NumberPicker
                .Setup(x => x.ChooseNumber(It.IsAny<int>()))
                .Returns(() => offers[0] * 3);

            negotiator.NegotiatePrice(eq);

            Assert.IsTrue(offers[1] > offers[0]);
            Assert.AreEqual(GameState.Player.Gold, playerGold + offers[1]);
        }

        [TestMethod]
        public void CancelNegotiation()
        {
            int offer = 0;
            int playerGold = Player.Gold;

            Services.BuybackFormatter
                .Setup(x => x.Offer(eq, It.IsAny<int>(), false))
                .Callback<Equipment, int, bool>((e, o, of) => offer = o);

            Services.QuickMenu.Setup(x => x.QuickMenuYesNo(true))
                .Returns(1);

            Services.NumberPicker
                .Setup(x => x.ChooseNumber(It.IsAny<int>()))
                .Returns(0);

            negotiator.NegotiatePrice(eq);

            Assert.AreEqual(playerGold, Player.Gold);
        }

        [TestMethod]
        public void MakeSaleIfAskIsLow()
        {
            int offer = 0;
            int ask = 0;
            int playerGold = Player.Gold;

            Services.BuybackFormatter
                .Setup(x => x.Offer(eq, It.IsAny<int>(), false))
                .Callback<Equipment, int, bool>((e, o, of) => offer = o);

            Services.QuickMenu.Setup(x => x.QuickMenuYesNo(true))
                .Returns(1);

            Services.NumberPicker
                .Setup(x => x.ChooseNumber(It.IsAny<int>()))
                .Returns(() => ask = (offer * 149) / 100);

            negotiator.NegotiatePrice(eq);

            Assert.AreEqual(playerGold + ask, Player.Gold);
        }

        [TestMethod]
        public void TerminateIfAskIsTooHigh()
        {
            int offer = 0;
            int playerGold = Player.Gold;

            Services.BuybackFormatter
                .Setup(x => x.Offer(eq, It.IsAny<int>(), false))
                .Callback<Equipment, int, bool>((e, o, of) => offer = o);

            Services.QuickMenu.Setup(x => x.QuickMenuYesNo(true))
                .Returns(1);

            Services.NumberPicker
                .Setup(x => x.ChooseNumber(It.IsAny<int>()))
                .Returns(() => 5000);

            Services.BuybackFormatter.Setup(x => x.ComeBackWhenSerious()).Verifiable();

            negotiator.NegotiatePrice(eq);

            Assert.AreEqual(playerGold, Player.Gold);
            Services.BuybackFormatter.Verify(x => x.ComeBackWhenSerious(), Times.Once);
        }

        [TestMethod]
        public void GradualNegotiation()
        {
            int initialOffer = 0;
            int ask = 0;
            int offer = 0;
            int playerGold = Player.Gold;
            bool finalOffer = false;

            Services.BuybackFormatter
                .Setup(x => x.Offer(eq, It.IsAny<int>(), It.IsAny<bool>()))
                .Callback<Equipment, int, bool>((e, o, fo) =>
                {
                    finalOffer = fo;
                    offer = o;
                    if (initialOffer == 0)
                        initialOffer = offer;
                });

            Services.QuickMenu.Setup(x => x.QuickMenuYesNo(true))
                .Returns(() =>
                {
                    if (offer > ask - 5 && finalOffer)
                        return 0;
                    else
                        return 1;
                });

            Services.NumberPicker
                .Setup(x => x.ChooseNumber(It.IsAny<int>()))
                .Returns(() =>
                {
                    int newAsk = Math.Max(ask - 5, offer + 2);

                    if (ask == 0)
                        newAsk = initialOffer * 3;
                    else if (newAsk >= ask)
                        newAsk = ask - 1;

                    ask = newAsk;
                    return newAsk;
                });

            negotiator.NegotiatePrice(eq);

            Assert.AreNotEqual(0, offer);
            Assert.AreNotEqual(0, ask);
            Assert.IsTrue((
                playerGold + offer == Player.Gold ||
                playerGold + ask == Player.Gold) &&
                Math.Abs(ask - offer) < 10);
        }

        [TestMethod]
        public void IncreaseAskPrice()
        {
            int initialOffer = 0;
            int ask = 0;
            int offer = 0;
            int playerGold = Player.Gold;
            bool finalOffer = false;

            Services.BuybackFormatter
                .Setup(x => x.Offer(eq, It.IsAny<int>(), It.IsAny<bool>()))
                .Callback<Equipment, int, bool>((e, o, fo) =>
                {
                    finalOffer = fo;
                    offer = o;
                    if (initialOffer == 0)
                        initialOffer = offer;
                });

            Services.QuickMenu.Setup(x => x.QuickMenuYesNo(true))
                .Returns(1);

            Services.NumberPicker
                .Setup(x => x.ChooseNumber(It.IsAny<int>()))
                .Returns(() =>
                {
                    int newAsk = ask + 1;

                    if (ask == 0)
                        newAsk = initialOffer * 3;

                    ask = newAsk;
                    return newAsk;
                });

            negotiator.NegotiatePrice(eq);

            Assert.AreNotEqual(0, offer);
            Assert.AreNotEqual(0, ask);
            Assert.AreEqual(playerGold, Player.Gold);
        }
    }
}
