using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbstractQuizzStructure;
using static DardiNestedApiModel;

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
        base.SetChild(this); // Important, must be set so that ApiManager will call Serialize(Quizzes/Questions/Answers)..
        // Set the configuration needed to get data for Quizzes/Questions/Ansers. 
        base.rootApiUrl = "https://dardi.ch/nested_api/api";
        base.apiQuizzesUrl = rootApiUrl + "/quizzes";
        base.apiQuestionsUrl = apiQuizzesUrl;
        base.apiAnswersUrl = apiQuizzesUrl;

        base.apiDataModelEndpointType = ApiDataModelEndpointType.FullyNested;

        // Set the configuration needed for the API token.
        base.hasToHaveTokenForApi = false;
        base.hasToLoginToGetToken = false;
    }


    public override Quizzes SerializeQuizzes(string jsonData)
    {
        return (Quizzes)JsonUtility.FromJson<DardiNestedApiModel.QuizzesInAPI>(jsonData);
    }

    public override Questions GetQuestionsForQuizz(object quizzId)
    {
        string json_quizzes_with_questions_with_answers = NetworkRequestManager.HttpGetRequest(apiQuestionsUrl);

        CheckIfNullAndLog(json_quizzes_with_questions_with_answers, $"[WARNING]: Response for {GetActualMethodName()} is null");

        DardiNestedApiModel.QuizzesInAPI quizzesData = JsonUtility.FromJson<DardiNestedApiModel.QuizzesInAPI>(json_quizzes_with_questions_with_answers);

        Questions questions = new Questions();
        foreach (QuizzInAPI quizzData in quizzesData.quizzes)
        {
            if (quizzData.id.ToString() == quizzId.ToString())
            {
                foreach (QuestionInAPI questionData in quizzData.questions)
                {
                    questionData.MapAPIValuesToAbstractClass();
                    questions.AddQuestion(questionData);
                }

                return questions;
            }
        }

        return null;
    }

    public override Answers GetAnswersForQuestion(object quizzId, object questionId)
    {
        string json_quizzes_with_questions_with_answers = NetworkRequestManager.HttpGetRequest(apiAnswersUrl);

        CheckIfNullAndLog(json_quizzes_with_questions_with_answers, $"[WARNING]: Response for {GetActualMethodName()} is null");

        DardiNestedApiModel.QuizzesInAPI quizzesData = JsonUtility.FromJson<DardiNestedApiModel.QuizzesInAPI>(json_quizzes_with_questions_with_answers);

        Answers answers = new Answers();
        foreach (QuizzInAPI quizzData in quizzesData.quizzes)
        {
            if (quizzData.id.ToString() == quizzId.ToString())
            {
                foreach (QuestionInAPI questionData in quizzData.questions)
                {
                    if (questionData.id.ToString() == questionId.ToString())
                    {
                        foreach (DardiNestedApiModel.AnswerInAPI answerData in questionData.answers)
                        {
                            answerData.MapAPIValuesToAbstractClass();
                            answers.AddAnswer(answerData);
                        }
                        
                        return answers;
                    }
                }
            }
        }

        return null;
    }
}