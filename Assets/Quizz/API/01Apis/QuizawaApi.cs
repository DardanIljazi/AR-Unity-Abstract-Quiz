using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
