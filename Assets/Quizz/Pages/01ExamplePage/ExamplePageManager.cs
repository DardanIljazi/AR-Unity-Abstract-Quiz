using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * ExamplePageManager is an example manager for the Example page
 * You can copy this code as a base for other pages
 * All pages must be referenced in PagesManager class (from editor)
 * Each page is derived from Page and each page manager from PageManager (without 's')
 */
public class ExamplePageManager : PageLogic
{
    public Text exampleText;
    string originalText = "";

    void Start()
    {
        originalText = exampleText.text;
    }

    public override void ActionToDoWhenPageShowed() // This method is called from PageSManager.Show(..) 
    {

    }

    public override void ActionToDoWhenPageGoingToBeHidden() // This method is called from PageSManager.Hide(...)
    {

    }
}

