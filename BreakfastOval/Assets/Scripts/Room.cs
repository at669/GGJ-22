using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : Space
{
    public List<GameObject> furniture;

    public Room(Vector2 size)
    {
        Size = size;
    }

    public void AddTile(Tile tile)
    {
        Tiles.Add(tile);
    }
}
