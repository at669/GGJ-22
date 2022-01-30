using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class CharacterTrigger : MonoBehaviour
{
    bool canInteract = false;

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                Debug.Log("interacting!");
                // TODO
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
}
