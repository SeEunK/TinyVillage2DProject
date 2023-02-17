using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public List<Item> mItemList = new List<Item>();

    
    public const int INVEN_SlOT_MAX_COUNT = 20;
    public Slot[] mSlotArray = new Slot[INVEN_SlOT_MAX_COUNT];



}
