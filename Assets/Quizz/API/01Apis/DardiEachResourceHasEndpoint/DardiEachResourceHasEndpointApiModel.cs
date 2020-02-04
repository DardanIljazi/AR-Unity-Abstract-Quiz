using System;
using System.Collections.Generic;
using UnityEngine;
using static AbstractQuizzStructure;

/**
 *  DardiEachResourceHasEndpointApiModel is the class that defines how is structured the API data for Dardi Each Resource Has Endpoint
 *  - It maps the data from API to the classes that are used everywhere in the application logic (Quizzes/Quizz/Questions/Question/Answers/Answer)
 */
public class DardiEachResourceHasEndpointApiModel : MonoBehaviour
{
    [Serializable]
    public class QuizzesInAPI : Quizzes
    {
        public List<QuizzInAPI> quizzes;

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
        public int id;
        public string title;
        public string description;

        public override void MapAPIValuesToAbstractClass()
        {
            base.SetQuizzId(this.id);
            base.SetQuizzTitle(this.title);
        }
    }

    [Serializable]
    public class QuestionsInAPI : Questions
    {
        public List<QuestionInAPI> questions;

        public override void MapAPIValuesToAbstractClass()
        {
            foreach (QuestionInAPI questionData in this.questions)
            {
                questionData.MapAPIValuesToAbstractClass(); // Map values
                base.AddQuestion(questionData);
            }
        }
    }

    [Serializable]
    public class QuestionInAPI : Question
    {
        public int id;
        public string question;
        public int quizzId;

        public override void MapAPIValuesToAbstractClass()
        {
            base.SetQuestionid(this.id);
            base.SetQuestionText(this.question);
        }
    }


    [Serializable]
    public class AnswersInAPI : Answers
    {
        public List<AnswerInAPI> answers;

        public override void MapAPIValuesToAbstractClass()
        {
            foreach (AnswerInAPI answerData in this.answers)
            {
                answerData.MapAPIValuesToAbstractClass(); // Map values
                base.AddAnswer(answerData);
            }
        }
    }

    [Serializable]
    public class AnswerInAPI : Answer
    {
        public int id;
        public string answer;
        public bool rightAnswer;
        public int questionId;
        public int quizzId;

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

