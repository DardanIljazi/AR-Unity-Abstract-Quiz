using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ApiData;

/**
 * SelectQuizzManager is the manager for the SelectQuizz page (shows the list of quizzes/manages data about list of quizzes and so on..)
 */
public class SelectQuizzManager : PageManager
{
    [Header("Link to the instance of scroller (contains all cells of data)")]
    public SelectQuizzScrollerController selectQuizzScrollerController;

    public override void ActionToDoWhenPageShowed()
    {
        GameManager.Instance.pagesManager.ShowLoadingPage();

        selectQuizzScrollerController.Initialize();

        QuizzesData quizzes = GameManager.Instance.apiManager.GetQuizzesListFromAPI();

        if (quizzes == null)
        {
            Debug.LogError("[WARNING]: quizzes is equal to null. Is your QuizzesData superclass class configured in the same way the API (json) data is ?");

            PopupManager.PopupAlert("Error", "Quizzes is equal to null (is data from API valid ?).\n" + ApiManager.lastHttpWebRequestErrorMessage);
        }


        // Get quizz list from API and put them into scroll list (SelectQuizzScrollerController)
        foreach (Quizz indexQuizz in quizzes)
        {
            QuizzData quizzData = JsonUtility.FromJson<QuizzData>(JsonUtility.ToJson(indexQuizz)); ;

            selectQuizzScrollerController.AddDataToScroller(quizzData.Clone() as QuizzData);
        }

        GameManager.Instance.pagesManager.HideLoadingPage();
    }

    public void QuizzSelected(QuizzData quizz)
    {
        GameManager.Instance.pagesManager.ShowNext();
        GameManager.Instance.respondQuizzManager.LoadQuizz(quizz);
    }
}
