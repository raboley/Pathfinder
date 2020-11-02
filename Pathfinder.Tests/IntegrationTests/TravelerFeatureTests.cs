using System.Numerics;
using Pathfinder.Pathing;
using Pathfinder.People;
using Pathfinder.Travel;
using Xunit;

namespace Pathfinder.Tests.IntegrationTests
{
    public class TravelerFeatureTests
    {
        [Fact]
        public void GetSignet()
        {
            var player = new Player();
            player.Nation = "Bastok";
            const string start = "A";
            player.Traveler = SetupTraveler(start);

            player.GetSignet();

            Assert.True(player.HasSignet);
        }

        [Fact]
        public void WalkFromWoodsToCity()
        {
            const string start = "A";
            const string end = "D";

            // setup the world
            var traveler = SetupTraveler(start);
            traveler.WalkToZone(end);


            Assert.Equal(end, traveler.CurrentZone.Name);
        }

        private static Traveler SetupTraveler(string start)
        {
            var world = ExampleWorld.Sample();
            var traveler = new Traveler(start, world, Vector3.Zero);
            return traveler;
        }
    }

    public class Player
    {
        public Traveler Traveler { get; set; }
        public bool HasSignet { get; set; }

        public string Nation { get; set; }

        public void GetSignet()
        {
            // Search List of NPCs for Signet NPC
            var signetNpc = Traveler.SearchForClosestSignetNpc(Nation);
            // Walk to Signet NPC zone
            Traveler.WalkToZone(signetNpc.MapName);
            // Interact with them to Get Signet
            Traveler.WalkToPosition(signetNpc.Position);
            // Get signet
            TalkToSignetPerson(signetNpc);
        }

        private void TalkToSignetPerson(Person signetNpc)
        {
            int dist = Pathfinding.GetDistancePos(Traveler.Position, signetNpc.Position);
            if (dist < 3)
                HasSignet = true;
        }
    }
}