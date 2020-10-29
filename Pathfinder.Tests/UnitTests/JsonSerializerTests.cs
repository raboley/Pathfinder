using Newtonsoft.Json;
using Pathfinder.WorldMap;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class JsonSerializerTests
    {
        [Fact]
        public void TestSerializeNodeToJson()
        {
            var want = GridSetup.SetupSmallGrid();

            string json = JsonConvert.SerializeObject(want);
            var got = JsonConvert.DeserializeObject<Grid>(json);

            GridSetup.AssertGridMapEqual(want.MapGrid, got.MapGrid);
        }
    }
}