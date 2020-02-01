using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Contains the classes/code that will be used everywhere in the application logic
 *  Each Api Model will have to map the data from API to these classes you'll find below (Quizzes/Quizz/Questions/Question/Answers/Answer)
 *  
 *  For example:
 *  - A Quizz can be represented as a title (For example: "Cat Quizz" / "Who Wants to Be a Millionaire?" aso..) 
 *    and an identification (to diferentiate it from other quizzes).
 *    
 *  - A Question can be respresented as a title (For example: "How many lives does a cat have ?" / "Which of these U.S. Presidents appeared on the television series 'Laugh-In'" aso..)
 *    and an identification (to diferentiate it from other questions).
 *    
 *  - ...
 *    
 *  --> All these abstractions are set in the classes below
 *      They should already be set fine for a quiz.
 *      It is not recommended to modify the code below
 *      ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
 */
public abstract class AbstractQuizzStructure : MonoBehaviour
{

    #region Quizzes
    public class Quizzes : IMappeableAPIDataToAbstractClass, ICloneable
    {
        private List<Quizz> _quizzes = new List<Quizz>(); // List of quizz

        // Api Model (that inherits from AbstractQuizzStructure) Will map the values of the API to the ones needed for Quizzes Class (_quizzes)
        public virtual void MapAPIValuesToAbstractClass()
        {
            throw new NotImplementedException();
        }

        // Will Serialize raw data (json) to the Quizzes class
        public virtual Quizzes SerializeJsonToQuizzes(string json)
        {
            throw new NotImplementedException();
        }


        public void AddQuizz(Quizz quizz)
        {
            _quizzes.Add(quizz);
        }

        public List<Quizz> GetQuizzesList()
        {
            return _quizzes;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    #endregion

    #region Quizz
    public class Quizz : IMappeableAPIDataToAbstractClass, ICloneable
    {
        private string _quizzTitle;
        private object _quizzId; // Can be string or int so we declare it as an object here

        // Api Model (that inherits from AbstractQuizzStructure) Will map the values of the API to the ones needed for Quizz Class (_quizzTitle, _quizzId)
        public virtual void MapAPIValuesToAbstractClass()
        {
            throw new NotImplementedException();
        }

        // Will Serialize raw data (json) to the Quizzes class
        public virtual Quizz SerializeJsonToQuizz(string json)
        {
            throw new NotImplementedException();
        }


        public string GetQuizzTitle()
        {
            return _quizzTitle;
        }

        public void SetQuizzTitle(string title)
        {
            this._quizzTitle = title;
        }

        public void SetQuizzId(object id)
        {
            this._quizzId = id;
        }

        public object GetQuizzId()
        {
            return this._quizzId;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    #endregion

    #region Questions
    public class Questions : IMappeableAPIDataToAbstractClass, ICloneable
    {
        private List<Question> _questionsList = new List<Question>(); // List of question

        // Api Model (that inherits from AbstractQuizzStructure) Will map the values of the API to the ones needed for Questions Class (_questionsList)
        public virtual void MapAPIValuesToAbstractClass()
        {
            throw new NotImplementedException();
        }

        // Will Serialize raw data (json) to the Questions class
        public virtual Questions SerializeJsonToQuestions(string json)
        {
            throw new NotImplementedException();
        }


        public List<Question> GetQuestionsList()
        {
            return _questionsList;
        }

        public void AddQuestion(Question quesion)
        {
            this._questionsList.Add(quesion);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    #endregion

    #region Question
    public class Question : IMappeableAPIDataToAbstractClass, ICloneable
    {
        private string _questionTitle;
        private object _questionId; // The questionId can be in string or int format so we declare it as an object

        // Api Model (that inherits from AbstractQuizzStructure) Will map the values of the API to the ones needed for Question Class (_questionTitle, _questionId)
        public virtual void MapAPIValuesToAbstractClass()
        {
            throw new NotImplementedException();
        }

        // Will Serialize raw data (json) to the Question class
        public virtual Question SerializeJsonToQuestion(string json)
        {
            throw new NotImplementedException();
        }


        public object GetQuestionId()
        {
            return _questionId;
        }

        public void SetQuestionid(object id)
        {
            this._questionId = id;
        }

        public string GetQuestionTitle()
        {
            return _questionTitle;
        }

        public void SetQuestionText(string questionValue)
        {
            this._questionTitle = questionValue;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    #endregion

    #region Answers
    public class Answers : IMappeableAPIDataToAbstractClass, ICloneable
    {
        private List<Answer> _answersList = new List<Answer>();

        // Api Model (that inherits from AbstractQuizzStructure) Will map the values of the API to the ones needed for Answers Class (_answersList)
        public virtual void MapAPIValuesToAbstractClass()
        {
            throw new NotImplementedException();
        }

        // Will Serialize raw data (json) to the Answers class
        public virtual Answers SerializeJsonToAnswers(string json)
        {
            throw new NotImplementedException();
        }


        public List<Answer> GetAnswersList()
        {
            return _answersList;
        }

        public void AddAnswer(Answer answer)
        {
            this._answersList.Add(answer);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    #endregion

    #region Answer
    public class Answer : IMappeableAPIDataToAbstractClass, ICloneable
    {
        private string _answerTitle;
        private bool _isCorrectAnswer;

        // Api Model (that inherits from AbstractQuizzStructure) Will map the values of the API to the ones needed for Answer Class (_answerTitle, _isCorrectAnswer)
        public virtual void MapAPIValuesToAbstractClass()
        {
            throw new NotImplementedException();
        }

        // Will Serialize raw data (json) to the Answer class
        public virtual Answer SerializeJsonToAnswer(string json)
        {
            throw new NotImplementedException();
        }


        public bool IsCorrectAnswer()
        {
            return this._isCorrectAnswer;
        }

        public void SetIsCorrectAnswer(bool correct)
        {
            this._isCorrectAnswer = correct;
        }

        public string GetDataToShowAsPossibleAnswer()
        {
            return _answerTitle;
        }

        public void SetDataToShowAsPossibleAnswer(string answerValue)
        {
            this._answerTitle = answerValue;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    #endregion

}

public interface IMappeableAPIDataToAbstractClass
{
    void MapAPIValuesToAbstractClass();
}
