using System.ComponentModel;
using System.Diagnostics;
using Pathfinder.Map.WorldMap;
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

        public void Added()
        {
            throw new System.NotImplementedException();
        }

        public void Removed()
        {
            throw new System.NotImplementedException();
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