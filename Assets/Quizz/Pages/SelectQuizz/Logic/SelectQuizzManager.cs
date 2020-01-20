using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectQuizzManager : MonoBehaviour
{
    [Header("Link to the instance of scroller (contains all cells of data)")]
    public SelectQuizzScrollerController selectQuizzScrollerController;

    public void Start()
    {
        selectQuizzScrollerController.Initialize();

        // Get quizz list from API and put them into scroll list (SelectQuizzScrollerController)
        foreach (ApiData.IndexQuizz indexQuizz in GameManager.Instance.api.getGameQuizzesFromAPI())
        {
            selectQuizzScrollerController.AddDataToScroller(new SelectQuizzData { title = indexQuizz.title, id = indexQuizz.id });
        }
    }

    [HideInInspector]
    public string selectedQuizzId;
    public void QuizzSelected(string quizzId)
    {
        selectedQuizzId = quizzId;
        GameManager.Instance.pagesManager.ShowNext();
    }
}
