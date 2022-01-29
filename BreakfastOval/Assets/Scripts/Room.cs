using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : Space
{
    public Vector2 Size;
    public List<GameObject> furniture;

    public Room(Vector2 size)
    {
        Size = size;
    }

    public void AddTile(Tile tile)
    {
        Tiles.Add(tile);
    }

    public static Vector2 GenerateSize()
    {
        return new Vector2(Random.Range(MapGenerator.RoomMinX, MapGenerator.RoomMaxX), Random.Range(MapGenerator.RoomMinY, MapGenerator.RoomMaxY));
    }
}
