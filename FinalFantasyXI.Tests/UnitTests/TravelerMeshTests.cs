using System.IO;
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
    }
}