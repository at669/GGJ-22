using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    static ArrowManager _instance;
    public static ArrowManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ArrowManager>();
            }
            return _instance;
        }
    }
    public static Vector3 target;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
