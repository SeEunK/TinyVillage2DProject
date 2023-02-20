using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance = null;

  
    public GameObject mInventroy = null;
    private MainHUD mMainHUD = null;
    private NpcShop mNpcShop = null;

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

        GameObject shopUI = this.transform.Find("NpcShop").gameObject;
        mNpcShop = shopUI.GetComponent<NpcShop>();
        mNpcShop.SetShopContents();

        SetActionButton(false);
        SetInventoryUI(false);
        SetInventoryButton(false);
        SetNpcShopPopupUI(false); 

    }

    public void SetNpcShopPopupUI(bool value)
    {
        mNpcShop.SetPopupActive(value);
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

   public NpcShop GetNpcShop()
    {
        return mNpcShop;
    }

    public MainHUD GetMainHud()
    {
        return mMainHUD; 
    }




}
