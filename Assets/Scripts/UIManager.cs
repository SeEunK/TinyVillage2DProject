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
    private Quest mQuestPopup = null;
    private ItemInfoPopup mItemInfoPopup = null;
    private SystemMessagePopup mSystemMessage = null;


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

        GameObject QuestPopup = this.transform.Find("QuestPopup").gameObject;
        mQuestPopup = QuestPopup.GetComponent<Quest>();
        mQuestPopup.InitQuest();

        GameObject ItemInfoPopup = this.transform.Find("ItemInfoPopup").gameObject;
        mItemInfoPopup = ItemInfoPopup.GetComponent<ItemInfoPopup>();

        GameObject SystemMessagePopup = this.transform.Find("SystemMessage").gameObject;
        mSystemMessage = SystemMessagePopup.GetComponent<SystemMessagePopup>();

        mSystemMessage.Init();
        SetAmountPopup(false);
        SetActionButton(false);
        SetInventoryUI(false);
        SetInventoryButton(false);
        SetNpcShopPopupUI(false);
        SetPlayerInfo(false);
        SetQuestPopup(false);
        SetQuestButton(false);
        SetItemInfoPopup(false);

    }

    public void SetSystemMessage(string message)
    {
        mSystemMessage.SetMessageText(message);
        mSystemMessage.ShowMessage();
    }

    public void SetItemInfoPopup(bool value)
    {
        mItemInfoPopup.SetActiveItemInfoPopup(value);
    }
    public void SetQuestPopup(bool value)
    {
        mQuestPopup.SetQuestMenu(value);
    }
    public void SetQuestButton(bool value)
    {
        mMainHUD.SetQuestButton(value);
    }
    public void UpdatePlayerInfo(int Hp ,int maxHP)
    {
        mMainHUD.UpdatePlayerHpBar(Hp,maxHP);
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
    public void SetPlayerInfo(bool value)
    {
        mMainHUD.SetPlayerInfo(value);
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

    public Quest GetQuestPopup()
    {
        return mQuestPopup;
    }

    public ItemInfoPopup GetItemInfoPopup()
    {
        return mItemInfoPopup;
    }

    public Inventory GetInventory()
    {
        return mInventroy.GetComponent<Inventory>();
    }

}
