using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;


public class SelectQuizzScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
    private List<SelectQuizzData> _data;
    public EnhancedScroller myScroller;
    public SelectQuizzCellView animalCellViewPrefab;

    public void Initialize()
    {
        _data = new List<SelectQuizzData>();
        myScroller.Delegate = this;
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
        SelectQuizzCellView cellView = scroller.GetCellView(animalCellViewPrefab) as
        SelectQuizzCellView;
        cellView.SetData(_data[dataIndex]);
        return cellView;
    }

    public void AddDataToScroller(SelectQuizzData data)
    {
        Debug.Log("data.title: " + data.title);
        this._data.Add(data);
        myScroller.ReloadData();
    }
}