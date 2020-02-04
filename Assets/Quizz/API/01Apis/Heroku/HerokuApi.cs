using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbstractQuizzStructure;
using static HerokuApiModel;

/**
 * HerokuApi does: 
 *  - Define which model it is attached to (here it is herokuApiModel)
 *  - Configure/Set how the code should get data
 * 
 * The apis like this one that are "partially nested" are not the easiest to implement (Look for DardiEachResourceHasEndpoint to have the easiest example). 
 * Because ApiManager works with fully restful api (where each resourece has an endpoint) and that this is not the case with HerokuApi, we have to override methods to define how we get and return data.
 * Have a look at GetQuestions() and GetAnswers() that redefine how to return Questions and Answers --> DardiEachResourceHasEndpoint class doesn't have to do this because for example
 */
public class HerokuApi : ApiManager
{
    public HerokuApiModel herokuApiModel;

    public HerokuApi()
    {
        base.SetChild(this); // Important, must be set so that ApiManager will call Serialize(Quizzes/Questions/Answers)..

        // Set the configuration needed to get data for Quizzes/Questions/Ansers. 
        base.rootApiUrl = "http://awa-quizz.herokuapp.com/api";
        base.apiQuizzesUrl = rootApiUrl+ "/quizzes";
        base.apiQuestionsUrl = rootApiUrl+ "/quizzes/{quizzId}";
        base.apiAnswersUrl = base.apiQuestionsUrl; // Answers are contained into questions (nested) = same link to get those datas

        base.apiDataModelEndpointType = ApiDataModelEndpointType.PartiallyNested;


        // Set the configuration needed for the API token.
        base.hasToHaveTokenForApi = true;
        base.hasToLoginToGetToken = false;
        base.tokenHttpEmplacement = TokenHttpEmplacement.Everywhere; // Put the token everywhere where we can (url/header/body)
        base.apiKeyParamName = "quizz-token"; // Define the name of the parameter used when token has to be sent over http (api_token={TOKEN} be it in url/header/body)
        base.apiToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VybmFtZSI6Imd1ZXN0IiwicGFzc3dvcmQiOiIkcGJrZGYyLXNoYTI1NiQyMDAwMCRjNjRWd3RnN0IuQThKeVJrN1AzL1h3JG9BRDloUnVEQTVkWVpKR1Y2cDNpdDBzYVFqdlFBemFZbi9wNW1kSGRDbDQifQ.P-KfTO8nq5oQNC_bIAY5VKOeNLyNbGE-gGrf0oIKQjc";
    }

    public override Quizzes SerializeQuizzes(string jsonData)
    {
        return (Quizzes)JsonUtility.FromJson<HerokuApiModel.QuizzesInAPI>(jsonData) as Quizzes;
    }

    public override Questions SerializeQuestions(string jsonData)
    {
        return (Questions)JsonUtility.FromJson<HerokuApiModel.QuestionsInAPI>(jsonData);
    }


    public override Answers GetAnswersForQuestion(object quizzId, object questionId)
    {
        // Replace {quizzId} and {questionid} in the link
        base.apiAnswersUrl = base._originalApiAnswersUrl.Replace("{quizzId}", quizzId.ToString()).Replace("{questionId}", questionId.ToString());

        string json_questions_with_answers = NetworkRequestManager.HttpGetRequest(apiAnswersUrl);

        CheckIfNullAndLog(json_questions_with_answers, $"[WARNING]: Response for {GetActualMethodName()} is null");


        HerokuApiModel.QuestionsInAPI questionsData = JsonUtility.FromJson<HerokuApiModel.QuestionsInAPI>(json_questions_with_answers);

        Answers answers = new Answers();
        // Let's begin to search the questionId we need in this case
        foreach (QuestionInAPI questionData in questionsData.questions)
        {
            if (questionId.ToString() == questionData.id)
            {
                // Now that we found the question with the questionId we need, let's get all answers 
                foreach (AnswerInAPI answerData in questionData.answers)
                {
                    answerData.MapAPIValuesToAbstractClass();
                    answers.AddAnswer(answerData);
                }

                return answers;
            }
        }

        return null;
    }

}
