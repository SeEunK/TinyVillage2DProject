using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData instance = null;


    public const int FARM_OBJECT_MAX_COUNT = 6;
    public const int FISHING_POINT_MAX_COUNT = 1;
    public const int LOGGING_OBJECT_MAX_COUNT = 6;
    public const int MINNING_OBJECT_MAX_COUNT = 6;
    public const int GATHERING_OBJECT_MAX_COUNT = 2;
    public const int ZONE_MAX_COUNT = 2;

    public List<FarmData> mFarmDataList = new List<FarmData>();
    public List<FishingData> mFishingDataList = new List<FishingData>();
    public List<ZoneData> mZoneList = new List<ZoneData>();
    public List<MiningData> mMiningDataList = new List<MiningData>();
    public List<LoggingData> mLoggingDataList = new List<LoggingData>();
    public List<GatherData> mGatherDataList = new List<GatherData>();

    public List<ItemData> mItemDataList = new List<ItemData>();

    [SerializeField]
    private int mGold = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            mGold = 1000000;

            {// Å×½ºÆ®¿ë ¾¾¾Ñ Áö±Þ
                Sprite[] itemImages = Resources.LoadAll<Sprite>("Sprites/Icon");
                Sprite itemIcon = itemImages[1];
                ItemData getItem = new ItemData(10, "¾¾¾Ñ", itemIcon, 99, 100);
                for (int i = 0; i < 5; i++)
                {
                    AddItem(getItem);
                }
            }

            {// Å×½ºÆ®¿ë ¹Ì³¢ Áö±Þ
                Sprite[] itemImages = Resources.LoadAll<Sprite>("Sprites/Icon");
                Sprite itemIcon = itemImages[16];
                ItemData getItem = new ItemData(11, "¹Ì³¢", itemIcon, 99, 50);

                for (int i = 0; i < 5; i++)
                {
                    AddItem(getItem);
                }
            }
           
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
        for (int i = 0; i < FARM_OBJECT_MAX_COUNT; i++)
        {
            mFarmDataList.Add(new FarmData());
            
        }

        for (int i = 0; i < FISHING_POINT_MAX_COUNT; i++)
        {
            mFishingDataList.Add(new FishingData());
        }

        for (int i = 0; i < MINNING_OBJECT_MAX_COUNT; i++)
        {
            mMiningDataList.Add(new MiningData());
        }

        for (int i = 0; i < LOGGING_OBJECT_MAX_COUNT; i++)
        {
            mLoggingDataList.Add(new LoggingData());
        }

        for (int i = 0; i < GATHERING_OBJECT_MAX_COUNT; i++)
        {
            mGatherDataList.Add(new GatherData());
        }

        for (int i = 0; i < ZONE_MAX_COUNT; i++)
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

    public int GetGold()
    {
        return mGold;
    }
    public void OnUpdateGold(int value)
    {
        mGold += value;
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
                if (mItemDataList[i].mCount > 1)
                {
                    mItemDataList[i].ReduceCount();
                }
                else
                {
                    mItemDataList.RemoveAt(i);
                }
                return;
            }
        }
    }

    public List<ItemData> GetInvenItemList()
    {
        return mItemDataList;
    }


    public void ItemSortInDesceding()
    {
        mItemDataList.Sort(delegate (ItemData A, ItemData B)
        {
                if (A.mCount < B.mCount) return 1;
                else if (A.GetCount() > B.GetCount()) return -1;
                return 0;

        });
    }

    public void ItemSortInAscending()
    {
        mItemDataList.Sort(delegate (ItemData A, ItemData B)
        {
            if (A.mCount < B.mCount) return -1;
            else if (A.GetCount() > B.GetCount()) return 1;
            return 0;

        });
    }
}
