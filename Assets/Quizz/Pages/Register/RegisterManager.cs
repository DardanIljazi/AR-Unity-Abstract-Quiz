using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AbstractQuizzStructure;

public class RegisterManager : PageLogic
{
    public Button registerButton;
    public Button backButton;
    public InputField[] listOfInputs;

    public void Start()
    {
        registerButton.onClick.AddListener(RegisterButtonClicked);
        backButton.onClick.AddListener(BackButtonClicked);
    }

    public void RegisterButtonClicked()
    {
        HideRegisterButton();
        Dictionary<string, string> inputNameWithValue = new Dictionary<string, string>();

        foreach(InputField input in listOfInputs)
        {
            if (input.text == "" || input.text.Length == 0)
            {
                PopupManager.PopupAlert("Empty input", "The input " + input.placeholder.GetComponent<Text>().text + " is empty, please fill it");
                return;
            }

            inputNameWithValue.Add(input.placeholder.GetComponent<Text>().text, input.text);
        }


        // The parameters name that are sent over http. for example pseudo={xxx}&password={xxx}. --> pseudo and password are parameters name. 
        // They have to be set in class inherited from Apimanager if the default param name doesn't correspond to what API waits
        string pseudoOrEmailParamName   = GameManager.Instance.GetApiManager().GetPseudoParamName(); // By default it's "pseudo".
        string passwordParamName        = GameManager.Instance.GetApiManager().GetPasswordParamName(); // By default it's "password".
        string firstNameParamName       = GameManager.Instance.GetApiManager().GetFirstNameParamName(); // By default it's "firstname".
        string lastNameParamName        = GameManager.Instance.GetApiManager().GetLastNameParamName(); // By default it's "lastname".
        string emailParamName           = GameManager.Instance.GetApiManager().GetEmailParamName(); // By default it's "email".

        // These pairs of key and values are the 
        var post_key_values = new Dictionary<string, string>
        {
            { pseudoOrEmailParamName, inputNameWithValue["Pseudo"] },
            { firstNameParamName, inputNameWithValue["Firstname"] },
            { lastNameParamName, inputNameWithValue["Lastname"] },
            { emailParamName, inputNameWithValue["Email"] },
            { passwordParamName, inputNameWithValue["Password"] },
        };

        if (!GameManager.Instance.GetApiManager().RegisterUserToGetApiToken(post_key_values))
        {
            Debug.LogError("[WARNING]: registration did not work !");
            PopupManager.PopupAlert("Error", NetworkRequestManager.lastHttpWebRequestErrorMessage);
            ShowRegisterButton();
            return;
        }

        PopupManager.PopupAlert("Registered", "Successfully registered! Now go to connection page", "Go to login", GameManager.Instance.pagesManager.ShowLoginPage);
    }

    void ShowRegisterButton()
    {
        registerButton.gameObject.SetActive(true);
    }

    void HideRegisterButton()
    {
        registerButton.gameObject.SetActive(false);
    }

    public void BackButtonClicked()
    {
        Debug.Log("BackButton");
        GameManager.Instance.pagesManager.GoBack();
    }
}
