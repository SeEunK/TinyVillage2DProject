using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public const int INVEN_SlOT_MAX_COUNT = 20;
   

    [SerializeField]
    private int mHorizontalSlotCount = 0;
    [SerializeField]
    private int mVerticalSlotCount = 0;

    public GridLayoutGroup mInventoryGridLayoutGroup = null;
    public RectTransform mContentAreaRectTransform;
    public GameObject mSlotPrefab;

    private List<Slot> mSlotList = new List<Slot>();
    private Slot[] mSlotArray = new Slot[INVEN_SlOT_MAX_COUNT];

    private void Awake()
    {
        mHorizontalSlotCount = mInventoryGridLayoutGroup.constraintCount;
        mVerticalSlotCount = 4;
        InitSlots();

    }

    public void UpdateInventoryList()
    {
        for (int i = 0; i < mSlotList.Count; i++)
        {
            if (i < UserData.instance.mItemDataList.Count)
            {
                mSlotList[i].SetItem(UserData.instance.mItemDataList[i]);
            }
            else
            {
                mSlotList[i].SetItem(null);
            }

        }
    }

    

    public void ItemRemove(int index)
    {
        ItemData item = mSlotList[index].GetItem();
      

        if (item.GetCount() > 1)
        {
            item.ReduceCount();
        }
        else
        {
            mSlotList[index] = null;
        }

    }

    private void InitSlots()
    {
        for (int j = 0; j < mVerticalSlotCount; j++)
        {

            for (int i = 0; i < mHorizontalSlotCount; i++)
            {
                int slotIndex = (j * mHorizontalSlotCount) + i;
                GameObject slotObject = Instantiate(mSlotPrefab);
                RectTransform slotRT = slotObject.GetComponent<RectTransform>();
                slotRT.SetParent(mContentAreaRectTransform);
                slotObject.SetActive(true);
                slotObject.name = ($"slot _ {slotIndex}");

                Slot slot = slotObject.GetComponent<Slot>();
                slot.SetIndex(slotIndex);

                mSlotList.Add(slot);
            }
        }

    }


    public void CloseInventory()
    {
        UIManager.instance.mInventroy.SetActive(false);
    }
}
