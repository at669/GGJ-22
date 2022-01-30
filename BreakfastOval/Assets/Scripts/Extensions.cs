using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum RoomType { Lobby, Kitchen, IT, HR, Engineering, Marketing, Finance, Bathroom, Custodial, Security }
public enum Direction { Error, North, East, South, West }
public enum TileType { Error, Room, Hall, Nothing }
public enum RelativeDirection { Error, Front, Left, Right, Back }
public enum FurnitureType { Error, Middle, Wall, Ceiling, AnyFloor }

public static class Extensions
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
        if (tile == null)
        {
            Debug.LogError("Input null tile!");
        }
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

    public static Direction RandomWeightedDirection(this Direction fromDir, List<Direction> options)
    {
        if (options.Count == 0)
        {
            return Direction.Error;
        }

        if (options.Count == 1)
        {
            return options[0];
        }

        float rand = Random.Range(0f, 1f);
        if (options.Contains(fromDir.GetOpposite()))
        {
            if (options.Count == 3)
            {
                return RandomDirection(fromDir);
            }
            else // if (options.Count == 2)
            {
                if (rand < 0.5f)
                {
                    return options[0];
                }
                else {
                    return options[1];
                }
            }
        }
        else
        {
            if (rand < 0.5f)
            {
                return options[0];
            }
            else {
                return options[1];
            }
        }
    }

    public static Direction RandomDirection(this Direction fromDir)
    {
        var dir = AllDirections();
        dir.Remove(fromDir);
        return fromDir.DirectionFromRelative(RandomRelativeDirection());
    }

    static RelativeDirection RandomRelativeDirection()
    {
        var rand = Random.Range(0f, 1f);
        if (rand < 0.6f)
        {
            return RelativeDirection.Front;
        }
        else if (rand < 0.8f)
        {
            return RelativeDirection.Left;
        }
        else
        {
            return RelativeDirection.Right;
        }
    }

    static Direction DirectionFromRelative(this Direction from, RelativeDirection rel)
    {
        switch (from)
        {
            case Direction.North:
                switch (rel)
                {
                    case RelativeDirection.Front:
                        return Direction.North;
                    case RelativeDirection.Left:
                        return Direction.West;
                    case RelativeDirection.Right:
                        return Direction.East;
                    case RelativeDirection.Back:
                        return Direction.South;
                }
                break;
            case Direction.East:
              switch (rel)
                {
                    case RelativeDirection.Front:
                        return Direction.East;
                    case RelativeDirection.Left:
                        return Direction.South;
                    case RelativeDirection.Right:
                        return Direction.North;
                    case RelativeDirection.Back:
                        return Direction.West;
                }
                break;
            case Direction.South:
                switch (rel)
                {
                    case RelativeDirection.Front:
                        return Direction.South;
                    case RelativeDirection.Left:
                        return Direction.East;
                    case RelativeDirection.Right:
                        return Direction.West;
                    case RelativeDirection.Back:
                        return Direction.North;
                }
                break;
            case Direction.West:
                switch (rel)
                {
                    case RelativeDirection.Front:
                        return Direction.West;
                    case RelativeDirection.Left:
                        return Direction.South;
                    case RelativeDirection.Right:
                        return Direction.North;
                    case RelativeDirection.Back:
                        return Direction.East;
                }
                break;
        }
        return Direction.Error;
    }

    static RelativeDirection RelativeFromDirection(this Direction from, Direction to)
    {
        switch (from)
        {
            case Direction.North:
                switch (to)
                {
                    case Direction.North:
                        return RelativeDirection.Front;
                    case Direction.East:
                        return RelativeDirection.Right;
                    case Direction.South:
                        return RelativeDirection.Back;
                    case Direction.West:
                        return RelativeDirection.Left;
                }
                break;
            case Direction.East:
                switch (to)
                {
                    case Direction.North:
                        return RelativeDirection.Left;
                    case Direction.East:
                        return RelativeDirection.Front;
                    case Direction.South:
                        return RelativeDirection.Right;
                    case Direction.West:
                        return RelativeDirection.Back;
                }
                break;
            case Direction.South:
                switch (to)
                {
                    case Direction.North:
                        return RelativeDirection.Back;
                    case Direction.East:
                        return RelativeDirection.Left;
                    case Direction.South:
                        return RelativeDirection.Front;
                    case Direction.West:
                        return RelativeDirection.Right;
                }
                break;
            case Direction.West:
                switch (to)
                {
                    case Direction.North:
                        return RelativeDirection.Right;
                    case Direction.East:
                        return RelativeDirection.Back;
                    case Direction.South:
                        return RelativeDirection.Left;
                    case Direction.West:
                        return RelativeDirection.Front;
                }
                break;
        }
        return RelativeDirection.Error;
    }

    public static bool CanCreateHallNeighbor(this Tile tile)
    {
        return tile.Coord.x > 0 && tile.Coord.x < MapGenerator.WorldSizeX - 1 && tile.Coord.y > 0 && tile.Coord.y < MapGenerator.WorldSizeY - 1;
    }

    public static bool InMapBounds(this Vector2 coord)
    {
        return coord.x >= 0 && coord.x < MapGenerator.WorldSizeX && coord.y >= 0 && coord.y < MapGenerator.WorldSizeY;
    }

    public static List<int> RandomOrder(int length)
    {
        var counter = new List<int>();
        var res = new List<int>();
        for (int i = 0; i < length; i++)
        {
            counter.Add(i);
        }

        for (int i = 0; i < length; i++)
        {
            int rand = Random.Range(0, counter.Count);
            res.Add(counter[rand]);
            counter.RemoveAt(rand);
        }

        // Debug.Log($"randomized {String.Join(", ", res)}");
        return res;
    }

    public static Quaternion RandomRightAngleRotation()
    {
        int rand = Random.Range(0, Room.FurnitureRotations.Length);
        return Room.FurnitureRotations[rand];
    }

    public static List<T> Join<T>(this List<T> first, List<T> second)
    {
        var res = new List<T>();
        foreach (var f in first)
        {
            res.Add(f);
        }
        foreach (var f in second)
        {
            res.Add(f);
        }
        return res;
    }
}
