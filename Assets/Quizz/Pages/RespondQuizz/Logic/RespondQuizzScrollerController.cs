using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using static AbstractQuizzStructure;

public class RespondQuizzScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
    private List<Answer> _data = new List<Answer>();
    public EnhancedScroller myScroller;
    public RespondQuizzCellView respondQuizzCellViewPrefab;

    public void Initialize()
    {
        myScroller.Delegate = this;
    }

    public void Reset()
    {
        myScroller.ClearAll();
        _data.Clear();
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 15f;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int
    dataIndex, int cellIndex)
    {
        RespondQuizzCellView cellView = scroller.GetCellView(respondQuizzCellViewPrefab) as
        RespondQuizzCellView;
        cellView.SetData(_data[dataIndex]);
        return cellView;
    }

    public void AddDataToScroller(Answer data)
    {
        this._data.Add(data);
        myScroller.ReloadData();
    }
}