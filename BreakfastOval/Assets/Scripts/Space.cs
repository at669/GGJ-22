using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Space
{
    public Rect Rect;
    public List<Tile> Tiles = new List<Tile>();
    public List<Tile> TilesWithWalls = new List<Tile>();
    public List<Tile> TilesWithoutWalls = new List<Tile>();

    public Tile SelectBorderTileForHall()
    {
        var validWalls = TilesWithWalls.Where(t => t.CanCreateHallNeighbor()).ToArray();
        int rand = Random.Range(0, validWalls.Length);
        return validWalls[rand];
    }

    public void ManageWalls(TileType type)
    {
        foreach (Tile t in Tiles)
        {
            bool hasWalls = false;
            hasWalls = CheckAssignNeighbor(t, Direction.North, type) || hasWalls;
            hasWalls = CheckAssignNeighbor(t, Direction.East, type) || hasWalls;
            hasWalls = CheckAssignNeighbor(t, Direction.South, type) || hasWalls;
            hasWalls = CheckAssignNeighbor(t, Direction.West, type) || hasWalls;
            if (hasWalls)
            {
                TilesWithWalls.Add(t);
            }
            else
            {
                TilesWithoutWalls.Add(t);
            }
        }
    }

    bool CheckAssignNeighbor(Tile t, Direction dir, TileType type)
    {
        var coordAt = t.GetCoord(dir, 1);
        if (CheckCoordInSpace(coordAt))
        {
            var tileToDir = MapGenerator.GetTileFromCoord(coordAt);
            if (tileToDir != null)
            {
                return tileToDir.AssignNeighborAt(dir.GetOpposite(), type);
            }
        }
        return true;
    }

    public bool CheckCoordInSpace(Vector2 coord)
    {
        return Tiles.Where(t => Vector2.Equals(t.Coord, coord)).Count() > 0;
    }
}
