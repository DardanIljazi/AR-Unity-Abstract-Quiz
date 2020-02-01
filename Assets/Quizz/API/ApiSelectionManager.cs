using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiSelectionManager : MonoBehaviour
{
    public ApiManager apiToUse;

    void Start()
    {
        if (apiToUse == null)
        {
            Debug.LogError("[WARNING]: Please select which API to use from \"API\" gameObject. Drag and drop an api from a child object of \"Apis\" into Api To Use");
            return;
        }
        GameManager.Instance.SetApiToUse(apiToUse);
    }
}
