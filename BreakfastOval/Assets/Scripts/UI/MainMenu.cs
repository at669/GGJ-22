using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject creditsMenu;

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
            else if(creditsMenu.activeInHierarchy)
            {
                // Go back to main menu
                creditsMenu.SetActive(false);
                mainMenu.SetActive(true);
            }
        }
    }

    public void StartButton()
    {
        // Start scene 1
        SceneManager.LoadScene("Game");
    }

    public void CreditsButton()
    {
        // mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void BackButton()
    {
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void SettingsButton()
    {
        // mainMenu.SetActive(false);
        settingsMenu.SetActive(true);

        // eh?
        // mainMenu.transform.Find("Settings").GetComponent<HoverButton>().DeactivateHovers();
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
