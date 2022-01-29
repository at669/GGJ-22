using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Direction { Error, North, East, South, West }
public enum TileType { Error, Room, Hall, Nothing }

public class Tile : MonoBehaviour
{
    public TileType TileType;
    public Vector2 Coord;
    public List<Direction> Walls = new List<Direction>();
    public TileType TileNorth = TileType.Error;
    public TileType TileEast = TileType.Error;
    public TileType TileSouth = TileType.Error;
    public TileType TileWest = TileType.Error;
    public GameObject WallNorthObj;
    public GameObject WallEastObj;
    public GameObject WallSouthObj;
    public GameObject WallWestObj;

    // Start is called before the first frame update
    void OnEnable()
    {
        Walls = TileExtensions.AllDirections();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AssignNeighborAt(Direction fromDir, TileType fromType)
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
    }

    public Direction SelectRandomWall() {
        int rand = Random.Range(0, Walls.Count);
        return Walls[rand];
    }
}
