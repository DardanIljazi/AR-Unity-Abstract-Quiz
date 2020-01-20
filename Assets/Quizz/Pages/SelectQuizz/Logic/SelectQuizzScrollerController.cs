using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;


public class SelectQuizzScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
    [Header("Link to the instance of scroller (contains all cells of data)")]
    public EnhancedScroller enhancedScroller;
    [Header("Link to the prefab to show for each data in scroller")]
    public SelectQuizzCellView selectQuizzCellView;

    private List<SelectQuizzData> _data;

    public void Initialize()
    {
        _data = new List<SelectQuizzData>();
        enhancedScroller.Delegate = this;
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
        SelectQuizzCellView cellView = scroller.GetCellView(selectQuizzCellView) as
        SelectQuizzCellView;
        cellView.SetData(_data[dataIndex]);
        return cellView;
    }

    public void AddDataToScroller(SelectQuizzData data)
    {
        _data.Add(data);
        enhancedScroller.ReloadData();
    }
}