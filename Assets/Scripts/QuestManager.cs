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

        QuestTableCreate();

    }

    public void QuestTableCreate()
    {
        // monster kill quest 
        List<QuestData> monsterKillQestList = new List<QuestData>();
        {
            QuestData quest = new QuestData("step 1. kill slime", QuestData.QuestConditionType.MonsterKill, 3);
            monsterKillQestList.Add(quest);
        }
        {
            QuestData quest = new QuestData("step 2. kill slime", QuestData.QuestConditionType.MonsterKill, 5);
            monsterKillQestList.Add(quest);
        }
        {
            QuestData quest = new QuestData("step 3. kill slime", QuestData.QuestConditionType.MonsterKill, 10);
            monsterKillQestList.Add(quest);
        }
        mQuestData.Add(QuestData.QuestConditionType.MonsterKill, monsterKillQestList);


        // fishing quest  ================================================================================
        List<QuestData> fishingQestList = new List<QuestData>();
        {
            QuestData quest = new QuestData("step 1. fishing", QuestData.QuestConditionType.Fishing, 3);
            fishingQestList.Add(quest);
        }
        {
            QuestData quest = new QuestData("step 2. fishing", QuestData.QuestConditionType.Fishing, 5);
            fishingQestList.Add(quest);
        }
        {
            QuestData quest = new QuestData("step 3. fishing", QuestData.QuestConditionType.Fishing, 10);
            fishingQestList.Add(quest);
        }
        mQuestData.Add(QuestData.QuestConditionType.MonsterKill, fishingQestList);
        // fishing quest  ================================================================================

        // farm quest ================================================================================
        List<QuestData> farmQuest = new List<QuestData>();
        {
            QuestData quest = new QuestData("step 1. farming", QuestData.QuestConditionType.Farming, 3);
            farmQuest.Add(quest);
        }
        {
            QuestData quest = new QuestData("step 2. farming", QuestData.QuestConditionType.Farming, 5);
            farmQuest.Add(quest);
        }
        {
            QuestData quest = new QuestData("step 3. farming", QuestData.QuestConditionType.Farming, 10);
            farmQuest.Add(quest);
        }
        mQuestData.Add(QuestData.QuestConditionType.MonsterKill, farmQuest);
        // farm quest ================================================================================
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
