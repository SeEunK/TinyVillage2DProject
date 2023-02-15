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
    public void FarmActionButtonEnable(bool value,  int step)
    {
        mBtnFarmAction.SetActive(value);

        switch (step)
        {
            case 0:
                // �� ���� -> ���ı� 
                mBtnImage.color = Color.white;
                mBtnImage.sprite = mIcoToolsSprites[0];
                // ���߿� �̹��� ��ü�� ����
                break;
            case 1:
                // ���� �� --> ����+�� 
                mBtnImage.color = Color.white;
                mBtnImage.sprite = mIcoToolsSprites[1];
                break;
            case 2:
            case 3:
                // ���� ��� 
                mBtnImage.color = Color.red;
                break;
            case 4:
                // ���� ����, ��Ȯ 
                mBtnImage.color = Color.white;
                mBtnImage.sprite = mIcoToolsSprites[2];
                break;

        }
    }


}
