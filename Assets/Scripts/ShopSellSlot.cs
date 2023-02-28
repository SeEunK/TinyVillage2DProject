using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ShopSellSlot : MonoBehaviour
{
    [SerializeField]
    private int mIndex;
    [SerializeField]
    private ItemData mItem; //팔려고 넣은 아이템

    [SerializeField]
    private GameObject mCountBg;
    [SerializeField]
    private int mItemCount;
    [SerializeField]
    private TMP_Text mTextItemCount;
    [SerializeField]
    private Image mItemImage;

    public NpcShop.ShopType mType = NpcShop.ShopType.Sell;
    public void Init(int index)
    {
        mIndex = index;
        mItem = null;
        Sprite emptyIcon = Resources.Load<Sprite>("Sprites/emptySellSlot");
        mItemImage.sprite = emptyIcon;
        mItemCount = 0;
        mTextItemCount.text = null;
        mCountBg.SetActive(false);
    }
    //슬롯에 아이템 설정.
    public void SetSellItem(ItemData item, int count)
    {
        
        this.mItem = item;
        this.mItemImage.sprite = mItem.mImage;
        AddSellItem(count);
    }

    // 아이템 설정
    public void AddSellItem(int count)
    {
        this.mItemCount += count;
        if (mItemCount > 1)
        {
            mTextItemCount.text = mItemCount.ToString();
            mCountBg.SetActive(true);
        }

        int sellprice = mItem.GetSellPrice() *count;
        UIManager.instance.GetNpcShop().AddSellPrice(sellprice);

    }
   
    public int GetItemCount()
    {
        return mItemCount;
    }

    public ItemData GetItem()
    {
        if(mItemCount == 0)
        {
            return null;
        }
        return mItem;
    }

    public int GetItemID()
    {
        if(mItem != null)
        {
           return mItem.GetID();
                
        }
        return -1;   
    }

    public void UnSetSellSlot()
    {
        if(mItem == null)
        {
            return;
        }
        ItemData temp = mItem;
        //판매금 차감
        int sellprice = temp.GetSellPrice()*mItemCount;
        UIManager.instance.GetNpcShop().AddSellPrice(-sellprice);
      
        // 유저 데이터에 다시 넣고
        for (int i = 0; i < mItemCount; i++)
        {
            UserData.instance.AddItem(temp);
        }
        // 슬롯 초기화.
        Init(mIndex);
    }

    public int GetIndex()
    {
        return mIndex;
    }

}
