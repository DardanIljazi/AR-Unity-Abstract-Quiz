using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageLogic : MonoBehaviour, IPage
{
    virtual public void ActionToDoWhenPageGoingToBeHidden() { }
    virtual public void ActionToDoWhenPageShowed() { }
}
