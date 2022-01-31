using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GetComponent<MeshRenderer>().enabled = false;
        transform.parent.Find("Handle").GetComponent<MeshRenderer>().enabled = false;
    }

    void OnTriggerExit(Collider other)
    {
        GetComponent<MeshRenderer>().enabled = true;
        transform.parent.Find("Handle").GetComponent<MeshRenderer>().enabled = true;
    }
}
