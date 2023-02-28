using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabController : MonoBehaviour
{

    public KsTabButton mSelectedTabButton = null;

    private void Start()
    {
        SelectButton(mSelectedTabButton);

    }
    public void SelectButton(KsTabButton button)
    {
        if(mSelectedTabButton != null)
        {
            mSelectedTabButton.UnSelect();
        }

        mSelectedTabButton = button;
        mSelectedTabButton.Select();
    }


}
