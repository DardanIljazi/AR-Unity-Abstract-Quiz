// Davide Carboni
// Main classes for the data

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ApiData
{
    #region Classes

    [Serializable]
    public class GameQuizzes : IEnumerable
    {
        public List<Quizz> data = new List<Quizz>(); // List of quizz

        public IEnumerator GetEnumerator()
        {
            return this.data.GetEnumerator();
        }
    }

    [Serializable]
    public class Answer
    {
        public string name;
        public bool value;
    }

    [Serializable]
    public class Question
    {
        public string question;
        public string image;
        public List<Answer> answers = new List<Answer>();
    }

    [Serializable]
    public class Creator
    {
        string id;
        string username;
    }

    [Serializable]
    public class GameQuizze
    {
        public string id;
        public string title;
        public string description;
        public string created_by;
        public List<Question> questions = new List<Question>();
        public int number_participants;

        public IEnumerator GetEnumerator()
        {
            return this.questions.GetEnumerator();
        }
    }

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
     * Only the return types (int/string..) should be changed according to the API
     */
    public class QuizzData : Quizz
    {
        public int GetQuizzId()
        {
            return id;
        }
    }




    [Serializable]
    public class ImageQuizzs
    {
        public int id;
        public string url;
        public int quizz_id;
    }

    #endregion
}


// Kept this one only to have a copy of the old api
public static class OldApiData
{
    #region Classes

    [Serializable]
    public class GameQuizzes : IEnumerable
    {
        public List<IndexQuizz> quizzes = new List<IndexQuizz>();

        public IEnumerator GetEnumerator()
        {
            return this.quizzes.GetEnumerator();
        }
    }

    [Serializable]
    public class Answer
    {
        public string name;
        public bool value;
    }

    public class RespondQuizzData : ApiData.Answer
    {
        public string GetDataToShowInCellView()
        {
            return name;
        }

        public void SetDataToShowInCellView(string data)
        {
            base.name = data;
        }
    }

    [Serializable]
    public class Question
    {
        public string question;
        public string image;
        public List<Answer> answers = new List<Answer>();
    }


    [Serializable]
    public class Creator
    {
        string id;
        string username;
    }

    [Serializable]
    public class GameQuizze
    {
        public string id;
        public string title;
        public string description;
        public string created_by;
        public List<Question> questions = new List<Question>();
        public int number_participants;

        public IEnumerator GetEnumerator()
        {
            return this.questions.GetEnumerator();
        }
    }

    [Serializable]
    public class IndexQuizz
    {
        public string title;
        public string image;
        public string description;
        public Creator created_by = new Creator();
        public int number_participants;
        public string id;

    }


    #endregion
}
