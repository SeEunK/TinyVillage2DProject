using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestCell : MonoBehaviour
{
    
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
        Sprite[] itemImages = Resources.LoadAll<Sprite>("Sprites/Icon");
        // 진행중인 퀘스트가 있는 경우
        if (mData != null)
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

            //보상
            
            switch (mData.mRewardType){
                case QuestData.RewardType.Gold:
                    {
                        Sprite itemIcon = itemImages[10];
                        mObjReward.transform.Find("icoReward").GetComponent<Image>().sprite = itemIcon;
                        break;
                    }
                case QuestData.RewardType.Item:
                    {
                        Sprite itemIcon = mData.mReward.mImage;
                        mObjReward.transform.Find("icoReward").GetComponent<Image>().sprite = itemIcon;
                        break;
                    }
            }
            mObjReward.SetActive(true);

            mTxtRewardCount.text = mData.mReawardCount.ToString();
            // 클리어 해서 보상을 받을 수 있을때
            if (accCount == mData.mTotalCount)
            {
                mObjReward.transform.Find("completeBg").gameObject.SetActive(true);
            }
            // 아직 보상을 받을 수 없을때
            else
            {
                mObjReward.transform.Find("completeBg").gameObject.SetActive(false);
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
            case QuestData.QuestConditionType.MonsterKill: { return "[Hunter]"; }
            case QuestData.QuestConditionType.Fishing: { return "[Fisherman]"; }
            case QuestData.QuestConditionType.Farming: { return "[Farmer]"; }
            case QuestData.QuestConditionType.Mining: { return "[Mining]"; }
            case QuestData.QuestConditionType.Logging: { return "[Logging]"; }
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

                switch (mData.mRewardType)
                {
                    case QuestData.RewardType.Gold:
                        {
                            UserData.instance.OnUpdateGold(mData.mReawardCount);
                            UIManager.instance.GetMainHud().UpdatePlayerGoldCount();
                            break;
                        }
                    case QuestData.RewardType.Item:
                        {
                            for (int i = 0; i < mData.mReawardCount; i++)
                            {
                                UserData.instance.AddItem(mData.mReward);
                            }
                            break;
                        }
                }

                // 해당 누적 카운트 0으로 초기화 
                QuestManager.instance.ResetAccCount(mData.mConditionType);
                // 업데이트
                this.UpdateQuestCell(mData.mConditionType);
            }
            else //보상 받을수없을때 클릭시 info popup 출력
            {
                switch (mData.mRewardType)
                {
                    case QuestData.RewardType.Item:
                        UIManager.instance.GetItemInfoPopup().UpdateItemInfo(mData.mReward, mData.mReawardCount, ItemInfoPopup.PopupType.Reward);
                        UIManager.instance.SetItemInfoPopup(true);
                        break;
                    case QuestData.RewardType.Gold:
                        //골드 리워드용 item 추가
                        Sprite[] itemImages = Resources.LoadAll<Sprite>("Sprites/Icon");
                        Sprite itemIcon = itemImages[10];
                        ItemData getItem = new ItemData(99, "gold", itemIcon, 9999, -1);

                        UIManager.instance.GetItemInfoPopup().UpdateItemInfo(getItem, mData.mReawardCount, ItemInfoPopup.PopupType.Reward);
                        UIManager.instance.SetItemInfoPopup(true);
                        break;
                }
            }
        }
        else
        {
            // 에러 
        }
    }
}
