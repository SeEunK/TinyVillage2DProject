using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.ShaderGraph.Internal;

public class SystemMessagePopup : MonoBehaviour
{
    public TMP_Text mTxtMessage = null;
    public float mDisplayTime = 0.0f;
    public float mTimer = 0.0f;
    public bool mIsDisplay = false;

    private void Awake()
    {
        mDisplayTime = 2.0f;
        mIsDisplay = false;
    }

    private void Update()
    {
        if (mIsDisplay)
        {
            mTimer += Time.deltaTime;
            if(mTimer >= mDisplayTime)
            {
                mTimer = 0.0f;
                HideMessagePopup();
            }
        }
    }

    public void Init()
    {
        mTimer = 0.0f;
        SetMessageText(null);
        HideMessagePopup();
    }
    public void SetMessageText(string text)
    {
        mTxtMessage.text = text;
    }

    public void ShowMessage()
    {
        if (mTxtMessage != null)
        {
            this.gameObject.SetActive(true);
            mIsDisplay = true;
        }
    }

    public void HideMessagePopup()
    {
        mIsDisplay = false;
        this.gameObject.SetActive(false);
    }

    

}
