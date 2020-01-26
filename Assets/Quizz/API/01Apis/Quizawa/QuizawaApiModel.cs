using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 
 */
public class QuizawaApiModel : AbstractQuizzStructure
{
    [Serializable]
    public class QuizzesData : Quizzes
    {
        public List<QuizzData> data = new List<QuizzData>(); // List of quizz

        // Mapping between API (QuizawaApiModel) and application logic data (Quizzes)
        public override void MapValuesFromAPIToApplicationLogicClass()
        {
            foreach (QuizzData quizzData in this.data)
            {
                base.AddQuizz(quizzData);
            }
        }
    }

    [Serializable]
    public class QuizzData : Quizz
    {
        public int id;
        public string title;
        public string description;
        public string image;
        public int active;
        public int user_id;
        public List<ImageQuizzsData> image_quizzs;

        // Mapping between API (QuizzData) and application logic data (Quizz)
        public override void MapValuesFromAPIToApplicationLogicClass()
        {
            base.SetQuizzId(this.id);
            base.SetQuizzTitle(this.title);
        }
    }

    [Serializable]
    public class QuestionsData : Questions, IEnumerable
    {
        public List<QuestionData> data = new List<QuestionData>(); // List of questions

        // Mapping between API (QuestionsData) and application logic data (Questions)
        public override void MapValuesFromAPIToApplicationLogicClass()
        {

        }

        public IEnumerator GetEnumerator()
        {
            return data.GetEnumerator();
        }
    }

    [Serializable]
    public class QuestionData : Question
    {
        public int id;
        public string question;
        public string image;
        public List<AnswerData> answers;

        // Mapping between API (QuestionData) and application logic data (Question)
        public override void MapValuesFromAPIToApplicationLogicClass()
        {

        }
    }

    [Serializable]
    public class AnswerData
    {
        public int id;
        public string value;
        public int correct;
        public int question_id;
    }

    [Serializable]
    public class ImageQuizzsData
    {
        public int id;
        public string url;
        public int quizz_id;
    }


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
        public override void MapValuesFromAPIToApplicationLogicClass()
        {

        }
    }

}
