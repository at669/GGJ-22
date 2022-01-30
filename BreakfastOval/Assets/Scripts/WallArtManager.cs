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
        renderer.enabled = Random.Range(0f, 1f) < MapGenerator.Instance.CHANCE_WALL;
    }
}
