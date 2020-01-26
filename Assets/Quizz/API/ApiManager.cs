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
using static AbstractQuizzStructure;
using static ApiManagerStructure;

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

    public bool hasToHaveTokenForApi;
    public bool hasToLoginToGetToken;

    public TokenHttpEmplacement tokenHttpEmplacement; // The place where token has to be put in HTTP (url, header, body)

    public string apiKeyParamName; // Define the key to use to assign token value in HTTP (f. ex: not_defined_api_paramname={API_TOKEN} in url/header/body). 
                                   // This should be modified in the class that inherits from ApiManager. 

    [Header("The api token can stay empty if user has to login to get it")]
    public string apiToken; // Should be defined into the class that inherits from APiManager (or later in the runtime) if token is used 

    private ApiManager child; // Contain a reference to the child that inherits from ApiManager. We give the task to serialize 


    public ApiManager()
    {
        this.apiToken = Constants.Api_Token_Not_Defined;
        this.apiKeyParamName = Constants.Api_Param_Name_Not_Defined;
        this.tokenHttpEmplacement = TokenHttpEmplacement.Everywhere;
        this.hasToHaveTokenForApi = true;
        this.hasToLoginToGetToken = true;
    }

    void Start()
    {

        if (apiUrl == null || apiUrl.Length == 0)
        {
            Debug.LogError("apiUrl is empty or not defined. Please fill it");
            return;
        }
        if (apiQuizzesUrl == null || apiQuizzesUrl.Length == 0)
        {
            Debug.LogError("apiQuizzesUrl is empty or not defined. Please fill it");
            return;
        }
        if (apiQuizzesQuestionUrl == null || apiQuizzesQuestionUrl.Length == 0)
        {
            Debug.LogError("apiQuizzesQuestionUrl is empty or not defined. Please fill it");
            return;
        }
    }


    // Get list of quizzes
    public override Quizzes GetQuizzesList()
    {
        string JSON_quizzes = NetworkRequestManager.HttpGetRequest(apiQuizzesUrl);
        if (JSON_quizzes == null)
        {
            Debug.LogError($"[WARNING]: Response for {GetActualMethodName()} is null");
        }

        Quizzes quizzesData = child.SerializeQuizzes(JSON_quizzes);

        if (quizzesData == null) // Exception/Error handling
        {
            Debug.LogError("[CRITICAL]: quizzesData is null");
        }

        return quizzesData;
    }
    public virtual Quizzes SerializeQuizzes(string json) // Child has to override this method so that data is serialized from child within GetQuizzesList
    {
        throw new NotImplementedException();
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

    // Connect to the api
    public ApiToken ConnectToQuizz(string pseudo, string password)
    {
        var post_key_values = new Dictionary<string, string>
        {
            { "pseudo", pseudo },
            { "password", password }
        };

        string JSON_connection = NetworkRequestManager.HttpPostRequest(apiLoginUrl, post_key_values);
        if (JSON_connection == null)
        {
            Debug.LogError("[CRITICAL]: JSON_connection is null");
        }

        ApiToken connectionData = child.SerializeApiToken(JSON_connection);
        if (connectionData == null)
        {
            Debug.LogError("[CRITICAL]: connectionQuizzData is null");
        }

        return connectionData;
    }
    public virtual ApiToken SerializeApiToken(string json) // Child has to override this method so that data is serialized from child within ConnectToQuizz
    {
        throw new NotImplementedException();
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

    public bool IsApiTokenDefined()
    {
        return this.apiToken != null && this.apiToken != Constants.Api_Token_Not_Defined && this.apiToken.Length > 0;
    }

    public override Quizz GetQuizzFromQuizzesList(object quizzId)
    {
        throw new NotImplementedException();
    }

    public override Question GetQuestionFromQuestionsList(object questionId)
    {
        throw new NotImplementedException();
    }

    public override Answer GetAnswerFromAnswersList(object answerId)
    {
        throw new NotImplementedException();
    }

    public void SetChild(ApiManager child)
    {
        this.child = child;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetActualMethodName()
    {
        var st = new System.Diagnostics.StackTrace(new System.Diagnostics.StackFrame(1));
        return st.GetFrame(0).GetMethod().Name;
    }
}
