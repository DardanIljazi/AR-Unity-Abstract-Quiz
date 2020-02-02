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
 * INFO:    The quizz has been thought to work with "lists". For example you will see a list of Quizz (Quizzes) and have to select one, 
 *          you will have a list of Question (Questions) that will appear one after an other (after responding to an Answer, itself contained in a list of Answer = Answers)
 *          
 *          For this reason, you only have to set the link containing those elements (apiQuizzesUrl, apiQuestionsUrl, apiAnswersUrl)
 */
public class ApiManager : ApiManagerStructure
{
    [Header("The api name. Can be used in view/pages to show the application/api/software name")]
    public string apiName;

    /**
     * Variables for endpoints (url) to get data
     */
    [Header("Url to the api")]
    public string apiUrl;

    [Header("Url to the quizzes")]
    public string apiQuizzesUrl;

    [Help("\nUrl to the questions.\nIf any quizzId is in the link, write {quizzId} at that place. Let empty if not used")]
    public string apiQuestionsUrl;
    private string _originalApiQuestionsUrl; // Copy of apiQuizzesQuestionUrl so that if it contains {quizzId} it will stay

    [Help("\nUrl to answers. \nIf any questionId is in the link, write {questionid} at that place. Let empty if not used")]
    public string apiAnswersUrl;
    private string _originalApiQuizzesQuestionAnswersList; // Copy of apiQuizzesQuestionUrl so that if it contains {quizzId} it will stay

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

    /**
     * Variables used to store the class that serializes and maps data to classes (Quizzes/Quizz/Questions/Question/Answers/Answer)
     * These are defined in the class inherited from ApiManager as this: base.apiModelClassForQuizzesSerialization = new ... (where ... is the api model to use to serialize data)
     */
    private object apiModelClassForQuizzesSerialization;
    private object apiModelClassForQuestionsSerialization;
    private object apiModelClassForAnswersSerialization;
    private object apiModelClassForApiTokeAfterLoginSerialization; // Will contain api model class that serializes the data returned to get an api token. This can be null as the api could not use any token


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
        // Default values for Login/Register page parameters name
        this.pseudoParamNameForApiLoginOrRegistering = "pseudo";
        this.passwordParamNameForApiLoginOrRegistering = "password";
        this.firstnameParamNameForApiRegistering = "firstname";
        this.lastNameParamNameForApiRegistering = "lastname";
        this.emailParamNameForApiRegistering = "email";
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
        CheckIfNullAndLog(apiModelClassForQuizzesSerialization, $"[CRITICAL]: apiModelClassForQuizzesSerialization is not set. Set it from the class that inherits ApiManager with SetClassToUseForQuizzesSerialization() like in example codes.");

        string json_quizzes = NetworkRequestManager.HttpGetRequest(apiQuizzesUrl);

        CheckIfNullAndLog(json_quizzes, $"[WARNING]: Response for {GetActualMethodName()} is null");

        // Serialize the json data to the class used in api model for Quizzes (because only api model knows how json data is structured and how to map these values)
        // apiModelClassForQuizzesSerialization is defined in the class inherited from ApiManager as base.apiModelClassForQuizzesSerialization = new ...)
        JsonUtility.FromJsonOverwrite(json_quizzes, apiModelClassForQuizzesSerialization);

        Quizzes quizzesData = apiModelClassForQuizzesSerialization as Quizzes;
        quizzesData.MapAPIValuesToAbstractClass(); // Maps the values from ApiModel to Quizzes. The mapping is defined in api model class that inherits from Quizzes


        CheckIfNullAndLog(quizzesData, $"[WARNING]: quizzesData is null");


        return quizzesData;
    }


    // Get list of questions for a quizz
    public override Questions GetQuestionsForQuizz(object quizzId)
    {
        CheckIfNullAndLog(apiModelClassForQuestionsSerialization, $"[CRITICAL]: apiModelClassForQuestionsSerialization is not set. Set it from the class that inherits ApiManager with SetClassToUseForQuestionsSerialization() like in example codes.");

        // Their could be {quizzId} in the link. In this case replace it with quizzId
        apiQuestionsUrl = _originalApiQuestionsUrl.Replace("{quizzId}", quizzId.ToString());

        string json_questions = NetworkRequestManager.HttpGetRequest(apiQuestionsUrl);

        CheckIfNullAndLog(json_questions, $"[WARNING]: Response for {GetActualMethodName()} is null");

        // Serialize the json data to the class used in api model for Questions (because only api model knows how json data is structured and how to map these values)
        // apiModelClassForQuestionsSerialization is defined in the class inherited from ApiManager as base.apiModelClassForQuestionsSerialization = new ...)
        JsonUtility.FromJsonOverwrite(json_questions, apiModelClassForQuestionsSerialization);

        Questions questionsData = apiModelClassForQuestionsSerialization as Questions;
        questionsData.MapAPIValuesToAbstractClass(); // Maps the values from ApiModel to Questions. The mapping is defined in api model class that inherits from Questions


        CheckIfNullAndLog(questionsData, $"[WARNING]: questionsData is null");


        return questionsData;
    }


    // Get list of answers for a question
    public override Answers GetAnswersForQuestion(object quizzId, object questionId)
    {
        CheckIfNullAndLog(apiModelClassForAnswersSerialization, $"[CRITICAL]: apiModelClassForAnswersSerialization is not set. Set it from the class that inherits ApiManager with SetClassToUseForAnswersSerialization() like in example codes.");

        string json_answers = NetworkRequestManager.HttpGetRequest(apiQuestionsUrl);

        CheckIfNullAndLog(json_answers, $"[WARNING]: Response for {GetActualMethodName()} is null");


        // Serialize the json data to the class used in api model for Answers (because only api model knows how json data is structured and how to map these values)
        // apiModelClassForAnswersSerialization is defined in the class inherited from ApiManager as base.apiModelClassForAnswersSerialization = new ...)
        JsonUtility.FromJsonOverwrite(json_answers, apiModelClassForAnswersSerialization);

        Answers answersData = apiModelClassForAnswersSerialization as Answers;
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

    public void SetClassToUseForQuizzesSerialization(object classToUseForSerialization)
    {
        this.apiModelClassForQuizzesSerialization = classToUseForSerialization;
    }

    public void SetClassToUseForQuestionsSerialization(object classToUseForSerialization)
    {
        this.apiModelClassForQuestionsSerialization = classToUseForSerialization;
    }

    public void SetClassToUseForAnswersSerialization(object classToUseForSerialization)
    {
        this.apiModelClassForAnswersSerialization = classToUseForSerialization;
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