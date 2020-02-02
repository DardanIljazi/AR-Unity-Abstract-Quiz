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
        base.apiUrl = "https://dardi.ch/nested_api/api/";
        base.apiQuizzesUrl = apiUrl + "/quizzes";
        base.apiQuestionsUrl = apiUrl + "/quizzes";
        base.apiAnswersUrl = apiUrl + "/quizzes";

        // Set the configuration needed for the API token.
        base.hasToHaveTokenForApi = false;
        base.hasToLoginToGetToken = false;

        // Define which class the ApiManager will use to serialize json data when code will call GetQuizzes() / GetQuestions() / GetAnswers()
        /*base.SetClassToUseForQuizzesSerialization(new DardiNestedApiModel.QuizzesInAPI());
        base.SetClassToUseForQuestionsSerialization(new DardiNestedApiModel.QuestionsInAPI());
        base.SetClassToUseForAnswersSerialization(new DardiNestedApiModel.AnswersInAPI());*/
    }
}
