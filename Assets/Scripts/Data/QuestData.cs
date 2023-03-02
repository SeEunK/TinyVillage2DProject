using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestData
{
    public enum QuestConditionType { MonsterKill, Fishing, Farming ,Mining ,Logging }
    public QuestConditionType mConditionType;

    public enum RewardType { Gold, Item}

    public string mName;
    public int mTotalCount;
    public RewardType mRewardType;
    public ItemData mReward = null; //mRewardType : Gold 일땐 null.
    public int mReawardCount;

    public bool mIsRewarded;

    public QuestData(string name, QuestConditionType type, int totalCount, RewardType rewardType, ItemData rewardItem, int rewardCount) 
    {
        this.mName= name;
        this.mConditionType = type;
        this.mTotalCount = totalCount;

        // 보상 정보
        this.mRewardType = rewardType;
        this.mReward = rewardItem;
        this.mReawardCount = rewardCount;
        // 보상 수령 여부 
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
