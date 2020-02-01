using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbstractQuizzStructure;


public class HerokuApi : ApiManagerFor<HerokuApiModel>
{
    public HerokuApi()
    {
        base.SetChild(this);
        base.apiUrl = "https://awa-quizz.herokuapp.com/api";
        base.apiQuizzesUrl = apiUrl+ "/quizzes";
        base.apiQuestionsUrl = apiUrl+ "/quizzes/{quizzId}";
        base.apiAnswersUrl = apiUrl+ "/quizzes/{quizzId}";


        base.hasToHaveTokenForApi = true;
        base.hasToLoginToGetToken = false;
        base.tokenHttpEmplacement = TokenHttpEmplacement.Everywhere; // Put the token everywhere where we can (url/header/body)
        base.apiKeyParamName = "quizz-token"; // Define the name of the parameter used when token has to be sent over http (api_token={TOKEN} be it in url/header/body)
        base.apiToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VybmFtZSI6Imd1ZXN0IiwicGFzc3dvcmQiOiIkcGJrZGYyLXNoYTI1NiQyMDAwMCRjNjRWd3RnN0IuQThKeVJrN1AzL1h3JG9BRDloUnVEQTVkWVpKR1Y2cDNpdDBzYVFqdlFBemFZbi9wNW1kSGRDbDQifQ.P-KfTO8nq5oQNC_bIAY5VKOeNLyNbGE-gGrf0oIKQjc";
    }


    #region Quizzes

    // Serializes data received from API (json), maps it to Quizzes class and returns it
    public override Quizzes SerializeQuizzes(string json)
    {
        if (json == null || json.Length == 0)
            return null;

        // We serialize data received from api to QuizzesData
        HerokuApiModel.GameQuizzes data = JsonUtility.FromJson<HerokuApiModel.GameQuizzes>(json);

        if (data == null)
            return null;

        // We map quizzesData values to Quizzes class (QuizzesData inherits from Quizzes).
        data.MapValuesFromAPIToApplicationLogicClass();


        return data;
    }

    #endregion

    #region Questions

    // Serializes data received from API (json), maps it to Questions class and returns it
    public override Questions SerializeQuestions(string json)
    {
        if (json == null || json.Length == 0)
            return null;

        // We serialize data received from api to QuizzesData
        HerokuApiModel.GameQuizzeForQuestions data = JsonUtility.FromJson<HerokuApiModel.GameQuizzeForQuestions>(json);

        if (data == null)
            return null;

        // We map quizzesData values to Quizzes class (QuizzesData inherits from Quizzes).
        data.MapValuesFromAPIToApplicationLogicClass();


        return data;
    }

    #endregion

    #region Answers
    public override Answers SerializeAnswers(string json) // Child has to override this method so that data is serialized from child within GetAnswersForQuestion
    {
        if (json == null || json.Length == 0)
            return null;


        // We serialize data received from api to AnswerData
        HerokuApiModel.GameQuizzForAnswers data = JsonUtility.FromJson<HerokuApiModel.GameQuizzForAnswers>(json);

        if (data == null)
            return null;

        // We map answersData values to Answers class (AnswerData inherits from Answers).
        data.MapValuesFromAPIToApplicationLogicClass();


        return data;
    }
    #endregion
}
