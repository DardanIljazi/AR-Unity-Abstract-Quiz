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
