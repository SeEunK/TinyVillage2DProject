using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance = null;

    
    public List<Sprite> mIcoToolsSprites = new List<Sprite>();

    public GameObject mBtnFarmAction = null;
    public Image mImgFarmAction = null;

    public GameObject mBtnFishingAction = null;
    public Image mImgFishingAction = null;

    public GameObject mBtnDoorAction = null;
    public Image mImgDoorAction = null;


    public GameObject mBtnInventory = null;
    public GameObject mInventroy = null;

    void Awake()
    {
        if(instance== null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        SetFarmActionButton(false);
        SetFishingActionButton(false);
        SetDoorActionButton(false);
        SetInventoryUI(false);
        mImgFarmAction = mBtnFarmAction.GetComponent<Image>();
        mImgFishingAction = mBtnFarmAction.GetComponent<Image>();
        mImgDoorAction = mBtnDoorAction.GetComponent<Image>();

    }

    public void SetInventoryUI(bool value)
    {
        mInventroy.SetActive(value);
    }


    public void SetDoorActionButton(bool value)
    {
        mBtnDoorAction.SetActive(value);
    }
    public void UpdateDoorActionButtonSprite()
    {
        mImgDoorAction.color = Color.yellow;
        mImgDoorAction.sprite = mIcoToolsSprites[6]; // 문 이미지로 변경 필요
    }



    public void SetFishingActionButton(bool value)
    {
        mBtnFishingAction.SetActive(value);
    }

    public void UpdateFishingActionButtonSprite(FishingData.State state)
    {
        switch (state)
        {
            case FishingData.State.None:
                // 빈 상태 ->낚시줄 던지기
                mImgFarmAction.color = Color.white;
                mImgFarmAction.sprite = mIcoToolsSprites[5];
                break;
            case FishingData.State.Start:
                // 낚시줄 던진 상태 . 낚기 (회수)
                mImgFarmAction.color = Color.red;
                mImgFarmAction.sprite = mIcoToolsSprites[5];
                break;
            case FishingData.State.Bait:
                // 물었음. 낚기 
            
                mImgFarmAction.color = Color.blue;
                mImgFarmAction.sprite = mIcoToolsSprites[5];
                break;
           

        }
    }


    public void SetFarmActionButton(bool value)
    {
        mBtnFarmAction.SetActive(value);
    }
    public void UpdateFarmActionButtonSprite(FarmData.State state)
    {
        switch (state)
        {
            case FarmData.State.None:
                // 빈 상태 -> 땅파기 
                mImgFarmAction.color = Color.white;
                mImgFarmAction.sprite = mIcoToolsSprites[0];
                break;
            case FarmData.State.Base:
                // 파진 땅 --> 씨앗+물 
                mImgFarmAction.color = Color.white;
                mImgFarmAction.sprite = mIcoToolsSprites[1];
                break;
            case FarmData.State.Wait:
                // 성장 대기 
                mImgFarmAction.color = Color.red;
                break;
            case FarmData.State.Done:
                // 성장 종료, 수확 
                mImgFarmAction.color = Color.white;
                mImgFarmAction.sprite = mIcoToolsSprites[2];
                break;

        }
    }


}
