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
 * ApiManager class contains all actions/methods that can be done to the api in a "generalized/generic" way (scenario cases have been thought and generalized here)
 * For example, some apis need a token, some not, some even need login to get an api token. All these cases have been implemented here.
 * 
 * 
 * INFO:    The quizz has been thought to work with restful api where each resource has an endpoint
 *          For example 
 *              - Resource (Quizzes)    :> Endpoint (www.example.com/api/quizzes)
 *              - Resource (Questions)  :> Endpoint (www.example.com/api/{quizzId}/questions)
 *              - Resource (Answers)    :> Endpoint (www.example.com/api/{quizzId/questions/{questionId}/answers)
 *          
 *          The code works too for 
 *              - Partially nested resources, for example:
 *                  - Resource (Quizzes)                :> Endpoint (www.example.com/api/quizzes)
 *                  - Resources (Questions + Answers)   :> Endpoint (www.example.com/api/quizzes/{quizzId})
 *                  
 *              - Fully nested resources, for example:
 *                  - Resources (Quizzes + Questions + Answers) :> Endpoint (www.example.com/api/quizzes)
 *             
 *              --> Those 2 last cases work but the code has to do more operations and rely on cache to get data, so it could be slower.
 *          
 */
public abstract class ApiManager: ApiManagerStructure
{
    [Header("The api name. Can be used in view/pages to show the application/api/software name")]
    public string apiName;

    /**
     * Variables for endpoints (url) to get data
     */
    [Header("Root url to the api")]
    public string rootApiUrl;

    [Header("Url to the quizzes")]
    public string apiQuizzesUrl;

    [Help("\nUrl to the questions.\nIf any quizzId is in the link, write {quizzId} at that place.\nLet empty if not used")]
    public string apiQuestionsUrl;
    private string _originalApiQuestionsUrl; // Copy of apiQuizzesQuestionUrl so that if it contains {quizzId} it will stay

    [Help("\nUrl to answers. \nIf any quizz id is in the link, write {quizzId} in that place.\nIf any questionId is in the link, write {questionid} at that place.\nLet empty if not used")]
    public string apiAnswersUrl;
    private string _originalApiAnswersUrl; // Copy of apiQuizzesQuestionUrl so that if it contains {quizzId} it will stay

    // The type of 
    public ApiDataModelEndpointType apiDataModelEndpointType;


    /**
     * Variables for token management
     */
    public bool hasToHaveTokenForApi;
    public bool hasToLoginToGetToken;
    public bool canRegisterAsUserToApi; // In order to get an api token

    [Header("Url to the api endpoint for login. Let empty if not used")]
    public string apiLoginUrl;

    [Header("Url to the registering endpoint (new user for API). Let empty if not used")]
    public string apiRegisteringUrl;

    [Header("Url to the api endpoint for registering user. Let empty if not used")]
    public string apiRegisterUrl;

    public TokenHttpEmplacement tokenHttpEmplacement; // The place where token has to be put in HTTP (url, header, body)

    public string apiKeyParamName; // Define the key to use to assign token value in HTTP (f. ex: not_defined_api_paramname={API_TOKEN} in url/header/body). 
                                   // This should be modified in the class that inherits from ApiManager. 

    [Header("The api token can stay empty/not defined if user has to login to get it")]
    public string apiToken; // Should be defined into the class that inherits from APiManager (or later in the runtime) if token is used 

    /**
     * Variables that are used for login/registering (to get token)
     * These are related to the Login page (LoginManager) and Register page (RegisterManager).
     */
    private string pseudoParamNameForApiLoginOrRegistering;
    private string passwordParamNameForApiLoginOrRegistering;
    private string firstnameParamNameForApiRegistering;
    private string lastNameParamNameForApiRegistering;
    private string emailParamNameForApiRegistering;

    private object apiModelClassForApiTokeAfterLoginSerialization; // Will contain api model class that serializes the data returned to get an api token. This can be null as the api could not use any token

    /**
     * Cached variables used when api may not be restful (the code is intended to work with resources that have all a endpoint
     * for example: 
     * - Resource (Quizzes) : Endpoint (www.example.com/api/quizzes)
     * - Resource (Questions) : Endpoint (www.example.com/api/{quizzId}/questions)
     * - Resource (Answers) : Endpoint (www.example.com/api/{quizzId/questions/{questionId}/answers)
     * 
     * Because api could not be structured like this, but for example partially or fully nested data, we keep cached elements so we can find Questions or Answers based on {quizzId} or {questionId}.
     */
    private Quizzes cachedQuizzes;
    private Questions cachedQuestions;
    private Answers cachedAnswers;


    /**
     * Reference to the child that inherits from this class
     */
    ApiManager child;

    public ApiManager()
    {
        // Set default values
        this.apiName = "Quiz";
        // Default values for variables about token
        this.apiToken = Constants.Api_Token_Not_Defined;
        this.apiKeyParamName = Constants.Api_Param_Name_Not_Defined;
        this.tokenHttpEmplacement = TokenHttpEmplacement.Everywhere;
        this.hasToHaveTokenForApi = false;
        this.hasToLoginToGetToken = false;
        this.canRegisterAsUserToApi = false;
        this.apiDataModelEndpointType = ApiDataModelEndpointType.FullyNested; // Use "worst" case scenario by default
        // Default values for Login/Register page parameters name
        this.pseudoParamNameForApiLoginOrRegistering = "pseudo";
        this.passwordParamNameForApiLoginOrRegistering = "password";
        this.firstnameParamNameForApiRegistering = "firstname";
        this.lastNameParamNameForApiRegistering = "lastname";
        this.emailParamNameForApiRegistering = "email";
    }

    public void SetChild(ApiManager actualApiManagerChild)
    {
        this.child = actualApiManagerChild;
    }


    void Start()
    {
        _originalApiQuestionsUrl = apiQuestionsUrl;
        _originalApiAnswersUrl = apiAnswersUrl;

        CheckIfNullAndLog(rootApiUrl, $"apiUrl is empty or not defined. Did you forget to fill it ?. Value: {rootApiUrl}", 1);
        CheckIfNullAndLog(apiQuizzesUrl, $"apiQuizzesUrl is empty or not defined. Did you forget to fill it ?. Value: {apiQuizzesUrl}", 1);
    }


    /**
     * Get quizzes
     */
    public override Quizzes GetQuizzes()
    {
        string json_quizzes = NetworkRequestManager.HttpGetRequest(apiQuizzesUrl);

        CheckIfNullAndLog(json_quizzes, $"[WARNING]: Response for {GetActualMethodName()} is null");


        Quizzes quizzesData = child.SerializeQuizzes(json_quizzes);
        quizzesData.MapAPIValuesToAbstractClass(); // Maps the values from ApiModel to Quizzes. The mapping is defined in api model class that inherits from Quizzes

        cachedQuizzes = quizzesData;

        CheckIfNullAndLog(quizzesData, $"[WARNING]: quizzesData is null");


        return quizzesData;
    }


    /** 
     * Get questions for a quizz.
     */
    public override Questions GetQuestionsForQuizz(object quizzId)
    {
        // If Api was set as FullyNested or PartiallyNested or that link don't contain {quizzId} and {questionId}, this method should not be called at all but should be overriden in the class that inherits from Api Manager
        if (apiDataModelEndpointType == ApiDataModelEndpointType.FullyNested ||
            apiDataModelEndpointType == ApiDataModelEndpointType.PartiallyNested ||
            !DoUrlsHaveQuizzIdAndQuestionIdInThem())
        {
            Debug.LogError(
                $"[WARNING]: Your api seems to be Fully or Partially nested because you set it like this or you didn't define {{quizzId}} and {{questionId}} in api urls\n" +
                $"In this case GetQuestionsForQuizz method should be overriden in the class that inherits from ApiManager. There, you should implement how questions are retrieved and return them. Look at documentation for examples"
                );
            return null;
        }


        // Replace {quizzId} in the link
        apiQuestionsUrl = _originalApiQuestionsUrl.Replace("{quizzId}", quizzId.ToString());

        string json_questions = NetworkRequestManager.HttpGetRequest(apiQuestionsUrl);

        CheckIfNullAndLog(json_questions, $"[WARNING]: Response for {GetActualMethodName()} is null");


        Questions questionsData = child.SerializeQuestions(json_questions);
        questionsData.MapAPIValuesToAbstractClass(); // Maps the values from ApiModel to Questions. The mapping is defined in api model class that inherits from Questions

        cachedQuestions = questionsData;

        CheckIfNullAndLog(questionsData, $"[WARNING]: questionsData is null");


        return questionsData;
    }


    /** 
     * Get answers for a quizz and a question.
     */
    public override Answers GetAnswersForQuestion(object quizzId, object questionId)
    {
        // If Api was set as FullyNested or PartiallyNested or that link don't contain {quizzId} and {questionId}, this method should not be called at all but should be overriden in the class that inherits from Api Manager
        if (apiDataModelEndpointType == ApiDataModelEndpointType.FullyNested ||
            apiDataModelEndpointType == ApiDataModelEndpointType.PartiallyNested ||
            !DoUrlsHaveQuizzIdAndQuestionIdInThem())
        {
            Debug.LogError(
                $"[WARNING]: Your api seems to be Fully or Partially nested because you set it like this or you didn't define {{quizzId}} and {{questionId}} in api urls\n" +
                $"In this case GetAnswersForQuestion method should be overriden in the class that inherits from ApiManager. There, you should implement how questions are retrieved and return them. Look at documentation for examples"
                );
            return null;
        }

        // Replace {quizzId} and {questionid} in the link
        apiAnswersUrl = _originalApiAnswersUrl.Replace("{quizzId}", quizzId.ToString());
        apiAnswersUrl = _originalApiAnswersUrl.Replace("{questionId}", questionId.ToString());

        string json_answers = NetworkRequestManager.HttpGetRequest(apiAnswersUrl);

        CheckIfNullAndLog(json_answers, $"[WARNING]: Response for {GetActualMethodName()} is null");


        Answers answersData = child.SerializeAnswers(json_answers);
        answersData.MapAPIValuesToAbstractClass(); // Maps the values from ApiModel to Answers. The mapping is defined in api model class that inherits from Answers


        CheckIfNullAndLog(answersData, $"[WARNING]: answersData is null");


        return answersData;
    }


    /**
     * Method that connects/logins to the api using POST request with what is sent in keyValuePairs.
     * If modification or some other special things/logic has to be implemented, override this method in the class that inherits from ApiManager
     * 
     * Works with what LoginManager send as parameters. It returns an api token that must be defined in api model
     */
    public virtual ApiToken LoginToGetApiToken(Dictionary<string, string> keyValuePairsToSend)
    {
        CheckIfNullAndLog(apiModelClassForApiTokeAfterLoginSerialization, $"[CRITICAL]: apiModelClassForApiTokenSerialization is not set. Set it from the class that inherits ApiManager with SetClassToUseForApiTokenSerialization() like in example codes.");

        string json_login = NetworkRequestManager.HttpPostRequest(apiLoginUrl, keyValuePairsToSend);

        CheckIfNullAndLog(json_login, $"[WARNING]: Response for {GetActualMethodName()} is null");


        // Serialize the json data to the class used in api model for ApiToken (because only api model knows how json data is structured and how to map these values)
        // apiModelClassForApiTokenSerialization is defined in the class inherited from ApiManager as base.apiModelClassForApiTokenSerialization = new ...)
        JsonUtility.FromJsonOverwrite(json_login, apiModelClassForApiTokeAfterLoginSerialization);

        ApiToken tokenData = apiModelClassForApiTokeAfterLoginSerialization as ApiToken;
        tokenData.MapAPIValuesToAbstractClass(); // Maps the values from ApiModel to Answers. The mapping is defined in api model class that inherits from Answers


        CheckIfNullAndLog(tokenData, $"tokenData is null");


        return tokenData;
    }


    /**
     * Method that register a user to the api.
     * Some api needs a user to be logged to be able to make requests in the api (have a look on LoginToGetApiToken for login)
     * Registration is sometimes needed because user has no login yet.
     * Even if some apis may return the api token after registration request, the common idea of "getting token" is not in registration but in login.
     * So here the app only returns true or false depending on the http request response (error or request not in 2xx response code).
     * The user should then be redirected to login where there should be a token response.
     */
    public virtual bool RegisterUserToGetApiToken(Dictionary<string, string> keyValuePairs)
    {
        string json_register = NetworkRequestManager.HttpPostRequest(apiRegisteringUrl, keyValuePairs);

        CheckIfNullAndLog(json_register, $"json_register is null");

        return json_register != null && NetworkRequestManager.lastHttpWebRequestErrorMessage == null;
    }

    
    public bool DoUrlsHaveQuizzIdAndQuestionIdInThem()
    {
        if (apiQuestionsUrl.Contains("{quizzId}") && 
            apiAnswersUrl.Contains("{quizzId}") && apiAnswersUrl.Contains("{questionId}")) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool HasToHaveToken()
    {
        return hasToHaveTokenForApi;
    }

    public override bool HasToLoginToGetToken()
    {
        return hasToLoginToGetToken;
    }

    public override bool CanRegisterAsUserToApi()
    {
        return canRegisterAsUserToApi;
    }

    public override TokenHttpEmplacement GetTokenttpEmplacement()
    {
        return tokenHttpEmplacement;
    }

    public string GetApiKeyParamName()
    {
        return apiKeyParamName;
    }

    public string GetPseudoParamName()
    {
        return pseudoParamNameForApiLoginOrRegistering;
    }

    public void SetPseudoOrEmailParamName(string paramName)
    {
        this.pseudoParamNameForApiLoginOrRegistering = paramName;
    }

    public string GetFirstNameParamName()
    {
        return firstnameParamNameForApiRegistering;
    }

    public void SetFirstNameParamName(string paramName)
    {
        this.firstnameParamNameForApiRegistering = paramName;
    }

    public string GetLastNameParamName()
    {
        return lastNameParamNameForApiRegistering;
    }

    public void SetLastNameParamName(string paramName)
    {
        this.lastNameParamNameForApiRegistering = paramName;
    }

    public string GetEmailParamName()
    {
        return emailParamNameForApiRegistering;
    }

    public void SetEmailParamName(string paramName)
    {
        this.emailParamNameForApiRegistering = paramName;
    }

    public string GetPasswordParamName()
    {
        return passwordParamNameForApiLoginOrRegistering;
    }

    public void SetPasswordParamName(string paramName)
    {
        this.passwordParamNameForApiLoginOrRegistering = paramName;
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

    public void SetClassToUseForApiTokenSerialization(object classToUseForSerialization)
    {
        this.apiModelClassForApiTokeAfterLoginSerialization = classToUseForSerialization;
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

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetActualMethodName()
    {
        var st = new System.Diagnostics.StackTrace(new System.Diagnostics.StackFrame(1));
        return st.GetFrame(0).GetMethod().Name;
    }

    public abstract Quizzes SerializeQuizzes(string jsonData);
    public abstract Questions SerializeQuestions(string jsonData);
    public abstract Answers SerializeAnswers(string jsonData);

    public void CheckIfNullAndLog(object whatToCheckForNull, string log, int type = 0)
    {
        if (whatToCheckForNull == null)
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