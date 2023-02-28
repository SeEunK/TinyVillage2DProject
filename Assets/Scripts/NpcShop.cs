using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcShop : MonoBehaviour
{

    public Button mTabButtonBuy = null;
    public Button mTabButtonSell = null;
    public GameObject mBuyPanel= null;
    public GameObject mSellPanel= null;

    public Color mSelectedColor = new Color(82 / 255f, 56 / 255f, 31 / 255f);
    public Color mUnSelectedColor = new Color(69 / 255f, 59 / 255f, 45 / 255f);

    // buy 
    public const int SHOP_SlOT_MAX_COUNT = 15;

    public GameObject mShopContentsArea = null;
    public GameObject mSlotPrefab;

    private ShopSlot[] mItemArray = new ShopSlot[SHOP_SlOT_MAX_COUNT];

    // sell
    public const int SHOP_SELL_SlOT_MAX_COUNT = 10;

    public GameObject mShopSellContentsArea = null;
    public GameObject mSellSlotPrefab;
    public TMP_Text mTxtTotalSellPrice = null;
    public Button mBtnSellDecision = null;

    private int mTotalSellPrice = 0;
    private ShopSellSlot[] mSellItemArray = new ShopSellSlot[SHOP_SELL_SlOT_MAX_COUNT];

    public enum ShopType { Buy, Sell}

    public void Start()
    {
        mTabButtonBuy.GetComponent<Image>().color = mSelectedColor;
        mTabButtonSell.GetComponent<Image>().color = mUnSelectedColor;

        mTabButtonBuy.Select();
        OpenBuyTab();
       
    }

    public void OpenBuyTab()
    {
        mTabButtonBuy.GetComponent<Image>().color = mSelectedColor;
        mTabButtonSell.GetComponent<Image>().color = mUnSelectedColor;

        ResetSellPanel();
        mSellPanel.SetActive(false);

        mBuyPanel.SetActive(true);
        UIManager.instance.GetInventory().SetState(Inventory.State.Buy);
        UIManager.instance.GetInventory().Open();
    }

    public void OpenSellTab()
    {
        mTabButtonSell.GetComponent<Image>().color = mSelectedColor;
        mTabButtonBuy.GetComponent<Image>().color = mUnSelectedColor;
        mBuyPanel.SetActive(false);
        mSellPanel.SetActive(true);
        UIManager.instance.GetInventory().SetState(Inventory.State.Sell);
        UIManager.instance.GetInventory().Open();
    }


    public void SetShopContents()
    {
        //mShopContentsArea = this.transform.Find("ShopBuyContentPnl").gameObject;
        InitSlots();

        //mShopSellContentsArea = this.transform.Find("ShopSellContentPnl").gameObject;
        InitSellPnl();
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
                ItemData item = new ItemData(3, "사과", itemIcon, 99, 10);

                ShopData shopData = new ShopData(i, item, 100, 10, ShopData.ProductType.QuantityLimit);

                slot.SetSlot(i, shopData);
            }
            else
            {
                Sprite itemIcon = itemImages[13];
                ItemData item = new ItemData(2, "당근", itemIcon, 99, 5);

                ShopData shopData = new ShopData(i, item, 900, -1, ShopData.ProductType.UnLimite);

                slot.SetSlot(i, shopData);
            }
            mItemArray[i] = slot;
        }

    }

    //판매 패널 초기화
    private void InitSellPnl()
    {
        // 상점 판매 슬롯 초기화
        
        for(int i = 0; i < mSellItemArray.Length; i++)
        {
            GameObject slotObject = Instantiate(mSellSlotPrefab);
            RectTransform slotRT = slotObject.GetComponent<RectTransform>();
            slotRT.SetParent(mShopSellContentsArea.transform);
            slotObject.SetActive(true);
            slotObject.name = ($"shopSellSlot _ {i}");

            ShopSellSlot slot = slotObject.GetComponent<ShopSellSlot>();
            mSellItemArray[i]= slot;
            slot.Init(i);    
        }

        // 판매 가격 초기화
        mTotalSellPrice = 0;
        mTxtTotalSellPrice.text = mTotalSellPrice.ToString();
    }


    public void SelectSellItem(ItemData item, int count)
    {
        for (int i = 0; i < mSellItemArray.Length; i++)
        {

            if (mSellItemArray[i].GetItemID() == item.GetID())
            {
                mSellItemArray[i].AddSellItem(count);
                return;
            }

        }

        int index = FindEmptySellSlotIndex();
        if (index == -1)
        {
            Debug.Log("빈 슬롯이 없습니다.");
        }
        else
        {
            mSellItemArray[index].SetSellItem(item, count);
        }
    }


    // 빈슬롯 찾아서 반환
    public int FindEmptySellSlotIndex()
    {
        for (int i = 0; i < mSellItemArray.Length; i++)
        {
           
            if(mSellItemArray[i].GetItem() == null)
            {
                return mSellItemArray[i].GetIndex();
            }
         
        }
        return -1;
    }

    // 판매 아이템 개수 추가
    public void AddSellPrice(int addPrice)
    {
        mTotalSellPrice += addPrice;
        mTxtTotalSellPrice.text = mTotalSellPrice.ToString();
    }

    // 판매 하기 버튼 클릭시 호출.
    public void OnSellItems()
    {
        //골드 넣어주고.
        UserData.instance.OnUpdateGold(mTotalSellPrice);
        UIManager.instance.GetMainHud().UpdatePlayerGoldCount();
        //슬롯 초기화
        CleanSellPanel();
    }
 




    //npc 상점 팝업 
    public void SetPopupActive(bool value)
    {
        if(value == true)
        {
            UIManager.instance.GetInventory().Open();
        }
        else
        {
            ResetSellPanel();
        }
        this.gameObject.SetActive(value);
    }
    public void CloseShopPopup()
    {
        ResetSellPanel();
        UIManager.instance.GetInventory().CloseInventory();
        this.gameObject.SetActive(false);
    }

    // 판매 결정안하고 남은것 다시 유저 인벤으로 넣는다.
    public void ResetSellPanel()
    {
        for (int i = 0; i < mSellItemArray.Length; i++)
        {
            ItemData item = mSellItemArray[i].GetItem();
            if (item != null)
            {
                mSellItemArray[i].UnSetSellSlot();
            }
        }
    }


    public void CleanSellPanel()
    {
        for(int i = 0; i < mSellItemArray.Length; i++)
        {
            mSellItemArray[i].Init(i);
        }

        mTotalSellPrice = 0;
        mTxtTotalSellPrice.text = mTotalSellPrice.ToString();
    }
}
