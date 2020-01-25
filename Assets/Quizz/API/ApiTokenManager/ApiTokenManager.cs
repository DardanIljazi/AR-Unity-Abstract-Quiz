﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ApiData;

public class ApiTokenManager : MonoBehaviour
{
    private ApiTokenData apiTokenData = null;

    public bool IsApiTokenDataDefined()
    {
        return apiTokenData != null;
    }

    public string GetApiToken()
    {
        if (apiTokenData == null)
        {
            Debug.LogError("[WARNING]: apiTokenData is equal to null !");
        }

        return apiTokenData.GetApiToken();
    }

    public void SetApiToken(string apiToken)
    {
        apiTokenData.SetApiToken(apiToken);
    }

    public void SetApiTokenData(ApiTokenData apiTokenData)
    {
        this.apiTokenData = apiTokenData;
    }
}
