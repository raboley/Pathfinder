using System.Numerics;
using Pathfinder.Map;
using Pathfinder.Persistence;
using Pathfinder.Travel;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class KnownNodeActorTests
    {
        [Fact(Skip = "don't think this is useful. Would be nice to save on every new known node, but oh well.")]
        public void KnownNodeActorCanUpdateKnownNodes()
        {
            var map = SetupZoneMap.SetupSmallGrid();
            string mapName = SetupPersister.GetCurrentMethodName();
            var persister = new FilePersister(mapName);
            var actor = new KnownNodeActor(persister, map);
            var traveler = new Traveler();
            var watcher = new Watcher(traveler, actor);

            var newPos = new Vector3(1, 0, 1);
            traveler.Position = newPos;

            var wantGrid = SetupZoneMap.SetupSmallGrid();
            wantGrid.AddKnownNode(newPos);

            var want = wantGrid.UnknownNodes;
            var got = persister.Load<ZoneMap>();

            Assert.Equal(want.Count, got.UnknownNodes.Count);
            for (var i = 0; i < want.Count; i++) Assert.Equal(want[i], got.UnknownNodes[i]);
        }
    }
}