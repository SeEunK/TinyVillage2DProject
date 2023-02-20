using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public void SetSlot(int index, ShopData product)
    {
        this.mIndex = index;
        this.mProduct = product;
    }

    private void Start()
    {
        mItemImage.sprite = mProduct.GetItem().mImage;
        UpdateItemCount();
        SetPriceText();
    }

    public void SetPriceText()
    {
        mTextPrice.text = mProduct.GetPrice().ToString();
                    
    }

    public void UpdateItemCount()
    {
       if(mProduct.GetCount() > 0)
        {
            mTextItemCount.text = mProduct.GetCount().ToString();
        }
        else if(mProduct.GetCount() == 0)
        {
            mTextItemCount.text = "Sold Out";
        }
       else
        {
            mCountBg.SetActive(false);
            mTextItemCount.enabled= false;
        }
    }

    public void ShopSlotClick()
    {
        Debug.LogFormat("click {0}", mIndex);
    }
}
