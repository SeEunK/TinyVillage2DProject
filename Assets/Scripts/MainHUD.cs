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

        mImgAction = mBtnAction.transform.Find("ImgActionIcon").GetComponent<Image>();
           
       
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

    public void UpdateFishingActionButtonSprite(FishingData.State state)
    {
        switch (state)
        {
            case FishingData.State.None:
                // �� ���� ->������ ������
                mBtnAction.GetComponent<Image>().color = Color.white;
                mImgAction.sprite = mIcoToolsSprites[5];
                break;
            case FishingData.State.Start:
                // ������ ���� ���� . ���� (ȸ��)
                mBtnAction.GetComponent<Image>().color = Color.red;
                mImgAction.sprite = mIcoToolsSprites[5];
                break;
            case FishingData.State.Bait:
                // ������. ���� 

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
                // �� ���� -> ���ı� 
                mImgAction.color = Color.white;
                mImgAction.sprite = mIcoToolsSprites[0];
                break;
            case FarmData.State.Base:
                // ���� �� --> ����+�� 
                mImgAction.color = Color.white;
                mImgAction.sprite = mIcoToolsSprites[1];
                break;
            case FarmData.State.Wait:
                // ���� ��� 
                mImgAction.color = Color.red;
                break;
            case FarmData.State.Done:
                // ���� ����, ��Ȯ 
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