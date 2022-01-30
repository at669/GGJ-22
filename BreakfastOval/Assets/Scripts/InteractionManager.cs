using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowPanel(bool show)
    {
        ButtonPanel.SetActive(show);
        PlayerManager.Instance.ToggleController(!show);
        Cursor.visible = show;
        ButtonPanel.transform.GetChild(0).GetComponent<Button>().Select();
    }

    public void ButtonClick(int val)
    {
        Debug.Log($"Button {val}");
        PlayerManager.Instance.ToggleController(true);
        Cursor.visible = false;
    }
}
