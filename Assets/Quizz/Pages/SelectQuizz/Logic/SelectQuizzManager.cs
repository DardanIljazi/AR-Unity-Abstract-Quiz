using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ApiData;

public class SelectQuizzManager : MonoBehaviour
{
    [Header("Link to the instance of scroller (contains all cells of data)")]
    public SelectQuizzScrollerController selectQuizzScrollerController;

    public void Start()
    {
        selectQuizzScrollerController.Initialize();

        QuizzesData quizzes = GameManager.Instance.api.GetQuizzesListFromAPI();
        
        if (quizzes == null)
        {
            Debug.LogError("[WARNING]: quizzes is equal to null. Is your QuizzesData superclass class configured in the same way the API (json) data is ?");

            PopupManager.PopupAlert("Error", "Quizzes is equal to null (is data from API valid ?).\n" + Api.lastHttpWebRequestErrorMessage);
        }

        // Get quizz list from API and put them into scroll list (SelectQuizzScrollerController)
        foreach (Quizz indexQuizz in quizzes)
        {
            QuizzData quizzData = JsonUtility.FromJson<QuizzData>(JsonUtility.ToJson(indexQuizz)); ;

            selectQuizzScrollerController.AddDataToScroller(quizzData.Clone() as QuizzData);
        }
    }

    public void QuizzSelected(QuizzData quizz)
    {
        GameManager.Instance.pagesManager.ShowNext();
        GameManager.Instance.respondQuizzManager.LoadQuizz(quizz);
    }
}
