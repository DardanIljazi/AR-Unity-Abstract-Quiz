using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbstractQuizzStructure;

/**
 *  HerokuApiModel is the class that defines how is structured the API data.
 *  - It maps the data from API to the classes that are used everywhere in the application logic (Quizzes/Quizz/Questions/Question/Answers/Answer)
 *  
 */
public class HerokuApiModel : ApiModel
{

    [Serializable]
    public class GameQuizzes : Quizzes, IEnumerable
    {
        public List<IndexQuizz> quizzes = new List<IndexQuizz>();

        public override void MapAPIValuesToAbstractClass()
        {
            foreach (IndexQuizz quizzData in this.quizzes)
            {
                quizzData.MapAPIValuesToAbstractClass(); // Map values for QuizzData to Quizz (QuizzData inherits from Quizz)
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

        public override void MapAPIValuesToAbstractClass()
        {
            foreach (QuestionData questionData in questions)
            {
                questionData.MapAPIValuesToAbstractClass();
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

        public override void MapAPIValuesToAbstractClass()
        {
            foreach (QuestionData question in questions)
            {
                foreach (Answer answer in question.answers)
                {
                    answer.MapAPIValuesToAbstractClass();
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

        public override void MapAPIValuesToAbstractClass()
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

        public override void MapAPIValuesToAbstractClass()
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

        public override void MapAPIValuesToAbstractClass()
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

