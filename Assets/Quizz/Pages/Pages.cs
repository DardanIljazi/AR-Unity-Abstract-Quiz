using UnityEngine;

/**
 * Pages is a class that manages GameObject/Quizz/View to show or to hide
 */
public class Pages : MonoBehaviour
{
    public  GameObject[] listofOrderedObjectToShow;
    private static int actualGameObjectShownIndex = 0;
    private static int lastShownId = 0;

    public RectTransform pageBase; // The object that will old the place of pages
    private GameObject lastParent;


    // Those 2 elements are only to have something "visual" in unity editor. They will be hidden when code is launched
    [Header("Destined to be hidden when code launched (only here for visual)")]
    public GameObject pageBaseUI;
    public GameObject pageBaseText; 

    public void Start()
    {
        pageBaseUI.SetActive(false);
        pageBaseText.SetActive(false);
    }

    public void Awake()
    {
        for(int i = 0; i < listofOrderedObjectToShow.Length; ++i)
        {
            listofOrderedObjectToShow[i].SetActive(false);
        }

        Show(0);
    }

    private  void Show(int index)
    {
        listofOrderedObjectToShow[lastShownId].SetActive(false);
        listofOrderedObjectToShow[index].SetActive(true);
        lastShownId = index;
    }

    public void ShowFirstPage()
    {
        actualGameObjectShownIndex = 0;
        Show(0);
    }

    private  void Hide(int index)
    {
        listofOrderedObjectToShow[index].SetActive(false);
    }

    public  void ShowPrevious()
    {
        if (actualGameObjectShownIndex > 0)
            actualGameObjectShownIndex--;

        Show(actualGameObjectShownIndex);
    }

    public  void ShowNext()
    {
        if (actualGameObjectShownIndex < listofOrderedObjectToShow.Length+1)
            actualGameObjectShownIndex++;

        Show(actualGameObjectShownIndex);
    }
}
