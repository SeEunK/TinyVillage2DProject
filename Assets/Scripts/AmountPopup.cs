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
    public int mAmount = 0;
    public TMP_Text mTxtTotal = null;
    public Button mBtnCancel = null;
    public Button mBtnOK = null;

    private ShopData mProduct = null;
    private GameObject mCaller = null;

    public void SetPopupInit(GameObject callObject, ShopData product)
    {
        mCaller = callObject;
        mProduct = product;
        mTxtTitle.text = product.GetName();

        // ���� �����ִ� Ÿ��üũ �߰�����
        InitSliderMinMaxValue();
        // ���� value �޾ƿͼ� totalValue ����
        UpdateTotalValue(mAmount);

    }


    public void OnChangeAmount()
    {
        mAmount = (int)mSlider.value;
        UpdateTotalValue(mAmount);
    }

    public void UpdateTotalValue(int amount)
    {
        // ���� ���� ����
        mTxtAmount.text = amount.ToString();
        // ���� * ��� �ݾ� ����
        int totalValue = mProduct.GetPrice() * amount;
        mTxtTotal.text = totalValue.ToString();
    }

    public void InitSliderMinMaxValue()
    {
        int minValue = 0;
        int maxValue = 0;

        if (mProduct.GetProductType() == ShopData.ProductType.UnLimite)
        {
            minValue = 1; 
            maxValue = 100;
        }
        else if(mProduct.GetProductType() == ShopData.ProductType.QuantityLimit)
        {
            maxValue = mProduct.GetCount();
            if(maxValue > 0)
            {
                minValue = 1;
            }
        }
        else
        {
            minValue = 1; 
            maxValue = 100;
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
        // ���� ���� 
        Debug.LogFormat("amount {0}", mAmount);
        //Debug.LogFormat("mSlider.value{0}", mSlider.value);

        ShopSlot slot = mCaller.GetComponent<ShopSlot>();
        slot.OnReduceItemCount(mAmount);

        // N�� add �ϴ� �� ������ �ϴ�, ������ŭ �ݺ� add
        for (int i = 0; i < mAmount; i++)
        {
            UserData.instance.AddItem(mProduct.GetItem());
        }
        // ���� ��� ����
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
      
    }
}
