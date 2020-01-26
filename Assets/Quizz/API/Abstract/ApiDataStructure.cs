using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Contains the abstract code that will be used everywhere in the application logic.
 *  Api classes will call MapValuesFromAPIToApplicationLogicClass to set which values (from API class) must be mapped to the values
 */
public abstract class AbstractQuizzStructure
{

    #region Quizzes
    public class Quizzes : IMappeableClassFromAPIData, ICloneable
    {
        private List<Quizz> quizzes = new List<Quizz>(); // List of quizz

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
    #endregion

    #region Quizz
    public class Quizz : IMappeableClassFromAPIData, ICloneable
    {
        private string quizzTitle;
        private object quizzId; // Can be string or int so we declare it as an object here

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

    #endregion

    #region Questions
    public class Questions : IMappeableClassFromAPIData, ICloneable
    {
        private string question;
        private List<Question> questionsList = new List<Question>();

        public virtual void MapValuesFromAPIToApplicationLogicClass() { } // Will be called from API data to map their values to the actual ones

        public void SetQuestion(string quest)
        {
            this.question = quest;
        }

        public string GetDataToShowAsQuestion()
        {
            return question;
        }

        public List<Question> GetQuestionsList()
        {
            return questionsList;
        }

        public void AddQuestion(Question quesiont)
        {
            this.questionsList.Add(quesiont);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    #endregion

    #region Question

    public class Question : IMappeableClassFromAPIData, ICloneable
    {
        private string question;
        private object questionId;

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

        public void SetQuestionText(string questionValue)
        {
            this.question = questionValue;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    #endregion

    #region Answers
    public class Answers : IMappeableClassFromAPIData, ICloneable
    {
        private List<Answer> answersList = new List<Answer>();

        public virtual void MapValuesFromAPIToApplicationLogicClass() { } // Will be called from API data to map their values to the actual ones

        public List<Answer> GetAnswersList()
        {
            return answersList;
        }

        public void AddAnswer(Answer answer)
        {
            this.answersList.Add(answer);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    #endregion

    #region Answer
    public class Answer : IMappeableClassFromAPIData, ICloneable
    {
        private string answer;
        private bool isCorrectAnswer;

        public virtual void MapValuesFromAPIToApplicationLogicClass() { } // Will be called from API data to map their values to the actual ones

        public bool IsCorrectAnswer()
        {
            return this.isCorrectAnswer;
        }

        public void SetIsCorrectAnswer(bool correct)
        {
            this.isCorrectAnswer = correct;
        }

        public string GetDataToShowAsPossibleAnswer()
        {
            return answer;
        }

        public void SetDataToShowAsPossibleAnswer(string answerValue)
        {
            this.answer = answerValue;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    #endregion

    #region ApiToken
    public class ApiToken : IMappeableClassFromAPIData, ICloneable 
    {
        private string token;
        private string apiKeyParamName;

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
    #endregion
}

public interface IMappeableClassFromAPIData
{
    void MapValuesFromAPIToApplicationLogicClass();
}
