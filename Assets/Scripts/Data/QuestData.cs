using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestData
{
    public enum QuestConditionType { MonsterKill, Fishing, Farming }
    public QuestConditionType mConditionType;

    public string mName;
    public int mTotalCount;
    public bool mIsRewarded;

    public QuestData(string name, QuestConditionType type, int totalCount) 
    {
        this.mName= name;
        this.mConditionType = type;
        this.mTotalCount = totalCount;
        SetRewarded (false);
    }

    public void SetRewarded(bool value)
    {
        mIsRewarded = value;
    }
    public bool IsRewarded()
    {
        return mIsRewarded;
    }
    public int GetTotalCount()
    {
        return mTotalCount;
    }



}
