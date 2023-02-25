using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestCell : MonoBehaviour
{
    // �����ͷ� �и��ؾߵǴ� ��
    const int REWARD_GOLD = 1000;


    public TMP_Text mTxtTitle = null;

    public TMP_Text mTxtDescription = null;
    public TMP_Text mTxtCount = null;

    // ���� ��ư 
    public GameObject mObjReward = null;
    public TMP_Text mTxtRewardCount = null;

    // �Ϸ� ������Ʈ
    public GameObject mObjComplete = null;

    // ������
    public QuestData mData = null;


    public void UpdateQuestCell(QuestData.QuestConditionType type)
    {
        mData = QuestManager.instance.GetQuestData(type);
        // �������� ����Ʈ�� �ִ� ���
        if(mData != null)
        {
            int accCount = QuestManager.instance.GetAccCount(type);
            if(accCount > mData.mTotalCount)
            {
                accCount = mData.mTotalCount;
            }

            // ����
            mTxtTitle.text = this.GetTitle(type);

            // ����
            mTxtDescription.gameObject.SetActive(true);
            mTxtDescription.text = mData.mName;

            mTxtCount.gameObject.SetActive(true);
            mTxtCount.text = string.Format("({0}/{1})", accCount, mData.mTotalCount);

            mObjReward.SetActive(true);
            mTxtRewardCount.text = REWARD_GOLD.ToString();
            // Ŭ���� �ؼ� ������ ���� �� ������
            if (accCount == mData.mTotalCount)
            {
                
            }
            // ���� ������ ���� �� ������
            else
            {

            }

            mObjComplete.SetActive(false);
        }
        // ��� �Ϸ��� ���
        else
        {
            // ����
            mTxtTitle.text = this.GetTitle(type);

            // ����
            mTxtDescription.gameObject.SetActive(false);
            mTxtCount.gameObject.SetActive(false);

            // ������ ��ư
            mObjReward.SetActive(false);

            // �Ϸ� �ؽ�Ʈ
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
        // �����Ͱ� �ִ� ��쿡�� ����
        if(mData != null)
        {
            // �������� �񱳰��� ũ�ų� ������ �����带 ���� ���� �� �ִٰ� �Ǵ�
            int accCount = QuestManager.instance.GetAccCount(mData.mConditionType);
            if (accCount >= mData.mTotalCount)
            {
                if (mData.IsRewarded())
                {
                    Debug.LogError("Already rewarded.");
                    return;
                }

                // ���� ���� 
                mData.SetRewarded(true);
                UserData.instance.OnUpdateGold(REWARD_GOLD);
                UIManager.instance.GetMainHud().UpdatePlayerGoldCount();

                // ������Ʈ
                this.UpdateQuestCell(mData.mConditionType);
            }
        }
        else
        {
            // ���� 
        }
    }
}
