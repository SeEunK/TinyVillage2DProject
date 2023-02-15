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
                // 빈 상태 -> 땅파기 
                mBtnImage.color = Color.white;
                mBtnImage.sprite = mIcoToolsSprites[0];
                // 나중에 이미지 교체로 변경
                break;
            case 1:
                // 파진 땅 --> 씨앗+물 
                mBtnImage.color = Color.white;
                mBtnImage.sprite = mIcoToolsSprites[1];
                break;
            case 2:
            case 3:
                // 성장 대기 
                mBtnImage.color = Color.red;
                break;
            case 4:
                // 성장 종료, 수확 
                mBtnImage.color = Color.white;
                mBtnImage.sprite = mIcoToolsSprites[2];
                break;

        }
    }


}
