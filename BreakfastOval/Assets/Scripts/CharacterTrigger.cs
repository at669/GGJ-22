using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
// using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class CharacterTrigger : MonoBehaviour
{
    public string characterName;
    public string line;
    public GameObject Canvas;
    public bool IsGoal = false;
    bool canInteract = false;

    void Start()
    {
        characterName = transform.parent.name.Substring(0, transform.parent.name.Length - "(Clone)".Length);
        line = Resources.Load<TextAsset>($"Text/{characterName}").text;
        Canvas.GetComponentInChildren<TextMeshProUGUI>().SetText(line);
        ToggleCanvas(false);
    }

    public void ToggleCanvas(bool val)
    {
        Canvas.SetActive(val);
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            // if (Keyboard.current.enterKey.wasPressedThisFrame)
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ToggleCanvas(false);
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
                    canInteract = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            ToggleCanvas(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            ToggleCanvas(false);
        }
    }
}
