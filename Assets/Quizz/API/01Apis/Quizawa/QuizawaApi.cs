using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbstractQuizzStructure;


public class QuizawaApi : ApiManagerFor
{
    public QuizawaApi()
    {
        base.SetChild(this);
        base.apiUrl = "http://192.168.1.111:8000/api";
        base.apiQuizzesUrl = apiUrl+ "/quizzes";
        base.apiQuestionsUrl = apiUrl+ "/quizzes/{quizzId}/questions";
        base.apiLoginUrl = apiUrl + "/users/login";

        base.hasToHaveTokenForApi = true;
        base.hasToLoginToGetToken = true;
        base.tokenHttpEmplacement = TokenHttpEmplacement.Everywhere; // Put the token everywhere where we can (url/header/body)
        base.apiKeyParamName = "api_token"; // Define the name of the parameter used when token has to be sent over http (api_token={TOKEN} be it in url/header/body)
    }


    // This is special here: The quizawa api doesn't give the opportunity to have answers in a separate endpoint/url. They are directly inside questions (nested elements).
    // Here we override the ApiManager method and return the answers that are inside the question
    public override Answers GetAnswersForQuestion(object quizzId, object questionId)
    {
        // Their could be {quizzId} in the link. In this case replace it with quizzId
        apiQuestionsUrl = base._originalApiQuestionsUrl.Replace("{quizzId}", quizzId.ToString());
        string json_questions_answers = NetworkRequestManager.HttpGetRequest(apiQuestionsUrl);

        if (json_questions_answers == null || json_questions_answers.Length == 0)
            return null;

        // Parse the data questions (json_questions_answers) received from backend into questionData
        QuizawaApiModel.QuestionsData questionsData = JsonUtility.FromJson<QuizawaApiModel.QuestionsData>(json_questions_answers);

        if (questionsData == null)
            return null;

        Debug.Log(JsonUtility.ToJson(questionsData.data));

        QuizawaApiModel.QuestionData wantedQuestionData = new QuizawaApiModel.QuestionData(); ; // The question data that we want to have (according to questionId)
        foreach (QuizawaApiModel.QuestionData questionData in questionsData.data) // Look for question into questionData.data (list of question)
        {
            if (questionData.id.ToString() == questionId.ToString())
            {
                wantedQuestionData = questionData;
                break;
            }
        }

        Answers answersToReturn = new Answers();

        // Put all AnswerData into Answers
        foreach (QuizawaApiModel.AnswerData answerData in wantedQuestionData.answers)
        {
            answerData.MapAPIValuesToAbstractClass();
            answersToReturn.AddAnswer(answerData);
        }


        return answersToReturn;
    }

    #region Quizzes

    // Serializes data received from API (json), maps it to Quizzes class and returns it
    public override Quizzes SerializeQuizzes(string json)
    {
        if (json == null || json.Length == 0)
            return null;

        // We serialize data received from api to QuizzesData
        QuizawaApiModel.QuizzesData quizzesData = JsonUtility.FromJson<QuizawaApiModel.QuizzesData>(json);

        if (quizzesData == null)
            return null;

        // We map quizzesData values to Quizzes class (QuizzesData inherits from Quizzes).
        quizzesData.MapAPIValuesToAbstractClass();


        return quizzesData;
    }

    #endregion

    #region Questions

    // Serializes data received from API (json), maps it to Questions class and returns it
    public override Questions SerializeQuestions(string json)
    {
        if (json == null || json.Length == 0)
            return null;

        // We serialize data received from api to QuizzesData
        QuizawaApiModel.QuestionsData questionsData = JsonUtility.FromJson<QuizawaApiModel.QuestionsData>(json);

        if (questionsData == null)
            return null;

        // We map quizzesData values to Quizzes class (QuizzesData inherits from Quizzes).
        questionsData.MapAPIValuesToAbstractClass();


        return questionsData;
    }

    #endregion

    #region Answers
    public override Answers SerializeAnswers(string json) // Child has to override this method so that data is serialized from child within GetAnswersForQuestion
    {
        if (json == null || json.Length == 0)
            return null;


        // We serialize data received from api to AnswerData
        QuizawaApiModel.AnswersFromQuestionData answersData = JsonUtility.FromJson<QuizawaApiModel.AnswersFromQuestionData>(json);

        if (answersData == null)
            return null;

        // We map answersData values to Answers class (AnswerData inherits from Answers).
        answersData.MapAPIValuesToAbstractClass();


        return answersData;
    }
    #endregion

    #region ApiToken

    // Serializes data received from API (json), maps it to ApiToken and returns it
    public override ApiToken SerializeApiToken(string json)
    {
        if (json == null || json.Length == 0)
            return null;

        // We serialize data received from api to UserData (this class contains the token for QuizawaApi)
        QuizawaApiModel.UserData userData = JsonUtility.FromJson<QuizawaApiModel.UserData>(json);

        if (userData == null)
            return null;

        // We map userData values to ApiToken class (UserData inherits from ApiToken).
        userData.MapAPIValuesToAbstractClass();


        return userData;
    }

    #endregion
}
