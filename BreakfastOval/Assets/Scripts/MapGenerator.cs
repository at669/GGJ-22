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
    public static int RoomMinX;
    public static int RoomMinY;
    public static int RoomMaxX;
    public static int RoomMaxY;
    public static int HallMinX;
    public static int HallMinY;
    public static int HallMaxX;
    public static int HallMaxY;
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
            GenerateFirstRoom();
        }
    }

    void GenerateFirstRoom()
    {
        // TODO: pooling
        foreach (var o in m_GeneratedRoomObjects)
        {
            Destroy(o);
        }

        // Find bottom left corner of current room
        int bottomLeftCurrX = Random.Range(roomMaxSize[0] / 2, worldSize[0] - roomMaxSize[0] / 2);
        int bottomLeftCurrY = Random.Range(roomMaxSize[1] / 2, worldSize[1] - roomMaxSize[1] / 2);

        // Generate current room
        var initRoom = GenerateRoom(bottomLeftCurrX, bottomLeftCurrY);
    }

    Room GenerateRoom(int bottomLeftX, int bottomLeftY)
    {
        var room = new Room(Space.GenerateSize(TileType.Room));
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
