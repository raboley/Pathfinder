using System;
using System.IO;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;
using Pathfinder.Map;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class NodeJsonConverter : JsonConverter<Node>
    {
        public static string SerializeNode(Node node)
        {
            // JObject jsonNode = new JObject();
            // JProperty jX = new JProperty("x", GridMath.ConvertFromFloatToInt(node.X));
            // jsonNode.Add(jX);
            // return jsonNode.ToString();

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            using (JsonTextWriter writer = new JsonTextWriter(sw))
            {
                writer.QuoteChar = '\'';
                writer.Formatting = Formatting.Indented;

                JsonSerializer ser = new JsonSerializer();
                ser.Serialize(writer, node);
            }

            return sb.ToString();
        }

        public override void WriteJson(JsonWriter writer, Node value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override Node ReadJson(JsonReader reader, Type objectType, Node existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
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
  'X': 0,
  'Y': 0,
  'Z': 0,
  'Unknown': true,
  'Walkable': true
}";


            var node = new Node(Vector3.Zero, true, true);

            string got = NodeJsonConverter.SerializeNode(node);

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