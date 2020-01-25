using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static ApiData;

/**
 * RespondQuizzManager is the manager for the RespondQuizz page (shows a list of possible response and manages the response selected)
 */
public class RespondQuizzManager : PageLogic
{
    public RespondQuizzScrollerController respondQuizzScrollerController;

    public Text quizzQuestion;
    public Image quizzImage;

    private int numberOfQuestions = 0;
    private int actualQuestionArrayIndex = 0;
    private string goodAnswer;
    private int rightResponses = 0;
    private int falseResponses = 0;

    private Questions questions;

    public void LoadQuizz(Quizz quizz)
    {
        this.Reset();
        respondQuizzScrollerController.Initialize();

        questions = GameManager.Instance.apiManager.GetQuestionsQuizzListFromAPI(quizz.GetQuizzId());

        // Error/Exception managing
        if (questions == null)
        {
            Debug.LogError("[WARNING]: questions is equal to null. Is your QuestionsQuizzData superclass class configured in the same way the API (json) data is ?");
            PopupManager.PopupAlert("Error", "Question is equal to null (is data from API valid ?).\n" + ApiManager.lastHttpWebRequestErrorMessage, "Return to menu", GameManager.Instance.pagesManager.ShowMenuPage);
            return;
        }

        numberOfQuestions = questions.GetQuestionsList().Count;

        // Error/Exception managing
        if (numberOfQuestions == 0)
        {
            Debug.LogError("[WARNING]: Number of questions is equal to 0, impossible to respond to this quizz.");
            PopupManager.PopupAlert("Error", "There is no question in this quizz\n" + ApiManager.lastHttpWebRequestErrorMessage, "Return to menu", GameManager.Instance.pagesManager.ShowMenuPage);
            return;
        }

        LoadQuestionAndPossibleResponses(0);
    }

    public void LoadQuestionAndPossibleResponses(int arrayIndex)
    {
        respondQuizzScrollerController.Reset();
        quizzQuestion.text = questions.GetQuestionsList()[arrayIndex].question;

        Question questionData = JsonUtility.FromJson<Question>(JsonUtility.ToJson(questions.GetQuestionsList()[arrayIndex]));

        // Error/Exception managing
        if (questionData == null)
        {
            Debug.LogError("[WARNING]: questionData is null");
            PopupManager.PopupAlert("Error", "QuestionData is null (is data from API valid ?).\n" + ApiManager.lastHttpWebRequestErrorMessage, "Return to menu", GameManager.Instance.pagesManager.ShowMenuPage);
            return;
        }

        for (int answerIndex = 0; answerIndex < questionData.answers.Count; ++answerIndex)
        {
            Answer answer = JsonUtility.FromJson<Answer>(JsonUtility.ToJson(questionData.answers[answerIndex])); ;

            Debug.Log(JsonUtility.ToJson(answer));

            if (answer.IsCorrectAnswer())
                goodAnswer = answer.GetDataToShowAsPossibleAnswer();

            respondQuizzScrollerController.AddDataToScroller(answer.Clone() as Answer);
        }

        // Error/Exception managing
        if (questionData.answers.Count == 0)
        {
            Debug.LogError("[WARNING]: No answer possible for this question");
            PopupManager.PopupAlert("Error", "No answer possible for this question", "Return to menu", GameManager.Instance.pagesManager.ShowMenuPage);
            return;
        }

        // Error/Exception managing
        if (goodAnswer == null)
        {
            Debug.LogError("[WARNING]: There is no good answer value");
            PopupManager.PopupAlert("Error", "There is no good answer value", "Return to menu", GameManager.Instance.pagesManager.ShowMenuPage);
            return;
        }
    }

    public void SaveResponseAndGoToNext(Answer answer)
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

    public void ResponseSelected(Answer answer)
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
