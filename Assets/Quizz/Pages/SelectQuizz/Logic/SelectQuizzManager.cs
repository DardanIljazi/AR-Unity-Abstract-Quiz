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
        } 

        // Get quizz list from API and put them into scroll list (SelectQuizzScrollerController)
        foreach (ApiData.Quizz indexQuizz in GameManager.Instance.api.GetQuizzesListFromAPI())
        {
            selectQuizzScrollerController.AddDataToScroller(new QuizzData { title = indexQuizz.title, id = indexQuizz.id });
        }
    }

    public void QuizzSelected(QuizzData quizz)
    {
        GameManager.Instance.pagesManager.ShowNext();
        GameManager.Instance.respondQuizzManager.LoadQuizz(quizz);
    }
}
