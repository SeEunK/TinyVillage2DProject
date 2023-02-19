using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvetoryButton : MonoBehaviour
{

    public void OpenInventory()
    {
        Inventory inven = UIManager.instance.mInventroy.GetComponent<Inventory>();
        inven.UpdateInventoryList();
        UIManager.instance.mInventroy.SetActive(true);

    }

    public void CloseInventory()
    {
        UIManager.instance.mInventroy.SetActive(false);
    }
}
