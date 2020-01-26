using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbstractQuizzStructure;

public class QuizawaApi : ApiManager
{
    public QuizawaApi()
    {
        base.apiUrl = "http://192.168.1.111:8000/api";
        base.apiQuizzesUrl = apiUrl+ "/quizzes";
        base.apiQuizzesQuestionUrl = apiUrl+ "/quizzes/questions";

        base.hasToHaveTokenForApi = true;
        base.hasToLoginToGetToken = true;
        base.tokenHttpEmplacement = TokenHttpEmplacement.Everywhere; // Put the token everywhere where we can (url/header/body)
        base.apiKeyParamName = "api_token"; // Define the name of the parameter used when token has to be sent over http (api_token={TOKEN} be it in url/header/body)

    }

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


}
