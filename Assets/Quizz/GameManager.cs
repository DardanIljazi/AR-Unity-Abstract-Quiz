// By Dardan Iljazi

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    /**
     * Managers in the game (put them all here)
     */
    public QuizzManager         quizzManager;
    public LoginManager         loginManager;
    public SelectQuizzManager   selectQuizzManager;
    public RespondQuizzManager  respondQuizzManager;
    public FinishQuizzManager   finishQuizzManager;
    public PopupManager         popupManager;
    public LoadingManager       loadingManager;
    public PagesManager         pagesManager;
    private ApiManager          apiManager;
    /** -- END of managers **/

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetApiToUse(ApiManager newApiManager)
    {
        if (apiManager != null)
        {
            // TODO: delete this warning if the tests shows that we can switch API at runtime without any problem
            Debug.LogError("[WARNING]: You try to define a new  apiManager when this was already set. This could work but has not been tested yet");
        }

        this.apiManager = newApiManager;
    }

    public ApiManager GetApiManager()
    {
        if (this.apiManager == null)
        {
            Debug.LogError("[CRITICAL]: The apiManager is not yet defined and you try to access it");
        }

        return this.apiManager;
    }
}
