using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * HerokuApiModel is the model that defines what is returned by the API and make the mapping to the application logic classes (with MapValuesFromAPIToApplicationLogicClass)
 * Every class should be declared with member exactly with the same type and name as the json
 */

public class HerokuApiModel : AbstractQuizzStructure
{

    [Serializable]
    public class GameQuizzes : Quizzes, IEnumerable
    {
        public List<IndexQuizz> quizzes = new List<IndexQuizz>();


        public override void MapValuesFromAPIToApplicationLogicClass()
        {
            foreach (IndexQuizz quizzData in this.quizzes)
            {
                quizzData.MapValuesFromAPIToApplicationLogicClass(); // Map values for QuizzData to Quizz (QuizzData inherits from Quizz)
                base.AddQuizz(quizzData);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this.quizzes.GetEnumerator();
        }
    }

    [Serializable]
    public class GameQuizzeForQuestions : Questions
    {
        public string id;
        public string title;
        public string description;
        public string created_by;
        public List<QuestionData> questions = new List<QuestionData>();
        public int number_participants;

        public IEnumerator GetEnumerator()
        {
            return this.questions.GetEnumerator();
        }

        public override void MapValuesFromAPIToApplicationLogicClass()
        {
            foreach (QuestionData questionData in questions)
            {
                questionData.MapValuesFromAPIToApplicationLogicClass();
                base.AddQuestion(questionData);
            }
        }
    }


    [Serializable] // Exactly the same structure as GameQuizze (copied)
                   // This is because elements are nested. We need to take each elements separately (Quizz/Questions/Answers)
                   // Answers are inside questions so we make a loop to add them in Answers class
    public class GameQuizzForAnswers : Answers
    {
        public string id;
        public string title;
        public string description;
        public string created_by;
        public List<QuestionData> questions = new List<QuestionData>();
        public int number_participants;

        public override void MapValuesFromAPIToApplicationLogicClass()
        {
            foreach (QuestionData question in questions)
            {
                foreach (Answer answer in question.answers) 
                {
                    answer.MapValuesFromAPIToApplicationLogicClass();
                    base.AddAnswer(answer);
                }
            }
        }
    }

    [Serializable]
    public class AnswerData : Answer
    {
        public string name;
        public bool value;

        public override void MapValuesFromAPIToApplicationLogicClass()
        {
            base.SetDataToShowAsPossibleAnswer(name);
            base.SetIsCorrectAnswer(value);
        }
    }

    [Serializable]
    public class QuestionData : Question
    {
        public string question;
        public string image;
        public List<AnswerData> answers = new List<AnswerData>();

        public override void MapValuesFromAPIToApplicationLogicClass()
        {
            base.SetQuestionText(question);
        }
    }



    [Serializable]
    public class Creator
    {
        string id;
        string username;
    }

    

    [Serializable]
    public class IndexQuizz : Quizz
    {
        public string title;
        public string image;
        public string description;
        public Creator created_by = new Creator();
        public int number_participants;
        public string id;

        public override void MapValuesFromAPIToApplicationLogicClass()
        {
            base.SetQuizzId(id);
            base.SetQuizzTitle(title);
        }

    }
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
 *      public int id;
 *      public string title;
 *      public string description;
 *      public string image;
 *  }
 *  
 *  --> And used like this:
 *  ---------------------------------------------------------------------------------
 *  
 *   QuizzesData quizzesData = JsonUtility.FromJson<QuizzesData>(JSON_DATA);
 *  
 */

#endregion

