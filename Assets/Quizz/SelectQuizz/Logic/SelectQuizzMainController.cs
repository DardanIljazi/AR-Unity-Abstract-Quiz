using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Main controller for the Select Quizz view (manages logic from view with api) 
 */
public class SelectQuizzMainController : MonoBehaviour
{
    
    public Api api;
    public SelectQuizzScrollerController selectQuizzScrollerController;
    public SelectQuizzTouchController selectQuizzTouchController;

    public RespondQuizzMainController respondQuizzMainController;

    public Pages pages;

    public void Start()
    {
        selectQuizzScrollerController.Initialize();
        selectQuizzTouchController.Initialize();

        // Get quizz list from API and put them into scroll list (SelectQuizzScrollerController)
        foreach (ApiData.IndexQuizz indexQuizz in api.getGameQuizzesFromAPI())
        {
            selectQuizzScrollerController.AddDataToScroller(new SelectQuizzData { title = indexQuizz.title, id = indexQuizz.id });
        }
    }

    private static SelectQuizzMainController singleton;
    public static SelectQuizzMainController getInstance()
    {
        if (singleton == null)
        {
            singleton = new SelectQuizzMainController();
        }

        return singleton;
    }


    public string selectedQuizzNumber;
    public void SetSelectedQuizzNumber(string quizzNumber)
    {
        selectedQuizzNumber = quizzNumber;
        pages.ShowNext();
    }


    /**
     * Delegates called from inside of SelectQuizzTouchController
     */
    public void QuizzSelected(string quizzNumber)
    {
        Debug.Log("Quizz Selected: " + quizzNumber);
        this.SetSelectedQuizzNumber(quizzNumber);
    }
}
