using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Space
{
    public Rect Rect;
    public List<Tile> TilesWithDoors = new List<Tile>();
    public List<Tile> Tiles = new List<Tile>();
    public List<Tile> TilesWithWalls = new List<Tile>();
    public List<Tile> TilesWithoutWalls = new List<Tile>();

    public void Reset()
    {
        Tiles = new List<Tile>();
        TilesWithDoors = new List<Tile>();
        TilesWithWalls = new List<Tile>();
        TilesWithoutWalls = new List<Tile>();
    }

    public Tile SelectEquivalentTileFromCoord(Space oldSpace, Vector2 oldCoord)
    {
        var xDiff = oldSpace.Rect.xMax - Rect.xMax;
        var yDiff = oldSpace.Rect.yMax - Rect.yMax;
        int x = (int)oldCoord.x - (int)xDiff;
        int y = (int)oldCoord.y - (int)yDiff;

        if (new Vector2(x, y).InMapBounds())
        {
            return MapGenerator.Map[x, y];
        }
        else
        {
            return null;
        }
    }

    public Tile SelectBorderTileForHall()
    {
        TilesWithWalls = Tiles.Where(t => t.Walls.Count > 0).ToList();
        var validWalls = TilesWithWalls.Where(t => t.CanCreateHallNeighbor() && t.Doors.Count == 0).ToArray();
        // var validWalls = TilesWithWalls.Where(t => t.CanCreateHallNeighbor()).ToArray();
        if (validWalls.Length == 0)
        {
            return null;
        }
        int rand = Random.Range(0, validWalls.Length);
        return validWalls[rand];
    }

    public void AssignDoorAt(Tile t, Direction dir, bool first = true)
    {
        TilesWithDoors.Add(t);
        t.AssignDoorAt(dir, first);
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
                if (!TilesWithWalls.Contains(t))
                {
                    TilesWithWalls.Add(t);
                }
                if (TilesWithoutWalls.Contains(t))
                {
                    TilesWithoutWalls.Remove(t);
                }
            }
            else
            {
                if (!TilesWithoutWalls.Contains(t))
                {
                    TilesWithoutWalls.Add(t);
                }
                if (TilesWithWalls.Contains(t))
                {
                    TilesWithWalls.Remove(t);
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="t"></param>
    /// <param name="dir"></param>
    /// <param name="type"></param>
    /// <returns>True if this tile will have a wall in given direction</returns>
    bool CheckAssignNeighbor(Tile t, Direction dir, TileType type)
    {
        bool res = true;
        var coordAt = t.GetCoord(dir, 1);
        if (CheckCoordInSpace(coordAt))
        {
            var tileToDir = MapGenerator.GetTileFromCoord(coordAt);
            if (tileToDir != null)
            {
                res = tileToDir.AssignNeighborAt(dir.GetOpposite(), type);
            }
        }

        return res;
    }

    public bool CheckCoordInSpace(Vector2 coord)
    {
        return Tiles.Where(t => t.Coord ==  coord).Count() > 0;
    }

    public List<Tile> GetUnoccupiedMiddleTiles()
    {
        return TilesWithoutWalls.Where(t => !t.occupied).ToList();
    }

    public List<Tile> GetUnoccupiedWallTiles()
    {
        return TilesWithWalls.Where(t => !t.occupied).ToList();
    }

    public List<Tile> GetAllUnoccupiedTiles()
    {
        return Tiles.Where(t => !t.occupied).ToList();
    }
}
