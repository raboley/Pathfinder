using Newtonsoft.Json;
using Pathfinder.Map;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class JsonSerializerTests
    {
        [Fact]
        public void TestSerializeZoneMapToJson()
        {
            var want = SetupZoneMap.SetupSmallGrid();

            var serializer = new ZoneMapSerializer();
            string json = serializer.Serialize(want);
            var got = serializer.DeSerialize(json);

            SetupZoneMap.AssertGridMapEqual(want.MapGrid, got.MapGrid);
        }
    }

    public class ZoneMapSerializer
    {
        public string Serialize(ZoneMap map)
        {
            return JsonConvert.SerializeObject(map, Formatting.Indented, new NodeConverter());
        }


        public ZoneMap DeSerialize(string json)
        {
            return JsonConvert.DeserializeObject<ZoneMap>(json);
        }
    }
}