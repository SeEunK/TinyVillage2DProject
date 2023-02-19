using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayObject : InteractionObject {

    public ZoneData.Name mGoToZoneName = ZoneData.Name.Field;

    protected override void Start()
    {
        base.Start();
        base.mType = ObjectType.Doorway;

    }


    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            GameObject playerObject = collision.gameObject;
            PlayerMovement player = playerObject.GetComponent<PlayerMovement>();

            player.SetInteractionObject(this);
            UIManager.instance.SetActionButton(true);
            UIManager.instance.GetMainHud(). UpdateDoorActionButtonSprite();

        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.CompareTag("Player"))
        {
            GameObject playerObject = other.gameObject;
            PlayerMovement player = playerObject.GetComponent<PlayerMovement>();

            UIManager.instance.SetActionButton(false);
            player.SetInteractionObject(null);

        }
    }

    public ZoneData.Name GetGoToZone()
    {
        return mGoToZoneName;
    }

}
