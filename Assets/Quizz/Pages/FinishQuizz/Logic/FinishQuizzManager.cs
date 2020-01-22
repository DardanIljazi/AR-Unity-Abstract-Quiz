using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class FinishQuizzManager : MonoBehaviour
{
    public Text textObject;
    public Button returnMenuButton;

    private string originalText; // Contain the original text set to textObject


    void Start()
    {
        returnMenuButton.onClick.AddListener(delegate { ReturnMainMenuButtonClicked(); });
    }

    void Awake()
    {
        originalText = textObject.text;
    }

    public void SetFinalScore(int rightResponses, int falseResponses, int numberOfQuestions)
    {
        textObject.text = originalText.Replace("%1", rightResponses.ToString()).Replace("%2", falseResponses.ToString());
    }


    void ReturnMainMenuButtonClicked()
    {
        GameManager.Instance.pagesManager.ShowFirstPage();
    }

}
