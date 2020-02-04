using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbstractQuizzStructure;

/**
 * SelectQuizzManager is the manager for the SelectQuizz page (shows the list of quizzes/manages data about list of quizzes and so on..)
 */
public class SelectQuizzManager : PageLogic
{
    [Header("Link to the instance of scroller (contains all cells of data)")]
    public SelectQuizzScrollerController selectQuizzScrollerController;

    public override void ActionToDoWhenPageShowed()
    {
        GameManager.Instance.pagesManager.ShowLoadingPage();
        selectQuizzScrollerController.Reset();
        selectQuizzScrollerController.Initialize();


        Quizzes quizzes = GameManager.Instance.GetApiManager().GetQuizzes();

        if (quizzes == null)
        {
            Debug.LogError("[WARNING]: quizzes is equal to null. Is your QuizzesData superclass class configured in the same way the API (json) data is ?");
            PopupManager.PopupAlert("Error", "Quizzes is equal to null (is data from API valid ?).\n" + NetworkRequestManager.lastHttpWebRequestErrorMessage);
        }

        // Put each quizz to the scroller
        foreach (Quizz quizz in quizzes.GetQuizzesList())
        {
            selectQuizzScrollerController.AddDataToScroller(quizz.Clone() as Quizz);
        }

        GameManager.Instance.pagesManager.HideLoadingPage();
    }

    // Called from the cell view (inside SelectQuizzScrollerController) when a quizz is selected
    public void SelectQuizzToShow(Quizz quizz)
    {
        GameManager.Instance.pagesManager.GoToPage("RespondQuizz");
        GameManager.Instance.respondQuizzManager.LoadQuizzQuestions(quizz);
    }
}
