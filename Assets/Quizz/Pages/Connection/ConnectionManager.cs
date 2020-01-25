using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ApiData;

/**
 * ConnectionManager is the manager for the Connection page (used when user has to connect to the API and receive token API for example)
 */
public class ConnectionManager : PageManager
{
    public Button connectButton;
    public InputField pseudoInput;
    public InputField passwordInput;

    void Start()
    {
        connectButton.onClick.AddListener(ConnectButtonClicked);
    }

    void ConnectButtonClicked()
    {
        ApiTokenData apiTokenData = GameManager.Instance.apiManager.ConnectToQuizz(pseudoInput.text, passwordInput.text);

        if (apiTokenData == null)
        {
            Debug.LogError("[WARNING]: apiTokenData is null");
            return;
        }
        else
        {
            GameManager.Instance.apiManager.apiTokenManager.SetApiTokenData(apiTokenData);
        }


        if (!GameManager.Instance.apiManager.apiTokenManager.IsApiTokenDataDefined() && 
            ApiManager.lastHttpWebRequestErrorMessage == null)
        {
            PopupManager.PopupAlert("Connection impossible", "Your login or password are not correct");
            passwordInput.text = "";
        }
        else if (GameManager.Instance.apiManager.apiTokenManager.GetApiToken() == null && ApiManager.lastHttpWebRequestErrorMessage != null)
        {
            PopupManager.PopupAlert("Connection impossible", "Error:" + ApiManager.lastHttpWebRequestErrorMessage);
        }
        else
        {
            GameManager.Instance.pagesManager.ShowNext();
        }
    }
}
