using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;

/**
 * QuizzManager manages the action to do first before going further to the quizz
 * It checks for connection and make it possible to choose which quizz api we want to use at runtime
 * 
 */
public class QuizzManager : MonoBehaviour
{
    Thread _thread; // Is the thread used to check connection to the api
    bool blockCondition = false;

    bool isStarted = false;
    bool isConnected = false;
    int count = 0;

    void Start()
    {
        isStarted = true;
        StartApiDataConnectionCheck();
    }

    public void StartApiDataConnectionCheck()
    {
        blockCondition = false;
        _thread = new Thread(CheckConnection);
        _thread.Start();
    }

    public void FinishedThread()
    {
        if (!isConnected)
        {
            PopupManager.PopupAlert("Error", "Connection impossible. Are you connected to internet ?\n\nOr maybe the API doesn't work?" + NetworkRequestManager.lastHttpWebRequestErrorMessage, "Retry", StartApiDataConnectionCheck);
        }
        else
        {
            if ( GameManager.Instance.GetApiManager().HasToHaveToken() && 
                !GameManager.Instance.GetApiManager().IsApiTokenDefined())
            {

                // If no login is needed and we have not defined yet an api token --> error (something was not configured properly ?)
                if (!GameManager.Instance.GetApiManager().HasToLoginToGetToken())
                {
                    Debug.LogError(
                        "Impossible to go further. You configuration contains some incoherence. " +
                        "You defined to need token for the api and this one is not defined. " +
                        "Furthemore, you didn't set hasToLoginTOGetToken as true. Please configure your class inherited from ApiManager properly or read the doc.");
                    return;
                }
                else // User has to login first to get token
                {
                    GameManager.Instance.pagesManager.GoToPage("Login");
                }
            } else
            {
                GameManager.Instance.pagesManager.ShowMenuPage();
            } 



        }
    }

    void CheckConnection()
    {
        count = 0;
        bool loopForConnection = true;
        while (loopForConnection)
        {
            count++;

            isConnected = IsApiUp();

            if (count >= 2)
                loopForConnection = false;
        }

        _thread.Abort();
    }

    public bool IsApiUp()
    {
        try
        {
            using (var client = new WebClient())
            using (client.OpenRead(GameManager.Instance.GetApiManager().apiUrl))
                return true;
        }
        catch (WebException e)
        {
            if (e.Status == WebExceptionStatus.ProtocolError)
            {
                return true;
            }

            return false;
        }
    }

    void Update()
    {
        if (!blockCondition && isStarted && _thread != null && !_thread.IsAlive) // When the thread has terminated to do his work
        {
            blockCondition = true;
            FinishedThread();
        }
    }
}
