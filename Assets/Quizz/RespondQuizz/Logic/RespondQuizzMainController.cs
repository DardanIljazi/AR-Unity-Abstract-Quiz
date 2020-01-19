using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/**
 * Main controller for the Select Quizz view (manages logic from view with api) 
 */
public class RespondQuizzMainController : MonoBehaviour
{
    
    public Api api;
    public RespondQuizzScrollerController respondQuizzScrollerController;
    public RespondQuizzTouchController respondQuizzTouchController;

    public Text quizzQuestion;
    public Image quizzImage;


    public SelectQuizzMainController selectQuizzMainController;

    private ApiData.GameQuizze gameQuizze;


    private int numberOfQuestions = 0;
    private int actualQuestionArrayIndex = 0;
    private string goodAnswer;
    private int rightResponses = 0;
    private int falseResponses = 0;

    public Pages pages;

    public void Start()
    {
        respondQuizzScrollerController.Initialize();
        respondQuizzTouchController.Initialize();

        gameQuizze = api.getGameQuizzeFromAPI("5c4ebb440eab7e0004ffcb76" /*selectQuizzMainController.selectedQuizzNumber*/);

        numberOfQuestions = gameQuizze.questions.Count;

        LoadQuestionAndPossibleResponses(0);
    }


    public void LoadQuestionAndPossibleResponses(int arrayIndex)
    {
        respondQuizzScrollerController.Reset();
        respondQuizzTouchController.Reset();

        quizzQuestion.text = gameQuizze.questions[arrayIndex].question;

        foreach (ApiData.Answer answer in gameQuizze.questions[arrayIndex].answers)
        {
            if (answer.value == true)
                goodAnswer = answer.name;


            RespondQuizzData respondQuizzData = new RespondQuizzData();
            respondQuizzData.SetDataToShowInCellView(answer.name);

            respondQuizzScrollerController.AddDataToScroller(respondQuizzData);
        }

    } 

    private static RespondQuizzMainController singleton;
    public static RespondQuizzMainController getInstance()
    {
        if (singleton == null)
        {
            singleton = new RespondQuizzMainController();
        }

        return singleton;
    }


    public void SaveResponseAndGoToNext(string response)
    {
        if (goodAnswer.Equals(response))
        {
            rightResponses++;
        }
        else
        {
            falseResponses++;
        }

        actualQuestionArrayIndex++;
            
        if (actualQuestionArrayIndex <= numberOfQuestions - 1)
        {
            LoadQuestionAndPossibleResponses(actualQuestionArrayIndex);
        }
        else
        {
            Finished();
        }
    } 

    public void Finished()
    {
        Debug.Log("Finished");
        Debug.Log("Résultat (right response: " + rightResponses + ", bad response: " + falseResponses + ")");
        pages.ShowNext();
    }

    /**
     * Delegates called from inside of RespondQuizzTouchController
     */
    public void ResponseSelected(string response)
    {
        Debug.Log("Response Selected: " + response);
        SaveResponseAndGoToNext(response);
    }

    public void downloadImage(string url, string pathToSaveImage)
    {
        WWW www = new WWW(url);
        StartCoroutine(_downloadImage(www, pathToSaveImage));
    }

    private IEnumerator _downloadImage(WWW www, string savePath)
    {
        yield return www;

        //Check if we failed to send
        if (string.IsNullOrEmpty(www.error))
        {
            UnityEngine.Debug.Log("Success");

            //Save Image
            saveImage(savePath, www.bytes);
        }
        else
        {
            UnityEngine.Debug.Log("Error: " + www.error);
        }
    }

    void saveImage(string path, byte[] imageBytes)
    {
        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        try
        {
            File.WriteAllBytes(path, imageBytes);
            Debug.Log("Saved Data to: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Save Data to: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }

    byte[] loadImage(string path)
    {
        byte[] dataByte = null;

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Debug.LogWarning("Directory does not exist");
            return null;
        }

        if (!File.Exists(path))
        {
            Debug.Log("File does not exist");
            return null;
        }

        try
        {
            dataByte = File.ReadAllBytes(path);
            Debug.Log("Loaded Data from: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Data from: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        return dataByte;
    }
}
