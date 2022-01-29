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

    public static Vector2 GenerateSize(Vector2 range)
    {
        return new Vector2(Random.Range(Mathf.Max(MapGenerator.RoomMinX, range.x), MapGenerator.RoomMaxX), Random.Range(Mathf.Max(MapGenerator.RoomMinY, range.y), MapGenerator.RoomMaxY));
    }
}
