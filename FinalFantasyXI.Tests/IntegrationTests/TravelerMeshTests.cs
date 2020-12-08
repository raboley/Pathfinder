using System.IO;
using FinalFantasyXI.XPathfinder;
using Xunit;

namespace FinalFantasyXI.Tests.UnitTests
{
    public class TravelerMeshTests
    {
        [Fact]
        public void NavDllExists()
        {
            var expectedDllPath = "FFXINAV.dll";

            var got = File.Exists(expectedDllPath);

            Assert.True(got);
        }

        [Fact]
        public void CanCreateNavClass()
        {
            var nav = new FFXINAV();

            var dir = Directory.GetCurrentDirectory();

            Assert.NotNull(nav);
            // Assert.Equal(want, got);
        }
    }
}