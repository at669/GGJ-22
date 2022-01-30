using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
// using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class CharacterTrigger : MonoBehaviour
{
    bool showLobby = true;
    public string characterName;
    public string line;
    public GameObject Canvas;
    public bool IsGoal = false;
    bool canInteract = false;

    void Start()
    {
        characterName = transform.parent.name.Substring(0, transform.parent.name.Length - "(Clone)".Length);
        ToggleCanvas(false);
    }

    public void ToggleCanvas(bool val)
    {
        if (showLobby && characterName == "Bird")
        {
            line = QuestionOptions.Instance.LobbyLine;
        }
        else
        {
            if (string.IsNullOrEmpty(characterName))
            {
                return;
            }
            line = Resources.Load<TextAsset>($"Text/{characterName}").text;
        }
        Canvas.GetComponentInChildren<TextMeshProUGUI>().SetText(line);
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
                if (IsGoal)
                {
                    IsGoal = false;
                    showLobby = false;
                    MapGenerator.Instance.IncrementGoal();
                }
                else
                {
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
            if (IsGoal)
            {
                PlayerManager.Instance.ToggleExclamation(true);
            }
            canInteract = true;
            ToggleCanvas(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (IsGoal)
            {
                PlayerManager.Instance.ToggleExclamation(false);
            }
            canInteract = false;
            ToggleCanvas(false);
        }
    }
}
