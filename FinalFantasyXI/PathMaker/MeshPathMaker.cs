using System.Numerics;

namespace FinalFantasyXI.PathMaker
{
    public interface PathMaker
    {
        bool CanSeeDestination(Vector3 start, Vector3 end);
        bool unload();
        bool unloadMeshBuilder();
        void FindPath(Vector3 start, Vector3 end);
        void FindClosestPath(Vector3 start, Vector3 end);
        double GetDistanceToWall();
        sbyte GetRotation(Vector3 start, Vector3 end);
        bool isNavMeshEnabled();
        bool IsValidPosition(Vector3 position);
    }

    public class MeshPathMaker
    {
    }
}