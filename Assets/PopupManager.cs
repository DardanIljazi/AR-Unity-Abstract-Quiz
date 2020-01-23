using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour, ICloneable
{
    public GameObject baseGameObject;
    public Text popupTitle;
    public Text popupText;
    public Button popupButton;
    public Text popupButtonText;

    public delegate void OnClickCallback();
    public OnClickCallback onClickCallback;

    // Start is called before the first frame update
    void Awake()
    {
    }

    public void ShowError(string title, string text, string buttonText = null, OnClickCallback callbackClickFunc=null)
    {
        popupButton.onClick.AddListener(ButtonClicked);

        this.popupTitle.text = title;
        this.popupText.text = text;
        if (buttonText != null)
            this.popupButtonText.text = buttonText;

        this.baseGameObject.SetActive(true);

        if(callbackClickFunc != null)
            onClickCallback += callbackClickFunc;

        Debug.Log(onClickCallback);
    }

    public void ButtonClicked()
    {
        Debug.Log(onClickCallback);
        onClickCallback?.Invoke();
        Debug.Log(popupText.text);
        baseGameObject.gameObject.SetActive(false);
        Destroy(this);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public static void PopupAlert(string title, string text, string buttonText = null, OnClickCallback callbackClickFunc=null)
    {
        (GameManager.Instance.popupManager.Clone() as PopupManager).ShowError(title, text, buttonText, callbackClickFunc);
    }

}
