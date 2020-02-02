using UnityEngine;

/**
 * PagesManager is a class that manages all the pages to be shown or hidden and call.
 */
public class PagesManager : MonoBehaviour
{
    [Header("All pages")]
    public Page[] listOfOrderedPagesToShow;
    private static int actualShownPageIndex = -1;
    private static int lastShownPageId = -1;

    [Header("Index of the page that represents the \"Menu\"")]
    public int indexOfMenuPage = -1; // This must be defined to know 

    [Header("Index of the page that represents the \"Login\"")]
    public int indexOfLoginPage = -1; // This must be defined to know 


    [Header("The page that is called when loading/work is processed in between pages")]
    public Page loadingPage;

    [Header("Those elements will be hidden when game is played")]
    public GameObject examplePage;

    public void Start()
    {
        examplePage.SetActive(false);
    }

    public void Awake()
    {
        for(int i = 0; i < listOfOrderedPagesToShow.Length; ++i)
        {
            listOfOrderedPagesToShow[i].gameObject.SetActive(false);
        }
    }

    private void Show(int index)
    {
        if (actualShownPageIndex != -1)
        {
            listOfOrderedPagesToShow[actualShownPageIndex].gameObject.SetActive(false);
            lastShownPageId = actualShownPageIndex;
        }

        listOfOrderedPagesToShow[index].gameObject.SetActive(true);
        actualShownPageIndex = index;

        listOfOrderedPagesToShow[index].pageLogic.ActionToDoWhenPageShowed();

    }

    public void ShowMenuPage()
    {
        Debug.Log("ShowMenupage");
        if (this.indexOfMenuPage == -1)
        {
            Debug.LogError("[WARNING]: Please defined indexOfMenuPage variable in the editor. This value represents the main menu (quizz menu for example) to show when there is error or when quizz is finished");
        }
        else
        {
            Show(indexOfMenuPage);
        }
    }

    public void ShowLoginPage()
    {
        Debug.Log("ShowLoginPage");
        if (this.indexOfLoginPage == -1)
        {
            Debug.LogError("[WARNING]: Please defined indexOfLoginPage variable in the editor. This value represents the login page to show when user has to log in when launching the software or after registering");
        }
        else
        {
            Show(indexOfLoginPage);
        }
    }

    public void ShowFirstPage()
    {
        Show(0);
    }

    private void Hide(int index)
    {
        listOfOrderedPagesToShow[index].pageLogic.ActionToDoWhenPageGoingToBeHidden();
        listOfOrderedPagesToShow[index].gameObject.SetActive(false);
    }

    public void ShowLoadingPage()
    {
        if (actualShownPageIndex > -1)
            listOfOrderedPagesToShow[actualShownPageIndex].gameObject.SetActive(false);

        loadingPage.gameObject.SetActive(true);
    }

    public void HideLoadingPage()
    {
        if (actualShownPageIndex > -1)
            listOfOrderedPagesToShow[actualShownPageIndex].gameObject.SetActive(true);
        loadingPage.gameObject.SetActive(false);
    }

    public void GoToPage(string pageName)
    {
        bool found = false;
        int index = 0;

        for (int pageIndex = 0; pageIndex < listOfOrderedPagesToShow.Length; pageIndex++)
        {
            if (listOfOrderedPagesToShow[pageIndex].pageName == pageName)
            {
                found = true;
                index = pageIndex;
                break;
            }
        }

        if (!found)
        {
            Debug.LogError("[WARNING]: pageName was not found in the listOfOrderedPagesToShow");
            return;
        }

        Show(index);
    }

    public void GoBack()
    {
        Show(lastShownPageId);
    }
}
