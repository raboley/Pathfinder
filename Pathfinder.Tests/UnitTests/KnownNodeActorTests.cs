using System.Numerics;
using Pathfinder.Travel;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class KnownNodeActorTests
    {
        [Fact]
        public void KnownNodeActorCanUpdateKnownNodes()
        {
            var map = SetupZoneMap.SetupSmallGrid();
            var actor = new KnownNodeActor(map);
            var traveler = new Traveler();
            var watcher = new Watcher(traveler, actor);

            var newPos = new Vector3(1, 0, 1);
            traveler.Position = newPos;

            var wantGrid = SetupZoneMap.SetupSmallGrid();
            wantGrid.AddKnownNode(newPos);

            var want = wantGrid.UnknownNodes;
            var got = map.UnknownNodes;


            SetupZoneMap.AssertListGridNodesEqual(want, got);
        }
    }
}