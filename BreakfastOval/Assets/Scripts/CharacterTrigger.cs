using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class CharacterTrigger : MonoBehaviour
{
    public bool IsGoal = false;
    bool canInteract = false;

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            // if (Keyboard.current.enterKey.wasPressedThisFrame)
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // TODO
                if (IsGoal)
                {
                    Debug.Log($"interacting with goal {transform.parent}!");
                    IsGoal = false;
                    MapGenerator.Instance.IncrementGoal();
                }
                else
                {
                    Debug.Log($"interacting {transform.parent} but not goal");
                    InteractionManager.Instance.ShowPanel(true);
                }
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
