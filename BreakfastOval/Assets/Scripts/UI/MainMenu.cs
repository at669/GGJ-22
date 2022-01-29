using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;

    // Start is called before the first frame update
    void Start()
    {
        settingsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(mainMenu.activeInHierarchy)
            {
                // Quit game confirmation menu
            }
            else if(settingsMenu.activeInHierarchy)
            {
                // Go back to main menu
                settingsMenu.SetActive(false);
                mainMenu.SetActive(true);
            }
        }
    }

    public void StartButton()
    {
        // Start scene 1
        // StartCoroutine(GameManager.instance.ChangeScene("#"));
    }

    public void SettingsButton()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);

        // eh?
        // mainMenu.transform.Find("Settings").GetComponent<HoverButton>().DeactivateHovers();
    }

    public void ExitButton()
    {
        //double check for WebGL build to not crash itch.io?
        Application.Quit();
    }
}
