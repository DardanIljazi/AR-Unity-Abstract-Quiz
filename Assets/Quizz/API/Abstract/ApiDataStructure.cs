using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ApiDataStructure
{
    public class Quizzes : ICloneable
    {
        public List<Quizz> quizzes = new List<Quizz>(); // List of quizz

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public IEnumerator GetEnumerator()
        {
            return this.quizzes.GetEnumerator();
        }
    }

    public class Quizz : ICloneable
    {
        string quizzTitle;
        object quizzId; // Can be string or id so we declare it as an object here

        public string GetQuizzTitle()
        {
            return quizzTitle;
        }

        public void SetQuizzTitle(string title)
        {
            this.quizzTitle = title;
        }

        public void SetQuizzId(object id)
        {
            this.quizzId = id;
        }

        public object GetQuizzId()
        {
            return this.quizzId;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class Questions : ICloneable
    {
        private string question;
        private List<Question> questionsList;

        public void SetQuestion(string quest)
        {
            this.question = quest;
        }

        public string GetDataToShowAsQuestion()
        {
            return question;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public List<Question> GetQuestionsList()
        {
            return questionsList;
        }
    }

    public class Question : ICloneable
    {
        string question;
        object questionId;

        public object GetQuestionId()
        {
            return questionId;
        }

        public void SetQuestionid(object id)
        {
            this.questionId = id;
        }

        public string GetQuestion()
        {
            return question;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class Answers
    {
        List<Answer> answersList;

        public List<Answer> GetAnswersList()
        {
            return answersList;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class Answer : ICloneable
    {
        string answer;
        bool isCorrectAnswer;

        public bool IsCorrectAnswer()
        {
            return this.isCorrectAnswer;
        }

        public string GetDataToShowAsPossibleAnswer()
        {
            return answer;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class ApiToken : ICloneable
    {
        string token;
        string apiKeyParamName;

        public string GetApiToken()
        {
            return token;
        }

        public void SetApiToken(string apiToken)
        {
            token = apiToken;
        }

        public string GetApiKeyParamName()
        {
            return apiKeyParamName;
        }

        public void SetApiKeyParamName(string paramName)
        {
            this.apiKeyParamName = paramName;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
