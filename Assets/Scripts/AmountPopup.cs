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

        // ���� �����ִ� Ÿ��üũ �߰�����
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

        // ���� value �޾ƿͼ� totalValue ����
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
        // ���� ���� 
        int amount = (int)mAmount;
        ShopSlot slot = mCaller.GetComponent<ShopSlot>();
        slot.OnReduceItemCount(amount);
        UpdateMaxValue();

        // N�� add �ϴ� �� ������ �ϴ�, ������ŭ �ݺ� add
        for (int i = 0; i < amount; i++)
        {
            UserData.instance.AddItem(mSelectItem);
        }
        // ���� ��� ����
        UserData.instance.OnUpdateGold(false, GetTotalValue());

        if (slot.GetState() == ShopSlot.State.SoldOut)
        {
            SetActiveAmountPopup(false);
        }
        // �����̴� �ٽ� 1�� �ʱ�ȭ
        SliderReset();
    }
}
