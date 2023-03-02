using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcObject : InteractionObject
{
    private Animator mAnimator;
    

    private void Awake()
    {
        mAnimator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        base.mType = ObjectType.Npc;
        mAnimator.SetBool("IsMove", false);
    }


    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            this.transform.GetComponent<NpcMovement>().Pause();
            //npc move stop
            mAnimator.SetBool("IsMove", false);
            mAnimator.SetBool("IsInteraction", true);

            GameObject playerObject = collision.gameObject;
            PlayerController player = playerObject.GetComponent<PlayerController>();

            player.SetInteractionObject(this);
            UIManager.instance.SetActionButton(true);
            UIManager.instance.GetMainHud().UpdateNPCActionButtonSprite();

        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.CompareTag("Player"))
        {
            this.transform.GetComponent<NpcMovement>().Resume();
            //npc move 
            mAnimator.SetBool("IsInteraction", false);
            mAnimator.SetBool("IsMove", true);

            GameObject playerObject = other.gameObject;
            PlayerController player = playerObject.GetComponent<PlayerController>();

            UIManager.instance.SetActionButton(false);
            player.SetInteractionObject(null);

        }
    }

    public void UpdateState()
    {
        UIManager.instance.SetNpcShopPopupUI(true);
        Debug.Log("npc interaction action : UpdateState");
    }


}
