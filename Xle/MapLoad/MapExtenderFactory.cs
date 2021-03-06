﻿using AgateLib;
using AgateLib.Foundation;
using Xle.Maps;

namespace Xle.MapLoad
{
    public interface IMapExtenderFactory
    {
        IMapExtender CreateMapExtender(XleMap map);
    }

    [Singleton]
    public class MapExtenderFactory : IMapExtenderFactory
    {
        private readonly IAgateServiceLocator serviceLocator;

        public MapExtenderFactory(IAgateServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public IMapExtender CreateMapExtender(XleMap map)
        {
            var result = serviceLocator.ResolveNamed<IMapExtender>(map.ExtenderName);

            result.TheMap = map;

            return result;
        }
    }
}
