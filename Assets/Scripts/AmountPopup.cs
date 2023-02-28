using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem.Utilities;
using static ItemInfoPopup;

public class AmountPopup : MonoBehaviour
{

    public TMP_Text mTxtTitle = null; 
    public Slider mSlider = null;
    public TMP_Text mTxtAmount = null;
    public int mAmount = 0;
    public TMP_Text mTxtTotal = null;
    public Button mBtnCancel = null;
    public Button mBtnOK = null;

    private ShopData mProduct = null;
    private GameObject mCaller = null;

    private ItemData mItem = null;

    public NpcShop.ShopType mType;

    //public void SetPopupInit(GameObject callObject, ShopData product)
    public void SetPopupInit(GameObject callObject,  NpcShop.ShopType type)
    {
        mCaller = callObject;
        mType = type;

        switch (mType)
        {
            case NpcShop.ShopType.Buy:
                mProduct = callObject.GetComponent<ShopSlot>().GetProduct();
                mTxtTitle.text = mProduct.GetName();
                // 수량 제한있는 상품 타입체크 추가하자
                InitSliderMinMaxValue();
                // 개당 value 받아와서 totalValue 설정
                UpdateTotalValue(mAmount);
                break;

            case NpcShop.ShopType.Sell:
                mItem = callObject.GetComponent<ItemInfoPopup>().GetItem();
                mTxtTitle.text = mItem.mName;
                InitSliderMinMaxValue();
                UpdateSellTotalValue(mAmount);
                break;

        }
    }


    public void OnChangeAmount()
    {
        mAmount = (int)mSlider.value;
        if (mType == NpcShop.ShopType.Buy)
        {
            UpdateTotalValue(mAmount);
        }
        else
        {
            UpdateSellTotalValue(mAmount);
        }
    }

    public void UpdateTotalValue(int amount)
    {
        // 선택 수량 갱신
        mTxtAmount.text = amount.ToString();
        // 수량 * 비용 금액 갱신
        int totalValue = mProduct.GetPrice() * amount;
        mTxtTotal.text = totalValue.ToString();
    }

    public void UpdateSellTotalValue(int amount)
    {
        // 선택 수량 갱신
        mTxtAmount.text = amount.ToString();
        // 수량 * 비용 금액 갱신
        int totalValue = mItem.GetSellPrice() * amount;
        mTxtTotal.text = totalValue.ToString();
    }

    public void InitSliderMinMaxValue()
    {
        int minValue = 0;
        int maxValue = 0;

        switch (mType)
        {
            case NpcShop.ShopType.Buy:
                {
                    if (mProduct.GetProductType() == ShopData.ProductType.UnLimite)
                    {
                        minValue = 1;
                        maxValue = 100;
                    }
                    else if (mProduct.GetProductType() == ShopData.ProductType.QuantityLimit)
                    {
                        maxValue = mProduct.GetCount();
                        if (maxValue > 0)
                        {
                            minValue = 1;
                        }
                    }
                    else
                    {
                        minValue = 1;
                        maxValue = 100;
                    }
                    break;
                }

            case NpcShop.ShopType.Sell:
                {
                    minValue = 1;
                    maxValue = mItem.GetCount();
                    
                    break;
                }

        }

        mSlider.minValue = minValue;
        mSlider.maxValue = maxValue;
        mSlider.value = minValue;
        mAmount = minValue;
   
    }

   


    public int GetTotalValue()
    {
        return mProduct.GetPrice() * mAmount;
    }

    public void SetActiveAmountPopup(bool value)
    {
        this.gameObject.SetActive(value);
    }

    public void OnOkButtonClick()
    {
        // 수량 차감 
        Debug.LogFormat("amount {0}", mAmount);
        //Debug.LogFormat("mSlider.value{0}", mSlider.value);

        switch (mType)
        {
            case NpcShop.ShopType.Buy:
                ShopSlot slot = mCaller.GetComponent<ShopSlot>();
                slot.OnReduceItemCount(mAmount);

                // N개 add 하는 게 없으니 일단, 수량만큼 반복 add
                for (int i = 0; i < mAmount; i++)
                {
                    UserData.instance.AddItem(mProduct.GetItem());
                }

                //인벤 갱신
                UIManager.instance.GetInventory().UpdateInventoryList(); 
                // 유저 골드 차감
                int totalValue = GetTotalValue();
                Debug.LogFormat("totalValue {0}", totalValue);
                UserData.instance.OnUpdateGold(-totalValue);
                UIManager.instance.GetMainHud().UpdatePlayerGoldCount();


                if (slot.GetState() == ShopSlot.State.SoldOut)
                {
                    SetActiveAmountPopup(false);
                    return;
                }
                InitSliderMinMaxValue();
                UpdateTotalValue(mAmount);

                break;

           case NpcShop.ShopType.Sell:
                // sell slote 에 아이템 세팅.
                UIManager.instance.GetNpcShop().SelectSellItem(mItem, mAmount);

                // 아이템 차감
                for (int i = 0; i < mAmount; i++)
                {
                    UserData.instance.RemoveItemByItemIndex(mItem.GetID());
                }

                if (UserData.instance.GetItemByID(mItem.GetID()) != null)
                {

                    InitSliderMinMaxValue();
                }
                else
                {
                    SetActiveAmountPopup(false);
                }

                // 인벤토리 갱신
                UIManager.instance.GetInventory().UpdateInventoryList();


                break;

        }


    }
}
