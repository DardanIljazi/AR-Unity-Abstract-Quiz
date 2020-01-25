using UnityEngine;

/**
 * PagesManager is a class that manages all the pages to be shown or hidden and call.
 */
public class PagesManager : MonoBehaviour
{
    [Header("All pages")]
    public Page[] listOfOrderedPagesToShow;
    private static int actualShownPageIndex = 0;
    private static int lastShownPageId = 0;

    [Header("Index of the page that represents the \"Menu\"")]
    public int indexOfMenuPage = -1; // This must be defined to know 

    [Header("The page that is called when loading/work is processed in between pages")]
    public Page loadingPage;

    [Header("Those elements will be hidden when game is played")]
    public GameObject pageBaseUI;
    public GameObject pageBaseText;

    public void Start()
    {
        pageBaseUI.SetActive(false);
        pageBaseText.SetActive(false);
    }

    public void Awake()
    {
        for(int i = 0; i < listOfOrderedPagesToShow.Length; ++i)
        {
            listOfOrderedPagesToShow[i].gameObject.SetActive(false);
        }

        Show(0);
    }

    private void Show(int index)
    {
        listOfOrderedPagesToShow[lastShownPageId].gameObject.SetActive(false);
        listOfOrderedPagesToShow[index].gameObject.SetActive(true);
        listOfOrderedPagesToShow[index].pageManager.ActionToDoWhenPageShowed();
        lastShownPageId = index;
    }

    public void ShowMenuPage()
    {
        if (this.indexOfMenuPage == -1)
        {
            Debug.LogError("[WARNING]: Please defined indexOfMenuPage variable in the editor. This value represents the main menu (quizz menu for example) to show when there is error or when quizz is finished");
        }
        else
        {
            actualShownPageIndex = indexOfMenuPage;
            Show(indexOfMenuPage);
        }
    }

    private void Hide(int index)
    {
        listOfOrderedPagesToShow[index].pageManager.ActionToDoWhenPageGoingToBeHidden();
        listOfOrderedPagesToShow[index].gameObject.SetActive(false);
    }

    public void ShowPrevious()
    {
        if (actualShownPageIndex > 0)
            actualShownPageIndex--;

        Show(actualShownPageIndex);
    }

    public void ShowNext()
    {
        if (actualShownPageIndex < listOfOrderedPagesToShow.Length+1)
            actualShownPageIndex++;

        Show(actualShownPageIndex);
    }

    public void ShowLoadingPage()
    {
        listOfOrderedPagesToShow[actualShownPageIndex].gameObject.SetActive(true);
        loadingPage.gameObject.SetActive(true);
    }

    public void HideLoadingPage()
    {
        listOfOrderedPagesToShow[actualShownPageIndex].gameObject.SetActive(true);
        loadingPage.gameObject.SetActive(false);
    }
}
