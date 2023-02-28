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
    // 아이템 정보
    public TMP_Text mTxtItemName = null;
    public Image mImgItem = null;
    public TMP_Text mTxtCount = null;
    public TMP_Text mTxtDescription = null;

    public ItemData mItem = null;
    // 버튼 
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
                    // 사용 효과 item 에 넣어두고 효과 적용필요


                    // 아이템 차감
                    UserData.instance.RemoveItemByItemIndex(mItem.GetID());

                    if (UserData.instance.GetItemByID(mItem.GetID()) != null)
                    {
                        // 아이템 정보창 갱신
                        UpdateItemInfo(mItem, mItem.GetCount(), PopupType.Inven);
                    }
                    else
                    {
                        SetActiveItemInfoPopup(false);
                    }
                    // 인벤토리 갱신
                    UIManager.instance.GetInventory().UpdateInventoryList();
                }

                break;

            case PopupType.Shop:
                {
                    int selectCount = 1;
                    if (mItem.IsStackItem() && mItem.GetCount() > 1)
                    {
                        SetActiveItemInfoPopup(false);
                        //수량 선택 팝업 오픈
                        AmountPopup amountPopup = UIManager.instance.GetAmountPopup();
                        amountPopup.SetPopupInit(this.gameObject, NpcShop.ShopType.Sell);

                        UIManager.instance.SetAmountPopup(true);
                    }
                    else
                    {
                        // sell slote 에 아이템 세팅.
                        UIManager.instance.GetNpcShop().SelectSellItem(mItem, selectCount);

                        // 아이템 차감
                        UserData.instance.RemoveItemByItemIndex(mItem.GetID());

                        if (UserData.instance.GetItemByID(mItem.GetID()) != null)
                        {

                            // 아이템 정보창 갱신
                            UpdateItemInfo(mItem, mItem.GetCount(), PopupType.Shop);
                        }
                        else
                        {
                            SetActiveItemInfoPopup(false);
                        }

                        // 인벤토리 갱신
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
