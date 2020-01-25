using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * ApiData define classes that represents: API data classes, Classes used over application logic from those API data classes
 * 
 * The 2 types of classes in this file:
 *      [+] Classes that contains the word "Data" at the end (like ConnectionData, QuizzesData, ...Data)
 *              - They represent the exact structure of json API data (response).
 *              --> If they are not the same as the API response, the serialization won't work
 *      
 *      [+] The other classes (like ApiToken, Quizzes, Questions, Question...)
 *              - Those classes inherits from "Data" classes and are used in application logic
 *              --> This ensures that if the API changes, only the "Data" classes must be changed and the application logic stay the same
 */
public static class ApiData
{

    #region Connection
    /** CONNECTION **/

    [Serializable]
    public class ConnectionData
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
     * If the token api is predefined (static) or not needed at all for the API it can be changed as follow:
     *  For static api token:
     *      - Delete the inheritance and return the token api as a string in GetApiToken()
     *      
     *  To not use any token:
     *      - Define the GetTokenHttpEmplacement() return as "return TokenHttpEmplacement.None;"
     */
    [Serializable]
    public class ApiToken : ConnectionData
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
                                              // For example in the url: www.example.com?api_token={API_TOKEN}
                                              //                                         ^^^^^^^^^
        {
            return "api_token";
        }

        public enum TokenttpEmplacement  // If a token is needed for to the api, there could be many possibilities of "emplacement"/places where this can be defined in HTTP request.
                                         // We can put token in Url (GET), Body (POST), Header (both) or try to put them in all of them with Everywhere enum or not use a token at all with None 
        {
            None = 0, // No token
            Header = 1, // Token is put in header
            BodyOrUrlParam = 2, // Token is put as a param in the url (only for GET request) or in body (only for POST request)
            Everywhere = 3, // Put the token everywhere: in header (POST/GET), in body (only for POST request) and in url (only for GET request)
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
    public class QuizzesData : IEnumerable
    {
        public List<QuizzData> data = new List<QuizzData>(); // List of quizz

        public IEnumerator GetEnumerator()
        {
            return this.data.GetEnumerator();
        }
    }

    [Serializable]
    public class Quizzes : QuizzesData
    {

    }

    /** -- END OF QUIZZES **/
    #endregion


    #region Quizz
    /** QUIZZ **/

    [Serializable]
    public class QuizzData
    {
        public int id;
        public string title;
        public string description;
        public string image;
        public int active;
        public int user_id;
        public List<ImageQuizzs> image_quizzs;
    }

    [Serializable]
    public class Quizz : QuizzData, ICloneable
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
    /** QUIZZ QUESTIONS **/

    [Serializable]
    public class QuestionsData : IEnumerable
    {
        public List<QuestionData> data = new List<QuestionData>(); // List of questions

        public IEnumerator GetEnumerator()
        {
            return data.GetEnumerator();
        }
    }
    [Serializable]
    public class Questions : QuestionsData, ICloneable
    {
        public List<QuestionData> GetQuestionsList()
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
    /** QUIZZ QUESTION **/

    [Serializable]
    public class QuestionData
    {
        public int id;
        public string question;
        public string image;
        public List<AnswerData> answers;
    }
    
    [Serializable]
    public class Question : QuestionData, ICloneable
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
    /** QUIZZ ANSWER **/

    [Serializable]
    public class AnswerData
    {
        public int id;
        public string value;
        public int correct;
        public int question_id;
    }

    [Serializable]
    public class Answer : AnswerData, ICloneable 
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
