using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using static ApiData;

public class RespondQuizzScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
    private List<AnswerQuizzData> _data;
    public EnhancedScroller myScroller;
    public RespondQuizzCellView respondQuizzCellViewPrefab;

    public void Initialize()
    {
        _data = new List<AnswerQuizzData>();
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

    public void AddDataToScroller(AnswerQuizzData data)
    {
        this._data.Add(data);
        myScroller.ReloadData();
    }
}