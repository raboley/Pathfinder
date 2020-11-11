using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using Pathfinder.Map;
using Pathfinder.Persistence;
using Pathfinder.Travel;

namespace Pathfinder
{
    public class KnownNodeActor : IActor
    {
        public KnownNodeActor(IPersister persister, ZoneMap zoneMap)
        {
            ZoneMap = zoneMap;
            Persister = persister;
        }

        public IPersister Persister { get; set; }
        public ZoneMap ZoneMap { get; set; }

        public void Added(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                Persister.Save(ZoneMap);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public void Removed(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                Persister.Save(ZoneMap);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
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