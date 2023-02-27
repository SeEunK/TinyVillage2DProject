using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcShop : MonoBehaviour
{
    // buy 
    public const int SHOP_SlOT_MAX_COUNT = 15;

    public GameObject mShopContentsArea = null;
    public GameObject mSlotPrefab;

    private ShopSlot[] mItemArray = new ShopSlot[SHOP_SlOT_MAX_COUNT];

    // sell
    public const int SHOP_SELL_SlOT_MAX_COUNT = 10;

    public GameObject mShopSellContentsArea = null;
    public GameObject mSellSlotPrefab;

    private ShopSellSlot[] mSellItemArray = new ShopSellSlot[SHOP_SELL_SlOT_MAX_COUNT];


    public void SetShopContents()
    {
        //mShopContentsArea = this.transform.Find("ShopBuyContentPnl").gameObject;
        InitSlots();
        
        //mShopSellContentsArea = this.transform.Find("ShopSellContentPnl").gameObject;
        InitSellSlots();
    }
    // 상점 리스트 갱신 함수
 

    // 상점 구매 슬롯 초기화
    private void InitSlots()
    {
        for (int i = 0; i < mItemArray.Length; i++)
        {
            GameObject slotObject = Instantiate(mSlotPrefab);
            RectTransform slotRT = slotObject.GetComponent<RectTransform>();
            slotRT.SetParent(mShopContentsArea.transform);
            slotObject.SetActive(true);
            slotObject.name = ($"shopSlot _ {i}");

            ShopSlot slot = slotObject.GetComponent<ShopSlot>();

            Sprite[] itemImages = Resources.LoadAll<Sprite>("Sprites/Icon");

            // [임시] 수량제한 상품 세팅을 위해 
            if (i == 0)
            {
                Sprite itemIcon = itemImages[2];
                ItemData item = new ItemData(3, "사과", itemIcon, 99);

                ShopData shopData = new ShopData(i, item, 100, 10, ShopData.ProductType.QuantityLimit);

                slot.SetSlot(i, shopData);
            }
            else
            {
                Sprite itemIcon = itemImages[13];
                ItemData item = new ItemData(2, "당근", itemIcon, 99);

                ShopData shopData = new ShopData(i, item, 900, -1, ShopData.ProductType.UnLimite);

                slot.SetSlot(i, shopData);
            }
            mItemArray[i] = slot;
        }

    }

    private void InitSellSlots()
    {
        for(int i = 0; i < mSellItemArray.Length; i++)
        {
            GameObject slotObject = Instantiate(mSellSlotPrefab);
            RectTransform slotRT = slotObject.GetComponent<RectTransform>();
            slotRT.SetParent(mShopSellContentsArea.transform);
            slotObject.SetActive(true);
            slotObject.name = ($"shopSellSlot _ {i}");

            ShopSellSlot slot = slotObject.GetComponent<ShopSellSlot>();

            mSellItemArray[i]= slot;
        }
    }

    public void SetPopupActive(bool value)
    {
        this.gameObject.SetActive(value);
    }
    public void CloseShopPopup()
    {
        this.gameObject.SetActive(false);
        
    }
}
