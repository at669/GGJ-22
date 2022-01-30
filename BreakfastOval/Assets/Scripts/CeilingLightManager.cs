using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingLightManager : MonoBehaviour
{
    public GameObject Light;

    // Start is called before the first frame update
    void OnEnable()
    {
        Light = transform.Find("Light").gameObject;
        Turn(false);
    }

    public void Turn(bool active)
    {
        Light.SetActive(active);
    }
}
