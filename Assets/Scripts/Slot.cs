using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{
    [SerializeField]
    private Image mSlotBg;
    [SerializeField]
    private Image mItemImage;
    private ItemData mItem;
    [SerializeField]
    private TMP_Text mTextItemCount;

    private int mIndex;
   
    public void SetIndex(int index)
    {
        mIndex = index;
    }
    public int GetIndex()
    {
        return mIndex;
    }

    public ItemData GetItem()
    {
        return mItem;
    }

    public void SetItem(ItemData item)
    {
        mItem = item;

        if(mItem != null)
        {
            SetItemCount(mItem.GetCount());
            mItemImage.sprite = item.mImage;
            mItemImage.color = new Color(1, 1, 1, 1);
            mSlotBg.color = new Color(1, 1, 1, 1);
            SetItemCount(mItem.GetCount());

        }
        else // 빈슬롯
        {
            mItemImage.sprite = null;
            mItemImage.color = new Color(1, 1, 1, 0);
            mSlotBg.color = new Color32(137, 137, 137, 255);
            SetItemCount(0);
        }
    }


    public bool HasItem()
    {
        if (mItem != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void RemoveItem()
    {
        mItemImage.sprite = null;
        HideItemCount();
    }

    public void SetItemCount(int count)
    {
        mTextItemCount.text = count.ToString();

        if (count > 1)
        {
            ShowItemCount();
        }
        else
        {
            HideItemCount();
        }
    }

    public void ShowItemCount()
    {
        mTextItemCount.enabled = true;
    }
    public void HideItemCount()
    {
        mTextItemCount.enabled = false;
    }

  

    public void OnSelectItem()
    {
        if (mItem != null)
        {
            Debug.LogFormat("OnSelectItem : {0}", mItem.mName);

            Inventory.State state = UIManager.instance.GetInventory().GetState();

            // 가방 --> 아이템 정보창 여는 경우
            switch (state)
            {
                case Inventory.State.None:
                    UIManager.instance.GetItemInfoPopup().UpdateItemInfo(mItem, mItem.GetCount(), ItemInfoPopup.PopupType.Inven);
                    UIManager.instance.SetItemInfoPopup(true);
                    break;
                case Inventory.State.Buy:
                    UIManager.instance.GetItemInfoPopup().UpdateItemInfo(mItem, mItem.GetCount(), ItemInfoPopup.PopupType.Reward);
                    UIManager.instance.SetItemInfoPopup(true);
                    break;

                case Inventory.State.Sell:
                    UIManager.instance.GetItemInfoPopup().UpdateItemInfo(mItem, mItem.GetCount(), ItemInfoPopup.PopupType.Shop);
                    UIManager.instance.SetItemInfoPopup(true);
                    break;

            }

        }

        Debug.Log("OnSelectItem");

    }
}
