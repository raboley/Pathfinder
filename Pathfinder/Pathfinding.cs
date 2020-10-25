using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Numerics;

namespace Pathfinder.Pathfinder
{
    public class Pathfinding 
    {
    public Grid Grid;
    // PathRequestManager requestManager;
    // void Awake() {
    //     requestManager = GetComponent<PathRequestManager>();
    //     MapGrid = GetComponent<Grid>();
    // }
    //
    // public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
    //     StartCoroutine(FindPath(startPos, targetPos));
    // }
    //
    public IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        yield return FindWaypoints(startPos, targetPos);
        // requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    public Vector3[] FindWaypoints(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        GridNode startGridNode = Grid.NodeFromWorldPoint(startPos);
        GridNode targetGridNode = Grid.NodeFromWorldPoint(targetPos);

        if (startGridNode.Walkable && targetGridNode.Walkable)
        {
            Heap<GridNode> openSet = new Heap<GridNode>(Grid.MaxSize);
            HashSet<GridNode> closedSet = new HashSet<GridNode>();

            openSet.Add(startGridNode);

            while (openSet.Count > 0)
            {
                GridNode currentGridNode = openSet.RemoveFirst();
                closedSet.Add(currentGridNode);


                if (currentGridNode == targetGridNode)
                {
                    sw.Stop();
                    Console.WriteLine("Path found: " + sw.ElapsedMilliseconds + "ms");
                    pathSuccess = true;
                    break;
                }

                foreach (GridNode neighbour in Grid.GetNeighbours(currentGridNode))
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentGridNode.GCost + GetDistance(currentGridNode, neighbour);

                    if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetGridNode);
                        neighbour.Parent = currentGridNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        }

        if (!pathSuccess)
        {
            return null;
        }
        waypoints = RetracePath(startGridNode, targetGridNode);
        return waypoints;
    }

    Vector3[] RetracePath(GridNode startGridNode, GridNode endGridNode) {
        List<GridNode> path = new List<GridNode>();
        GridNode currentGridNode = endGridNode;
    
        while (currentGridNode != startGridNode) {
            path.Add(currentGridNode);
            currentGridNode = currentGridNode.Parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    
    }
    
    Vector3[] SimplifyPath(List<GridNode> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.Zero;
    
        for (int i = 1; i < path.Count; i++) {
            Vector2 directionNew = new Vector2(path[i-1].GridX - path[i].GridX, path[i-1].GridY - path[i].GridY);
            if (directionNew != directionOld) {
                waypoints.Add(path[i].WorldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }
    
    int GetDistance(GridNode gridNodeA, GridNode gridNodeB) {
        int dstX = Math.Abs(gridNodeA.GridX - gridNodeB.GridX);
        int dstY = Math.Abs(gridNodeA.GridY - gridNodeB.GridY);
    
        if (dstX > dstY) {
            return 14*dstY + 10* (dstX-dstY);
        }
    
        return 14*dstX + 10 * (dstY-dstX);
    }
    }
}
