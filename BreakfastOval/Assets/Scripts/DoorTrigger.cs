using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoorTrigger : MonoBehaviour
{
    Tile tile;

    void Awake()
    {
        tile = transform.parent.GetComponent<Tile>();
        // if (tile.GetSpace() == PlayerManager.Instance.currentSpace)
        // {
        //     PlayerManager.Instance.currentDoorDir = tile.Doors[0];
        // }
    }

    void OnTriggerEnter(Collider other)
    {
        var space = tile.GetSpace();
        PlayerManager.Instance.currentDoorTile = tile;
        PlayerManager.Instance.currentDoorDir = tile.Doors[0];
        if (PlayerManager.Instance.currentSpace != space)
        {
            PlayerManager.Instance.currentSpace = space;
        }
    }
}
