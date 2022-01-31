using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerManager : MonoBehaviour
{
    FirstPersonController controller;
    bool active = true;
    public Room GoalRoom;
    public GameObject GoalCharacter;
    public int PrevGoalRoomIdx;
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
    public GameObject exclamation;
    public GameObject arrow;

    void Start()
    {
        controller = GetComponent<FirstPersonController>();
    }

    public void ToggleExclamation(bool val)
    {
        exclamation.SetActive(val);
        arrow.SetActive(!val);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Keyboard.current.digit2Key.wasPressedThisFrame)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            active = !active;
            ToggleController(active);
        }

        bottomLeftOffset = transform.position - new Vector3(currentSpace.Rect.xMin, 0, currentSpace.Rect.yMin);
    }

    public void ToggleController(bool on)
    {
        controller.enabled = on;
    }

    public void TeleportPlayer(Vector3 pos)
    {
        gameObject.SetActive(false);
        transform.position = pos;
        gameObject.SetActive(true);
    }
}
