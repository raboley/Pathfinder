using System.ComponentModel;
using System.Diagnostics;

namespace Pathfinder.Travel
{
    public class Zoner
    {
        public Zoner(Traveler traveler, World world)
        {
            Traveler = traveler;
            Traveler.Walker.PropertyChanged += ZoneWalker;
            World = world;
        }

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
                    Traveler.Walker.Zoning = true;
                    Traveler.Walker.CurrentPosition = boundary.ToPosition;
                    Traveler.Position = boundary.ToPosition;
                    Traveler.CurrentZone = World.GetZoneByName(boundary.ToZone);
                }
            }
        }
    }
}