using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using TMPro;
using EnhancedScrollerDemos.GridSelection;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static AbstractQuizzStructure;

public class RespondQuizzCellView : EnhancedScrollerCellView, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    public BoxCollider boxCollider; // Used for the raycast
    public Answer _data;

    public Text textObject;

    public void SetData(Answer data)
    {
        Debug.Log("SETDATA");
        Debug.Log(JsonUtility.ToJson(_data));
        _data = data;
        ReloadData();
    }

    public void ReloadData()
    {
        textObject.text = _data.GetDataToShowAsPossibleAnswer();
    }

    public void Update()
    {
        if (transform.hasChanged)
        {
            boxCollider.size = GetComponent<RectTransform>().rect.size;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("OnPointerUp");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        Debug.Log(JsonUtility.ToJson(_data));
        GameManager.Instance.respondQuizzManager.ResponseSelected(_data);
    }
}

