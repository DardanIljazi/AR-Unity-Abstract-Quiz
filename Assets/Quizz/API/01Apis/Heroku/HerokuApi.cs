using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * HerokuApi does: 
 *  - Define which model it is attached to (here it is HerokuApiModel)
 *  - Configure/Set how the code should get data
 *      --> Defines the link, if it should have a token to get data
 */
public class HerokuApi : ApiManagerFor<HerokuApiModel>
{

    public HerokuApi()
    {
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


}
