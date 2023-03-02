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
        Sprite[] itemImages = Resources.LoadAll<Sprite>("Sprites/Icon");
        // �������� ����Ʈ�� �ִ� ���
        if (mData != null)
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

            //����
            
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
            // Ŭ���� �ؼ� ������ ���� �� ������
            if (accCount == mData.mTotalCount)
            {
                mObjReward.transform.Find("completeBg").gameObject.SetActive(true);
            }
            // ���� ������ ���� �� ������
            else
            {
                mObjReward.transform.Find("completeBg").gameObject.SetActive(false);
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

                // �ش� ���� ī��Ʈ 0���� �ʱ�ȭ 
                QuestManager.instance.ResetAccCount(mData.mConditionType);
                // ������Ʈ
                this.UpdateQuestCell(mData.mConditionType);
            }
            else //���� ������������ Ŭ���� info popup ���
            {
                switch (mData.mRewardType)
                {
                    case QuestData.RewardType.Item:
                        UIManager.instance.GetItemInfoPopup().UpdateItemInfo(mData.mReward, mData.mReawardCount, ItemInfoPopup.PopupType.Reward);
                        UIManager.instance.SetItemInfoPopup(true);
                        break;
                    case QuestData.RewardType.Gold:
                        //��� ������� item �߰�
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
            // ���� 
        }
    }
}
