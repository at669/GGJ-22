using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class WallArtManager : MonoBehaviour
{
    Renderer renderer;

    void OnEnable()
    {
        renderer = GetComponent<Renderer>();
        renderer.enabled = transform.GetComponentInParent<Tile>().Doors.Count == 0 && Random.Range(0f, 1f) < MapGenerator.Instance.CHANCE_WALL;
    }
}
