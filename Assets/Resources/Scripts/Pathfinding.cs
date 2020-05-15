using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding Instance { get; private set; }

    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    public Pathfinding(int width, int height, Vector2 origin)
    {
        Instance = this;
        grid = new Grid<PathNode>(width, height, 1f, origin, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public List<Vector2> FindPath(Vector2 startPos, Vector2 endPos)
    {
        grid.GetXY(startPos, out int startX, out int startY);
        grid.GetXY(endPos, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null)
            return null;
        else
        {
            List<Vector2> vectorPath = new List<Vector2>();
            foreach (PathNode node in path)
            {
                vectorPath.Add(new Vector3(node.x, node.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * 0.5f + grid.GetOrigin());
            }
            return vectorPath;
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        if (startNode == null || endNode == null)
            return null;

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode node = grid.GetGridObject(x, y);
                node.g = int.MaxValue;
                node.CalculateF();
                node.parent = null;
            }
        }

        startNode.g = 0;
        startNode.h = Heuristic(startNode, endNode);
        startNode.CalculateF();

        while (openList.Count > 0)
        {
            PathNode current = GetBestNode(openList);
            if (current == endNode)
                return CalculatePath(endNode);

            openList.Remove(current);
            closedList.Add(current);

            foreach (PathNode neighbour in GetNeighbours(current))
            {
                if (closedList.Contains(neighbour))
                    continue;
                if (!neighbour.isWalkable)
                {
                    closedList.Add(neighbour);
                    continue;
                }

                int tentativeG = current.g + Heuristic(current, neighbour);
                if (tentativeG < neighbour.g)
                {
                    neighbour.parent = current;
                    neighbour.g = tentativeG;
                    neighbour.h = Heuristic(neighbour, endNode);
                    neighbour.CalculateF();

                    if (!openList.Contains(neighbour))
                        openList.Add(neighbour);
                }
            }
        }

        // openList is empty
        return null;
    }

    private List<PathNode> GetNeighbours(PathNode n)
    {
        List<PathNode> neighbours = new List<PathNode>();

        if (n.x - 1 >= 0)
        {
            neighbours.Add(grid.GetGridObject(n.x - 1, n.y));

            if (n.y - 1 >= 0)
                neighbours.Add(grid.GetGridObject(n.x - 1, n.y - 1));

            if (n.y + 1 < grid.GetHeight())
                neighbours.Add(grid.GetGridObject(n.x - 1, n.y + 1));
        }
        if (n.x + 1 < grid.GetWidth())
        {
            neighbours.Add(grid.GetGridObject(n.x + 1, n.y));

            if (n.y - 1 >= 0)
                neighbours.Add(grid.GetGridObject(n.x + 1, n.y - 1));

            if (n.y + 1 < grid.GetHeight())
                neighbours.Add(grid.GetGridObject(n.x + 1, n.y + 1));
        }
        if (n.y - 1 >= 0)
            neighbours.Add(grid.GetGridObject(n.x, n.y - 1));
        if (n.y + 1 < grid.GetHeight())
            neighbours.Add(grid.GetGridObject(n.x, n.y + 1));

        return neighbours;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode> { endNode };
        PathNode current = endNode;
        while (current.parent != null)
        {
            path.Add(current.parent);
            current = current.parent;
        }
        path.Reverse();

        return path;
    }

    private int Heuristic(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetBestNode(List<PathNode> nodes)
    {
        PathNode bestNode = nodes[0];

        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].f < bestNode.f)
                bestNode = nodes[i];
        }

        return bestNode;
    }

    public Grid<PathNode> GetGrid()
    {
        return grid;
    }
}
