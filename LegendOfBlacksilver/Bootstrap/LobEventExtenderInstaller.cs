using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ERY.Xle.LoB.MapExtenders;
using ERY.Xle.LoB.MapExtenders.Castle;
using ERY.Xle.LoB.MapExtenders.Castle.EventExtenders;
using ERY.Xle.LoB.MapExtenders.Citadel;
using ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders;
using ERY.Xle.LoB.MapExtenders.Labyrinth;
using ERY.Xle.LoB.MapExtenders.Labyrinth.EventExtenders;
using ERY.Xle.LoB.MapExtenders.Outside.Events;
using ERY.Xle.Maps.Extenders;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;
using ERY.Xle.XleEventTypes.Extenders.Common;
using ERY.Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.Bootstrap
{
    public class LobEventExtenderInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<EventExtender>().UsingFactoryMethod<EventExtender>(factory).LifestyleTransient());

            container.Register(Classes.FromAssemblyContaining<LobFactory>()
                .BasedOn<EventExtender>()
                .WithServiceSelf()
                .Configure(c => c.Named(EventName(c.Implementation)))
                .LifestyleTransient());

            container.Register(Classes.FromAssemblyContaining<XleColor>()
                .BasedOn<EventExtender>()
                .WithServiceSelf()
                .Configure(c => c.Named(EventName(c.Implementation)))
                .LifestyleTransient());
        }

        private string EventName(Type type)
        {
            if (type == typeof(StoreRaftExtender))
                return "StoreRaft";

            return type.Name;
        }

        private EventExtender factory(IKernel kernel, CreationContext context)
        {
            var map = context.AdditionalArguments["map"] as MapExtender;
            var evt = context.AdditionalArguments["evt"] as XleEvent;
            var defaultExtender = context.AdditionalArguments["defaultExtender"] as Type;

            var outside = map as OutsideExtender;
            var town = map as TownExtender;
            var castle = map as DurekCastle;
            var lab = map as LabyrinthBase;
            var cit1 = map as CitadelGround;
            var cit2 = map as CitadelUpper;

            if (map is OutsideExtender)
                return CreateOutsideEvent(kernel, outside, evt, defaultExtender);
            if (map is TownExtender)
                return CreateNamedEvent(kernel, evt.ExtenderName);
            if (castle != null)
                return CreateCastleEvent(kernel, castle, evt, defaultExtender);
            if (lab != null)
                return CreateLabyrinthEvent(kernel, lab, evt, defaultExtender);
            if (cit1 != null)
                return CreateCitadel1Event(kernel, lab, evt, defaultExtender);
            if (cit2 != null)
                return CreateCitadel2Event(kernel, cit2, evt, defaultExtender);

            throw new NotImplementedException();
        }

        private EventExtender CreateCitadel1Event(IKernel kernel, LabyrinthBase lab, XleEvent evt, Type defaultExtender)
        {
            if (evt is Door)
                return new CitadelDoor();

            if (evt is ChangeMapEvent)
                return new PasswordTeleporter();

            string name = evt.ExtenderName.ToLowerInvariant();

            switch (name)
            {
                case "wizard": return new Wizard();
                case "jester": return new Jester();
                case "floorpuzzle": return new FloorPuzzle();
                default:
                    return CreateDefaultExtender(kernel, defaultExtender);
            }
        }

        private EventExtender CreateCitadel2Event(IKernel kernel, CitadelUpper cit2, XleEvent evt, Type defaultExtender)
        {        
            string name = evt.ExtenderName.ToLowerInvariant();

            if (name == "mantrek")
            {
                var mantrek = new Mantrek();

                if (Lob.Story.MantrekKilled)
                    mantrek.EraseMantrek(cit2.TheMap);

                return mantrek;
            }
            if (name == "staffportal")
                return new StaffPortal();
            if (name == "elf")
                return new Elf();
            if (name == "tattoo")
                return new Tattoo();

            if (evt is ChangeMapEvent)
                return new ChangeMapTeleporter();

            return CreateDefaultExtender(kernel, defaultExtender);
        }

        private EventExtender CreateLabyrinthEvent(IKernel kernel, LabyrinthBase lab, XleEvent evt, Type defaultExtender)
        {
            if (evt is Door)
                return new LabyrinthDoor();
            if (evt is ChangeMapEvent)
                return new ChangeMapTeleporter();

            return CreateDefaultExtender(kernel, defaultExtender);
        }

        private EventExtender CreateCastleEvent(IKernel kernel, DurekCastle castle, XleEvent evt, Type defaultExtender)
        {
            if (evt is TreasureChestEvent)
                return new Chest { ChestArrayIndex = 0 };
            if (evt is Door)
            {
                if ((evt as Door).RequiredItem == (int)LobItem.FalconFeather)
                    return new FeatherDoor();
                else
                    return new CastleDoor();
            }

            switch (evt.ExtenderName)
            {
                case "King": return new King();
                case "Seravol": return new Seravol();
                case "Arman": return new Arman();
                case "DaisMessage": return new DaisMessage();
                case "AngryOrcs": return new AngryOrcs(castle);
                case "AngryCastle": return new AngryCastle(castle);
                case "SingingCrystal": return new SingingCrystal();
            }

            return CreateDefaultExtender(kernel, defaultExtender);
        }


        private EventExtender CreateNamedEvent(IKernel kernel, string name)
        {
            return kernel.Resolve<EventExtender>(name);
        }

        private EventExtender CreateOutsideEvent(IKernel kernel, OutsideExtender outside, XleEvent evt, Type defaultExtender)
        {
            if (evt is ChangeMapEvent)
            {
                ChangeMapEvent e = (ChangeMapEvent)evt;

                if (e.MapID == 40 || e.MapID == 45)
                {
                    return kernel.Resolve<Drawbridge>();
                }
            }

            if (evt is ChangeMapEvent)
                return kernel.Resolve<ChangeMapQuestion>();
            else
                return CreateDefaultExtender(kernel, defaultExtender);
        }

        private EventExtender CreateDefaultExtender(IKernel kernel, Type defaultExtender)
        {
            return (EventExtender)kernel.Resolve(defaultExtender);
        }
    }
}