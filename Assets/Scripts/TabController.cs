using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabController : MonoBehaviour
{

    public TabButton mSelectedTabButton = null;

    private void Start()
    {
        SelectButton(mSelectedTabButton);

    }
    public void SelectButton(TabButton button)
    {
        if(mSelectedTabButton != null)
        {
            mSelectedTabButton.UnSelect();
        }

        mSelectedTabButton = button;
        mSelectedTabButton.Select();
    }


}
