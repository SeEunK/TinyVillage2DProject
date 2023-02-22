using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcShop : MonoBehaviour
{
    public const int SHOP_SlOT_MAX_COUNT = 15;

    public GameObject mShopContentsArea = null;
    public GameObject mSlotPrefab;

    private ShopSlot[] mItemArray = new ShopSlot[SHOP_SlOT_MAX_COUNT];



    public void SetShopContents()
    {
        mShopContentsArea = this.transform.Find("ShopContentPnl").gameObject;
        InitSlots();
    }
    // ���� ����Ʈ ���� �Լ�
 

    // ���� ���� �ʱ�ȭ
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

            // [�ӽ�] �������� ��ǰ ������ ���� 
            if (i == 0)
            {
                Sprite itemIcon = itemImages[2];
                ItemData item = new ItemData(3, "���", itemIcon, 99);

                ShopData shopData = new ShopData(i, item, 100, 3, ShopData.ProductType.QuantityLimit);

                slot.SetSlot(i, shopData);
            }
            else
            {
                Sprite itemIcon = itemImages[13];
                ItemData item = new ItemData(2, "���", itemIcon, 99);

                ShopData shopData = new ShopData(i, item, 900, -1, ShopData.ProductType.UnLimite);

                slot.SetSlot(i, shopData);
            }
            mItemArray[i] = slot;
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
