using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanQrCodePleaseManager : PageLogic
{
    public Button goToQuizzListButton;

    void Start()
    {
        goToQuizzListButton.onClick.AddListener(ButtonGoToQuizzListClicked);
    }   

    public override void ActionToDoWhenPageGoingToBeHidden()
    {
        
    }

    public override void ActionToDoWhenPageShowed()
    {
        
    }

    public void ButtonGoToQuizzListClicked()
    {
        GameManager.Instance.pagesManager.GoToPage("SelectQuizz");
    }
}
