using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour, IPage
{
    virtual public void ActionToDoWhenPageGoingToBeHidden() { }
    virtual public void ActionToDoWhenPageShowed() { }
}
