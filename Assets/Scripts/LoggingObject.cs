using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoggingObject : InteractionObject
{
    public List<Sprite> mHeadSprites = new List<Sprite>();
    public List<Sprite> mBodySprites = new List<Sprite>();
    public SpriteRenderer mHeadRenderer = null;
    public SpriteRenderer mBodyRenderer = null;
    public double mSpawnTurm = 3.0d;
    public int mIndex = -1;

    public Collider2D mObjectCollider = null;

    protected override void Start()
    {
        base.Start();
        base.mType = ObjectType.Logging;
        

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

            UIManager.instance.GetMainHud().UpdateLoggingActionButtonSprite(UserData.instance.mLoggingDataList[mIndex].GetState());

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


    public void UpdateState()
    {
        if (UserData.instance.mLoggingDataList[mIndex].GetState() != LoggingData.State.Empty)
        {
            Sprite[] itemImages = Resources.LoadAll<Sprite>("Sprites/Icon");
            Sprite itemIcon = itemImages[18];
            ItemData getItem = new ItemData(7, "나무토막", itemIcon, 99, 20);
            UserData.instance.AddItem(getItem);

            QuestManager.instance.AddAccCount(QuestData.QuestConditionType.Logging, 1);

            AddStep();

        }
        UpdateSprite();
    }


    public void UpdateSprite()
    {
        switch (UserData.instance.mLoggingDataList[mIndex].GetState())
        {

            case LoggingData.State.Empty:
                mObjectCollider.enabled = false;

                mHeadRenderer.sprite = mHeadSprites[0];
                mBodyRenderer.sprite = mBodySprites[0];
                UIManager.instance.GetMainHud().UpdateLoggingActionButtonSprite(UserData.instance.mLoggingDataList[mIndex].GetState());
                break;
            case LoggingData.State.Full:
                mObjectCollider.enabled = true;
                mHeadRenderer.sprite = mHeadSprites[1];
                mBodyRenderer.sprite = mBodySprites[1];
                UIManager.instance.GetMainHud().UpdateLoggingActionButtonSprite(UserData.instance.mLoggingDataList[mIndex].GetState());
                break;
            case LoggingData.State.Half:
                mObjectCollider.enabled = true;
                mHeadRenderer.sprite = mHeadSprites[0];
                mBodyRenderer.sprite = mBodySprites[2];
                UIManager.instance.GetMainHud().UpdateLoggingActionButtonSprite(UserData.instance.mLoggingDataList[mIndex].GetState());
                break;

        }
    }

    public void AddStep()
    {
        switch (UserData.instance.mLoggingDataList[mIndex].GetState())
        {
            case LoggingData.State.Full:
                UserData.instance.mLoggingDataList[mIndex].SetState(LoggingData.State.Half);
                break;
            case LoggingData.State.Half:
                UserData.instance.mLoggingDataList[mIndex].SetState(LoggingData.State.Empty);
                break;
            case LoggingData.State.Empty:
                UserData.instance.mLoggingDataList[mIndex].SetState(LoggingData.State.Full);
                break;

        }
    }


    public void Update()
    {
        if (UserData.instance.mLoggingDataList[mIndex].GetState() == LoggingData.State.Empty)
        {
            if (UserData.instance.mLoggingDataList[mIndex].IsRespawnComplete(mSpawnTurm) == true)
            {
                AddStep();
                UpdateSprite();


            }
        }
    }
}
