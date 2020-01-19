// Davide Carboni
// Manage the single quiz

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class QuizzeManager : MonoBehaviour
{
    public GameObject api;                                      // the api to get all data
    private ApiData.GameQuizze quizzeData;                      // The base data for the selected quiz
    public List<int> selectedQuestions = new List<int>();       // selected qiuz list  
    public int nbQuestions = 0;                                 // number of questions in the game 
    public int answeredQuestion = 0;                            // the global number of answered questions
    public int answeredTrueQuestion = 0;                        // the number of answer true questions
    public bool isFinished = false;                             // flags to set if the game if finished
    private string id = string.Empty;                           // the selecteded quiz id
    public bool isEmpty = false;                                // to check if the are quizzes available for this game  

    public ApiData.GameQuizze QuizzeData { get => quizzeData; set => quizzeData = value; }

    /// <summary>
    /// Initial data values
    /// </summary>
    void OnEnable()
    {
        //this.id = GameManager.selectedQuizz;                                          // get selected quiz id from game manager
        //this.QuizzeData = api.GetComponent<Api>().getGameQuizzeFromAPI(this.id);      // get data from api for the selected quiz  
        //this.nbQuestions = this.QuizzeData.questions.Count;                           // count the available quiz
    }

    /// <summary>
    /// Get random question from all disponible quizzes
    /// </summary>
    /// <returns></returns>
    public ApiData.Question GetRandomQuestion()
    {
        if (selectedQuestions.Count >= nbQuestions) return null;                                          // verify if there are the place of a new question
        System.Random rnd = new System.Random();                                                          
        int q;
        while (selectedQuestions.Contains(q = rnd.Next(0, QuizzeData.questions.Count))){ }                // get a random quiz that is not already in the list        
        selectedQuestions.Add(q);                                                                         // add the index of selected question in the list
        return QuizzeData.questions[q];                                                                   // return the selected quiz        
    }

    /// <summary>
    /// Check every time if the game is finished and if the are available quiz for this game
    /// </summary>
    void Update()
    {
        isFinished = (answeredQuestion == selectedQuestions.Count) ? true : false;
        isEmpty = (nbQuestions == 0) ? true : false;
    }

}
