using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Grid<PathNode> grid;
    public readonly int x, y;

    public int g, h, f;
    public bool isWalkable;
    public PathNode parent;

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    public override string ToString()
    {
        return x + "," + y;
    }

    public void CalculateF()
    {
        f = g + h;
    }
}
