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
                    // 사용 효과 반영 (일단, hp 증감 효과만 존재) 
                    UserData.instance.OnUpdateHp(mItem.mHpValue);

                    // main hud 갱신
                    UIManager.instance.GetMainHud().UpdatePlayerHpBar(UserData.instance.GetHp(), UserData.instance.GetMaxHp());

                    // 시스템 메시지 출력
                    if (mItem.mHpValue > 0)
                    {
                        UIManager.instance.SetSystemMessage(string.Format("{0} 아이템을 사용하여, HP가 {1} 만큼 회복되었습니다.", mItem.mName, mItem.mHpValue));
                    }
                    else
                    {
                        UIManager.instance.SetSystemMessage(string.Format("{0} 아이템을 사용하여, HP가 {1} 만큼 차감되었습니다.", mItem.mName, mItem.mHpValue));
                    }

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

                    // HP가 
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
