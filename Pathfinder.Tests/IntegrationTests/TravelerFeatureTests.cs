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
            traveler.WalkToZone(end);


            Assert.Equal(end, traveler.CurrentZone.Name);
        }
    }
}