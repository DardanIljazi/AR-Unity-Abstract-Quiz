// Based on Davide Carboni's work done in 2019, took and modified/adapted by Dardan Iljazi in 2020 for a new project
// Api Data Connection: Get data from a url

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using System.Threading;
using static ApiData;
using System;

public class Api : MonoBehaviour
{
    private string api_URL = "http://192.168.1.111:8000/api/quizzes";            // Api url
    //private string api_URL = "https://awa-quizz.herokuapp.com/api/quizzes";            // Api url
    //public string api_URL = "http://www.api.carboni.ch/quizzes";                     // Api url for test 
    public string Api_URL { get => api_URL; set => api_URL = value; }
    public bool isStarted = false;                                                     // Api loaded state 
    public bool isConnected = false;                                                   // Api connection state
    private int count = 0;

    // Get list of quizzes from url
    public QuizzesData GetQuizzesListFromAPI()
    {
        string JSON_quizzes = CallHttpWebRequest(api_URL);           // get the json
        if (JSON_quizzes == null)
        {
            Debug.LogError("[CRITICAL]: (GetQuizzesListFromAPI) The url response doesn't return anything");
        }

        QuizzesData quizzesData = JsonUtility.FromJson<QuizzesData>(JSON_quizzes);      // convert all data into defined classes 

        if (quizzesData == null)
        {
            Debug.LogError("[CRITICAL]: quizzesData is null");
        }

        return quizzesData;
    }

    // Get the selected question
    public QuestionsQuizzData GetQuestionsQuizzListFromAPI(int id)
    {
        string JSON_quizze = CallHttpWebRequest(api_URL + "/" + id.ToString() +"/questions");   // get the json
        if (JSON_quizze == null)
        {
            Debug.LogError("[CRITICAL]: (GetQuestionsQuizzListFromAPI) The url response doesn't return anything");
        }

        QuestionsQuizzData questionsQuizzData = JsonUtility.FromJson<QuestionsQuizzData>(JSON_quizze);

        if (questionsQuizzData == null)
        {
            Debug.LogError("[CRITICAL]: questionsQuizzData is null");
        }

        return questionsQuizzData;     // convert all data into defined classes  
    }

    // get base string from API
    public string getStringDataFromAPI()
    {
        return CallHttpWebRequest(api_URL);                                // get the string data values
    }

    // Get all Data for api
    public static string lastHttpWebRequestErrorMessage = null;
    private string CallHttpWebRequest(string URL)
    {
        lastHttpWebRequestErrorMessage = null;
        try
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            string userApiToken = "2HFBBCs4TiiCbsJ470QjVrVjcntE5BEaee0VzXTNWuQ6rE8Ado1t29o05grDpo14mldCN4wPw31wAiJI";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL + "?api_token=" + userApiToken);
            Debug.Log(URL + "?api_token=" + userApiToken);
            req.Accept = "text/xml,text/plain,text/html,application/json";
            req.Method = "GET";
            HttpWebResponse result = (HttpWebResponse)req.GetResponse();

            Stream ReceiveStream = result.GetResponseStream();
            StreamReader reader = new StreamReader(ReceiveStream, System.Text.Encoding.UTF8);
            return reader.ReadToEnd();
        }
        catch(WebException e)
        {
            lastHttpWebRequestErrorMessage = e.Message;
            return null;
        }
    }


    bool _threadRunning;
    Thread _thread;
    Action actionAfterThread;
    bool blockCondition = false;
    private void Start()
    {
        isStarted = true;
        actionAfterThread = FinishedThread;
        StartNewThread();
    }

    public void StartNewThread()
    {
        blockCondition = false;
        _thread = new Thread(CheckConnection);
        _thread.Start();
    }

    public void FinishedThread()
    {
        if (!isConnected)
        {
            PopupManager.PopupAlert("Error", "Connection impossible. Are you connected to internet ?", "Retry", StartNewThread);
        }
        else
        {
            GameManager.Instance.pagesManager.ShowNext();
        }
    }

    void CheckConnection()
    {
        _threadRunning = true;

        count = 0;
        string res = null;
        bool loopForConnection = true;
        while (loopForConnection)                                         // check every 5 time if connection is available
        {
            count++;
            res = getStringDataFromAPI();

            if (res != null)
            {
                isConnected = true;
            }

            if (count >= 2)
                loopForConnection = false;

        }


        _threadRunning = false;
        _thread.Abort();
    }

    void Update()
    {
        if (!blockCondition && isStarted && !_thread.IsAlive)
        {
            blockCondition = true;
            FinishedThread();
        }
    }

}
