using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainHUD: MonoBehaviour
{
    public List<Sprite> mIcoToolsSprites = new List<Sprite>();

    public GameObject mBtnAction = null;
    public Image mImgAction = null;

    public GameObject mBtnInventory = null;
  

    private void Awake()
    {

        mImgAction = mBtnAction.GetComponent<Image>();
       
    }

    public void SetActionButton(bool value)
    {
        mBtnAction.SetActive(value);
    }

    public void SetInventoryButton(bool value)
    {
        mBtnInventory.SetActive(value);
    }

    public void UpdateDoorActionButtonSprite()
    {
        mImgAction.color = Color.yellow;
        mImgAction.sprite = mIcoToolsSprites[6];
    }

    public void UpdateFishingActionButtonSprite(FishingData.State state)
    {
        switch (state)
        {
            case FishingData.State.None:
                // ºó »óÅÂ ->³¬½ÃÁÙ ´øÁö±â
                mImgAction.color = Color.white;
                mImgAction.sprite = mIcoToolsSprites[5];
                break;
            case FishingData.State.Start:
                // ³¬½ÃÁÙ ´øÁø »óÅÂ . ³¬±â (È¸¼ö)
                mImgAction.color = Color.red;
                mImgAction.sprite = mIcoToolsSprites[5];
                break;
            case FishingData.State.Bait:
                // ¹°¾úÀ½. ³¬±â 

                mImgAction.color = Color.blue;
                mImgAction.sprite = mIcoToolsSprites[5];
                break;


        }
    }

    public void UpdateFarmActionButtonSprite(FarmData.State state)
    {
        switch (state)
        {
            case FarmData.State.None:
                // ºó »óÅÂ -> ¶¥ÆÄ±â 
                mImgAction.color = Color.white;
                mImgAction.sprite = mIcoToolsSprites[0];
                break;
            case FarmData.State.Base:
                // ÆÄÁø ¶¥ --> ¾¾¾Ñ+¹° 
                mImgAction.color = Color.white;
                mImgAction.sprite = mIcoToolsSprites[1];
                break;
            case FarmData.State.Wait:
                // ¼ºÀå ´ë±â 
                mImgAction.color = Color.red;
                break;
            case FarmData.State.Done:
                // ¼ºÀå Á¾·á, ¼öÈ® 
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
        inven.UpdateInventoryList();
        UIManager.instance.mInventroy.SetActive(true);

    }



}
