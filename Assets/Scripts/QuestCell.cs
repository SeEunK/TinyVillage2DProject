using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestCell : MonoBehaviour
{
    // 데이터로 분리해야되는 값
    const int REWARD_GOLD = 1000;


    public TMP_Text mTxtTitle = null;

    public TMP_Text mTxtDescription = null;
    public TMP_Text mTxtCount = null;

    // 보상 버튼 
    public GameObject mObjReward = null;
    public TMP_Text mTxtRewardCount = null;

    // 완료 오브젝트
    public GameObject mObjComplete = null;

    // 데이터
    public QuestData mData = null;


    public void UpdateQuestCell(QuestData.QuestConditionType type)
    {
        mData = QuestManager.instance.GetQuestData(type);
        // 진행중인 퀘스트가 있는 경우
        if(mData != null)
        {
            int accCount = QuestManager.instance.GetAccCount(type);
            if(accCount > mData.mTotalCount)
            {
                accCount = mData.mTotalCount;
            }

            // 제목
            mTxtTitle.text = this.GetTitle(type);

            // 정보
            mTxtDescription.gameObject.SetActive(true);
            mTxtDescription.text = mData.mName;

            mTxtCount.gameObject.SetActive(true);
            mTxtCount.text = string.Format("({0}/{1})", accCount, mData.mTotalCount);

            mObjReward.SetActive(true);
            mTxtRewardCount.text = REWARD_GOLD.ToString();
            // 클리어 해서 보상을 받을 수 있을때
            if (accCount == mData.mTotalCount)
            {
                
            }
            // 아직 보상을 받을 수 없을때
            else
            {

            }

            mObjComplete.SetActive(false);
        }
        // 모두 완료한 경우
        else
        {
            // 제목
            mTxtTitle.text = this.GetTitle(type);

            // 정보
            mTxtDescription.gameObject.SetActive(false);
            mTxtCount.gameObject.SetActive(false);

            // 리워드 버튼
            mObjReward.SetActive(false);

            // 완료 텍스트
            mObjComplete.SetActive(true);
        }
    }

    public string GetTitle(QuestData.QuestConditionType type)
    {
        switch(type)
        {
            case QuestData.QuestConditionType.MonsterKill: { return "Monster Kill"; }
            case QuestData.QuestConditionType.Fishing: { return "Fishing"; }
            case QuestData.QuestConditionType.Farming: { return "Farming"; }
        }
        return "Unknown";
    }

    public void OnReward()
    {
        // 데이터가 있는 경우에만 동작
        if(mData != null)
        {
            // 누적값과 비교값이 크거나 같을때 리워드를 보상 받을 수 있다고 판단
            int accCount = QuestManager.instance.GetAccCount(mData.mConditionType);
            if (accCount >= mData.mTotalCount)
            {
                if (mData.IsRewarded())
                {
                    Debug.LogError("Already rewarded.");
                    return;
                }

                // 보상 지급 
                mData.SetRewarded(true);
                UserData.instance.OnUpdateGold(REWARD_GOLD);
                UIManager.instance.GetMainHud().UpdatePlayerGoldCount();

                // 업데이트
                this.UpdateQuestCell(mData.mConditionType);
            }
        }
        else
        {
            // 에러 
        }
    }
}
