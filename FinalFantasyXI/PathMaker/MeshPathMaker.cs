using System.IO;
using System.Numerics;
using FinalFantasyXI.XPathfinder;

namespace FinalFantasyXI.PathMaker
{
    public interface IPathMaker
    {
        bool Load(string zoneName);

        // bool CanSeeDestination(Vector3 start, Vector3 end);
        // bool unload();
        // bool unloadMeshBuilder();
        // void FindPath(Vector3 start, Vector3 end);
        // void FindClosestPath(Vector3 start, Vector3 end);
        // double GetDistanceToWall();
        // sbyte GetRotation(Vector3 start, Vector3 end);
        // bool isNavMeshEnabled();
        // bool IsValidPosition(Vector3 position);
    }

    public class XPathMaker : IPathMaker
    {
        private FFXINAV _ffxinav;
        public string NavMeshDirectory;

        public XPathMaker()
        {
            _ffxinav = new FFXINAV();
        }

        public bool Load(string zoneName)
        {
            var fullPath = Path.Combine(NavMeshDirectory, zoneName);
            if (!Directory.Exists(fullPath))
                throw new FileNotFoundException("Couldn't find navmesh for zone: " + zoneName + " at path: " +
                                                fullPath);

            _ffxinav.Load(fullPath);
            return _ffxinav.IsNavMeshEnabled();
        }
    }
}