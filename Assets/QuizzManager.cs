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
    public string whichApiToUse = "Quizawa";

    Thread _thread; // Is the thread used to check connection to the api
    bool blockCondition = false;

    public bool isStarted = false;
    public bool isConnected = false;
    private int count = 0;
    

    void Start()
    {
        isStarted = true;
        StartApiDataConnectionCheck();

        if (whichApiToUse == "Quizawa")
        {
            GameManager.Instance.apiManager = new QuizawaApi();
        }
        else
        {
            Debug.LogError($"The api {whichApiToUse} is not defined");
        }
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
            //GameManager.Instance.pagesManager.ShowFirstPage();
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
            using (client.OpenRead(GameManager.Instance.apiManager.apiUrl))
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
