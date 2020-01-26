using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ApiData;

public abstract class ApiManagerStructure
{
    public abstract bool HasToUseToken();


    public abstract Quizzes     GetQuizzesList();
    public abstract Questions   GetQuestionsListForQuizz(int quizzId);
    public abstract Question    GetQuestionFromQuestionsList(int questionId);
    public abstract Answers     GetAnswersForQuestion(int questionId);
    public abstract Answer      GetAnswerFromAnswersList(int answerId);

}