using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShopData;

public class ShopSlot : MonoBehaviour
{
    [SerializeField]
    private int mIndex;
    private ShopData mProduct;
    [SerializeField]
    private GameObject mCountBg;
    [SerializeField]
    private TMP_Text mTextItemCount;
    [SerializeField]
    private Image mItemImage;
    [SerializeField]
    private TMP_Text mTextPrice;

    public enum State { None, SoldOut}
    public State mState = State.None;
    public void SetSlot(int index, ShopData product)
    {
        this.mIndex = index;
        this.mProduct = product;
        
    }

    private void Start()
    {
        mState = State.None;
        mItemImage.sprite = mProduct.GetItem().mImage;
        UpdateItemCount();
        SetPriceText();
    }

    public void SetPriceText()
    {
        mTextPrice.text = mProduct.GetPrice().ToString();
                    
    }

    public ShopData GetProduct()
    {
        return mProduct;
    }

    public State GetState()
    {
        return mState;
    }

    public void UpdateItemCount()
    {
        ShopData.ProductType type  = mProduct.GetProductType();

        switch (type)
        {
            case ProductType.UnLimite:
                mCountBg.SetActive(false);
                mTextItemCount.enabled = false;
                break;

            case ProductType.QuantityLimit:
                if (mProduct.GetCount() > 0)
                {
                    mTextItemCount.text = mProduct.GetCount().ToString();
                    //AmountPopup amountPopup = UIManager.instance.GetAmountPopup();
                    //amountPopup.UpdateMaxValue();

                }
                else if (mProduct.GetCount() == 0)
                {
                    mTextItemCount.color = Color.red;
                    mTextItemCount.text = "Sold Out";
                    mState = State.SoldOut;


                }
                break;
        }
      
    }
    public void OnReduceItemCount(int value)
    {
       
        switch (mProduct.GetProductType())
        {
            case ProductType.UnLimite:
                break;
            case ProductType.QuantityLimit:
                if (mState != State.SoldOut)
                {
                    mProduct.mCount -= value;
                    UpdateItemCount();
                }
                else
                {
                    return;
                }
                break;
        }
        
    }

    public void ShopSlotClick()
    {
        Debug.LogFormat("click {0}", mIndex);

        if (mState == State.SoldOut)
        {
            Debug.LogFormat("click {0} item SOLD OUT!", mIndex);
            return;
        }

        AmountPopup amountPopup = UIManager.instance.GetAmountPopup();

        string pupupTitle = mProduct.GetItem().mName;
        amountPopup.SetPopupInit(this.gameObject, pupupTitle, mProduct);
        
        UIManager.instance.SetAmountPopup(true);
    }
}
