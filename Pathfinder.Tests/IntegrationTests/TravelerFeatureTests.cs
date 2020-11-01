using System.Numerics;
using Pathfinder.Travel;
using Xunit;

namespace Pathfinder.Tests.IntegrationTests
{
    public class TravelerFeatureTests
    {
        // [Fact]
        // public void GetSignet()
        // {
        //     Assert.Equal(want, got);
        // }

        [Fact]
        public void WalkFromWoodsToCity()
        {
            const string start = "A";
            const string end = "D";

            // setup the world
            var world = World.Sample();

            // Setup traveler in a zone, and World
            var traveler = new Traveler(start, world, Vector3.Zero);

            // setup the world pathfinder
            var worldPathfinder = new WorldPathfinder {World = world};

            // Find zones to travel through from here to there
            worldPathfinder.FindWorldPathToZone(start, end);

            // have traveler walk from here to there
            foreach (var zone in worldPathfinder.ZonesToTravelThrough)
            {
                traveler.GoToZone(zone.Name);
            }

            Assert.Equal(end, traveler.CurrentZoneName);
        }
    }
}