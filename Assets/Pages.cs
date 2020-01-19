using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Pages is a class that manages GameObject/Quizz/View to show or to hide
 */
public class Pages : MonoBehaviour
{
    public  GameObject[] listofOrderedObjectToShow;
    private static int actualGameObjectShownIndex = 0;
    private static int lastShownId = 0;

    public void Awake()
    {
        for(int i = 0; i < listofOrderedObjectToShow.Length; ++i)
        {
            listofOrderedObjectToShow[i].SetActive(false);
        }

        Show(0);
    }

    private  void Show(int index)
    {
        listofOrderedObjectToShow[lastShownId].SetActive(false);
        listofOrderedObjectToShow[index].SetActive(true);
        lastShownId = index;
    }

    private  void Hide(int index)
    {
        listofOrderedObjectToShow[index].SetActive(false);
    }

    public  void ShowPrevious()
    {
        if (actualGameObjectShownIndex > 0)
            actualGameObjectShownIndex--;

        Show(actualGameObjectShownIndex);
    }

    public  void ShowNext()
    {
        if (actualGameObjectShownIndex < listofOrderedObjectToShow.Length+1)
            actualGameObjectShownIndex++;

        Show(actualGameObjectShownIndex);
    }
}
