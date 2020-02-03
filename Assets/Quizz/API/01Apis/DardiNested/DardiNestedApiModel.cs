using System;
using System.Collections.Generic;
using UnityEngine;
using static AbstractQuizzStructure;

/**
 *  DardiNestedApiModel is the class that defines how is structured the API data for Dardi Nested Api
 *  - It maps the data from API to the classes that are used everywhere in the application logic (Quizzes/Quizz/Questions/Question/Answers/Answer)
 */
public class DardiNestedApiModel : MonoBehaviour
{
    [Serializable]
    public class QuizzesInAPI : Quizzes
    {
        public List<QuizzInAPI> quizzes { get; set; }

        public override void MapAPIValuesToAbstractClass()
        {
            foreach (QuizzInAPI quizzData in this.quizzes)
            {
                quizzData.MapAPIValuesToAbstractClass(); // Map values
                base.AddQuizz(quizzData);
            }
        }
    }

    [Serializable]
    public class QuizzInAPI : Quizz
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public List<QuestionInAPI> questions { get; set; }

        public override void MapAPIValuesToAbstractClass()
        {
            base.SetQuizzId(this.id);
            base.SetQuizzTitle(this.title);
        }
    }

    [Serializable]
    public class QuestionsInAPI : Questions // Because we are in nested resources case, we have access to questions only by going from "top to bottom". 
                                            // So we must go on each quizz and go again inside of them to have questions
    {
        public List<QuizzInAPI> quizzes { get; set; }

        public override void MapAPIValuesToAbstractClass()
        {
            // Go over quizzes
            foreach (QuizzInAPI quizzData in this.quizzes)
            {
                // Go over questions in quizz
                foreach (QuestionInAPI questionData in quizzData.questions)
                {
                    questionData.MapAPIValuesToAbstractClass();
                    base.AddQuestion(questionData);
                }
            }
        }
    }

    [Serializable]
    public class QuestionInAPI : Question
    {
        public int id { get; set; }
        public string question { get; set; }
        public List<AnswerInAPI> answers { get; set; }

        public override void MapAPIValuesToAbstractClass()
        {
            base.SetQuestionid(this.id);
            base.SetQuestionText(this.question);
        }
    }


    [Serializable]
    public class AnswersInAPI : Answers // Because we are in nested resources case, we have access to questions only by going from "top to bottom". 
                                        // So we must go on each quizz and go again inside of them to have questions, and again to have answers
    {
        public List<QuizzInAPI> quizzes { get; set; }

        public override void MapAPIValuesToAbstractClass()
        {
            // Go over quizzes
            foreach (QuizzInAPI quizzData in this.quizzes)
            {
                // Go over questions in quizz
                foreach (QuestionInAPI questionData in quizzData.questions)
                {
                    // Go over answers in question
                    foreach (AnswerInAPI answerInAPI in questionData.answers)
                    {
                        answerInAPI.MapAPIValuesToAbstractClass();
                        base.AddAnswer(answerInAPI);
                    }
                }
            }
        }
    }

    [Serializable]
    public class AnswerInAPI : Answer
    {
        public int id { get; set; }
        public string answer { get; set; }
        public bool rightAnswer { get; set; }

        public override void MapAPIValuesToAbstractClass()
        {
            base.SetDataToShowAsPossibleAnswer(this.answer);
            base.SetIsCorrectAnswer(this.rightAnswer);
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

