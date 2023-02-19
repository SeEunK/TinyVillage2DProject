using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData 
{
    public int mId;
    public string mName;
    public Sprite mImage;
    public int mMaxStackCount;
    public int mCount;

    public ItemData(int index, string name, Sprite image,  int MaxStackCount)
    {
        mId = index; 
        mName = name; 
        mImage = image;
        mMaxStackCount = MaxStackCount;
        mCount = 1;
    }

 
    public int GetID()
    {
        return mId;
    }

    public int GetCount()
    {
        return mCount;  
    }
    public void AddCount()
    {
        mCount++;
    }
    public void ReduceCount()
    {
        mCount--;
    }

    public int MaxStackCount()
    {
        return mMaxStackCount;
    }

    public bool IsStackItem()
    {
        if (mMaxStackCount > 1) 
        { 
            return true; 
        }
        else 
        { 
            return false; 
        }
    }

}
