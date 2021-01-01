using System;
using System.IO;
using System.Security.Policy;
using FinalFantasyXI.XPathfinder;

namespace ffxi
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var expectedDllPath = "FFXINAV.dll";
            var got = File.Exists(expectedDllPath);
            Console.WriteLine("Was found: " + got);
            
            var nav = new FFXINAV();
            
        }
    }
}