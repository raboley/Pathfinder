using System.Diagnostics;
using System.IO;
using Xunit;

namespace FinalFantasyXI.Tests.UnitTests
{
    public class TravelerMeshTests
    {
        [Fact]
        public void NavDllExists()
        {
            var want = "exists";
            var got = "";

            var dir = Directory.GetCurrentDirectory();

            if (File.Exists("FFXINAV.dll"))
            {
                FileVersionInfo FFXINAVversion = FileVersionInfo.GetVersionInfo("FFXINAV.dll");
                got = string.Format(@"FFXINAV.dll Found: Version: ({0})", FFXINAVversion.FileVersion);
            }

            Assert.Equal(want, got);
        }
    }
}