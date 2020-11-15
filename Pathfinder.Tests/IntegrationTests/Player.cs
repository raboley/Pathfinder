using Pathfinder.Pathing;
using Pathfinder.People;
using Pathfinder.Travel;

namespace Pathfinder.Tests.IntegrationTests
{
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
            Traveler.GoToPosition(signetNpc.Position);
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