using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem.Utilities;

public class AmountPopup : MonoBehaviour
{

    public TMP_Text mTxtTitle = null; 
    public Slider mSlider = null;
    public TMP_Text mTxtAmount = null;
    public float mAmount = 0.0f;
    public TMP_Text mTxtTotal = null;
    public int mPerValue = 0;
  
    public Button mBtnCancel = null;
    public Button mBtnOK = null;

    public float mMinValue = 0.0f;
    public float mMaxValue = 0.0f;

    private ItemData mSelectItem = null;

    private ShopData mProduct = null;
    private GameObject mCaller = null;




    public void SetPopupInit(GameObject callObject, string title, ShopData product)
    {
        mCaller = callObject;

        mProduct = product;
        
        mTxtTitle.text = title;

        mMinValue = 1;
        mSlider.minValue = mMinValue;

        // 수량 제한있는 타입체크 추가하자
        if (mProduct.GetProductType() == ShopData.ProductType.UnLimite)
        {
            mMaxValue = 100;
        }
        else
        {
            mMaxValue = mProduct.GetCount();
        }
        mSlider.maxValue = mMaxValue;


        mSelectItem = mProduct.GetItem();

        // 개당 value 받아와서 totalValue 설정
        mPerValue = mProduct.GetPrice();
        int totalValue = mPerValue * (int)mAmount;
        mTxtTotal.text = totalValue.ToString();
    }


    public void OnChangeAmount()
    {
        mAmount = mSlider.value;
        UpDateTotalValue((int)mAmount);
        mTxtAmount.text = mAmount.ToString();
    }

    public void UpDateTotalValue(int amount)
    {
        int totalValue = mPerValue * amount;
        mTxtTotal.text = totalValue.ToString();
    }

    public void UpdateMaxValue()
    {
        if (mProduct.GetProductType() == ShopData.ProductType.UnLimite)
        {
            return;
        }
        else if(mProduct.GetProductType() == ShopData.ProductType.QuantityLimit)
        {
            mMaxValue = mProduct.GetCount();
        }
            
        mSlider.maxValue = mMaxValue;
    }

    public void SliderReset()
    {
        mSlider.value = 1;
    
        mAmount = mSlider.value;
        mTxtAmount.text = mAmount.ToString();
        UpDateTotalValue(1);
    }

    public int GetTotalValue()
    {
        return mPerValue * (int)mAmount;
    }

    public void SetActiveAmountPopup(bool value)
    {
        this.gameObject.SetActive(value);
    }

    public void OnOkButtonClick()
    {
        // 수량 차감 
        int amount = (int)mAmount;
        ShopSlot slot = mCaller.GetComponent<ShopSlot>();
        slot.OnReduceItemCount(amount);
        UpdateMaxValue();

        // N개 add 하는 게 없으니 일단, 수량만큼 반복 add
        for (int i = 0; i < amount; i++)
        {
            UserData.instance.AddItem(mSelectItem);
        }
        // 유저 골드 차감
        UserData.instance.OnUpdateGold(false, GetTotalValue());

        if (slot.GetState() == ShopSlot.State.SoldOut)
        {
            SetActiveAmountPopup(false);
        }
        // 슬라이더 다시 1로 초기화
        SliderReset();
    }
}
