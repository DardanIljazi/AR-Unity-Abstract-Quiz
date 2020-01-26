using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Contains the abstract code that will be used everywhere in the application logic.
 *  Api will 
 */
public abstract class AbstractQuizzStructure
{
    public class Quizzes : IMappeableClassFromAPIData, ICloneable
    {
        List<Quizz> quizzes = new List<Quizz>(); // List of quizz

        public virtual void MapValuesFromAPIToApplicationLogicClass() { } // Will be called from API data to map their values to the actual ones

        public void AddQuizz(Quizz quizz)
        {
            quizzes.Add(quizz);
        }

        public List<Quizz> GetQuizzesList()
        {
            return quizzes;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class Quizz : IMappeableClassFromAPIData, ICloneable
    {
        string quizzTitle;
        object quizzId; // Can be string or id so we declare it as an object here

        public virtual void MapValuesFromAPIToApplicationLogicClass() { } // Will be called from API data to map their values to the actual ones

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

    public class Questions : IMappeableClassFromAPIData, ICloneable
    {
        private string question;
        private List<Question> questionsList;

        public virtual void MapValuesFromAPIToApplicationLogicClass() { } // Will be called from API data to map their values to the actual ones

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

    public class Question : IMappeableClassFromAPIData, ICloneable
    {
        string question;
        object questionId;

        public virtual void MapValuesFromAPIToApplicationLogicClass() { } // Will be called from API data to map their values to the actual ones

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

    public class Answers : IMappeableClassFromAPIData, ICloneable
    {
        List<Answer> answersList;

        public virtual void MapValuesFromAPIToApplicationLogicClass() { } // Will be called from API data to map their values to the actual ones

        public List<Answer> GetAnswersList()
        {
            return answersList;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class Answer : IMappeableClassFromAPIData, ICloneable
    {
        string answer;
        bool isCorrectAnswer;

        public virtual void MapValuesFromAPIToApplicationLogicClass() { } // Will be called from API data to map their values to the actual ones

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

    public class ApiToken : IMappeableClassFromAPIData, ICloneable 
    {
        string token;
        string apiKeyParamName;

        public virtual void MapValuesFromAPIToApplicationLogicClass() { } // Will be called from API data to map their values to the actual ones

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

public interface IMappeableClassFromAPIData
{
    void MapValuesFromAPIToApplicationLogicClass();
}
