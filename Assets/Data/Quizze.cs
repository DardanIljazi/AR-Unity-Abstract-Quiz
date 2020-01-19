// Davide Carboni
// Class Quiz to use for the Quiz Game Object

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Quizze : MonoBehaviour
{
    public string id;
    public string title;
    public string description;
    public string created_by;
    public int number_participants;

    public GameObject quizzeManager;
    private ApiData.GameQuizze gameQuizze;

    public void Start()
    {
        /*gameQuizze = quizzeManager.GetComponent<QuizzeManager>().QuizzeData;
        this.id = gameQuizze.id;
        this.title = gameQuizze.title;
        this.description = gameQuizze.description;
        this.created_by = gameQuizze.created_by;
        this.number_participants = gameQuizze.number_participants;*/
    }
}
