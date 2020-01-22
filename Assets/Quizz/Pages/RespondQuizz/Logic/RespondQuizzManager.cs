using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static ApiData;

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


    public void LoadQuizz(QuizzData quizz)
    {
        this.Reset();
        respondQuizzScrollerController.Initialize();

        gameQuizze = GameManager.Instance.api.GetQuestionsQuizzListFromAPI(quizz.GetQuizzId());

        numberOfQuestions = gameQuizze.questions.Count;

        LoadQuestionAndPossibleResponses(0);
    }

    public void LoadQuestionAndPossibleResponses(int arrayIndex)
    {
        respondQuizzScrollerController.Reset();

        quizzQuestion.text = gameQuizze.questions[arrayIndex].question;

        foreach (AnswerQuizzData answer in gameQuizze.questions[arrayIndex].answers)
        {
            if (answer.value == true)
                goodAnswer = answer.name;

            AnswerQuizzData answerQuizzData = new AnswerQuizzData(answer);

            respondQuizzScrollerController.AddDataToScroller(answerQuizzData);
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

    public void Reset()
    {
        numberOfQuestions = 0;
        actualQuestionArrayIndex = 0;
        rightResponses = 0;
        falseResponses = 0;
    }

    public void Finished()
    {
        GameManager.Instance.pagesManager.ShowNext();
        GameManager.Instance.finishQuizzManager.SetFinalScore(rightResponses, falseResponses, numberOfQuestions);
    }
}
