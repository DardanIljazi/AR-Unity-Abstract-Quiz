using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbstractQuizzStructure;


public class QuizawaApi : ApiManager
{
    public QuizawaApi()
    {
        base.SetChild(this);
        base.apiUrl = "http://192.168.1.111:8000/api";
        base.apiQuizzesUrl = apiUrl+ "/quizzes";
        base.apiQuizzesQuestionUrl = apiUrl+ "/quizzes/questions";
        base.apiLoginUrl = apiUrl + "/users/login";

        base.hasToHaveTokenForApi = true;
        base.hasToLoginToGetToken = true;
        base.tokenHttpEmplacement = TokenHttpEmplacement.Everywhere; // Put the token everywhere where we can (url/header/body)
        base.apiKeyParamName = "api_token"; // Define the name of the parameter used when token has to be sent over http (api_token={TOKEN} be it in url/header/body)
    }

    #region Quizzes

    // Serializes data received from API (json), maps it to Quizzes class and returns it
    public override Quizzes SerializeQuizzes(string json)
    {
        // We serialize data received from api to QuizzesData
        QuizawaApiModel.QuizzesData quizzesData = JsonUtility.FromJson<QuizawaApiModel.QuizzesData>(json);

        // We map quizzesData values to Quizzes class (QuizzesData inherits from Quizzes).
        quizzesData.MapValuesFromAPIToApplicationLogicClass();


        return quizzesData;
    }

    #endregion

    #region Questions

    #endregion

    #region Answers

    #endregion

    #region ApiToken

    // Serializes data received from API (json), maps it to ApiToken and returns it
    public override ApiToken SerializeApiToken(string json)
    {
        // We serialize data received from api to UserData (this class contains the token for QuizawaApi)
        QuizawaApiModel.UserData userData = JsonUtility.FromJson<QuizawaApiModel.UserData>(json); 

        // We map userData values to ApiToken class (UserData inherits from ApiToken).
        userData.MapValuesFromAPIToApplicationLogicClass();


        return userData;
    }

    #endregion
}
