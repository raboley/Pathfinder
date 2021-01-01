using System;
using System.IO;
using FinalFantasyXI.XPathfinder;
using NUnit.Framework;

namespace FFXI.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
            Assert.True(true);
        }
        
        [Test]
        public void NavDllExists()
        {
            var expectedDllPath = "FFXINAV.dll";

            var got = File.Exists(expectedDllPath);

            Assert.True(got);
        }
        
        [Test]
        public void CanCreateNavClass()
        {
            var nav  = new FFXINAV();

            var dir = Directory.GetCurrentDirectory();
            
            Assert.NotNull(nav);
            // Assert.Equal(want, got);
        }
    }
}