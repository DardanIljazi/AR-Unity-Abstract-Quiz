using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespondQuizzTouchController : MonoBehaviour
{
    Ray ray;
    RaycastHit raycastHit;

    bool touched = false;
    int lastTapCount = 0;
    bool blockTouchOrClick = false;


    public RespondQuizzMainController RespondQuizzMainController; // Reference to the main controller (so we can pass him delegates)

    delegate void DelegateOnResponseCellTouched(string quizzNumber);
    DelegateOnResponseCellTouched OnResponseCellTouched;

    public void Initialize()
    {
        OnResponseCellTouched += RespondQuizzMainController.ResponseSelected; // Call QuizzSelected method from RespondQuizzMainController when a cell is clicked/touchced
    }

    public void Reset()
    {
        touched = false;
        lastTapCount = 0;
        blockTouchOrClick = false;
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
            RespondQuizzCellView RespondQuizzCellView = raycastHit.collider.gameObject.GetComponentInParent<RespondQuizzCellView>();

            if (RespondQuizzCellView != null)
            {
                blockTouchOrClick = true;
                // Call the methods
                OnResponseCellTouched(RespondQuizzCellView._data.name);
            }
        }
    }
}
