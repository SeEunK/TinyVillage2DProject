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
        mImgFarmAction = mBtnFarmAction.GetComponent<Image>();
        mImgFishingAction = mBtnFarmAction.GetComponent<Image>();
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
                // ºó »óÅÂ ->³¬½ÃÁÙ ´øÁö±â
                mImgFarmAction.color = Color.white;
                mImgFarmAction.sprite = mIcoToolsSprites[5];
                break;
            case FishingData.State.Start:
                // ³¬½ÃÁÙ ´øÁø »óÅÂ . ³¬±â (È¸¼ö)
                mImgFarmAction.color = Color.red;
                mImgFarmAction.sprite = mIcoToolsSprites[5];
                break;
            case FishingData.State.Bait:
                // ¹°¾úÀ½. ³¬±â 
            
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
                // ºó »óÅÂ -> ¶¥ÆÄ±â 
                mImgFarmAction.color = Color.white;
                mImgFarmAction.sprite = mIcoToolsSprites[0];
                break;
            case FarmData.State.Base:
                // ÆÄÁø ¶¥ --> ¾¾¾Ñ+¹° 
                mImgFarmAction.color = Color.white;
                mImgFarmAction.sprite = mIcoToolsSprites[1];
                break;
            case FarmData.State.Wait:
                // ¼ºÀå ´ë±â 
                mImgFarmAction.color = Color.red;
                break;
            case FarmData.State.Done:
                // ¼ºÀå Á¾·á, ¼öÈ® 
                mImgFarmAction.color = Color.white;
                mImgFarmAction.sprite = mIcoToolsSprites[2];
                break;

        }
    }


}
