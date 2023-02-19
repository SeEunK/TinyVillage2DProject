using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance = null;

  
    public GameObject mInventroy = null;
    private MainHUD mMainHUD = null;

    void Awake()
    {
        if(instance== null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            Init();
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }

    }

    public void Init()
    {

        GameObject mainHUD = this.transform.Find("MainHUD").gameObject;
        mMainHUD = mainHUD.GetComponent<MainHUD>();
        
        SetActionButton(false);
        SetInventoryUI(false);
        SetInventoryButton(false);

    }

    public void SetInventoryUI(bool value)
    {
        mInventroy.SetActive(value);
    }

    public void SetActionButton(bool value)
    {
        mMainHUD.SetActionButton(value);
    }
    public void SetInventoryButton(bool value)
    {
        mMainHUD.SetInventoryButton(value);
    }

   

    public MainHUD GetMainHud()
    {
        return mMainHUD; 
    }




}
