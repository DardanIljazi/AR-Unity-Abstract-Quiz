using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * LoadingManager is the manager for the Loading page (used when something has to be loaded/calculated...)
 */
public class LoadingManager : PageLogic
{
    public Text loadingText;
    string originalText = "";

    void Start()
    {
        originalText = loadingText.text;
    }

    public override void ActionToDoWhenPageShowed()
    {
        StartLoading();
    }

    public override void ActionToDoWhenPageGoingToBeHidden()
    {
        StopLoading();
    }

    public void StartLoading()
    {
        StartCoroutine(Loading());
    }

    string actualLoadingPoints = "";
    int numberOfLoadingsPoints = 0;
    bool continueLoading = true;
    private IEnumerator Loading()
    {
        while (continueLoading) {

            yield return new WaitForSeconds(1);
            if (numberOfLoadingsPoints >= 3)
            {
                actualLoadingPoints = "";
                numberOfLoadingsPoints = 0;
            }

            actualLoadingPoints += ".";
            numberOfLoadingsPoints++;
            loadingText.text = originalText + actualLoadingPoints;
         }
    }

    public void StopLoading()
    {
        continueLoading = false;
        StopCoroutine(Loading());
    }
   
}
