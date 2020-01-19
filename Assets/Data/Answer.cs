// Davide Carboni
// Class answer to use for the Answer Game Object

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Answer : MonoBehaviour
{
    public string name;
    public bool value;
    public bool clicked = false;

    public void Init(ApiData.Answer answer)
    {
        if (answer != null)
        {
            this.name = answer.name;
            this.value = answer.value;
        }
    }
}
