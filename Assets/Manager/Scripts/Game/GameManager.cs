// Davide Carboni
// Game Manager: Class to manwge the game
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static int nQuizz = 0;                  // number of available quizz 
    public static string selectedQuizz;            // selected quiz from menu

    /// <summary>
    /// Instantiate the class that are available in all game scenes
    /// </summary>
    public static GameManager Instance
    {
        get;
        set;
    }

    /// <summary>
    /// On load: take the class available in all games scenes
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        Instance = this;
    }
}
