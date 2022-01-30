using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : Space
{
    public GameObject Character;
    public static Quaternion[] FurnitureRotations = new Quaternion[]
    {
        Quaternion.identity, Quaternion.Euler(0, 90, 0), Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 270, 0)
    };
    public const int FURNITURE_RATE = 6;
    public const int LIGHT_RATE = 8;

    public static string[] CharacterNames = new string[]
    {
        "Bird", "Bunny", "Cat", "Dog", "Frog", "Lion", "Mouse", "Penguin", "Pig", "Tanuki"
    };

    public static Dictionary<RoomType, string> RoomTypeToCharacter = new Dictionary<RoomType, string>
    {
        { RoomType.Lobby, "Bird" },
        { RoomType.Kitchen, "Bunny" },
        { RoomType.IT, "Cat" },
        { RoomType.Bathroom, "Dog" },
        { RoomType.Broom, "Frog" }
    };

    public static Dictionary<RoomType, string[]> CeilingFurnitureNames = new Dictionary<RoomType, string[]>
    {
        { RoomType.Lobby, new string[] { }},
        { RoomType.Kitchen, new string[] { }},
        { RoomType.IT, new string[] { }},
        { RoomType.Bathroom, new string[] { }},
        { RoomType.Broom, new string[] { }}
    };
    public static Dictionary<RoomType, string[]> WallFurnitureNames = new Dictionary<RoomType, string[]>
    {
        { RoomType.Lobby, new string[] { "Cylinder" }},
        { RoomType.Kitchen, new string[] { "Cylinder" }},
        { RoomType.IT, new string[] { "Cylinder" }},
        { RoomType.Bathroom, new string[] { "Cylinder" }},
        { RoomType.Broom, new string[] { "Cylinder" }}
    };
    public static Dictionary<RoomType, string[]> MiddleFurnitureNames = new Dictionary<RoomType, string[]>
    {
        { RoomType.Lobby, new string[] { "Cube" }},
        { RoomType.Kitchen, new string[] { "Cube" }},
        { RoomType.IT, new string[] { "Cube" }},
        { RoomType.Bathroom, new string[] { "Cube" }},
        { RoomType.Broom, new string[] { "Cube" }}
    };
    public static Dictionary<RoomType, string[]> AnyFurnitureNames = new Dictionary<RoomType, string[]>
    {
        { RoomType.Lobby, new string[] { }},
        { RoomType.Kitchen, new string[] { }},
        { RoomType.IT, new string[] { }},
        { RoomType.Bathroom, new string[] { }},
        { RoomType.Broom, new string[] { }}
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

    List<Tile> GetUnoccupiedSpaceByType(FurnitureType type)
    {
        switch (type)
        {
            case FurnitureType.Middle:
                return GetUnoccupiedMiddleTiles();
            case FurnitureType.Wall:
                return GetUnoccupiedWallTiles();
            case FurnitureType.Ceiling:
                return Tiles;
            case FurnitureType.AnyFloor:
                return GetAllUnoccupiedTiles();
            default:
                return Tiles;
        }
    }

    List<string> GetFurnitureOptionsByType(FurnitureType type)
    {
        switch (type)
        {
            case FurnitureType.Middle:
                return MiddleFurnitureNames[RoomType].ToList().Join(AnyFurnitureNames[RoomType].ToList());
            case FurnitureType.Wall:
                return WallFurnitureNames[RoomType].ToList().Join(AnyFurnitureNames[RoomType].ToList());
            case FurnitureType.Ceiling:
                return CeilingFurnitureNames[RoomType].ToList();
            case FurnitureType.AnyFloor:
                return AnyFurnitureNames[RoomType].ToList();
            default:
                return AnyFurnitureNames[RoomType].ToList();
        }
    }

    public (List<Tile>, List<string>) SelectFurnitureTiles(FurnitureType type, int num = -1)
    {
        var spaces = GetUnoccupiedSpaceByType(type);
        var furnOptions = GetFurnitureOptionsByType(type);
        // Generate number based on available tiles
        if (num == -1)
        {
            num = Mathf.Max(1, Mathf.CeilToInt(spaces.Count / FURNITURE_RATE));
        }

        var resTiles = new List<Tile>();
        var resFurniture = new List<string>();
        var order = Extensions.RandomOrder(num);
        for (int i = 0; i < num; i++)
        {
            var tiles = GetUnoccupiedSpaceByType(type);
            if (tiles.Count == 0)
            {
                continue;
            }
            tiles[order[i]].occupied = true;
            resTiles.Add(tiles[order[i]]);
            resFurniture.Add(furnOptions[Random.Range(0, furnOptions.Count)]);
        }

        return (resTiles, resFurniture);
    }

    public Tile SelectCharacterTile(bool tryCenter)
    {
        var spaces = tryCenter ? GetUnoccupiedMiddleTiles() : GetAllUnoccupiedTiles();
        if (spaces.Count == 0)
        {
            return null;
        }
        int rand = Random.Range(0, spaces.Count);
        spaces[rand].occupied = true;
        return spaces[rand];
    }

    public List<Tile> SelectAnyTiles()
    {
        var resTiles = new List<Tile>();
        var num = Mathf.Max(1, Mathf.CeilToInt(Tiles.Count / LIGHT_RATE));
        var order = Extensions.RandomOrder(num);

        for (int i = 0; i < num; i++)
        {
            var tiles = Tiles;
            if (tiles.Count == 0)
            {
                continue;
            }
            resTiles.Add(tiles[order[i]]);
        }

        return resTiles;
    }
}
