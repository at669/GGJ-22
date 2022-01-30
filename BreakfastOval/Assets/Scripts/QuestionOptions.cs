using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionOptions : MonoBehaviour
{
    static QuestionOptions _instance;
    public static QuestionOptions Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<QuestionOptions>();
            }
            return _instance;
        }
    }
    public List<QuestionType> Questions = new List<QuestionType>();
}

[Serializable]
public class QuestionType
{
    public string Question;
    public string Answer0;
    public string Answer1;
}
