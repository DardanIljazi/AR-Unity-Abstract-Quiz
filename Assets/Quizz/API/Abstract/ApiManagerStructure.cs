using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbstractQuizzStructure;

public abstract class ApiManagerStructure : MonoBehaviour
{
    public abstract bool HasToHaveToken();
    public abstract bool HasToLoginToGetToken();

    public enum TokenHttpEmplacement  // If a token is needed for to the api, there could be many possibilities of "emplacement"/places where this can be defined in HTTP request.
                                      // We can put token in Url (GET), Body (POST), Header (both) or try to put them in all of them with Everywhere enum or not use a token at all with None 
    {
        None = 0, // No token
        Header = 1, // Token is put in header
        BodyOrUrlParam = 2, // Token is put as a param in the url (only for GET request) or in body (only for POST request)
        Everywhere = 3, // Put the token everywhere: in header (POST/GET), in body (only for POST request) and in url (only for GET request)
    }
    
    public abstract TokenHttpEmplacement GetTokenttpEmplacement();

    public abstract Quizzes     GetQuizzesList();
    public abstract Quizz       GetQuizzFromQuizzesList(object quizzId);
    public abstract Questions   GetQuestionsListForQuizz(object quizzId);
    public abstract Question    GetQuestionFromQuestionsList(object questionId);
    public abstract Answers     GetAnswersForQuestion(object questionId);
    public abstract Answer      GetAnswerFromAnswersList(object answerId);
}