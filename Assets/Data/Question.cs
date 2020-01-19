// Davide Carboni
// Class question to use for the Question Game Object

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;


public class Question : MonoBehaviour
{
    public string question;
    public string image;
    public int questionNb = -1;
    public bool isAvailable = false;
    public bool isAnswered = false;

    public GameObject title;
    public GameObject quizzeManager;
    public GameObject answer1;
    public GameObject answer2;
    public GameObject answer3;
    public GameObject answer4;
    public GameObject canvasPanel;
    public GameObject canvas;

    /// <summary>
    /// Generate the panel for each question
    /// </summary>
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Resize the main panel if the are less then 4 answer
    /// </summary>
    public void ResizePanel()
    {
        if (questionNb == 3)
        {
            canvas.GetComponent<RectTransform>().sizeDelta= new Vector2(1825.224f, 800);
            canvasPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 60);
        }

        if (questionNb == 2)
        {
            canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(1825.224f, 700);
            canvasPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 110);
        }
    }

    /// <summary>
    ///  Get random question and set all data in the answer button
    /// </summary>
    public void Init()
    {
        ApiData.Question q = quizzeManager.GetComponent<QuizzeManager>().GetRandomQuestion();                   // get random question
        if (q == null)                                                                                          // disable the canvas if the question is null
        {
            this.gameObject.SetActive(false);
            return;
        }
        this.questionNb = q.answers.Count;                                                                     // take the size of answer from the question
        this.question = q.question;                                                                            // take the text of the question
        this.image = q.image;                                                                                  // take the image of the question
        title.GetComponent<TextMeshProUGUI>().text = q.question;                                               // change the title in the panel with the current question text           
        if (answer1 != null && q.answers.Count >= 1) answer1.GetComponent<Answer>().Init(q.answers[0]);        // set the button answer1 if exsite a value for the answer
        if (answer2 != null && q.answers.Count >= 2) answer2.GetComponent<Answer>().Init(q.answers[1]);        // set the button answer2 if exsite a value for the answer
        if (answer3 != null && q.answers.Count >= 3) answer3.GetComponent<Answer>().Init(q.answers[2]);        // set the button answer3 if exsite a value for the answer
        if (answer4 != null && q.answers.Count >= 4) answer4.GetComponent<Answer>().Init(q.answers[3]);        // set the button answer4 if exsite a value for the answer
        ResizePanel();                                                                                         // resize the canvas if necessary
    }
}
