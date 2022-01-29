using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int MAX_ITER = 100;
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
    List<Hall> m_Halls = new List<Hall>();

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

        Generate();
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
            Generate();
        }
    }

    void Generate()
    {
        for (int i = 0; i < MAX_ITER; i++)
        {
            Debug.Log($"ITERATION {i}");
            ResetAll();
            if (GenerateFirstRoom())
            {
                return;
            }
        }
    }

    void ResetAll()
    {
        m_Rooms.ForEach(s => s.Reset());
        m_Halls.ForEach(s => s.Reset());

        m_Rooms = new List<Room>();
        m_Halls = new List<Hall>();

        Map = new Tile[worldSize[0], worldSize[1]];
        // TODO: pooling?
        foreach (var o in m_GeneratedRoomObjects)
        {
            Destroy(o);
        }
    }

    bool GenerateFirstRoom()
    {
        // Find bottom left corner of current room
        int bottomLeftCurrX = Random.Range(roomMaxSize[0] / 2, worldSize[0] - roomMaxSize[0] - 1);
        int bottomLeftCurrY = Random.Range(roomMaxSize[1] / 2, worldSize[1] - roomMaxSize[1] - 1);

        // Generate current room
        Debug.Log("starting 0 room");
        var initRoom = GenerateRoom(new Vector2(bottomLeftCurrX, bottomLeftCurrY), Vector2.zero, false);
        m_Rooms.Add(initRoom);

        // Generate first hall
        Debug.Log("starting 0 hall");
        var initWallTile = initRoom.SelectBorderTileForHall();
        if (initWallTile == null)
        {
            Debug.Log($"return fail");
            return false;
        }
        Debug.Log($"initial wall tile is {initWallTile.Coord}");
        var initHall = GenerateHall(initWallTile);
        m_Halls.Add(initHall);

        for (int i = 1; i < numRooms; i++)
        {
            // Generate second room
            Debug.Log($"starting {i} room");
            var hallTile = initHall.GetLastTile();
            if (hallTile == null)
            {
                Debug.Log("why ise hall tile null");
                return false;
            }
            var doorWall = SelectRandomEmptyWall(hallTile);
            if (doorWall == Direction.Error)
            {
                Debug.Log($"return fail");
                return false;
            }
            hallTile.AssignDoorAt(doorWall);
            Debug.Log($"grabbing last hall tile {hallTile.Coord} in dir {doorWall}");
            // TODO: get bounds for second room based on attempted bottom left + range + clamp based on existing map
            // Find bottom left corner of next room
            (var bottomLeft, var range) = GetBottomLeftCorner(hallTile, doorWall);
            Debug.Log($"grabbing {bottomLeft} as bottom left corner of next room with range {range}");
            var room1 = GenerateRoom(bottomLeft, range, true, doorWall.GetOpposite());
            m_Rooms.Add(room1);

            if (i == numRooms - 1)
            {
                break;
            }

            // Hall 2
            Debug.Log($"starting {i} hall");
            var wallTile = room1.SelectBorderTileForHall();
            if (wallTile == null)
            {
                Debug.Log($"return fail");
                return false;
            }
            Debug.Log($"initial wall tile is {wallTile.Coord}");
            var hall = GenerateHall(wallTile);
            m_Halls.Add(hall);

            initHall = hall;
        }

        // // Generate second room
        // Debug.Log("starting second room");
        // var hallTile = initHall.GetLastTile();
        // var doorWall = SelectRandomEmptyWall(hallTile);
        // if (doorWall == Direction.Error)
        // {
        //     Debug.Log($"return fail");
        //     return false;
        // }
        // hallTile.AssignDoorAt(doorWall);
        // Debug.Log($"grabbing last hall tile {hallTile.Coord} in dir {doorWall}");
        // // TODO: get bounds for second room based on attempted bottom left + range + clamp based on existing map
        // // Find bottom left corner of next room
        // (var bottomLeft, var range) = GetBottomLeftCorner(hallTile, doorWall);
        // Debug.Log($"grabbing {bottomLeft} as bottom left corner of next room with range {range}");
        // var room1 = GenerateRoom(bottomLeft, range, true, doorWall.GetOpposite());
        // m_Rooms.Add(room1);

        // // Hall 2
        // Debug.Log("starting second hall");
        // var wall2 = room1.SelectBorderTileForHall();
        // if (wall2 == null)
        // {
        //     Debug.Log($"return fail");
        //     return false;
        // }
        // Debug.Log($"initial wall tile is {wall2.Coord}");
        // var hall2 = GenerateHall(wall2);
        // m_Halls.Add(hall2);

        // // Room 3
        // Debug.Log("starting 3 room");
        // var hall3 = hall2.GetLastTile();
        // var door3 = SelectRandomEmptyWall(hall3);
        // if (doorWall == Direction.Error)
        // {
        //     Debug.Log($"return fail");
        //     return false;
        // }
        // hall3.AssignDoorAt(door3);
        // Debug.Log($"grabbing last hall tile {hall3.Coord} in dir {door3}");
        // // Find bottom left corner of next room
        // (var bot3, var range3) = GetBottomLeftCorner(hall3, door3);
        // Debug.Log($"grabbing {bot3} as bottom left corner of next room with range {range3}");
        // var room3 = GenerateRoom(bot3, range3, true, door3.GetOpposite());
        // m_Rooms.Add(room3);

        Debug.Log("return success");
        return true;
    }

    (Vector2, Vector2) GetBottomLeftCorner(Tile fromTile, Direction fromDir)
    {
        Vector2 range = Vector2.zero;
        float bottomLeftX = 0;
        float bottomLeftY = 0;

        switch (fromDir)
        {
            case Direction.North:
                // bottomLeftX = Mathf.Min(0, Random.Range(fromTile.Coord.x - roomMaxSize[0] / 2, fromTile.Coord.x + roomMaxSize[0] / 2));
                bottomLeftX = fromTile.Coord.x;
                bottomLeftY = fromTile.Coord.y + 1;
                break;
            case Direction.East:
                bottomLeftX = fromTile.Coord.x + 1;
                bottomLeftY = fromTile.Coord.y;
                // bottomLeftY = Mathf.Min(0, Random.Range(fromTile.Coord.y - roomMaxSize[1] / 2, fromTile.Coord.y + roomMaxSize[1] / 2));
                break;
            case Direction.South:
                bottomLeftX = Mathf.Max(0, Random.Range((int)fromTile.Coord.x - roomMaxSize[0] / 2, (int)fromTile.Coord.x));
                bottomLeftY = Mathf.Max(0, Random.Range((int)fromTile.Coord.y - roomMaxSize[1], (int)fromTile.Coord.y - roomMinSize[1]));
                range.x = Mathf.Abs(fromTile.Coord.x - bottomLeftX);
                range.y = Mathf.Abs(fromTile.Coord.y - bottomLeftY) + 1;
                break;
            case Direction.West:
                bottomLeftX = Mathf.Max(0, Random.Range((int)fromTile.Coord.x - roomMaxSize[0], (int)fromTile.Coord.x - roomMinSize[0]));
                bottomLeftY = Mathf.Max(0, Random.Range((int)fromTile.Coord.y - roomMaxSize[1] / 2, (int)fromTile.Coord.y));
                range.x = Mathf.Abs(fromTile.Coord.x - bottomLeftX) + 1;
                range.y = Mathf.Abs(fromTile.Coord.y - bottomLeftY);
                break;
        }

        return (new Vector2(bottomLeftX, bottomLeftY), range);
    }

    Direction SelectRandomEmptyWall(Tile tile)
    {
        // Debug.Log($"selecting tile {tile.Coord}");
        var options = new List<Direction>();

        var checkTile = tile.GetCoord(Direction.North, 1);
        if (CheckCanCreateTileAt(checkTile))
        {
            options.Add(Direction.North);
        }
        checkTile = tile.GetCoord(Direction.East, 1);
        if (CheckCanCreateTileAt(checkTile))
        {
            options.Add(Direction.East);
        }
        checkTile = tile.GetCoord(Direction.South, 1);
        if (CheckCanCreateTileAt(checkTile))
        {
            options.Add(Direction.South);
        }
        checkTile = tile.GetCoord(Direction.West, 1);
        if (CheckCanCreateTileAt(checkTile))
        {
            options.Add(Direction.West);
        }

        if (options.Count == 0)
        {
            Debug.Log("no options!");
            return Direction.Error;
        }

        return options[Random.Range(0, options.Count)];
    }

    Hall GenerateHall(Tile fromTile)
    {
        bool isFirst = true;
        Debug.Log($"generating hall from {fromTile.Coord}");
        var hall = new Hall(Hall.GenerateLength());
        for (int i = 0; i < hall.Length; i++)
        {
            bool isLast = i == hall.Length - 1;
            bool addedLastTile = false;
            var dir = SelectRandomEmptyWall(fromTile);
            var coord = fromTile.GetCoord(dir, 1);

            if (Map[(int)coord.x, (int)coord.y] == null)
            {
                var tileObj = Instantiate(tilePrefab, new Vector3(coord.x, 0, coord.y), Quaternion.identity);
                var tile = tileObj.GetComponent<Tile>();
                tile.Coord = coord;
                tileObj.name = $"Tile ({tile.Coord.x},{tile.Coord.y})";
                tile.TileType = TileType.Hall;
                tile.FloorObj.GetComponent<Renderer>().material.color = Color.yellow;

                if (isFirst)
                {
                    fromTile.AssignDoorAt(dir);
                    tile.AssignDoorAt(dir.GetOpposite());
                }

                hall.AddTile(tile, isLast);
                if (isLast)
                {
                    addedLastTile = true;
                }

                Map[(int)coord.x, (int)coord.y] = tile;
                // TODO: manage destroying via space
                m_GeneratedRoomObjects.Add(tileObj);

                fromTile = tile;
                isFirst = false;
            }
            if (isLast && !addedLastTile)
            {
                if (hall.Tiles.Count > 0)
                {
                    Debug.Log($"failed to add last tile to hall, coutn is {hall.Tiles.Count}, desired length was {hall.Length}, setting last tile to that");
                    hall.SetLastTile(hall.Tiles[hall.Tiles.Count - 1]);
                    hall.Length = hall.Tiles.Count;
                }
            }
        }

        ManageSpaceWalls(hall, TileType.Hall);
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

    // Vector2 GetMinBoundsMap(Vector2 bottomLeft, Vector2 range)
    // {
    //     var res = Vector2.zero;

    // }

    Room GenerateRoom(Vector2 bottomLeft, Vector2 range, bool containConnection = true, Direction doorWall = Direction.Error)
    {
        Debug.Log($"generating room from {bottomLeft}");
        Room room;
        room = new Room(Room.GenerateSize(range));
        room.Rect = new Rect(bottomLeft.x, bottomLeft.y, room.Size.x, room.Size.y);

        // var clampMinX = Mathf.Max(bottomLeft.x, )

        var clampMaxX = Mathf.Min(bottomLeft.x + room.Size.x, worldSize[0]);
        var clampMaxY = Mathf.Min(bottomLeft.y + room.Size.y, worldSize[1]);

        for (int i = (int)bottomLeft.x; i < clampMaxX; i++)
        {
            for (int j = (int)bottomLeft.y; j < clampMaxY; j++)
            {
                if (Map[i, j] == null)
                {
                    var tileObj = Instantiate(tilePrefab, new Vector3(i, 0, j), Quaternion.identity);
                    var tile = tileObj.GetComponent<Tile>();
                    tile.Coord = new Vector2(i, j);
                    tileObj.name = $"Tile ({tile.Coord.x},{tile.Coord.y})";
                    tile.TileType = TileType.Room;
                    room.AddTile(tile);

                    if (doorWall != Direction.Error)
                    {
                        tile.AssignDoorAt(doorWall);
                    }
                    Map[i, j] = tile;
                    // TODO: manage destroying via space
                    m_GeneratedRoomObjects.Add(tileObj);
                }
                // else {
                //     Debug.Log($"map already contained {i},{j}!");
                // }
            }
        }

        ManageSpaceWalls(room, TileType.Room);
        return room;
    }

    void ManageSingleWall(Direction dir, Tile t, Space space)
    {
        if (!t.Walls.Contains(dir))
        {
            if (space.TilesWithWalls.Contains(t))
            {
                space.TilesWithWalls.Remove(t);
            }
            space.TilesWithoutWalls.Add(t);

            switch (dir)
            {
                case Direction.North:
                    t.WallNorthObj.SetActive(false);
                    break;
                case Direction.East:
                    t.WallEastObj.SetActive(false);
                    break;
                case Direction.South:
                    t.WallSouthObj.SetActive(false);
                    break;
                case Direction.West:
                    t.WallWestObj.SetActive(false);
                    break;
            }
        }
        else
        {
            space.TilesWithWalls.Add(t);
        }
    }

    void ManageSpaceWalls(Space space, TileType type)
    {
        space.ManageWalls(type);

        // Turn off applicable prefab parts
        foreach (Tile t in space.Tiles)
        {
            ManageSingleWall(Direction.North, t, space);
            ManageSingleWall(Direction.South, t, space);
            ManageSingleWall(Direction.East, t, space);
            ManageSingleWall(Direction.West, t, space);
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
