using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectQuizzTouchController : MonoBehaviour
{
    Ray ray;
    RaycastHit raycastHit;

    bool touched = false;
    int lastTapCount = 0;
    bool blockTouchOrClick = false;


    public SelectQuizzMainController selectQuizzMainController; // Reference to the main controller (so we can pass him delegates)

    delegate void DelegateOnQuizzCellTouched(string quizzNumber);
    DelegateOnQuizzCellTouched OnQuizzCellTouched;

    public void Initialize()
    {
        OnQuizzCellTouched += selectQuizzMainController.QuizzSelected; // Call QuizzSelected method from SelectQuizzMainController when a cell is clicked/touchced
    }

    public void Start()
    {

        if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.OSXEditor ||
            Application.platform == RuntimePlatform.LinuxEditor)
        {
            Debug.Log("This code is launched on a computer");
        }
        else // Phones (and maybe other platform not taken into account)
        {
            Debug.Log("This code is CERTAINLY launched on a phone");
        }

    }


    void Update()
    {
        if (blockTouchOrClick)
            return;

        
        if (Application.platform == RuntimePlatform.WindowsEditor || 
            Application.platform == RuntimePlatform.OSXEditor || 
            Application.platform == RuntimePlatform.LinuxEditor)
        {
            touched = Input.GetMouseButton(0);
            if (touched)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            }
        }
        else // Phones (and maybe other platform not taken into account)
        {
            if (Input.GetTouch(0).tapCount != lastTapCount)
            {
                ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                lastTapCount = Input.GetTouch(0).tapCount;
                touched = true;
            }
            else
            {
                touched = false;
            }
        }


        if (touched && Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
        {
            Debug.Log("Clicked a quizz");
            SelectQuizzCellView selectQuizzCellView = raycastHit.collider.gameObject.GetComponentInParent<SelectQuizzCellView>();

            if (selectQuizzCellView != null)
            {
                blockTouchOrClick = true;
                // Call the methods
                OnQuizzCellTouched(selectQuizzCellView._data.id);
            }
        }
    }
}
