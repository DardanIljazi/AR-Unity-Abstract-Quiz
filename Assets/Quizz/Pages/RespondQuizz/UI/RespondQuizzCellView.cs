using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using TMPro;
using EnhancedScrollerDemos.GridSelection;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static ApiData;

public class RespondQuizzCellView : EnhancedScrollerCellView, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    public BoxCollider boxCollider; // Used for the raycast
    public AnswerQuizzData _data;

    public Text textObject;

    public void SetData(AnswerQuizzData data)
    {
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
        GameManager.Instance.respondQuizzManager.ResponseSelected(_data);
    }
}

