using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance = null;


    public GameObject mBtnFarmAction = null;

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
                mBtnFarmAction.GetComponent<Image>().color = Color.green; 
                // 나중에 이미지 교체로 변경
                break;
            case 1:
                mBtnFarmAction.GetComponent<Image>().color = Color.blue;
                break;
        }
    }


}
