using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using static AbstractQuizzStructure;


static class Constants
{
    public const string Api_Token_Not_Defined = "api_token_not_defined";
    public const string Api_Param_Name_Not_Defined = "api_param_name_not_defined";
}

/**
 * ApiManager class contains all actions/methods that can be done to the api in a "generic" way
 */
public class ApiManager : ApiManagerStructure
{
    [Header("Url to the api")]
    public string apiUrl;

    [Header("Url to the quizzes")]
    public string apiQuizzesUrl;

    [Help("\nUrl to the questions.\nIf any quizzId is in the link, write {quizzId} at that place. Let empty if not used")]
    public string apiQuestionsUrl;
    [NonSerialized]
    public string _originalApiQuestionsUrl; // Copy of apiQuizzesQuestionUrl so that if it contains {quizzId} it will stay

    [Help("\nUrl to answers. \nIf any questionId is in the link, write {questionid} at that place. Let empty if not used")]
    public string apiAnswersUrl;
    [NonSerialized]
    public string _originalApiQuizzesQuestionAnswersList; // Copy of apiQuizzesQuestionUrl so that if it contains {quizzId} it will stay

    public bool hasToHaveTokenForApi;
    public bool hasToLoginToGetToken;

    [Header("Url to the api endpoint for login. Let empty if not used")]
    public string apiLoginUrl;

    public TokenHttpEmplacement tokenHttpEmplacement; // The place where token has to be put in HTTP (url, header, body)

    public string apiKeyParamName; // Define the key to use to assign token value in HTTP (f. ex: not_defined_api_paramname={API_TOKEN} in url/header/body). 
                                   // This should be modified in the class that inherits from ApiManager. 

    [Header("The api token can stay empty/not defined if user has to login to get it")]
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
        _originalApiQuestionsUrl = apiQuestionsUrl;
        _originalApiQuizzesQuestionAnswersList = apiAnswersUrl;

        CheckIfNullAndLog(apiUrl, $"apiUrl is empty or not defined. Did you forget to fill it ?. Value: {apiUrl}", 1);
        CheckIfNullAndLog(apiQuizzesUrl, $"apiQuizzesUrl is empty or not defined. Did you forget to fill it ?. Value: {apiQuizzesUrl}", 1);
    }


    // Get list of quizzes
    public override Quizzes GetQuizzes()
    {
        string json_quizzes = NetworkRequestManager.HttpGetRequest(apiQuizzesUrl);

        CheckIfNullAndLog(json_quizzes, $"[WARNING]: Response for {GetActualMethodName()} is null");


        Quizzes quizzesData = child.SerializeQuizzes(json_quizzes);

        CheckIfNullAndLog(quizzesData, $"[WARNING]: quizzesData is null");


        return quizzesData;
    }
    public virtual Quizzes SerializeQuizzes(string json) // Child has to override this method so that data is serialized from child within GetQuizzesList
    {
        throw new NotImplementedException();
    }


    // Get list of questions for a quizz
    public override Questions GetQuestionsForQuizz(object quizzId)
    {
        // Their could be {quizzId} in the link. In this case replace it with quizzId
        apiQuestionsUrl = _originalApiQuestionsUrl.Replace("{quizzId}", quizzId.ToString());

        string json_questions = NetworkRequestManager.HttpGetRequest(apiQuestionsUrl);

        CheckIfNullAndLog(json_questions, $"[WARNING]: Response for {GetActualMethodName()} is null");


        Questions questionsQuizzData = child.SerializeQuestions(json_questions);

        CheckIfNullAndLog(questionsQuizzData, $"[WARNING]: questionsQuizzData is null");


        return questionsQuizzData;
    }
    public virtual Questions SerializeQuestions(string json) // Child has to override this method so that data is serialized from child within GetQuestionsListForQuizz
    {
        throw new NotImplementedException();
    }

    // Get list of answers for a question
    public override Answers GetAnswersForQuestion(object quizzId, object questionId)
    {
        string json_answers = NetworkRequestManager.HttpGetRequest(apiQuestionsUrl);

        CheckIfNullAndLog(json_answers, $"[WARNING]: Response for {GetActualMethodName()} is null");

        Answers answersData = child.SerializeAnswers(json_answers);

        CheckIfNullAndLog(answersData, $"[WARNING]: questionsQuizzData is null");


        return answersData;
    }
    public virtual Answers SerializeAnswers(string json) // Child has to override this method so that data is serialized from child within GetAnswersForQuestion
    {
        throw new NotImplementedException();
    }


    // Register to the api
    // TODO: This is a "generic" way to generate. Should be modified or put into QuizawaApi class
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

        CheckIfNullAndLog(JSON_register, $"JSON_register is null");


        ApiToken registrationData = JsonUtility.FromJson<ApiToken>(JSON_register);

        CheckIfNullAndLog(registrationData, $"registrationData is null");


        return registrationData;
    }

    // Connect to the api
    public ApiToken ConnectToApi(string pseudo, string password)
    {
        var post_key_values = new Dictionary<string, string>
        {
            { "pseudo", pseudo },
            { "password", password }
        };

        string JSON_connection = NetworkRequestManager.HttpPostRequest(apiLoginUrl, post_key_values);

        CheckIfNullAndLog(JSON_connection, $"JSON_connection is null");


        ApiToken connectionData = child.SerializeApiToken(JSON_connection);

        CheckIfNullAndLog(JSON_connection, $"connectionQuizzData is null");


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

    public override Quizz GetQuizz(object quizzId)
    {
        throw new NotImplementedException();
    }

    public override Question GetQuestion(object quizzId, object questionId)
    {
        throw new NotImplementedException();
    }

    public override Answer GetAnswer(object quizzId, object questionId, object answerId)
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

    public void CheckIfNullAndLog(object what, string log, int type=0)
    {
        if (what == null)
        {
            switch (type)
            {
                case 0:
                    Debug.LogError(log);
                    break;
                case 1:
                    Debug.Log(log);
                    break;
            }
        }
    }

    
}
