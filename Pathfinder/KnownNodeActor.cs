using System.ComponentModel;
using System.Diagnostics;
using Pathfinder.Travel;
using Pathfinder.WorldMap;

namespace Pathfinder
{
    public class KnownNodeActor : IActor
    {
        public KnownNodeActor(Grid grid)
        {
            Grid = grid;
        }

        public Grid Grid { get; set; }

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
                Grid.AddKnownNode(traveler.Position);
            }
        }
    }
}