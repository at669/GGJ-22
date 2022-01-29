using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hall : Space
{
    Tile LastTile;
    public int Length;

    public Hall(int len)
    {
        Length = len;
    }

    public void AddTile(Tile tile, bool isLast = false)
    {
        Tiles.Add(tile);
        if (isLast)
        {
            LastTile = tile;
        }
    }

    public static int GenerateLength()
    {
        return Random.Range(MapGenerator.HallMinLen, MapGenerator.HallMaxLen);
    }

    public Tile GetLastTile()
    {
        return LastTile;
    }
}
