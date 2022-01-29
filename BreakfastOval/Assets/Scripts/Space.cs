using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Space
{
    public Rect Rect;
    public Vector2 Size;
    public List<Tile> Tiles = new List<Tile>();
    public List<Tile> TilesWithWalls = new List<Tile>();
    public List<Tile> TilesWithoutWalls = new List<Tile>();

    public Tile SelectRandomBorderTile()
    {
        int rand = Random.Range(0, TilesWithWalls.Count);
        return TilesWithWalls[rand];
    }

    public static Vector2 GenerateSize(TileType type)
    {
        int minX = 0;
        int minY = 0;
        int maxX = 0;
        int maxY = 0;

        switch(type) {
            case TileType.Room:
                minX = MapGenerator.RoomMinX;
                minY = MapGenerator.RoomMinY;
                maxX = MapGenerator.RoomMaxX;
                maxY = MapGenerator.RoomMaxY;
                break;
            case TileType.Hall:
                minX = MapGenerator.HallMinX;
                minY = MapGenerator.HallMinY;
                maxX = MapGenerator.HallMaxX;
                maxY = MapGenerator.HallMaxY;
                break;
            case TileType.Nothing:
                break;
        }
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    public void ManageWalls(TileType type)
    {
        foreach (Tile t in Tiles)
        {
            CheckAssignNeighbor(t, Direction.North, type);
            CheckAssignNeighbor(t, Direction.East, type);
            CheckAssignNeighbor(t, Direction.South, type);
            CheckAssignNeighbor(t, Direction.West, type);
        }
    }

    void CheckAssignNeighbor(Tile t, Direction dir, TileType type)
    {
        var coordAt = t.GetCoord(dir, 1);
        // Debug.Log($"looking at tile {t.Coord}, checking {dir}");
        if (CheckCoordInSpace(coordAt))
        {
            // Debug.Log($"neighbor {coordAt} was in space");
            var tileToDir = MapGenerator.GetTileFromCoord(coordAt);
            if (tileToDir != null)
            {
                tileToDir.AssignNeighborAt(dir.GetOpposite(), type);
            }
        }
    }

    public bool CheckCoordInSpace(Vector2 coord)
    {
        // Debug.Log($"checking if coord in space {coord}");
        // foreach (Tile t in Tiles)
        // {
        //     Debug.Log($"{t.Coord} is in space");
        // }

        // foreach (Tile t in Tiles.Where(t => Vector2.Equals(t.Coord, coord)))
        // {
        //     Debug.Log($"claiming {t.Coord} is in SPace");
        // }
        // Debug.Log($"returning {Tiles.Where(t => Vector2.Equals(t.Coord, coord)).Count() > 0} if {coord} was in space");
        return Tiles.Where(t => Vector2.Equals(t.Coord, coord)).Count() > 0;
    }
}
