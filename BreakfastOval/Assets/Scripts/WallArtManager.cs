using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class WallArtManager : MonoBehaviour
{
    Tile tile;
    Renderer renderer;

    void OnEnable()
    {
        renderer = GetComponent<Renderer>();
    }

    public void Resolve()
    {
        tile = transform.GetComponentInParent<Tile>();
        renderer.enabled = tile.Doors.Count == 0 && tile.Walls.Count > 0 && Random.Range(0f, 1f) < MapGenerator.Instance.CHANCE_WALL;
    }
}
