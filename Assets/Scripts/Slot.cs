using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{
    [SerializeField]
    private Image mItemImage;
    private Item mItem;


    public Item GetItem()
    {
        return mItem;
    }

    public void SetItem(Item item)
    {
        mItem = item;

        if(mItem != null)
        {
            mItemImage.sprite = item.mImage;
            mItemImage.color = new Color(1, 1, 1, 1);

        }
        else
        {
            mItemImage.color = new Color(1, 1, 1, 0);
        }
    }


    
}
