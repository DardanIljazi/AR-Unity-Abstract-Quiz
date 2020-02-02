using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AbstractQuizzStructure;

/**
 * LoginManager is the manager for the Login page (used when user has to connect to the API and receive token API for example)
 */
public class LoginManager : PageLogic
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

    public override void ActionToDoWhenPageShowed()
    {
        if (!GameManager.Instance.GetApiManager().CanRegisterAsUserToApi())
        {
            registerButton.gameObject.SetActive(false);
        }
        else
        {
            registerButton.gameObject.SetActive(true);
        }
    }

    void ConnectButtonClicked()
    {
        // The parameters name that are sent over http. for example pseudo={xxx}&password={xxx}. --> pseudo and password are parameters name. 
        // They have to be set in class inherited from Apimanager if the default param name doesn't correspond to what API waits
        string pseudoOrEmailParamName   = GameManager.Instance.GetApiManager().GetPseudoParamName(); // By default it's "pseudo".
        string passwordParamName        = GameManager.Instance.GetApiManager().GetPasswordParamName(); // By default it's "password".

        var post_key_values = new Dictionary<string, string>
        {
            { pseudoOrEmailParamName , pseudoInput.text },
            { passwordParamName, passwordInput.text }
        };

        ApiToken apiTokenData = GameManager.Instance.GetApiManager().LoginToGetApiToken(post_key_values);

        if (apiTokenData == null)
        {
            Debug.LogError("[WARNING]: apiTokenData is null");
            return;
        }
        else
        {
            GameManager.Instance.GetApiManager().SetApiToken(apiTokenData.GetApiToken());
        }

        if (!GameManager.Instance.GetApiManager().IsApiTokenDefined() && 
            NetworkRequestManager.lastHttpWebRequestErrorMessage == null)
        {
            PopupManager.PopupAlert("Connection impossible", "Your login or password are not correct");
            passwordInput.text = "";
        }
        else if (GameManager.Instance.GetApiManager().GetApiToken() == null && NetworkRequestManager.lastHttpWebRequestErrorMessage != null)
        {
            PopupManager.PopupAlert("Connection impossible", "Error:" + NetworkRequestManager.lastHttpWebRequestErrorMessage);
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
