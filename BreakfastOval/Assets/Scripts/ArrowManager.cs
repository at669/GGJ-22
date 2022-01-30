using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public static Vector3 target;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
