using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using System.Threading;
using static ApiData;
using System;
using System.Web;
using System.Text;

/**
 * ApiManager class contains all actions/methods that can be done to the api
 * Here must be define every action and return data according to ApiData class
 */
public class ApiManager : MonoBehaviour
{
    public string api_url { get; set; } = "http://192.168.1.111:8000/api";

    public static string lastHttpWebRequestErrorMessage = null;
    public ApiTokenManager apiTokenManager;

    // Get list of quizzes
    public Quizzes GetQuizzesListFromAPI()
    {
        string JSON_quizzes = HttpGetRequest(api_url + "/quizzes");
        if (JSON_quizzes == null) // Exception/Error handling
        {
            Debug.LogError("[CRITICAL]: (GetQuizzesListFromAPI) The url response doesn't return anything");
        }

        Quizzes quizzesData = JsonUtility.FromJson<Quizzes>(JSON_quizzes); // convert all data into defined classes 
        if (quizzesData == null) // Exception/Error handling
        {
            Debug.LogError("[CRITICAL]: quizzesData is null");
        }

        return quizzesData;
    }

    // Get the question for the quizz id
    public Questions GetQuestionsQuizzListFromAPI(int id)
    {
        string JSON_quizze = HttpGetRequest(api_url + "/quizzes/" + id.ToString() +"/questions");
        if (JSON_quizze == null)
        {
            Debug.LogError("[CRITICAL]: (GetQuestionsQuizzListFromAPI) The url response doesn't return anything");
        }

        Questions questionsQuizzData = JsonUtility.FromJson<Questions>(JSON_quizze);
        if (questionsQuizzData == null)
        {
            Debug.LogError("[CRITICAL]: questionsQuizzData is null");
        }

        return questionsQuizzData;
    }

    // Connect to the api
    public ApiToken ConnectToQuizz(string pseudo, string password)
    {
        var post_key_values = new Dictionary<string, string>
        {
            { "pseudo", pseudo },
            { "password", password }
        };

        string JSON_connection = HttpPostRequest(api_url + "/users/login", post_key_values);
        if (JSON_connection == null)
        {
            Debug.LogError("[CRITICAL]: JSON_connection is null");
        }

        ApiToken connectionQuizzData = JsonUtility.FromJson<ApiToken>(JSON_connection);
        if (connectionQuizzData == null)
        {
            Debug.LogError("[CRITICAL]: connectionQuizzData is null");
        }

        return connectionQuizzData;
    }

    public string tryGetResponseFromAPI()
    {
        return HttpGetRequest(api_url + "/quizzes");
    }

    private string HttpGetRequest(string url)
    {
        lastHttpWebRequestErrorMessage = null;

        try
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Accept = "text/xml,text/plain,text/html,application/json";
            req.Method = "GET";
            req.Timeout = 2000;

            if (GameManager.Instance.apiManager.apiTokenManager.IsApiTokenDataDefined()) // If Api token is defined
            {
                string postData = null; // Workaround to pass the ref postData (this make no sense in GET request) --> Has to be changed
                SetTokenAccordingToEmplacement(ref req, "GET", ref postData, ApiToken.GetTokenHttpEmplacement());
            }

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

    private string HttpPostRequest(string url, Dictionary<string, string> postParameters)
    {
        lastHttpWebRequestErrorMessage = null;

        try
        {
            string postData = "";

            int keyIndex = 0;
            foreach (string key in postParameters.Keys)
            {
                postData += HttpUtility.UrlEncode(key) + "="
                      + HttpUtility.UrlEncode(postParameters[key]);
                if (keyIndex < postParameters.Keys.Count - 1)
                    postData += "&";

                keyIndex++;
            }

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Accept = "text/xml,text/plain,text/html,application/json";
            req.Method = "POST";
            req.Timeout = 2000;

            if (GameManager.Instance.apiManager.apiTokenManager.IsApiTokenDataDefined()) // If Api token is defined
            {
                SetTokenAccordingToEmplacement(ref req, "POST", ref postData, ApiToken.GetTokenHttpEmplacement());
            }

            byte[] data = Encoding.ASCII.GetBytes(postData);

            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;

            Stream requestStream = req.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)req.GetResponse();

            Stream responseStream = myHttpWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

            string pageContent = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            responseStream.Close();

            myHttpWebResponse.Close();

            Debug.Log("pageContent: ");
            Debug.Log(pageContent);
            return pageContent;
        }
        catch (WebException e)
        {
            Debug.LogError("[WebException]: " + e.Message);
            lastHttpWebRequestErrorMessage = e.Message;
            return null;
        }
    }

    public void SetTokenAccordingToEmplacement(ref HttpWebRequest req, string method, ref string bodyData, ApiToken.TokenttpEmplacement tokenttpEmplacement)
    {
        if (tokenttpEmplacement == ApiToken.TokenttpEmplacement.BodyOrUrlParam || 
            tokenttpEmplacement == ApiToken.TokenttpEmplacement.Everywhere)
        {
            if (method == "GET") // Put token in url
            {
                PutTokenInUrl(ref req);
            }
            else if (method == "POST") // Put token in body
            {
                PutTokenInBody(ref bodyData);
            }
            else // Not "GET" or "POST"
            {
                Debug.LogError("[WARNING]: The parameter (method) passed to SetTokenAccordingToEmplacement is neither \"GET\" or \"POST\"");
            }
        }

        if (tokenttpEmplacement == ApiToken.TokenttpEmplacement.Header ||
            tokenttpEmplacement == ApiToken.TokenttpEmplacement.Everywhere)
        {
            PutTokenInHeader(ref req);
        }
    }

    void PutTokenInUrl(ref HttpWebRequest req)
    {
        // It is somethign tricky here: We can't modify url after creating HttpWebRequest but here we need it (because api token is not directly into the url "logic" but is appart)
        // We create a new HttpWebRequest (reqWithToken) with the api token inside of it.
        // We have to assign values of req to reqWithToken by cloning req with WebRequestExtensions.CloneRequest

        string actualUrl = req.RequestUri.ToString();
        string modifiedUrl = actualUrl;

        if (req.RequestUri.Query == null || req.RequestUri.Query.Length == 0) // There is no query (?param=value&param2=value2..) already set in the url
        {
            modifiedUrl += "?";
            modifiedUrl += ApiToken.GetApiKeyParam() + "=" + GameManager.Instance.apiManager.apiTokenManager.GetApiToken();
        }
        else
        {
            modifiedUrl += "&";
            modifiedUrl += ApiToken.GetApiKeyParam() + "=" + GameManager.Instance.apiManager.apiTokenManager.GetApiToken();
        }

        HttpWebRequest reqWithToken = WebRequestExtensions.CloneRequest(req, new Uri(modifiedUrl));
        req = reqWithToken;
    }

    void PutTokenInBody(ref string bodyData)
    {
        if (bodyData != null)
        {
            bodyData += "&";
            bodyData += ApiToken.GetApiKeyParam() + "=" + GameManager.Instance.apiManager.apiTokenManager.GetApiToken();
        }
        else
        {
            bodyData += ApiToken.GetApiKeyParam() + "=" + GameManager.Instance.apiManager.apiTokenManager.GetApiToken();
        }
    }

    void PutTokenInHeader(ref HttpWebRequest req)
    {
        req.Headers[ApiToken.GetApiKeyParam()] = GameManager.Instance.apiManager.apiTokenManager.GetApiToken();
    }

    Thread _thread;
    Action actionAfterThread;
    bool blockCondition = false;

    public bool isStarted = false;   // Api loaded state 
    public bool isConnected = false; // Api connection state
    private int count = 0; // Api number of try to access to it (when no connection)

    void Start()
    {
        isStarted = true;
        actionAfterThread = FinishedThread;
    }

    public void StartApiDataQuerying()
    {
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

        count = 0;
        string res = null;
        bool loopForConnection = true;
        while (loopForConnection)
        {
            count++;
            res = tryGetResponseFromAPI();

            if (res != null)
            {
                isConnected = true;
            }

            if (count >= 2)
                loopForConnection = false;
        }

        _thread.Abort();
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
