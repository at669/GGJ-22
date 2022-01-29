using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int[] worldSize = new int[2];
    public static Tile[,] Map;
    public int numRooms;
    public int[] hallLength = new int[2];
    public int[] roomMinSize = new int[2];
    public static int WorldSizeX;
    public static int WorldSizeY;
    public static int RoomMinX;
    public static int RoomMinY;
    public static int RoomMaxX;
    public static int RoomMaxY;
    public static int HallMinLen;
    public static int HallMaxLen;
    public int[] roomMaxSize = new int[2];
    public GameObject tilePrefab;
    List<GameObject> m_GeneratedRoomObjects = new List<GameObject>();
    List<Room> m_Rooms = new List<Room>();

    // Start is called before the first frame update
    void Start()
    {
        RoomMinX = roomMinSize[0];
        RoomMinY = roomMinSize[1];
        RoomMaxX = roomMaxSize[0];
        RoomMaxY = roomMaxSize[1];
        HallMinLen = hallLength[0];
        HallMaxLen = hallLength[1];
        WorldSizeX = worldSize[0];
        WorldSizeY = worldSize[1];

        Map = new Tile[worldSize[0], worldSize[1]];
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(worldSize[0] / 2, 0, worldSize[1] / 2), new Vector3(worldSize[0], 0.1f, worldSize[1]));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ResetAll();
            GenerateFirstRoom();
        }
    }

    void ResetAll()
    {
        Map = new Tile[worldSize[0], worldSize[1]];
        // TODO: pooling?
        foreach (var o in m_GeneratedRoomObjects)
        {
            Destroy(o);
        }
    }

    void GenerateFirstRoom()
    {
        // Find bottom left corner of current room
        int bottomLeftCurrX = Random.Range(roomMaxSize[0] / 2, worldSize[0] - roomMaxSize[0] / 2);
        int bottomLeftCurrY = Random.Range(roomMaxSize[1] / 2, worldSize[1] - roomMaxSize[1] / 2);

        // Generate current room
        var initRoom = GenerateRoom(bottomLeftCurrX, bottomLeftCurrY);
        var wallTile = initRoom.SelectBorderTileForHall();
        GenerateHall(wallTile);
    }

    Hall GenerateHall(Tile fromTile)
    {
        var hall = new Hall(Hall.GenerateLength());
        for (int i = 0; i < hall.Length; i++)
        {
            var options = new List<Direction>();
            var checkTile = fromTile.GetCoord(Direction.North, 1);
            if (CheckCanCreateTileAt(checkTile))
            {
                options.Add(Direction.North);
            }
            checkTile = fromTile.GetCoord(Direction.East, 1);
            if (CheckCanCreateTileAt(checkTile))
            {
                options.Add(Direction.East);
            }
            checkTile = fromTile.GetCoord(Direction.South, 1);
            if (CheckCanCreateTileAt(checkTile))
            {
                options.Add(Direction.South);
            }
            checkTile = fromTile.GetCoord(Direction.West, 1);
            if (CheckCanCreateTileAt(checkTile))
            {
                options.Add(Direction.West);
            }

            var dir = options[Random.Range(0, options.Count)];

            var coord = fromTile.GetCoord(dir, 1);

            var tileObj = Instantiate(tilePrefab, new Vector3(coord.x, 0, coord.y), Quaternion.identity);
            var tile = tileObj.GetComponent<Tile>();
            tile.Coord = coord;
            tile.TileType = TileType.Hall;
            hall.AddTile(tile);

            Map[(int)coord.x, (int)coord.y] = tile;
            // TODO: manage destroying via space
            m_GeneratedRoomObjects.Add(tileObj);

            fromTile = tile;
        }

        ManageWalls(hall, TileType.Hall);
        return hall;
    }

    bool CheckCanCreateTileAt(Vector2 coord)
    {
        if (coord.x >= Map.GetLength(0) || coord.y >= Map.GetLength(1) || coord.x < 0 || coord.y < 0)
        {
            return false;
        }
        if (Map[(int)coord.x, (int)coord.y] != null)
        {
            return false;
        }

        return true;
    }

    Room GenerateRoom(int bottomLeftX, int bottomLeftY)
    {
        var room = new Room(Room.GenerateSize());
        room.Rect = new Rect(bottomLeftX, bottomLeftY, room.Size.x, room.Size.y);

        var clampX = Mathf.Min(bottomLeftX + room.Size.x, worldSize[0]);
        var clampY = Mathf.Min(bottomLeftY + room.Size.y, worldSize[1]);

        for (int i = bottomLeftX; i < clampX; i++)
        {
            for (int j = bottomLeftY; j < clampY; j++)
            {
                var tileObj = Instantiate(tilePrefab, new Vector3(i, 0, j), Quaternion.identity);
                var tile = tileObj.GetComponent<Tile>();
                tile.Coord = new Vector2(i, j);
                tile.TileType = TileType.Room;
                room.AddTile(tile);

                Map[i, j] = tile;
                // TODO: manage destroying via space
                m_GeneratedRoomObjects.Add(tileObj);
            }
        }

        ManageWalls(room, TileType.Room);
        return room;
    }

    void ManageWalls(Space space, TileType type)
    {
        space.ManageWalls(type);

        // Turn off applicable prefab parts
        foreach (Tile t in space.Tiles)
        {
            if (!t.Walls.Contains(Direction.North))
            {
                t.WallNorthObj.SetActive(false);
            }
            if (!t.Walls.Contains(Direction.South))
            {
                t.WallSouthObj.SetActive(false);
            }
            if (!t.Walls.Contains(Direction.East))
            {
                t.WallEastObj.SetActive(false);
            }
            if (!t.Walls.Contains(Direction.West))
            {
                t.WallWestObj.SetActive(false);
            }
        }
    }

    public static Tile GetTileFromCoord(Vector2 coord)
    {
        if (coord.x < Map.GetLength(0) && coord.y < Map.GetLength(1))
        {
            return Map[(int)coord.x, (int)coord.y];
        }
        else
        {
            return null;
        }
    }
}
