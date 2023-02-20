using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData instance = null;


    public List<FarmData> mFarmDataList = new List<FarmData>();
    public List<FishingData> mFishingDataList = new List<FishingData>();
    public List<ZoneData> mZoneList = new List<ZoneData>();

    public List<ItemData> mItemDataList = new List<ItemData>();



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }

        }

    }

    private void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            mFarmDataList.Add(new FarmData());
        }

        mFishingDataList.Add(new FishingData());

        for (int i = 0; i < 2; i++)
        {
            mZoneList.Add(new ZoneData());
        }
    }

    public ItemData GetItemByID(int id)
    {
        for (int i = 0; i < mItemDataList.Count; i++)
        {
            if (mItemDataList[i].mId == id)
            {
                return mItemDataList[i];
            }
        }
        return null;
    }

    public void AddItem(ItemData item)
    {
        if (item.IsStackItem())
        {
            ItemData origin = this.GetItemByID(item.GetID());

            if (origin != null)
            {
                origin.AddCount();
                return;
            }
        }
        mItemDataList.Add(item);
    }

    public void RemoveItemBySlotIndex(int slotIndex)
    {
        mItemDataList.RemoveAt(slotIndex);
    }
    public void RemoveItemByItemIndex(int itemID)
    {
        for (int i = 0; i < mItemDataList.Count; i++)
        {
            if (mItemDataList[i].mId == itemID)
            {
                mItemDataList.RemoveAt(i);
                return;
            }
        }
    }

    public List<ItemData> GetInvenItemList()
    {
        return mItemDataList;
    }




}
