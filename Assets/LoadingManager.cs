using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Text loadingText;
    string originalText = "";
    void Start()
    {
        originalText = loadingText.text;
        StartLoading();
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
            }

            actualLoadingPoints += ".";
            numberOfLoadingsPoints++;
            loadingText.text = originalText + actualLoadingPoints;
         }
    }

    public void StopLoading()
    {
        continueLoading = false;
    }
}
