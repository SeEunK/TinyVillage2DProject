using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
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
                if (item.mHpValue != 0)
                {
                    mTxtBtnText.text = "USE";
                    mBtnObj.SetActive(true);
                }
                else 
                { 
                    mBtnObj.SetActive(false); 
                }
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
                    // ��� ȿ�� �ݿ� (�ϴ�, hp ���� ȿ���� ����) 
                    UserData.instance.OnUpdateHp(mItem.mHpValue);

                    // main hud ����
                    UIManager.instance.GetMainHud().UpdatePlayerHpBar(UserData.instance.GetHp(), UserData.instance.GetMaxHp());

                    // �ý��� �޽��� ���
                    if (mItem.mHpValue > 0)
                    {
                        UIManager.instance.SetSystemMessage(string.Format("{0} �������� ����Ͽ�, HP�� {1} ��ŭ ȸ���Ǿ����ϴ�.", mItem.mName, mItem.mHpValue));
                    }
                    else
                    {
                        UIManager.instance.SetSystemMessage(string.Format("{0} �������� ����Ͽ�, HP�� {1} ��ŭ �����Ǿ����ϴ�.", mItem.mName, mItem.mHpValue));
                    }

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

                    // HP�� 
                    if (UserData.instance.GetHp() <= 0)
                    {
                        Debug.Log("player die");
                    }
                }

                break;

            case PopupType.Shop:
                {
                    int selectCount = 1;
                    if (mItem.IsStackItem() && mItem.GetCount() > 1)
                    {
                        SetActiveItemInfoPopup(false);
                        //���� ���� �˾� ����
                        AmountPopup amountPopup = UIManager.instance.GetAmountPopup();
                        amountPopup.SetPopupInit(this.gameObject, NpcShop.ShopType.Sell);

                        UIManager.instance.SetAmountPopup(true);
                    }
                    else
                    {
                        // sell slote �� ������ ����.
                        UIManager.instance.GetNpcShop().SelectSellItem(mItem, selectCount);

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
                }
                break;
        }

    }

    public ItemData GetItem()
    {
        return mItem;
    }

    public void SetActiveItemInfoPopup(bool value)
    {
        this.gameObject.SetActive(value);
    }
}
