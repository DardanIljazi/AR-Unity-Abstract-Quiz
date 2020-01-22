﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using TMPro;
using EnhancedScrollerDemos.GridSelection;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static ApiData;

// For click: https://stackoverflow.com/questions/53517667/unity-ar-how-can-i-trigger-a-button-in-the-scene
public class SelectQuizzCellView : EnhancedScrollerCellView, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    public BoxCollider boxCollider; // Used for the raycast
    public QuizzData _data;
    
    /**
     * All elements to modify when new data come
     */
    public Text textObject;


    public void SetData(QuizzData data)
    {
        _data = data;
        ReloadData();
    }

    public void ReloadData()
    {
        textObject.text = _data.title;
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
        //Debug.Log("OnPointerClick");
        GameManager.Instance.selectQuizzManager.QuizzSelected(_data);
    }
}

