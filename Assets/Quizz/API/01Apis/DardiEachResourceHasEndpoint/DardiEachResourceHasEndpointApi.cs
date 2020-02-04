using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbstractQuizzStructure;

/**
 * DardiNestedApi does: 
 *  - Define which model it is attached to (here it is DardiEachResourceHasEndpointApiModel)
 *  - Configure/Set how the code should get data
 * 
 * The apis like this one that have one endpoint (url) per model are the easiest to implement. Others: partially or fully nested api have to have more code written for the same result
 */
public class DardiEachResourceHasEndpointApi : ApiManager
{
    public DardiEachResourceHasEndpointApiModel dardiEachResourceHasEndpointApiModel;

    public DardiEachResourceHasEndpointApi()
    {
        base.SetChild(this); // Important, must be set so that ApiManager will call Serialize(Quizzes/Questions/Answers)..

        // Set the configuration needed to get data for Quizzes/Questions/Ansers. 
        base.rootApiUrl = "http://dardi.ch/each_resource_has_endpoint/api";
        base.apiQuizzesUrl = rootApiUrl + "/quizzes";
        base.apiQuestionsUrl = rootApiUrl + "/quizzes/{quizzId}/questions";
        base.apiAnswersUrl = rootApiUrl + "/quizzes/{quizzId}/questions/{questionId}/answers";

        base.apiDataModelEndpointType = ApiDataModelEndpointType.EachModelHasAnEndpoint;

        // Set the configuration needed for the API token.
        base.hasToHaveTokenForApi = false;
        base.hasToLoginToGetToken = false;
    }

    public override Quizzes SerializeQuizzes(string jsonData)
    {
        return (Quizzes)JsonUtility.FromJson<DardiEachResourceHasEndpointApiModel.QuizzesInAPI>(jsonData) as Quizzes;
    }

    public override Questions SerializeQuestions(string jsonData)
    {
        return (Questions)JsonUtility.FromJson<DardiEachResourceHasEndpointApiModel.QuestionsInAPI>(jsonData);
    }

    public override Answers SerializeAnswers(string jsonData)
    {
        return (Answers)JsonUtility.FromJson<DardiEachResourceHasEndpointApiModel.AnswersInAPI>(jsonData);
    }
}
