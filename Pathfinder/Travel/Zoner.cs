using System.ComponentModel;
using System.Diagnostics;

namespace Pathfinder.Travel
{
    public class Zoner
    {
        public Zoner(IWalker walker, Traveler traveler, World world)
        {
            Walker = walker;
            Traveler = traveler;
            World = world;
        }

        public IWalker Walker { get; set; }
        public Traveler Traveler { get; set; }
        public World World { get; set; }

        public void ZoneWalker(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentPosition")
            {
                var walker = sender as IWalker;
                Debug.Assert(walker != null, nameof(walker) + " != null");
                var position = walker.CurrentPosition;
                if (Traveler.AllBorderZonePoints.Contains(position) && walker.Zoning == false)
                {
                    var boundary = Traveler.GetZoneBorderToNameFromPoint(position);
                    Traveler.CurrentZone = World.GetZoneByName(boundary.ToZone);
                    Walker.CurrentPosition = boundary.ToPosition;
                    Walker.Zoning = true;
                }
            }
        }
    }
}