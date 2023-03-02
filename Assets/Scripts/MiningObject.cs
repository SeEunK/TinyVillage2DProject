using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningObject : InteractionObject
{
    public List<Sprite> mSprites = new List<Sprite>();
    public SpriteRenderer mRenderer;
    public double mSpawnTurm = 3.0d;
    public int mIndex = -1;

    public Collider2D mObjectCollider = null;
    protected override void Start()
    {
        base.Start();
        base.mType = ObjectType.Mining;
        mRenderer = GetComponent<SpriteRenderer>();

        
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            GameObject playerObject = collision.gameObject;
            PlayerController player = playerObject.GetComponent<PlayerController>();

            player.SetInteractionObject(this);

            UIManager.instance.SetActionButton(true);

            UIManager.instance.GetMainHud().UpdateMiningActionButtonSprite(UserData.instance.mMiningDataList[mIndex].GetState());

        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.CompareTag("Player"))
        {
            GameObject playerObject = other.gameObject;
            PlayerController player = playerObject.GetComponent<PlayerController>();

            UIManager.instance.SetActionButton(false);

            player.SetInteractionObject(null);

        }
    }


    public void UpdateState()
    {
        if(UserData.instance.mMiningDataList[mIndex].GetState() != MiningData.State.Empty)
        {
            Sprite[] itemImages = Resources.LoadAll<Sprite>("Sprites/Icon");
            Sprite itemIcon = itemImages[8];
            ItemData getItem = new ItemData(8, "µπ∏Õ¿Ã", itemIcon, 99, 30);
            UserData.instance.AddItem(getItem);

            QuestManager.instance.AddAccCount(QuestData.QuestConditionType.Mining, 1);

            AddStep();

        }
        UpdateSprite();
    }


    public void UpdateSprite()
    {
        switch (UserData.instance.mMiningDataList[mIndex].GetState())
        {

            case MiningData.State.Empty:
                mObjectCollider.enabled = false;
                mRenderer.sprite = mSprites[0];
                UIManager.instance.GetMainHud().UpdateMiningActionButtonSprite(UserData.instance.mMiningDataList[mIndex].GetState());
                break;
            case MiningData.State.Full:
                mObjectCollider.enabled = true;
                mRenderer.sprite = mSprites[1];
                UIManager.instance.GetMainHud().UpdateMiningActionButtonSprite(UserData.instance.mMiningDataList[mIndex].GetState());
                break;
            case MiningData.State.Half:
                mObjectCollider.enabled = true;
                mRenderer.sprite = mSprites[2];
                UIManager.instance.GetMainHud().UpdateMiningActionButtonSprite(UserData.instance.mMiningDataList[mIndex].GetState());
                break;

        }
    }

    public void AddStep()
    {
        switch (UserData.instance.mMiningDataList[mIndex].GetState())
        {
            case MiningData.State.Full:
                UserData.instance.mMiningDataList[mIndex].SetState(MiningData.State.Half);
                break;
            case MiningData.State.Half:
                UserData.instance.mMiningDataList[mIndex].SetState(MiningData.State.Empty);
                break;
            case MiningData.State.Empty:
                UserData.instance.mMiningDataList[mIndex].SetState(MiningData.State.Full);
                break;

        }
    }


    public void Update()
    {
        if (UserData.instance.mMiningDataList[mIndex].GetState() == MiningData.State.Empty)
        {
            if (UserData.instance.mMiningDataList[mIndex].IsRespawnComplete(mSpawnTurm) == true)
            {
                AddStep();
                UpdateSprite();


            }
        }
    }
}
