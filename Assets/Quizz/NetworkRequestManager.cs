using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using UnityEngine;
using static ApiManagerStructure;

public class NetworkRequestManager : MonoBehaviour
{
    public static string lastHttpWebRequestErrorMessage = null;

    public static string HttpGetRequest(string url)
    {
        lastHttpWebRequestErrorMessage = null;

        try
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Accept = "text/xml,text/plain,text/html,application/json";
            req.Method = "GET";
            req.Timeout = 2000;

            if (GameManager.Instance.apiManager.HasToHaveToken())
            {
                string postData = null; // Workaround to pass the ref postData (this make no sense in GET request) --> Has to be changed later
                SetTokenAccordingToEmplacement(ref req, "GET", ref postData, GameManager.Instance.apiManager.GetTokenttpEmplacement());
            }

            HttpWebResponse result = (HttpWebResponse)req.GetResponse();

            Stream ReceiveStream = result.GetResponseStream();
            StreamReader reader = new StreamReader(ReceiveStream, System.Text.Encoding.UTF8);
            return reader.ReadToEnd();
        }
        catch (WebException e)
        {
            var resp = new StreamReader(e.Response.GetResponseStream()).ReadToEnd();
            Debug.LogError("[WebException]: " + e.Message + "\n" + resp);
            lastHttpWebRequestErrorMessage = e.Message + "\n" + resp;
            return null;
        }
    }

    public static string HttpPostRequest(string url, Dictionary<string, string> postParameters)
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

            if (GameManager.Instance.apiManager.IsTokenDefined()) // If Api token is defined
            {
                SetTokenAccordingToEmplacement(ref req, "POST", ref postData, GameManager.Instance.apiManager.GetTokenttpEmplacement());
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
            var resp = new StreamReader(e.Response.GetResponseStream()).ReadToEnd();
            Debug.LogError("[WebException]: " + e.Message + "\n" + resp);
            lastHttpWebRequestErrorMessage = e.Message + "\n" + resp;
            return null;
        }
    }

    public static void SetTokenAccordingToEmplacement(ref HttpWebRequest req, string method, ref string bodyData, TokenHttpEmplacement tokenttpEmplacement)
    {
        if (tokenttpEmplacement == TokenHttpEmplacement.BodyOrUrlParam ||
            tokenttpEmplacement == TokenHttpEmplacement.Everywhere)
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

        if (tokenttpEmplacement == TokenHttpEmplacement.Header ||
            tokenttpEmplacement == TokenHttpEmplacement.Everywhere)
        {
            PutTokenInHeader(ref req);
        }
    }

    public static void PutTokenInUrl(ref HttpWebRequest req)
    {
        // It is somethign tricky here: We can't modify url after creating HttpWebRequest but here we need it (because api token is not directly into the url "logic" but is appart)
        // We create a new HttpWebRequest (reqWithToken) with the api token inside of it.
        // We have to assign values of req to reqWithToken by cloning req with WebRequestExtensions.CloneRequest

        string actualUrl = req.RequestUri.ToString();
        string modifiedUrl = actualUrl;

        if (req.RequestUri.Query == null || req.RequestUri.Query.Length == 0) // There is no query (?param=value&param2=value2..) already set in the url
        {
            modifiedUrl += "?";
            modifiedUrl += GameManager.Instance.apiManager.GetApiKeyParamName() + "=" + GameManager.Instance.apiManager.GetApiToken();
        }
        else
        {
            modifiedUrl += "&";
            modifiedUrl += GameManager.Instance.apiManager.GetApiKeyParamName() + "=" + GameManager.Instance.apiManager.GetApiToken();
        }

        HttpWebRequest reqWithToken = WebRequestExtensions.CloneRequest(req, new Uri(modifiedUrl));
        req = reqWithToken;
    }

    public static void PutTokenInBody(ref string bodyData)
    {
        if (bodyData != null)
        {
            bodyData += "&";
            bodyData += GameManager.Instance.apiManager.GetApiKeyParamName() + "=" + GameManager.Instance.apiManager.GetApiToken();
        }
        else
        {
            bodyData += GameManager.Instance.apiManager.GetApiKeyParamName() + "=" + GameManager.Instance.apiManager.GetApiToken();
        }
    }

    public static void PutTokenInHeader(ref HttpWebRequest req)
    {
        req.Headers[GameManager.Instance.apiManager.GetApiKeyParamName()] = GameManager.Instance.apiManager.GetApiToken();
    }
}
