using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ImageRecognitionExample : MonoBehaviour
{
    public ARTrackedImageManager aRTrackedImageManager;

    void Start()
    {
        Debug.Log("ImageRecognitionExample");
    }

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
        }
    }


    void Update()
    {
        
    }
}
