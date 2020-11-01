using Newtonsoft.Json;
using Pathfinder.Map;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class JsonSerializerTests
    {
        [Fact]
        public void TestSerializeNodeToJson()
        {
            var want = SetupZoneMap.SetupSmallGrid();

            string json = JsonConvert.SerializeObject(want);
            var got = JsonConvert.DeserializeObject<ZoneMap>(json);

            SetupZoneMap.AssertGridMapEqual(want.MapGrid, got.MapGrid);
        }
    }
}