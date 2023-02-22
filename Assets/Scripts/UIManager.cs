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

 
    private AmountPopup mAmountPopup = null;

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

        GameObject amountPopup = this.transform.Find("AmountPopup").gameObject;
        mAmountPopup= amountPopup.GetComponent<AmountPopup>();

        SetAmountPopup(false);
        SetActionButton(false);
        SetInventoryUI(false);
        SetInventoryButton(false);
        SetNpcShopPopupUI(false); 

    }

    public void SetAmountPopup(bool value)
    {
        mAmountPopup.SetActiveAmountPopup(value);
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

    public AmountPopup GetAmountPopup()
    {
        return mAmountPopup;
    }


}
