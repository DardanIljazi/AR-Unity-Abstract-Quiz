using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ImageRecognitionExample : MonoBehaviour
{
    public ARTrackedImageManager aRTrackedImageManager;

    public void OnEnable()
    {
        aRTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }
    public void OnDisable()
    {
        aRTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage trackedImage in args.added)
        {
            Debug.Log("name: " +trackedImage.referenceImage.name);
            Debug.Log("transform:" + trackedImage.transform);

            Debug.Log("Entered !!");
            SelectQuizzInIndex(trackedImage.referenceImage.name);
            
        }
    }

    // Will be deleted lated, just for the show
    public void SelectQuizzInIndex(string imageWithNumber)
    {
        Debug.Log("Selected quizz !!");

        GameManager.Instance.pagesManager.GoToPage("SelectQuizz");
    }
}
