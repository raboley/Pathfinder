using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using Pathfinder.Map;
using Pathfinder.Travel;

namespace Pathfinder
{
    public class KnownNodeActor : IActor
    {
        public KnownNodeActor(ZoneMap zoneMap)
        {
            ZoneMap = zoneMap;
        }

        public ZoneMap ZoneMap { get; set; }

        public void Added(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Removed(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Updated(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Position")
            {
                var traveler = sender as Traveler;
                Debug.Assert(traveler != null, nameof(traveler) + " != null");
                ZoneMap.AddKnownNode(traveler.Position);
            }
        }
    }
}