using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance = null;
    public Dictionary<QuestData.QuestConditionType, int> mQuestCounts = new Dictionary<QuestData.QuestConditionType, int>();
    public Dictionary<QuestData.QuestConditionType, List<QuestData>> mQuestData = new Dictionary<QuestData.QuestConditionType, List<QuestData>>();

   
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Init()
    {
        mQuestCounts.Add(QuestData.QuestConditionType.MonsterKill, 0);
        mQuestCounts.Add(QuestData.QuestConditionType.Fishing, 0);
        mQuestCounts.Add(QuestData.QuestConditionType.Farming, 0);
        mQuestCounts.Add(QuestData.QuestConditionType.Mining, 0);
        mQuestCounts.Add(QuestData.QuestConditionType.Logging, 0);
        mQuestCounts.Add(QuestData.QuestConditionType.Gathering, 0);

        QuestTableCreate();

    }

    public void QuestTableCreate()
    {
        // item reward 용 임시로 하나 생성.
        Sprite[] itemImages = Resources.LoadAll<Sprite>("Sprites/Icon");
        Sprite itemIcon = itemImages[13];
        ItemData rewardItem = new ItemData(1, "무", itemIcon, 99, 20);

        // monster kill quest 
        List<QuestData> monsterKillQestList = new List<QuestData>();
        {
            QuestData quest = new QuestData("step 1. kill slime 3 times", QuestData.QuestConditionType.MonsterKill, 3, QuestData.RewardType.Item, rewardItem, 1);
            monsterKillQestList.Add(quest);
        }
        {
            QuestData quest = new QuestData("step 2. kill slime 5 times", QuestData.QuestConditionType.MonsterKill, 5, QuestData.RewardType.Gold, null, 3000);
            monsterKillQestList.Add(quest);
        }
        {
            QuestData quest = new QuestData("step 3. kill slime 10 times", QuestData.QuestConditionType.MonsterKill, 10, QuestData.RewardType.Item, rewardItem, 3);
            monsterKillQestList.Add(quest);
        }
        mQuestData.Add(QuestData.QuestConditionType.MonsterKill, monsterKillQestList);


        // fishing quest  ================================================================================
        List<QuestData> fishingQestList = new List<QuestData>();
        {
            QuestData quest = new QuestData("step 1. fishing 3 times", QuestData.QuestConditionType.Fishing, 3 ,QuestData.RewardType.Gold, null, 1000);
            fishingQestList.Add(quest);
        }
        {
            QuestData quest = new QuestData("step 2. fishing 5 times", QuestData.QuestConditionType.Fishing, 5, QuestData.RewardType.Gold, null, 3000);
            fishingQestList.Add(quest);
        }
        {
            QuestData quest = new QuestData("step 3. fishing 10 times", QuestData.QuestConditionType.Fishing, 10, QuestData.RewardType.Item, rewardItem, 1);
            fishingQestList.Add(quest);
        }
        mQuestData.Add(QuestData.QuestConditionType.Fishing, fishingQestList);
        // fishing quest  ================================================================================

        // farm quest ================================================================================
        List<QuestData> farmQuest = new List<QuestData>();
        {
            QuestData quest = new QuestData("step 1. farming 3 times", QuestData.QuestConditionType.Farming, 3, QuestData.RewardType.Gold, null, 1000);
            farmQuest.Add(quest);
        }
        {
            QuestData quest = new QuestData("step 2. farming 5 times", QuestData.QuestConditionType.Farming, 5, QuestData.RewardType.Gold, null, 3000);
            farmQuest.Add(quest);
        }
        {
            
            QuestData quest = new QuestData("step 3. farming 10 times", QuestData.QuestConditionType.Farming, 10, QuestData.RewardType.Item, rewardItem, 1);
            farmQuest.Add(quest);
        }
        mQuestData.Add(QuestData.QuestConditionType.Farming, farmQuest);
        // farm quest ================================================================================

        // Mining quest ================================================================================
        List<QuestData> MiningQuest = new List<QuestData>();
        {
            QuestData quest = new QuestData("step 1. Mining 3 times", QuestData.QuestConditionType.Mining, 3, QuestData.RewardType.Gold, null, 1000);
            MiningQuest.Add(quest);
        }
        {
            QuestData quest = new QuestData("step 2. Mining 5 times", QuestData.QuestConditionType.Mining, 5, QuestData.RewardType.Gold, null, 3000);
            MiningQuest.Add(quest);
        }
        {

            QuestData quest = new QuestData("step 3. Mining 10 times", QuestData.QuestConditionType.Mining, 10, QuestData.RewardType.Item, rewardItem, 1);
            MiningQuest.Add(quest);
        }
        mQuestData.Add(QuestData.QuestConditionType.Mining, MiningQuest);
        // Mining quest ================================================================================

        // Logging quest ================================================================================
        List<QuestData> LoggingQuest = new List<QuestData>();
        {
            QuestData quest = new QuestData("step 1. Logging 3 times", QuestData.QuestConditionType.Logging, 3, QuestData.RewardType.Gold, null, 1000);
            LoggingQuest.Add(quest);
        }
        {
            QuestData quest = new QuestData("step 2. Logging 5 times", QuestData.QuestConditionType.Logging, 5, QuestData.RewardType.Gold, null, 3000);
            LoggingQuest.Add(quest);
        }
        {

            QuestData quest = new QuestData("step 3. Logging 10 times", QuestData.QuestConditionType.Logging, 10, QuestData.RewardType.Item, rewardItem, 1);
            LoggingQuest.Add(quest);
        }
        mQuestData.Add(QuestData.QuestConditionType.Logging, LoggingQuest);
        // Logging quest ================================================================================


        // Gathering quest ================================================================================
        List<QuestData> gatheringQuest = new List<QuestData>();
        {
            QuestData quest = new QuestData("step 1. gathering 3 times", QuestData.QuestConditionType.Gathering, 3, QuestData.RewardType.Gold, null, 1000);
            gatheringQuest.Add(quest);
        }
        {
            QuestData quest = new QuestData("step 2. gathering 5 times", QuestData.QuestConditionType.Gathering, 5, QuestData.RewardType.Gold, null, 3000);
            gatheringQuest.Add(quest);
        }
        {
            QuestData quest = new QuestData("step 3. gathering 10 times", QuestData.QuestConditionType.Gathering, 10, QuestData.RewardType.Item, rewardItem, 1);
            gatheringQuest.Add(quest);
        }
        mQuestData.Add(QuestData.QuestConditionType.Gathering, gatheringQuest);
        // Gathering quest ================================================================================
    }

    public void AddAccCount(QuestData.QuestConditionType type, int count)
    {
        if (mQuestCounts.ContainsKey(type))
        {
            mQuestCounts[type] += count;
        }
        else
        {
            Debug.LogFormat("not find key: {0}", type);
        }
    }

    public int GetAccCount(QuestData.QuestConditionType type)
    {
       return mQuestCounts[type];
    }

    public void ResetAccCount(QuestData.QuestConditionType type)
    {
        mQuestCounts[type] = 0;
    }
    
    //현재 진행중인 단계의 완료 조건 count 가져오기 (보상 수령을 받지 않은걸로 체크) 
    public QuestData GetQuestData(QuestData.QuestConditionType type)
    {
        List<QuestData> questList = mQuestData[type];

        for(int i = 0; i< questList.Count; i++)
        {
            if (questList[i].IsRewarded() == false)
            {
                return questList[i];
            }
        }
        //모든단계의 보상 수령이 끝난 경우
        return null;
    }
}
