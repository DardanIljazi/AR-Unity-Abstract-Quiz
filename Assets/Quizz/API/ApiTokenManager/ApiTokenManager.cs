using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ApiData;

public class ApiTokenManager : MonoBehaviour
{
    private ApiToken apiToken = null;

    public bool IsApiTokenDefined()
    {
        return apiToken != null;
    }

    public string GetApiToken()
    {
        if (apiToken == null)
        {
            Debug.LogError("[WARNING]: apiTokenData is equal to null !");
        }

        return apiToken.GetApiToken();
    }

    public void SetApiToken(ApiToken apiToken)
    {
        this.apiToken = apiToken;
    }
}
