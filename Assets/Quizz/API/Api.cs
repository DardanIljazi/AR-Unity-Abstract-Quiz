// Davide Carboni
// Api Data Connection: Get data from a url

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using System.Threading;

public class Api : MonoBehaviour
{
    private string api_URL = "https://awa-quizz.herokuapp.com/api/quizzes";            // Api url
    //public string api_URL = "http://www.api.carboni.ch/quizzes";                     // Api url for test 
    public string Api_URL { get => api_URL; set => api_URL = value; }
    public bool isStarted = false;                                                     // Api loaded state 
    public bool isConnected = false;                                                   // Api connection state
    private int count = 0;
    private Thread _thread = null;
    private bool _threadRunning = false;


    // get all quizzes from url
    public  ApiData.GameQuizzes getGameQuizzesFromAPI()
    {
        string JSON_quizzes = CallHttpWebRequest(api_URL);                    // get the json file
        return JsonUtility.FromJson<ApiData.GameQuizzes>(JSON_quizzes);      // convert all data into defined classes 
    }

    // get a selected quiz from url
    public ApiData.GameQuizze getGameQuizzeFromAPI(string nQuizze)
    {
        string JSON_quizze = CallHttpWebRequest(api_URL + "/" + nQuizze);   // get the json file
        return JsonUtility.FromJson<ApiData.GameQuizze>(JSON_quizze);       // convert all data into defined classes  
    }

    // get base string from API
    public string getStringDataFromAPI()
    {
        return CallHttpWebRequest(api_URL);                                // get the string data values
    }

    // Get all Data for api
    private string CallHttpWebRequest(string URL)
    {
        try
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
            req.Headers["quizz-token"] = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VybmFtZSI6Imd1ZXN0IiwicGFzc3dvcmQiOiIkcGJrZGYyLXNoYTI1NiQyMDAwMCRjNjRWd3RnN0IuQThKeVJrN1AzL1h3JG9BRDloUnVEQTVkWVpKR1Y2cDNpdDBzYVFqdlFBemFZbi9wNW1kSGRDbDQifQ.P-KfTO8nq5oQNC_bIAY5VKOeNLyNbGE-gGrf0oIKQjc";
            req.Accept = "text/xml,text/plain,text/html,application/json";
            req.Method = "GET";
            HttpWebResponse result = (HttpWebResponse)req.GetResponse();
            Stream ReceiveStream = result.GetResponseStream();
            StreamReader reader = new StreamReader(ReceiveStream, System.Text.Encoding.UTF8);
            return reader.ReadToEnd();
        }
        catch
        {
            return null;
        }
    }

    private void Start()
    {
        isStarted = true;
        _thread = new Thread(CheckConnection);                         
    }

    private void CheckConnection()
    {
        count = 0;
        _threadRunning = true;
        string res = null;
       
        while (_threadRunning)                                         // check every 5 time if connection is available
        {
            _threadRunning = (count >= 5) ? false : true;              // stop chacking after 5 times to upgrade the connection state 
            count++;
            res = getStringDataFromAPI();
        }
        isConnected = (res == null) ? false : true;                    // upgrade connection state
        _thread.Abort();                                               // stop checking for connection

    }

    void Update()
    {
        if (!_thread.IsAlive)                                       // Check for internet connection
        {
            _thread = new Thread(CheckConnection);                  // start checking in background
            _thread.Start();
        }
    }

}
