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


    // ���� ��ư 
    public GameObject mSortBtnObj = null;
    public TMP_Text mTxtSortBtn = null;

    public bool mIsAscending = false; // falae : ���� ���� true : ���� ���� ���п� 

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
        // ���� ����Ʈ 1�� ���� sort ���� ����.
        if (mSlotList[0].GetItem() == null || mSlotList[1].GetItem() == null)
        {
            return;
        }
        // ��������
        if (mIsAscending == false)
        {
            mIsAscending = true;
            // �������� ��ư���� ���
            UpdateSortButton();

            // ���� �������� ������ �������� ����.
            UserData.instance.ItemSortInDesceding();

            // ���� ����� ���� �����Ϳ� ���缭 �κ� ����
            UpdateInventoryList();
        }
        // ��������
        else
        {
            mIsAscending = false;
            // �������� ��ư���� ���
            UpdateSortButton();

            // ���� ������ ������ �������� ���� ����
            UserData.instance.ItemSortInAscending();

            // ����� ���� �����Ϳ� ���缭 �κ� ����
            UpdateInventoryList();
        }
    }
    public void UpdateSortButton()
    {
        if (mIsAscending == false)
        {
            mTxtSortBtn.text = "Sort ��";
        }
        else
        {
            mTxtSortBtn.text = "Sort ��";
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
