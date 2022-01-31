using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    static InteractionManager _instance;
    public static InteractionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InteractionManager>();
            }
            return _instance;
        }
    }

    public GameObject ButtonPanel;
    public GameObject PausePanel;
    TextMeshProUGUI Question;
    TextMeshProUGUI Answer0;
    TextMeshProUGUI Answer1;
    bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        Question = ButtonPanel.transform.Find("Question").GetComponent<TextMeshProUGUI>();
        Answer0 = ButtonPanel.transform.Find("Answer0").GetComponentInChildren<TextMeshProUGUI>();
        Answer1 = ButtonPanel.transform.Find("Answer1").GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     if (!paused)
        //     {
        //         Pause();
        //     }
        //     else
        //     {
        //         Resume();
        //     }
        // }
    }

    public void Resume()
    {
        PausePanel.SetActive(false);
        PlayerManager.Instance.ToggleController(false);
        // Time.timeScale = 1;
        paused = false;
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        PlayerManager.Instance.ToggleController(true);
        // Time.timeScale = 0;
        paused = true;
    }

    void AssignValues()
    {
        int rand = Random.Range(0, QuestionOptions.Instance.Questions.Count);
        var q = QuestionOptions.Instance.Questions[rand];
        Question.SetText(q.Question);
        Answer0.SetText(q.Answer0);
        Answer1.SetText(q.Answer1);
    }

    public void ShowPanel(bool show)
    {
        AssignValues();
        ButtonPanel.SetActive(show);
        PlayerManager.Instance.ToggleController(!show);
        Cursor.visible = show;
        ButtonPanel.transform.Find("Answer0").GetComponent<Button>().Select();
    }

    public void ButtonClick(int val)
    {
        ShowPanel(false);

        float rand = Random.Range(0f, 1f);
        if (rand < 0.5f)
        {
            Debug.Log("regenerating lol");
            MapGenerator.Instance.Generate(true);
        }
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
