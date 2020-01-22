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

    private QuestionsQuizzData questions;


    public void LoadQuizz(QuizzData quizz)
    {
        this.Reset();
        respondQuizzScrollerController.Initialize();

        questions = GameManager.Instance.api.GetQuestionsQuizzListFromAPI(quizz.GetQuizzId());

        if (questions == null)
        {
            Debug.LogError("[WARNING]: questions is equal to null. Is your QuestionsQuizzData superclass class configured in the same way the API (json) data is ?");
        }

        numberOfQuestions = questions.GetQuestionsList().Count;

        LoadQuestionAndPossibleResponses(0);
    }

    public void LoadQuestionAndPossibleResponses(int arrayIndex)
    {
        respondQuizzScrollerController.Reset();

        quizzQuestion.text = questions.GetQuestionsList()[arrayIndex].question;

        foreach (AnswerQuizzData answer in questions.GetQuestionsList()[arrayIndex].answers)
        {
            if (answer.IsCorrectAnswer())
                goodAnswer = answer.GetDataToShowAsPossibleAnswer();

            AnswerQuizzData answerQuizzData = new AnswerQuizzData(answer);

            respondQuizzScrollerController.AddDataToScroller(answerQuizzData);
        }
    }

    public void SaveResponseAndGoToNext(AnswerQuizzData answer)
    {
        if (goodAnswer.Equals(answer))
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

    public void ResponseSelected(AnswerQuizzData answer)
    {
        SaveResponseAndGoToNext(answer);
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
