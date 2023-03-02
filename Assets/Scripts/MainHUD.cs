using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainHUD: MonoBehaviour
{
    public List<Sprite> mIcoToolsSprites = new List<Sprite>();

    public GameObject mBtnAction = null;
    public Image mImgAction = null;

    public GameObject mBtnInventory = null;
    public GameObject mBtnQuest = null;

    public GameObject mPlayerInfo = null;
    public Image mImgPlayerHpFill = null;
    public TMP_Text mHpCount = null;
    public TMP_Text mGoldCount = null;

    private void Awake()
    {
        GameObject HpInfo = mPlayerInfo.transform.Find("HpInfo").gameObject;
        GameObject GoldInfo = mPlayerInfo.transform.Find("GoldInfo").gameObject;

        mImgAction = mBtnAction.transform.Find("ImgActionIcon").GetComponent<Image>();
        mImgPlayerHpFill = HpInfo.transform.Find("imgHpFill").GetComponent<Image>();
        mHpCount = mImgPlayerHpFill.transform.Find("txtHpCount").GetComponent<TMP_Text>();

        mGoldCount = GoldInfo.transform.Find("txtGoldCount").GetComponent<TMP_Text>();
    }

    public void UpdatePlayerHpBar(int hp, int maxHp)
    {
        mImgPlayerHpFill.fillAmount = (float)hp / (float)maxHp;
        mHpCount.text = string.Format("{0}/{1}", hp, maxHp);
    }

    public void UpdatePlayerGoldCount()
    {
        mGoldCount.text = string.Format("{0}", UserData.instance.GetGold());
    }

    public void SetPlayerInfo(bool value)
    {
        mPlayerInfo.SetActive(value);
    }
    public void SetActionButton(bool value)
    {
        mBtnAction.SetActive(value);
       // mImgAction.enabled = value;
    }

    public void SetInventoryButton(bool value)
    {
        mBtnInventory.SetActive(value);
    }
    public void SetQuestButton(bool value)
    {
        mBtnQuest.SetActive(value);
    }

    public void UpdateNPCActionButtonSprite()
    {
        mBtnAction.GetComponent<Image>().color = Color.white;
        mImgAction.sprite = mIcoToolsSprites[7];
    }

    public void UpdateDoorActionButtonSprite()
    {
        mBtnAction.GetComponent<Image>().color = Color.white;
        mImgAction.sprite = mIcoToolsSprites[6];
    }

    public void UpdateLoggingActionButtonSprite(LoggingData.State state)
    {
        switch(state)
        {
            case LoggingData.State.Empty:
                mBtnAction.SetActive(false);
                break;

            case LoggingData.State.Full:
            case LoggingData.State.Half:
                mImgAction.sprite = mIcoToolsSprites[4];
                break;

        }
    }



    public void UpdateGatheringActionButtonSprite(GatherData.State state)
    {
        switch (state)
        {
            case GatherData.State.None:
                // 빈 상태 -> 버튼 hide
                mBtnAction.SetActive(false);

                break;

            case GatherData.State.Full:
                // 채집 가능 상태.
                mBtnAction.GetComponent<Image>().color = Color.white;
                mImgAction.sprite = mIcoToolsSprites[3];//채집은 손같은걸로 icon 추가해서 변경해주자
                break;

            case GatherData.State.Half:

                //열매는 안열렸지만 벌목가능 상태
                mBtnAction.GetComponent<Image>().color = Color.white;
                mImgAction.sprite = mIcoToolsSprites[4]; 
                break;
        }
    }





    public void UpdateMiningActionButtonSprite(MiningData.State state)
    {
        switch (state)
        {
            case MiningData.State.Empty:
                // 빈 상태 -> 버튼 hide
                mBtnAction.SetActive(false);
               
                break;

            case MiningData.State.Full:
            case MiningData.State.Half:

                // 채광 가능 상태.
                mBtnAction.GetComponent<Image>().color = Color.white;
                mImgAction.sprite = mIcoToolsSprites[3];
                break;
        }
    }


    public void UpdateFishingActionButtonSprite(FishingData.State state)
    {
        switch (state)
        {
            case FishingData.State.None:
                // 빈 상태 ->낚시줄 던지기
                mBtnAction.GetComponent<Image>().color = Color.white;
                mImgAction.sprite = mIcoToolsSprites[5];
                break;
            case FishingData.State.Start:
                // 낚시줄 던진 상태 . 낚기 (회수)
                mBtnAction.GetComponent<Image>().color = Color.red;
                mImgAction.sprite = mIcoToolsSprites[5];
                break;
            case FishingData.State.Bait:
                // 물었음. 낚기 

                mBtnAction.GetComponent<Image>().color = Color.blue;
                mImgAction.sprite = mIcoToolsSprites[5];
                break;


        }
    }

    public void UpdateFarmActionButtonSprite(FarmData.State state)
    {
        switch (state)
        {
            case FarmData.State.None:
                // 빈 상태 -> 땅파기 
                mImgAction.color = Color.white;
                mImgAction.sprite = mIcoToolsSprites[0];
                break;
            case FarmData.State.Base:
                // 파진 땅 --> 씨앗+물 
                mImgAction.color = Color.white;
                mImgAction.sprite = mIcoToolsSprites[1];
                break;
            case FarmData.State.Wait:
                // 성장 대기 
                mImgAction.color = Color.red;
                break;
            case FarmData.State.Done:
                // 성장 종료, 수확 
                mImgAction.color = Color.white;
                mImgAction.sprite = mIcoToolsSprites[2];
                break;

        }
    }


    public void OnButtonAction()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerController = player.GetComponent<PlayerMovement>();
            playerController.Interaction();
        }
        else
        {
            Debug.Log("Not Find player");
        }
    }




    public void OpenInventory()
    {
        Inventory inven = UIManager.instance.mInventroy.GetComponent<Inventory>();
        inven.Open();

    }

    public void OpenQuestMenu()
    {
        UIManager.instance.GetQuestPopup().UpdateQuestCell();
        UIManager.instance.SetQuestPopup(true);
    }

}
