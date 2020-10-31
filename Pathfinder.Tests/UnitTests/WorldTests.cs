using System.Numerics;
using Pathfinder.Pathing;
using Pathfinder.People;
using Pathfinder.Travel;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class WorldTests
    {
        [Fact]
        public void MappingThreadOpens()
        {
            const string map = @"
Visualization of the path
s = start
e = end
w = waypoint
x = obstacle
-------------------------------
|     |     |     |     |  e  |
-------------------------------
|     |     |     |  w  |     |
-------------------------------
|     |     |     |     |     |
-------------------------------
|     |     |     |     |     |
-------------------------------
|  s  |     |     |     |     |
-------------------------------
";

            const string expectedUnknowns = @"
-------------------------------
|  ?  |  ?  |  ?  |  ?  |     |
-------------------------------
|  ?  |  ?  |  ?  |     |  ?  |
-------------------------------
|  ?  |  ?  |     |  ?  |  ?  |
-------------------------------
|  ?  |     |  ?  |  ?  |  ?  |
-------------------------------
|     |  ?  |  ?  |  ?  |  ?  |
-------------------------------
";
            // start the watcher and map
            var peopleCollection = PeopleManagerTests.SetupPeopleCollection();
            var spyActor = new SpyActor();
            var peopleManager = new PeopleManager {People = peopleCollection};
            var watcher = new CollectionWatcher<Person>(peopleManager.People, spyActor);

            // Do something else
            Vector3[] want =
            {
                new Vector3(1f, 0f, 1f),
                new Vector3(2f, 0f, 2f)
            };
            var goal = new Vector3(2, 0, 2);

            var grid = GridSetup.SetupMediumGrid();

            var pathfinding = new Pathfinding();
            pathfinding.Grid = grid;

            var navigator = new Traveler();
            navigator.Position = new Vector3(-2, 0, -2);
            navigator.Pathfinder = pathfinding;

            var got = navigator.PathfindAndWalkToFarAwayWorldMapPosition(goal);

            string actualUnknonws = grid.PrintKnown();
            // string printedPath = grid.PrintPath(got);

            Assert.Equal(expectedUnknowns, actualUnknonws);
            Assert.Equal(goal, navigator.Position);
            Assert.Equal(want.Length, got.Length);
            for (var i = 0; i < want.Length; i++) Assert.Equal(want[i], got[i]);

            // check that the updated map has fewer unknown nodes
            Assert.Equal(want.Length, grid.UnknownNodes.Count);
        }
    }
}