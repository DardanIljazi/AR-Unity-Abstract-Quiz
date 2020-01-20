using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class FinishQuizzManager : MonoBehaviour
{
    public Text textObject;
    public void SetFinalScore(int rightResponses, int falseResponses, int numberOfQuestions)
    {
        textObject.text = textObject.text.Replace("%1", rightResponses.ToString()).Replace("%2", falseResponses.ToString());
    }
}
