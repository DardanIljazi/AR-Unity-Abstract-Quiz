using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * DardiNestedApi does: 
 *  - Define which model it is attached to (here it is DardiNestedApiModel)
 *  - Configure/Set how the code should get data
 *  - Because this is nested resources, all the url will have the same link. Only how we retrieve data changes (this is defined in the DardiNestedApiModel)
 */
public class DardiNestedApi : ApiManager
{
    public DardiNestedApiModel dardiNestedApiModel;

    public DardiNestedApi()
    {
        // Set the configuration needed to get data for Quizzes/Questions/Ansers. 
        base.rootApiUrl = "https://dardi.ch/nested_api/api/";
        base.apiQuizzesUrl = rootApiUrl + "/quizzes";
        base.apiQuestionsUrl = rootApiUrl + "/quizzes";
        base.apiAnswersUrl = rootApiUrl + "/quizzes";

        // Set the configuration needed for the API token.
        base.hasToHaveTokenForApi = false;
        base.hasToLoginToGetToken = false;
    }

    public override AbstractQuizzStructure.Answers SerializeAnswers(string jsonData)
    {
        throw new System.NotImplementedException();
    }

    public override AbstractQuizzStructure.Questions SerializeQuestions(string jsonData)
    {
        throw new System.NotImplementedException();
    }

    public override AbstractQuizzStructure.Quizzes SerializeQuizzes(string jsonData)
    {
        throw new System.NotImplementedException();
    }
}
