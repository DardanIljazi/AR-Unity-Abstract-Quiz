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

        // Error/Exception managing
        if (questions == null)
        {
            Debug.LogError("[WARNING]: questions is equal to null. Is your QuestionsQuizzData superclass class configured in the same way the API (json) data is ?");
            PopupManager.PopupAlert("Error", "Question is equal to null (is data from API valid ?).\n" + Api.lastHttpWebRequestErrorMessage, "Return to menu", GameManager.Instance.pagesManager.ShowMenuPage);
        }

        numberOfQuestions = questions.GetQuestionsList().Count;

        // Error/Exception managing
        if (numberOfQuestions == 0)
        {
            Debug.LogError("[WARNING]: Number of questions is equal to 0, impossible to respond to this quizz.");
            PopupManager.PopupAlert("Error", "There is no question in this quizz\n" + Api.lastHttpWebRequestErrorMessage, "Return to menu", GameManager.Instance.pagesManager.ShowMenuPage);
        }

        LoadQuestionAndPossibleResponses(0);
    }

    public void LoadQuestionAndPossibleResponses(int arrayIndex)
    {
        respondQuizzScrollerController.Reset();
        quizzQuestion.text = questions.GetQuestionsList()[arrayIndex].question;

        QuestionQuizzData questionData = JsonUtility.FromJson<QuestionQuizzData>(JsonUtility.ToJson(questions.GetQuestionsList()[arrayIndex]));

        // Error/Exception managing
        if (questionData == null)
        {
            Debug.LogError("[WARNING]: questionData is null");
            PopupManager.PopupAlert("Error", "QuestionData is null (is data from API valid ?).\n" + Api.lastHttpWebRequestErrorMessage, "Return to menu", GameManager.Instance.pagesManager.ShowMenuPage);
        }

        for (int answerIndex = 0; answerIndex < questionData.answers.Count; ++answerIndex)
        {
            AnswerQuizzData answer = JsonUtility.FromJson<AnswerQuizzData>(JsonUtility.ToJson(questionData.answers[answerIndex])); ;

            Debug.Log(JsonUtility.ToJson(answer));

            if (answer.IsCorrectAnswer())
                goodAnswer = answer.GetDataToShowAsPossibleAnswer();

            respondQuizzScrollerController.AddDataToScroller(answer.Clone() as AnswerQuizzData);
        }

        // Error/Exception managing
        if (questionData.answers.Count == 0)
        {
            Debug.LogError("[WARNING]: No answer possible for this question");
            PopupManager.PopupAlert("Error", "No answer possible for this question", "Return to menu", GameManager.Instance.pagesManager.ShowMenuPage);
        }

        // Error/Exception managing
        if (goodAnswer == null)
        {
            Debug.LogError("[WARNING]: There is no good answer value");
            PopupManager.PopupAlert("Error", "There is no good answer value", "Return to menu", GameManager.Instance.pagesManager.ShowMenuPage);
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
