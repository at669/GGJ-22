using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Room GoalRoom;
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
        // private set;
    }
    public Space currentSpace;
    public Tile currentDoorTile;
    public Direction currentDoorDir;
    public Vector3 bottomLeftOffset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bottomLeftOffset = transform.position - new Vector3(currentSpace.Rect.xMin, 0, currentSpace.Rect.yMin);
    }

    public void TeleportPlayer(Vector3 pos)
    {
        gameObject.SetActive(false);
        transform.position = pos;
        gameObject.SetActive(true);
    }
}
