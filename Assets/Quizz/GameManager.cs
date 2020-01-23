// Davide Carboni
// Game Manager: Class to manwge the game
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    /**
     * Managers in the game (put them all here)
     */
    public SelectQuizzManager   selectQuizzManager;
    public RespondQuizzManager  respondQuizzManager;
    public FinishQuizzManager   finishQuizzManager;
    public PopupManager         popupManager;
    public Pages pagesManager;
    public Api api;
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
}
