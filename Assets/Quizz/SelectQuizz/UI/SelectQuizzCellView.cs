using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using TMPro;
using EnhancedScrollerDemos.GridSelection;
using UnityEngine.UI;

public class SelectQuizzCellView : EnhancedScrollerCellView
{
    public BoxCollider boxCollider; // Used for the raycast
    public SelectQuizzData _data;

    /**
     * All elements to modify when new data come
     */
    public Text textObject;

    public void SetData(SelectQuizzData data)
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
}

