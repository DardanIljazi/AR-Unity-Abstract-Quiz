using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using System.Threading;
using System;
using System.Web;
using System.Text;
using System.Runtime.CompilerServices;
using static ApiDataStructure;

static class Constants
{
    public const string Api_Token_Not_Defined = "api_token_not_defined";
    public const string Api_Param_Name_Not_Defined = "api_param_name_not_defined";
}
/**
 * ApiManager class contains all actions/methods that can be done to the api
 * Here must be define every action and return data according to ApiData class
 */
public class ApiManager : ApiManagerStructure
{

    public string apiUrl;
    public string apiQuizzesUrl;
    public string apiQuizzesQuestionUrl;
    public string apiLoginUrl;
    public bool hasToHaveTokenForApi = true;
    public bool hasToLoginToGetToken = true;
    public TokenHttpEmplacement tokenHttpEmplacement = TokenHttpEmplacement.Everywhere; // The place where token has to be put in HTTP (url, header, body)
    public string apiKeyParamName = Constants.Api_Param_Name_Not_Defined; // Define the key to use to assign token value in HTTP (f. ex: not_defined_api_paramname={API_TOKEN} in url/header/body). 
                                                                 // This should be modified in the class that inherits from ApiManager. 
    public string apiToken = Constants.Api_Token_Not_Defined; // Should be defined into the class that inherits from APiManager (or later in the runtime) if token is used 

    // Get list of quizzes
    public override Quizzes GetQuizzesList()
    {
        string JSON_quizzes = NetworkRequestManager.HttpGetRequest(apiQuizzesUrl);
        if (JSON_quizzes == null)
        {
            Debug.LogError($"[WARNING]: Response for {GetActualMethodName()} is null");
        }

        Quizzes quizzesData = JsonUtility.FromJson<Quizzes>(JSON_quizzes); // convert all data into defined classes 
        if (quizzesData == null) // Exception/Error handling
        {
            Debug.LogError("[CRITICAL]: quizzesData is null");
        }

        return quizzesData;
    }
    
    public override Questions GetQuestionsListForQuizz(object quizzId)
    {
        string JSON_quizze = NetworkRequestManager.HttpGetRequest(apiQuizzesQuestionUrl);
        if (JSON_quizze == null)
        {
            Debug.LogError($"[WARNING]: Response for {GetActualMethodName()} is null");
        }

        Questions questionsQuizzData = JsonUtility.FromJson<Questions>(JSON_quizze);
        if (questionsQuizzData == null)
        {
            Debug.LogError("[CRITICAL]: questionsQuizzData is null");
        }

        return questionsQuizzData;
    }

    public override bool HasToHaveToken()
    {
        return hasToHaveTokenForApi;
    }

    public override bool HasToLoginToGetToken()
    {
        return hasToLoginToGetToken;
    }

    public override TokenHttpEmplacement GetTokenttpEmplacement()
    {
        return tokenHttpEmplacement;
    }

    public string GetApiKeyParamName()
    {
        return apiKeyParamName;
    }

    public string GetApiToken()
    {
        return apiToken;
    }

    public void SetApiToken(string token)
    {
        this.apiToken = token;
    }

    public bool IsTokenDefined()
    {
        return this.apiToken != Constants.Api_Token_Not_Defined;
    }

    // Connect to the api
    public ApiToken ConnectToQuizz(string pseudo, string password)
    {
        var post_key_values = new Dictionary<string, string>
        {
            { "pseudo", pseudo },
            { "password", password }
        };

        string JSON_connection = NetworkRequestManager.HttpPostRequest(apiUrl + "/users/login", post_key_values);
        if (JSON_connection == null)
        {
            Debug.LogError("[CRITICAL]: JSON_connection is null");
        }

        ApiToken connectionData = JsonUtility.FromJson<ApiToken>(JSON_connection);
        if (connectionData == null)
        {
            Debug.LogError("[CRITICAL]: connectionQuizzData is null");
        }

        return connectionData;
    }

    // Register to api
    public ApiToken RegisterToApi(string pseudo, string firstname, string lastname, string email, string password)
    {
        var post_key_values = new Dictionary<string, string>
        {
            { "pseudo", pseudo },
            { "firstname", firstname },
            { "lastname", lastname },
            { "email", email },
            { "password", password },
        };

        string JSON_register = NetworkRequestManager.HttpPostRequest(apiUrl + "/users", post_key_values);
        if (JSON_register == null)
        {
            Debug.LogError("[CRITICAL]: JSON_register is null");
        }

        ApiToken registrationData = JsonUtility.FromJson<ApiToken>(JSON_register);
        if (registrationData == null)
        {
            Debug.LogError("[CRITICAL]: registrationData is null");
        }

        return registrationData;
    }

    public override Quizz GetQuizzFromQuizzesList(object quizzId)
    {
        throw new NotImplementedException();
    }

    public override Question GetQuestionFromQuestionsList(object questionId)
    {
        throw new NotImplementedException();
    }

    public override Answers GetAnswersForQuestion(object questionId)
    {
        string JSON_answers = NetworkRequestManager.HttpGetRequest(apiQuizzesQuestionUrl);
        if (JSON_answers == null)
        {
            Debug.LogError($"[WARNING]: Response for {GetActualMethodName()} is null");
        }

        Answers answersData = JsonUtility.FromJson<Answers>(JSON_answers);
        if (answersData == null)
        {
            Debug.LogError("[CRITICAL]: questionsQuizzData is null");
        }

        return answersData;
    }

    public override Answer GetAnswerFromAnswersList(object answerId)
    {
        throw new NotImplementedException();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetActualMethodName()
    {
        var st = new System.Diagnostics.StackTrace(new System.Diagnostics.StackFrame(1));
        return st.GetFrame(0).GetMethod().Name;
    }
}
