using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static AbstractQuizzStructure;

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

    private Quizz _quizz;
    private Questions _questions;

    public override void ActionToDoWhenPageGoingToBeHidden()
    {
        
    }

    public override void ActionToDoWhenPageShowed()
    {
        
    }

    public void LoadQuizzQuestions(Quizz quizz)
    {
        this.Reset();
        respondQuizzScrollerController.Reset();
        respondQuizzScrollerController.Initialize();

        this._quizz = quizz;
        this._questions = GameManager.Instance.GetApiManager().GetQuestionsForQuizz(quizz.GetQuizzId());


        // Error/Exception managing
        if (this._questions == null)
        {
            Debug.LogError("[WARNING]: questions is equal to null. Is your QuestionsQuizzData superclass class configured in the same way the API (json) data is ?");
            PopupManager.PopupAlert("Error", "Question is equal to null (is data from API valid ?).\n" + NetworkRequestManager.lastHttpWebRequestErrorMessage, "Return to menu", GameManager.Instance.pagesManager.ShowMenuPage);
            return;
        }

        numberOfQuestions = _questions.GetQuestionsList().Count;

        // Error/Exception managing
        if (numberOfQuestions == 0)
        {
            Debug.LogError("[WARNING]: Number of questions is equal to 0, impossible to respond to this quizz.");
            PopupManager.PopupAlert("Error", "There is no question in this quizz\n" + NetworkRequestManager.lastHttpWebRequestErrorMessage, "Return to menu", GameManager.Instance.pagesManager.ShowMenuPage);
            return;
        }

        LoadQuestionAndAnswersForIndex(0);
    }

    public void LoadQuestionAndAnswersForIndex(int arrayIndex)
    {
        respondQuizzScrollerController.Reset();
        quizzQuestion.text = _questions.GetQuestionsList()[arrayIndex].GetQuestionTitle();

        Question question = _questions.GetQuestionsList()[arrayIndex];
        Answers answers = GameManager.Instance.GetApiManager().GetAnswersForQuestion(this._quizz.GetQuizzId(), question.GetQuestionId());


        // Error/Exception managing
        if (question == null)
        {
            Debug.LogError("[WARNING]: questionData is null");
            PopupManager.PopupAlert("Error", "QuestionData is null (is data from API valid ?).\n" + NetworkRequestManager.lastHttpWebRequestErrorMessage, "Return to menu", GameManager.Instance.pagesManager.ShowMenuPage);
            return;
        }

        for (int answerIndex = 0; answerIndex < answers.GetAnswersList().Count; ++answerIndex)
        {
            Answer answer = answers.GetAnswersList()[answerIndex];

            if (answer.IsCorrectAnswer())
                goodAnswer = answer.GetDataToShowAsPossibleAnswer();

            respondQuizzScrollerController.AddDataToScroller(answer.Clone() as Answer);
        }

        // Error/Exception managing
        if (answers.GetAnswersList().Count == 0)
        {
            Debug.LogError("[WARNING]: No answer possible for this question");
            PopupManager.PopupAlert("Error", "No answer possible for this question", "Return to menu", GameManager.Instance.pagesManager.ShowMenuPage);
            return;
        }

        // Error/Exception managing
        if (goodAnswer == null)
        {
            Debug.LogError("[WARNING]: There is no good answer value");
            PopupManager.PopupAlert("Error", "This question can't be responded, there is no good answer value returned by API", "Return to menu", GameManager.Instance.pagesManager.ShowMenuPage);
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
            LoadQuestionAndAnswersForIndex(actualQuestionArrayIndex);
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
        
        _quizz = null;
        _questions = null;
        numberOfQuestions = 0;
        actualQuestionArrayIndex = 0;
        rightResponses = 0;
        falseResponses = 0;
    }

    public void Finished()
    {
        GameManager.Instance.pagesManager.GoToPage("FinishQuizz");
        GameManager.Instance.finishQuizzManager.SetFinalScore(rightResponses, falseResponses, numberOfQuestions);
    }
}
