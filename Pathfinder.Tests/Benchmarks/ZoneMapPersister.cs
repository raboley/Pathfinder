using System;
using System.IO;
using Pathfinder.Map;
using Pathfinder.Persistence;
using Pathfinder.Tests.UnitTests;
using Xunit;

namespace Pathfinder.Tests.Benchmarks
{
    public class ZoneMapPersisterTests
    {
        [Fact]
        public void BenchmarkZoneTextPersister()
        {
            int time = Benchmark.Run("StreamReader.ReadToEnd", 5, () =>
            {
                const string want = @"
-------------------
|  ?  |  ?  |  ?  |
-------------------
|  ?  |     |     |
-------------------
|     |  ?  |     |
-------------------
";
                var persister = SetupPersister.SetupTestFileTextPersister();
                persister.Save(want);

                string got = persister.Load<string>();
                persister.Delete();
            });

            int benchTime = 5;
            if (time > benchTime)
            {
                Assert.True(false);
            }
        }


        [Fact]
        public void BenchmarkSerializeZoneMap()
        {
            var want = SetupZoneMap.SetupSmallGrid();

            var serializer = new ZoneMapSerializer();

            int time = Benchmark.Run("StreamReader.ReadToEnd", 5, () =>
            {
                string json = serializer.Serialize(want);
                var got = serializer.DeSerialize(json);
            });

            int benchTime = 1;
            Assert.True(time < benchTime,
                "time per iteration was: " + time + "ms which is greater than expected time of: " + benchTime + "ms");
        }

        [Fact]
        public void BenchmarkSerializeThenPersistZoneMap()
        {
            var want = SetupZoneMap.SetupBigGrid();

            var serializer = new ZoneMapSerializer();
            var persister = SetupPersister.SetupTestFileTextPersister();
            string mapName = SetupPersister.GetCurrentMethodName();
            persister.FileName = mapName;

            int time = Benchmark.Run("StreamReader.ReadToEnd", 5, () =>
            {
                string json = serializer.Serialize(want);
                persister.Save(json);

                string got = persister.Load<string>();
                persister.Delete();
                var map = serializer.DeSerialize(got);
            });

            int benchTime = 20;
            Assert.True(time < benchTime,
                "time per iteration was: " + time + "ms which is greater than expected time of: " + benchTime + "ms");
        }

        [Fact]
        public void BenchmarkZoneMapBigGrid()
        {
            string mapName = SetupPersister.GetCurrentMethodName();
            var want = SetupZoneMap.SetupBigGrid();
            var persister = new FilePersister(mapName);
            // Path assumes to start from ./debug/ so we want to set it to the test fixtures dir.
            string grandParentDirectory = Directory.GetParent(persister.FilePath).FullName;
            string parentDirectory = Directory.GetParent(grandParentDirectory).FullName;
            persister.FilePath = Path.Combine(parentDirectory, "fixtures");

            int time = Benchmark.Run("StreamReader.ReadToEnd", 5, () =>
            {
                persister.Save(want);
                var got = persister.Load<ZoneMap>();
                persister.Delete();
            });

            int benchTime = 20;
            Assert.True(time < benchTime,
                "time per iteration was: " + time + "ms which is greater than expected time of: " + benchTime + "ms");
        }

        [Fact(Skip = "takes a while only run if you want to test run time")]
        public void BenchmarkZoneMapSuperBig()
        {
            string mapName = SetupPersister.GetCurrentMethodName();
            var want = SetupZoneMap.SetupSuperBigGrid();
            var persister = new FilePersister(mapName);
            // Path assumes to start from ./debug/ so we want to set it to the test fixtures dir.
            string grandParentDirectory = Directory.GetParent(persister.FilePath).FullName;
            string parentDirectory = Directory.GetParent(grandParentDirectory).FullName;
            persister.FilePath = Path.Combine(parentDirectory, "fixtures");

            int time = Benchmark.Run("StreamReader.ReadToEnd", 5, () =>
            {
                persister.Save(want);
                var got = persister.Load<ZoneMap>();
                persister.Delete();
            });

            int benchTime = 600;
            Assert.True(time < benchTime,
                "time per iteration was: " + time + "ms which is greater than expected time of: " + benchTime + "ms");
        }
    }

    public class ZoneMapPersister : IPersister
    {
        public ZoneMapPersister(string mapName)
        {
            MapName = mapName;
        }

        public string MapName { get; set; }

        public void Save<T>(T serializableData)
        {
            throw new NotImplementedException();
        }

        public T Load<T>()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public bool Exists()
        {
            throw new NotImplementedException();
        }
    }
}