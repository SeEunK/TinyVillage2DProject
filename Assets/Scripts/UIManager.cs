using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance = null;


    public GameObject mBtnFarmAction = null;
    public List<Sprite> mIcoToolsSprites = new List<Sprite>();
    public Image mBtnImage = null;

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
        mBtnImage = mBtnFarmAction.GetComponent<Image>();
    }

    public void SetFarmActionButton(bool value)
    {
        mBtnFarmAction.SetActive(value);
    }
    public void UpdateActionButtonSprite(FarmData.State state)
    {
        switch (state)
        {
            case FarmData.State.None:
                // �� ���� -> ���ı� 
                mBtnImage.color = Color.white;
                mBtnImage.sprite = mIcoToolsSprites[0];
                break;
            case FarmData.State.Base:
                // ���� �� --> ����+�� 
                mBtnImage.color = Color.white;
                mBtnImage.sprite = mIcoToolsSprites[1];
                break;
            case FarmData.State.Wait:
                // ���� ��� 
                mBtnImage.color = Color.red;
                break;
            case FarmData.State.Done:
                // ���� ����, ��Ȯ 
                mBtnImage.color = Color.white;
                mBtnImage.sprite = mIcoToolsSprites[2];
                break;

        }
    }


}
