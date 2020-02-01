using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * QuizawaApiModel is the model that defines what is returned by the API and make the mapping to the application logic classes (with MapValuesFromAPIToApplicationLogicClass)
 * Every class should be declared with member exactly with the same type and name as the json
 */

public class QuizawaApiModel : AbstractQuizzStructure
{

    #region Quizzes

    [Serializable]
    public class QuizzesData : Quizzes
    {
        public List<QuizzData> data = new List<QuizzData>(); // List of quizz

        // Mapping between API (QuizawaApiModel) and application logic data (Quizzes)
        public override void MapAPIValuesToAbstractClass()
        {
            foreach (QuizzData quizzData in this.data)
            {
                quizzData.MapAPIValuesToAbstractClass(); // Map values for QuizzData to Quizz (QuizzData inherits from Quizz)
                base.AddQuizz(quizzData);
            }
        }
    }

    #endregion

    #region Quizz

    [Serializable]
    public class QuizzData : Quizz
    {
        public int id;
        public string title;
        public string description;
        public string image;
        public int active;
        public int user_id;
        public List<ImageQuizzsData> image_quizzs; // A list of images that can be used for AR tracking

        // Mapping between API (QuizzData) and application logic data (Quizz)
        public override void MapAPIValuesToAbstractClass()
        {
            base.SetQuizzId(this.id);
            base.SetQuizzTitle(this.title);
        }
    }

    [Serializable]
    public class ImageQuizzsData // Contain a 
    {
        public int id;
        public string url;
        public int quizz_id;
    }

    #endregion

    #region Questions

    [Serializable]
    public class QuestionsData : Questions
    {
        public List<QuestionData> data = new List<QuestionData>(); // List of questions

        // Mapping between API (QuestionsData) and application logic data (Questions)
        public override void MapAPIValuesToAbstractClass()
        {
            foreach (QuestionData questionData in this.data)
            {
                questionData.MapAPIValuesToAbstractClass(); // Map values for QuestionData to Question (QuestionData inherits from Question)
                base.AddQuestion(questionData);
            }
        }
    }

    #endregion

    #region Question

    [Serializable]
    public class QuestionData : Question
    {
        public int id;
        public string question;
        public string image;
        public List<AnswerData> answers; // Is mapped to Answers in AnswersFromQuestionData class 

        // Mapping between API (QuestionData) and application logic data (Question)
        public override void MapAPIValuesToAbstractClass()
        {
            base.SetQuestionid(this.id);
            base.SetQuestionText(question);
        }
    }

    #endregion

    #region Answers

    [Serializable]
    public class AnswersFromQuestionData : Answers // Special here: the answers are attached with question when we get question from backend.
                                                   // Here we redefine exactly the same members as Question class but with inheritance to Answers and we map 
    {
        public int id;
        public string question;
        public string image;
        public List<AnswerData> answers;

        // Mapping between API (AnswerData) and application logic data (Answer)
        public override void MapAPIValuesToAbstractClass()
        {
            foreach (AnswerData answerData in this.answers)
            {
                answerData.MapAPIValuesToAbstractClass(); // Map values for AnswerData to Answer (AnswerData inherits from Answer)
                base.AddAnswer(answerData);
            }
        }
    }

    #endregion

    #region Answer

    [Serializable]
    public class AnswerData : Answer
    {
        public int id;
        public string value;
        public int correct;
        public int question_id;

        // Mapping between API (AnswerData) and application logic data (Answer)
        public override void MapAPIValuesToAbstractClass()
        {
            base.SetIsCorrectAnswer(this.correct == 1); // Api return correct as 0 when not correct and 1 when correct
            base.SetDataToShowAsPossibleAnswer(this.value);
        }
    }

    #endregion

    #region ApiToken

    [Serializable]
    public class UserData : ApiToken // We define this has ApiToken because that's the manner to get the token from this API (from the user)
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

        // Mapping between API (UserData) and application logic data (ApiToken) --> Here we use it like this because API token is present in UserData
        public override void MapAPIValuesToAbstractClass()
        {
            base.SetApiToken(this.api_token);
        }
    }
    #endregion
}


#region Example of json to class with JsonUtility.FromJson<Class>(JSON_DATA)
/**
 *
 * For example:
 * JSON_DATA: 
 * 
 * {
 *   "data": [
 *      {
 *          "id": 1,
 *          "title": "quizzTitle
 *          "description": "quizzDescription",
 *          "image": "https://i.ytimg.com/vi/U7VmBgp9D9o/maxresdefault.jpg"
 *      },
 *      {
 *          "id": 2,
 *          "title": "quizzTitle
 *          "description": "quizzDescription",
 *          "image": "https://i.ytimg.com/vi/U7VmBgp9D9o/maxresdefault.jpg"
 *      }
 *   ]
 * }
 *  
 *  --> Could be declared as the classes bellow:
 *  ---------------------------------------------------------------------------------
 *  
 *  [Serializable]
 *  public class QuizzesData 
 *  {
 *      public List<QuizzData> data = new List<QuizzData>();
 *  }
 *  
 *  [Serializable]
 *  public class QuizzData 
 *  {
 *      public List<QuizzData> data = new List<QuizzData>();
 *  }
 *  
 *  --> And used like this:
 *  ---------------------------------------------------------------------------------
 *  
 *   QuizzesData quizzesData = JsonUtility.FromJson<QuizzesData>(JSON_DATA);
 *  
 */

#endregion

