using System.IO;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;
using Pathfinder.Map;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class JsonSerializer
    {
        public static string Serialize<T>(T thing)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            using (JsonTextWriter writer = new JsonTextWriter(sw))
            {
                writer.QuoteChar = '\'';
                writer.Formatting = Formatting.Indented;

                Newtonsoft.Json.JsonSerializer ser = new Newtonsoft.Json.JsonSerializer();
                ser.Serialize(writer, thing);
            }

            return sb.ToString();
        }
    }

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

        [Fact]
        public void SerializeTurnsNodesIntoCoordsWithProps()
        {
            string want = @"{
  'GridX': 0,
  'GridY': 0,
  'X': 0,
  'Y': 0,
  'Z': 0,
  'Unknown': true,
  'Walkable': true
}";


            var node = new Node(Vector3.Zero, true, true);

            string got = JsonSerializer.Serialize(node);

            Assert.Equal(want, got);
        }

        [Fact]
        public void SerializeMakesACleanMap()
        {
            string want = @"{
  'MapGrid': [
    [
      {
        'GridX': 0,
        'GridY': 0,
        'X': -1,
        'Y': 0,
        'Z': -1,
        'Unknown': true,
        'Walkable': true
      },
      {
        'GridX': 0,
        'GridY': 1,
        'X': -1,
        'Y': 0,
        'Z': 0,
        'Unknown': true,
        'Walkable': true
      },
      {
        'GridX': 0,
        'GridY': 2,
        'X': -1,
        'Y': 0,
        'Z': 1,
        'Unknown': true,
        'Walkable': true
      }
    ],
    [
      {
        'GridX': 1,
        'GridY': 0,
        'X': 0,
        'Y': 0,
        'Z': -1,
        'Unknown': true,
        'Walkable': true
      },
      {
        'GridX': 1,
        'GridY': 1,
        'X': 0,
        'Y': 0,
        'Z': 0,
        'Unknown': true,
        'Walkable': true
      },
      {
        'GridX': 1,
        'GridY': 2,
        'X': 0,
        'Y': 0,
        'Z': 1,
        'Unknown': true,
        'Walkable': true
      }
    ],
    [
      {
        'GridX': 2,
        'GridY': 0,
        'X': 1,
        'Y': 0,
        'Z': -1,
        'Unknown': true,
        'Walkable': true
      },
      {
        'GridX': 2,
        'GridY': 1,
        'X': 1,
        'Y': 0,
        'Z': 0,
        'Unknown': true,
        'Walkable': true
      },
      {
        'GridX': 2,
        'GridY': 2,
        'X': 1,
        'Y': 0,
        'Z': 1,
        'Unknown': true,
        'Walkable': true
      }
    ]
  ],
  'MapName': 'Small ZoneMap'
}";


            var map = SetupZoneMap.SetupSmallGrid();
            string got = JsonSerializer.Serialize(map);

            Assert.Equal(want, got);
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