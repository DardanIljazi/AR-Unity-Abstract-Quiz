using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ApiData;

/**
 * ConnectionManager is the manager for the Connection page (used when user has to connect to the API and receive token API for example)
 */
public class ConnectionManager : PageLogic
{
    public Button connectButton;
    public Button registerButton;
    public InputField pseudoInput;
    public InputField passwordInput;

    void Start()
    {
        connectButton.onClick.AddListener(ConnectButtonClicked);
        registerButton.onClick.AddListener(RegisterButtonClicked);
    }

    void ConnectButtonClicked()
    {
        ApiToken apiTokenData = GameManager.Instance.apiManager.ConnectToQuizz(pseudoInput.text, passwordInput.text);

        if (apiTokenData == null)
        {
            Debug.LogError("[WARNING]: apiTokenData is null");
            return;
        }
        else
        {
            GameManager.Instance.apiManager.apiTokenManager.SetApiToken(apiTokenData);
        }


        if (!GameManager.Instance.apiManager.apiTokenManager.IsApiTokenDefined() && 
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
            GameManager.Instance.pagesManager.GoToPage("ScanQrCodePlease");
        }
    }

    void RegisterButtonClicked()
    {
        GameManager.Instance.pagesManager.GoToPage("Register");
    }
}
