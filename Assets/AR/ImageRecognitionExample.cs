using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using static AbstractQuizzStructure;

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
            SelectQuizzInIndex(trackedImage.referenceImage.name);
        }
    }

    public void SelectQuizzInIndex(object imageNameAsAQuizzId)
    {
        // Here we create a new Quizz and define its id (
        Quizz quizzToShow = new Quizz();
        quizzToShow.SetQuizzId(imageNameAsAQuizzId);

        GameManager.Instance.selectQuizzManager.SelectQuizzToShow(quizzToShow);
    }
}
