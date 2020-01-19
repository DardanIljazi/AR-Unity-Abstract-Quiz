// Davide Carboni
// Mange score and other into the panel mangaer during the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelManager : MonoBehaviour
{
    public GameObject quizzeManager;
    public GameObject score;
    public GameObject answeredQuestion;
    public GameObject answeredCorrectQuestion;

    /// <summary>
    /// Get all data from quizze manager and refresh data in the panel
    /// </summary>
    void Update()
    {
       // calculate score value and update
       score.GetComponent<TextMeshProUGUI>().text = "SCORE: " + (quizzeManager.GetComponent<QuizzeManager>().answeredTrueQuestion * 10).ToString();
       // update the answered question
       answeredQuestion.GetComponent<TextMeshProUGUI>().text = "FOUNDED: " + quizzeManager.GetComponent<QuizzeManager>().answeredQuestion.ToString() + " / " + quizzeManager.GetComponent<QuizzeManager>().selectedQuestions.Count.ToString();
       // update the true answered question
       answeredCorrectQuestion.GetComponent<TextMeshProUGUI>().text = "CORRECT: " + quizzeManager.GetComponent<QuizzeManager>().answeredTrueQuestion.ToString() + " / " + quizzeManager.GetComponent<QuizzeManager>().selectedQuestions.Count.ToString();
    }
}
