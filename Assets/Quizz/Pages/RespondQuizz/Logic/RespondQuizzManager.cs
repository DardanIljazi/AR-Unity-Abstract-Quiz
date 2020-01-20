using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class RespondQuizzManager : MonoBehaviour
{
    public RespondQuizzScrollerController respondQuizzScrollerController;

    public Text quizzQuestion;
    public Image quizzImage;

    private int numberOfQuestions = 0;
    private int actualQuestionArrayIndex = 0;
    private string goodAnswer;
    private int rightResponses = 0;
    private int falseResponses = 0;

    private ApiData.GameQuizze gameQuizze;
    public void Start()
    {
        respondQuizzScrollerController.Initialize();

        gameQuizze = GameManager.Instance.api.getGameQuizzeFromAPI(GameManager.Instance.selectQuizzManager.selectedQuizzId);

        numberOfQuestions = gameQuizze.questions.Count;

        LoadQuestionAndPossibleResponses(0);
    }

    public void LoadQuestionAndPossibleResponses(int arrayIndex)
    {
        respondQuizzScrollerController.Reset();

        quizzQuestion.text = gameQuizze.questions[arrayIndex].question;

        foreach (ApiData.Answer answer in gameQuizze.questions[arrayIndex].answers)
        {
            if (answer.value == true)
                goodAnswer = answer.name;

            RespondQuizzData respondQuizzData = new RespondQuizzData();
            respondQuizzData.SetDataToShowInCellView(answer.name);

            respondQuizzScrollerController.AddDataToScroller(respondQuizzData);
        }
    } 

    public void SaveResponseAndGoToNext(string response)
    {
        if (goodAnswer.Equals(response))
        {
            rightResponses++;
        }
        else
        {
            falseResponses++;
        }

        actualQuestionArrayIndex++;
            
        if (actualQuestionArrayIndex <= numberOfQuestions - 1)
        {
            LoadQuestionAndPossibleResponses(actualQuestionArrayIndex);
        }
        else
        {
            Finished();
        }
    }

    public void ResponseSelected(string response)
    {
        SaveResponseAndGoToNext(response);
    }

    public void Finished()
    {
        GameManager.Instance.finishQuizzManager.SetFinalScore(rightResponses, falseResponses, numberOfQuestions);
        GameManager.Instance.pagesManager.ShowNext();
    }
}
