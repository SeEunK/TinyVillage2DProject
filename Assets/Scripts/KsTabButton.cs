using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class KsTabButton : MonoBehaviour
{
    public UnityEvent onTabSelected;
    public UnityEvent onTabUnselected;

    public TabController mTabController = null;

    public Button mButton = null;

    public void Select()
    {
        if (onTabSelected != null)
        {
            mButton.Select();
            onTabSelected.Invoke();
        }
    } 

    public void UnSelect()
    {
        if (onTabUnselected != null)
        {
            
            onTabUnselected.Invoke();
        }
    }

    public void OnSelectTab(KsTabButton button)
    {
        mTabController.SelectButton(button);
    }
}
