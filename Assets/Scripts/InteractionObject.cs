using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    public enum ObjectType { None, Fishing, Mining, Gathering, Logging, Farming }

    public ObjectType mType = ObjectType.None;
    public int mStep = -1;

    private void Start()
    {
        mStep = 0;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject playerObject = collision.gameObject;
            PlayerMovement player = playerObject.GetComponent<PlayerMovement>();
            
            player.SetInteractionObject(this, mStep);
            
            
            UIManager.instance.FarmActionButtonEnable(true, mStep);

        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject playerObject = other.gameObject;
            PlayerMovement player = playerObject.GetComponent<PlayerMovement>();

            UIManager.instance.SetFarmActionButton(false);
            player.SetInteractionObject(null, -1);

        }
    }
}
