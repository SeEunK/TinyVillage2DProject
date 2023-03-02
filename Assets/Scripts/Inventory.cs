using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    [SerializeField]
    private int mHorizontalSlotCount = 0;
    [SerializeField]
    private int mVerticalSlotCount = 0;

    public GridLayoutGroup mInventoryGridLayoutGroup = null;
    public RectTransform mContentAreaRectTransform;
    public GameObject mSlotPrefab;


    // 정렬 버튼 
    public GameObject mSortBtnObj = null;
    public TMP_Text mTxtSortBtn = null;

    public bool mIsAscending = false; // falae : 내림 차순 true : 오름 차순 구분용 

    private List<Slot> mSlotList = new List<Slot>();

    public enum State { None, Buy, Sell }
    public State mState = State.None;

    private void Awake()
    {
        mHorizontalSlotCount = mInventoryGridLayoutGroup.constraintCount;
        mVerticalSlotCount = 6;
        mState = State.None;
        mIsAscending = false;
        UpdateSortButton();
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

    public State GetState()
    {
        return mState;
    }

    public void SetState(State state)
    {
        mState = state;
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

    public void ItemSort()
    {
        // 슬롯 리스트 1개 이하 sort 진행 없음.
        if (mSlotList[0].GetItem() == null || mSlotList[1].GetItem() == null)
        {
            return;
        }
        // 내림차순
        if (mIsAscending == false)
        {
            mIsAscending = true;
            // 오름차순 버튼으로 토글
            UpdateSortButton();

            // 유저 데이터의 아이템 내림차순 정렬.
            UserData.instance.ItemSortInDesceding();

            // 정렬 변경된 유저 데이터에 맞춰서 인벤 갱신
            UpdateInventoryList();
        }
        // 오름차순
        else
        {
            mIsAscending = false;
            // 내림차순 버튼으로 토글
            UpdateSortButton();

            // 유저 데이터 아이템 오름차순 정렬 진행
            UserData.instance.ItemSortInAscending();

            // 변경된 유저 데이터에 맞춰서 인벤 갱신
            UpdateInventoryList();
        }
    }
    public void UpdateSortButton()
    {
        if (mIsAscending == false)
        {
            mTxtSortBtn.text = "Sort ▼";
        }
        else
        {
            mTxtSortBtn.text = "Sort ▲";
        }
    }

    public void Open(State state)
    {
        SetState(state);
        UpdateInventoryList();
        this.gameObject.SetActive(true);
    }

    public void CloseInventory()
    {
        SetState(State.None);
        this.gameObject.SetActive(false);
        //UIManager.instance.mInventroy.SetActive(false);
        UIManager.instance.SetItemInfoPopup(false);
    }
}
