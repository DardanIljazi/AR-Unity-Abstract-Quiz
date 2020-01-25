using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * ApiData define classes that represents the Api response structure (json)
 * 
 * Because Api can change over time, the below classes that contain the word  "Data" at the end (like ApiTokenData, QuestionData, ...Data) should not be renamed
 * Those classes are used in the application logic and inherit from other classes that represent data from API (for example Connection, Quizzes, Questions, Question...)
 * This ensures that even if the API change, application logic (should) stay the same and has not to be changed.
 */
public static class ApiData
{
    #region Connection
    /** CONNECTION **/

    [Serializable]
    public class Connection
    {
        public int id;
        public string email;
        public object email_verified_at;
        public string password;
        public string pseudo;
        public string firstname;
        public string lastname;
        public int admin;
        public int creator;
        public string api_token;
        public int classroom_id;
        public object deleted_at;
    }


    /**
     * Inherit from Connection because that's the manner we use to get the api token with the actual API. 
     * If the token api is predefined (static) or not needed at all for the API, just delete the inheritance and return the token api as a string in GetApiToken()
     */
    [Serializable]
    public class ApiTokenData : Connection
    {
        public string GetApiToken()
        {
            return base.api_token;
        }

        public void SetApiToken(string apiToken)
        {
            base.api_token = apiToken;
        }

        public static string GetApiKeyParam() // The parameter key that the server waits for the api
        {
            return "api_token";
        }

        public enum TokenttpEmplacement  // If a token is needed for to the api, there could be many possibilities of "emplacement" when doing HTTP request.
                                         // We can put token in Url (GET), Body (POST), Header (both) or try to put them in all of them with Everywhere enum or not use a token at all with None 
        {
            None = 0, // No token
            Header = 1, // Token is put in header
            BodyOrUrlParam = 2, // Token is put as a param in the url (only for GET request) or in body (only for POST request)
            Everywhere = 3, // Put the token in header (POST/GET), in body (only for POST request) and in url (only for GET request)
        }

        public static TokenttpEmplacement GetTokenHttpEmplacement()
        {
            return TokenttpEmplacement.Everywhere;
        }
    }
    /** -- END OF CONNECTION **/
    #endregion

    #region Quizzes (list)

    /** QUIZZES **/

    [Serializable]
    public class Quizzes : IEnumerable
    {
        public List<Quizz> data = new List<Quizz>(); // List of quizz

        public IEnumerator GetEnumerator()
        {
            return this.data.GetEnumerator();
        }
    }

    [Serializable]
    public class QuizzesData : Quizzes
    {

    }

    /** -- END OF QUIZZES **/
    #endregion

    #region Quizz
    /**
     * QUIZZ
     */
    [Serializable]
    public class Quizz
    {
        public int id;
        public string title;
        public string description;
        public string image;
        public int active;
        public int user_id;
        public List<ImageQuizzs> image_quizzs;
    }
    /**
     * QuizzData should has to stay as a class over future versions. 
     * It gives the possibility to change easily the API while keeping the working code intact
     * Only the return types (int/string..) should be changed according to the API and what's inside the methods
     */
    [Serializable]
    public class QuizzData : Quizz, ICloneable
    {
        public int GetQuizzId()
        {
            return id;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    [Serializable]
    public class ImageQuizzs
    {
        public int id;
        public string url;
        public int quizz_id;
    }
    /** -- END OF QUIZZ **/
    #endregion

    #region Quizz questions (list)
    /**
     * QUIZZ QUESTIONS
     */
    [Serializable]
    public class Questions : IEnumerable
    {
        public List<Question> data = new List<Question>(); // List of questions

        public IEnumerator GetEnumerator()
        {
            return data.GetEnumerator();
        }
    }
    [Serializable]
    public class QuestionsData : Questions, ICloneable
    {
        public List<Question> GetQuestionsList()
        {
            return data;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    /** -- END OF QUIZZ QUESTIONS **/
    #endregion

    #region Quizz question
    /**
     * QUIZZ QUESTION
     */
    [Serializable]
    public class Question
    {
        public int id;
        public string question;
        public string image;
        public List<Answer> answers;
    }
    /**
     * QuestionQuizzData should has to stay as a class over future versions. 
     * It gives the possibility to change easily the API while keeping the working code intact
     * Only the return types (int/string..) should be changed according to the API and what's inside the methods
     */
    [Serializable]
    public class QuestionData : Question, ICloneable
    {
        public string GetDataToShowAsQuestion()
        {
            return question;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    /** -- END OF QUIZZ QUESTIONS **/
    #endregion

    #region Quizz answer
    /**
     * QUIZZ ANSWER
     */
    [Serializable]
    public class Answer
    {
        public int id;
        public string value;
        public int correct;
        public int question_id;
    }

    [Serializable]
    public class AnswerData : Answer, ICloneable // 
    {
        public string GetDataToShowAsPossibleAnswer()
        {
            return value;
        }

        public bool IsCorrectAnswer()
        {
            return correct == 1;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    /** -- END OF QUIZZ ANSWER **/
    #endregion
}
