using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RespondQuizzData : ApiData.Answer
{
    public string GetDataToShowInCellView()
    {
        return name;
    }

    public void SetDataToShowInCellView(string data)
    {
        base.name = data;
    }
}
