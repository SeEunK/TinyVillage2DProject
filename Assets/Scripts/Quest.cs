using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    public GameObject mQuestPopupContentArea = null;
    public GameObject mQuestCell = null;

    public List<QuestCell> mQuestList = new List<QuestCell>();

      

    public void InitQuest()
    {
        List<QuestData.QuestConditionType> enumList = CreateQuestTypeList();

        for (int i = 0; i< enumList.Count; i++)
        {
            GameObject cellObject = Instantiate(mQuestCell);
            RectTransform cellRect = cellObject.GetComponent<RectTransform>();

            cellRect.SetParent(mQuestPopupContentArea.transform);
            cellObject.name = string.Format("questCell_{0}", i);

            QuestCell cell = cellObject.GetComponent<QuestCell>();

            cell.UpdateQuestCell(enumList[i]);

            mQuestList.Add(cell);
        }
    }

    public void UpdateQuestCell()
    {
        List<QuestData.QuestConditionType> enumList = CreateQuestTypeList();

        for (int i = 0; i < enumList.Count; i++)
        {
            QuestCell cell = mQuestList[i];
            cell.UpdateQuestCell(enumList[i]);
        }
    }

    public List<QuestData.QuestConditionType> CreateQuestTypeList()
    {
        string[] strings = Enum.GetNames(typeof(QuestData.QuestConditionType));
        List<QuestData.QuestConditionType> enums = new List<QuestData.QuestConditionType>();
        
        for(int i = 0; i<strings.Length; i++)
        {
            string str = strings[i];
            QuestData.QuestConditionType type = (QuestData.QuestConditionType)Enum.Parse(typeof(QuestData.QuestConditionType), str);
            enums.Add(type);

        }

        return enums;
    }

    public void SetQuestMenu(bool value)
    {
        this.gameObject.SetActive(value);
    }

    public void CloseQuestMenu()
    {
        this.gameObject.SetActive(false);
        UIManager.instance.SetItemInfoPopup(false);
    }

}
