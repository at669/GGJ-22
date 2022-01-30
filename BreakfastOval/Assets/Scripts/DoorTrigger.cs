using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoorTrigger : MonoBehaviour
{
    public bool ShouldRegenOnEnter = false;

    Tile tile;

    void Awake()
    {
        tile = transform.parent.GetComponent<Tile>();
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

        if (ShouldRegenOnEnter)
        {
            MapGenerator.Instance.Generate(true);
            ShouldRegenOnEnter = false;
        }
    }
}
