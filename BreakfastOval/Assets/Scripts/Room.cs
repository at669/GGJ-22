using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : Space
{
    public static string[] characterTypes = new string[] { "Sphere" };
    public static Dictionary<RoomType, string[]> furnitureNames = new Dictionary<RoomType, string[]> {
        { RoomType.Lobby, new string[] { "Cube" }},
        { RoomType.Kitchen, new string[] { "Cube" }},
        { RoomType.IT, new string[] { "Cube" }},
        { RoomType.Bathroom, new string[] { "Cube" }},
        { RoomType.Broom, new string[] { "Cube" }}
    };
    public RoomType RoomType;
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

    public (List<Tile>, List<string>) SelectMiddleFurnitureTiles(int num)
    {
        var resTiles = new List<Tile>();
        var resFurniture = new List<string>();
        var order = Extensions.RandomOrder(num);
        for (int i = 0; i < num; i++)
        {
            resTiles.Add(TilesWithoutWalls[order[i]]);
            resFurniture.Add(furnitureNames[RoomType][Random.Range(0, furnitureNames[RoomType].Length)]);
            Debug.Log($"grabbing tile {resTiles[resTiles.Count - 1]} for furniture placement withi object {resFurniture[resFurniture.Count - 1]}");
        }

        return (resTiles, resFurniture);
    }

    public Tile SelectCharacterTile()
    {
        var spaces = GetUnoccupiedMiddleTiles();
        int rand = Random.Range(0, spaces.Count);
        return spaces[rand];
    }
}
