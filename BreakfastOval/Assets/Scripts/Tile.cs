using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        Walls = Extensions.AllDirections();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="fromDir"></param>
    /// <param name="fromType"></param>
    /// <returns>True if there are still walls</returns>
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

        return Walls.Count > 0;
    }

    public Direction SelectRandomWall() {
        int rand = Random.Range(0, Walls.Count);
        return Walls[rand];
    }
}
