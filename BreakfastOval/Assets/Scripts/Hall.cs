using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hall : Space
{
    public int Length;

    public Hall(int len)
    {
        Length = len;
    }

    public void AddTile(Tile tile)
    {
        Tiles.Add(tile);
    }

    public static int GenerateLength()
    {
        return Random.Range(MapGenerator.HallMinLen, MapGenerator.HallMaxLen);
    }
}
