using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    GameObject capsule;
    bool active = true;
    public Room GoalRoom;
    public GameObject GoalCharacter;
    public int GoalRoomIdx;
    static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerManager>();
            }
            return _instance;
        }
    }
    public Space currentSpace;
    public Tile currentDoorTile;
    public Direction currentDoorDir;
    public Vector3 bottomLeftOffset;

    void Start()
    {
        capsule = transform.Find("PlayerCapsule").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            active = !active;
            capsule.SetActive(active);
        }

        bottomLeftOffset = transform.position - new Vector3(currentSpace.Rect.xMin, 0, currentSpace.Rect.yMin);
    }

    public void TeleportPlayer(Vector3 pos)
    {
        gameObject.SetActive(false);
        transform.position = pos;
        gameObject.SetActive(true);
    }
}
