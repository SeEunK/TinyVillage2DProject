using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPopup : MonoBehaviour
{
    
    public enum PopupType { Inven, Shop , Reward}
    public PopupType mType;
    // ������ ����
    public TMP_Text mTxtItemName = null;
    public Image mImgItem = null;
    public TMP_Text mTxtCount = null;
    public TMP_Text mTxtDescription = null;

    public ItemData mItem = null;
    // ��ư 
    public GameObject mBtnObj = null;
    public TMP_Text mTxtBtnText = null;

    


    public void UpdateItemInfo(ItemData item,int count, PopupType type)
    {
        mItem = item;
        mType = type;
        mTxtItemName.text = mItem.mName;
        mImgItem.sprite = mItem.mImage;
        mTxtDescription.text = "item desc!!!!!!!!!!!!!!!!";

        if (mItem.IsStackItem())
        {
            mTxtCount.text = count.ToString();
        }
        else
        {
            mTxtCount.enabled = false;
        }

        switch (mType)
        {
            case PopupType.Inven:
                mTxtBtnText.text = "USE";
                mBtnObj.SetActive(true);
                break;

            case PopupType.Shop:
                mTxtBtnText.text = "SELL";
                mBtnObj.SetActive(true);
                break;

            case PopupType.Reward:
                mBtnObj.SetActive(false);
                break;
        }
    }


    public void OnClickButton() 
    { 

        switch (mType)
        {
            case PopupType.Inven:
                {
                    // ��� ȿ�� item �� �־�ΰ� ȿ�� �����ʿ�


                    // ������ ����
                    UserData.instance.RemoveItemByItemIndex(mItem.GetID());

                    if (UserData.instance.GetItemByID(mItem.GetID()) != null)
                    {
                        // ������ ����â ����
                        UpdateItemInfo(mItem, mItem.GetCount(), PopupType.Inven);
                    }
                    else
                    {
                        SetActiveItemInfoPopup(false);
                    }
                    // �κ��丮 ����
                    UIManager.instance.GetInventory().UpdateInventoryList();
                }

                break;

            case PopupType.Shop:
                {
                    // ������ �ǸŰ��� �ʿ� 

                    // ������ ����
                    UserData.instance.RemoveItemByItemIndex(mItem.GetID());

                    if (UserData.instance.GetItemByID(mItem.GetID()) != null)
                    {
                        // ������ ����â ����
                        UpdateItemInfo(mItem, mItem.GetCount(), PopupType.Shop);
                    }
                    else
                    {
                        SetActiveItemInfoPopup(false);
                    }

                    // �κ��丮 ����
                    UIManager.instance.GetInventory().UpdateInventoryList();
                }
                break;
        }

    }

    public void SetActiveItemInfoPopup(bool value)
    {
        this.gameObject.SetActive(value);
    }
}
