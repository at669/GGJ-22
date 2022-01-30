using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileType TileType;
    public Vector2 Coord;
    public List<Direction> Doors = new List<Direction>();
    public List<Direction> Walls = new List<Direction>();
    public TileType TileNorth = TileType.Error;
    public TileType TileEast = TileType.Error;
    public TileType TileSouth = TileType.Error;
    public TileType TileWest = TileType.Error;
    public GameObject WallNorthObj;
    public GameObject WallEastObj;
    public GameObject WallSouthObj;
    public GameObject WallWestObj;
    public GameObject FloorObj;
    public DoorTrigger DoorTrigger;

    // Start is called before the first frame update
    void OnEnable()
    {
        Walls = Extensions.AllDirections();
    }

    public void AssignDoorAt(Direction dir, bool first = true)
    {
        var wallObj = GetWallObject(dir);
        DoorTrigger.gameObject.SetActive(true);
        var wallRend = wallObj.GetComponent<Renderer>();
        var wallColl = wallObj.GetComponent<Collider>();
        if (Walls.Contains(dir))
        {
            Walls.Remove(dir);
        }
        Doors.Add(dir);
        if (!first)
        {
            wallObj.SetActive(true);
            wallColl.isTrigger = true;
            wallRend.material.color = Color.blue;
            wallObj.transform.localScale = new Vector3(0.9f, wallObj.transform.localScale.y, wallObj.transform.localScale.z);
        }
        else
        {
            wallRend.enabled = false;
            wallColl.enabled = false;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="fromDir"></param>
    /// <param name="fromType"></param>
    /// <returns>True if there is a wall on this tile in fromDir</returns>
    public bool AssignNeighborAt(Direction fromDir, TileType fromType)
    {
        switch (fromDir)
        {
            case Direction.North:
                TileNorth = fromType;
                break;
            case Direction.East:
                TileEast = fromType;
                break;
            case Direction.South:
                TileSouth = fromType;
                break;
            case Direction.West:
                TileWest = fromType;
                break;
        }

        if (fromType == TileType)
        {
            Walls.Remove(fromDir);
        }

        return Walls.Contains(fromDir);
    }

    public Direction SelectRandomWall() {
        int rand = Random.Range(0, Walls.Count);
        return Walls[rand];
    }

    public GameObject GetWallObject(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return WallNorthObj;
            case Direction.East:
                return WallEastObj;
            case Direction.South:
                return WallSouthObj;
            case Direction.West:
                return WallWestObj;
            default:
                return null;
        }
    }

    public Space GetSpace()
    {
        foreach (var r in MapGenerator.Rooms.SelectMany(r => r.Tiles.Where(t => t.Coord == Coord).Select(t => r)))
        {
            return r;
        }

        foreach (var r in MapGenerator.Halls.SelectMany(r => r.Tiles.Where(t => t.Coord == Coord).Select(t => r)))
        {
            return r;
        }

        return null;
    }

    public override string ToString()
    {
        return Coord.ToString();
    }
}
