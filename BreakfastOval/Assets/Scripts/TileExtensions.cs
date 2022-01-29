using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileExtensions
{
    public static Vector2 Invalid = new Vector2(-1, -1);

    public static List<Direction> AllDirections()
    {
        var res = new List<Direction>();
        res.Add(Direction.North);
        res.Add(Direction.East);
        res.Add(Direction.South);
        res.Add(Direction.West);
        return res;
    }

    public static Vector2 GetCoord(this Tile tile, Direction dir, int dist)
    {
        int xDiff = 0;
        int yDiff = 0;
        switch (dir)
        {
            case Direction.North:
                yDiff = dist;
                break;
            case Direction.East:
                xDiff = dist;
                break;
            case Direction.South:
                yDiff = -dist;
                break;
            case Direction.West:
                xDiff = -dist;
                break;
        }

        return new Vector2(tile.Coord.x + xDiff, tile.Coord.y + yDiff);
    }

    public static Direction GetOpposite(this Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return Direction.South;
            case Direction.East:
                return Direction.West;
            case Direction.South:
                return Direction.North;
            case Direction.West:
                return Direction.East;
            default:
                return Direction.Error;
        }
    }
}
