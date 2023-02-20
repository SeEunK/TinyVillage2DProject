using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopData
{
    
  public int mProductId;
  public ItemData mItem;
  public int mPrice;
  public int mCount; // -1 : 수량 제한 없음. || 1 ~  : 수량 제한

    public ShopData(int productId,  ItemData item,  int price, int count)
    {
        this.mProductId = productId;
        this.mItem = item;
        this.mPrice = price;
        this.mCount = count;
    }

    public int GetCount()
    {
        return mCount;
    }
    public int GetPrice()
    {
        return mPrice;
    }

    public ItemData GetItem()
    {
        return mItem;
    }

}
