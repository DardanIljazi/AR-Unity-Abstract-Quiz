using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiModel : AbstractQuizzStructure
{
    // Will Serialize raw data (json) to the Quizzes class
    public Quizzes SerializeJsonToQuizzes(string json)
    {
        throw new NotImplementedException();
    }

    // Will Serialize raw data (json) to the Quizzes class
    public  Quizz SerializeJsonToQuizz(string json)
    {
        throw new NotImplementedException();
    }

    // Will Serialize raw data (json) to the Questions class
    public  Questions SerializeJsonToQuestions(string json)
    {
        throw new NotImplementedException();
    }

    // Will Serialize raw data (json) to the Question class
    public  Question SerializeJsonToQuestion(string json)
    {
        throw new NotImplementedException();
    }

    // Will Serialize raw data (json) to the Answers class
    public  Answers SerializeJsonToAnswers(string json)
    {
        throw new NotImplementedException();
    }

    // Will Serialize raw data (json) to the Answer class
    public  Answer SerializeJsonToAnswer(string json)
    {
        throw new NotImplementedException();
    }

    public ApiToken SerializeJsonToApiToken(string json)
    {
        throw new NotImplementedException();
    }
}
