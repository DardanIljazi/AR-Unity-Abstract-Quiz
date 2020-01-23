// Based on Davide Carboni's work in 2019
// Main classes for the data

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ApiData
{
    #region NEW API
        #region Quizzes (list)
        /**
         * QUIZZES
         */
        [Serializable]
        public class Quizzes : IEnumerable
        {
            public List<Quizz> data = new List<Quizz>(); // List of quizz

            public IEnumerator GetEnumerator()
            {
                return this.data.GetEnumerator();
            }
        }
        /**
         * QuizzesData should has to stay as a class over future versions. 
         * It gives the possibility to change easily the API while keeping the working code intact
         * Only the return types (int/string..) should be changed according to the API and what's inside the methods
         */
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
        public class QuestionsQuizzData : Questions, ICloneable
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
        public class QuestionQuizzData : Question, ICloneable
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
        /**
         * AnswerQuizzData should has to stay as a class over future versions. 
         * It gives the possibility to change easily the API while keeping the working code intact
         * Only the return types (int/string..) should be changed according to the API and what's inside the methods
         */
        [Serializable]
        public class AnswerQuizzData : Answer, ICloneable
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

    #endregion

}
