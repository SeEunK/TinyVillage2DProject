using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringObject : InteractionObject
{

    public List<Sprite> mSprites = new List<Sprite>();
    public SpriteRenderer mRenderer;
    public double mSpawnTurm = 3.0d;
    public int mIndex = -1;

    public Collider2D mObjectCollider = null;
    protected override void Start()
    {
        base.Start();
        base.mType = ObjectType.Gathering;
     


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

            UIManager.instance.GetMainHud().UpdateGatheringActionButtonSprite(UserData.instance.mGatherDataList[mIndex].GetState());

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
        if (UserData.instance.mGatherDataList[mIndex].GetState() == GatherData.State.Full)
        {
            Sprite[] itemImages = Resources.LoadAll<Sprite>("Sprites/Icon");
            Sprite itemIcon = itemImages[8];
            ItemData getItem = new ItemData(6, "블루베리", itemIcon, 99, 20);
            UserData.instance.AddItem(getItem);

            QuestManager.instance.AddAccCount(QuestData.QuestConditionType.Gathering, 1);

            AddStep();
        }

        else if (UserData.instance.mGatherDataList[mIndex].GetState() == GatherData.State.Half)
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
        switch (UserData.instance.mGatherDataList[mIndex].GetState())
        {

            case GatherData.State.None:
                mObjectCollider.enabled = false;
                mRenderer.sprite = mSprites[0];
                UIManager.instance.GetMainHud().UpdateGatheringActionButtonSprite(UserData.instance.mGatherDataList[mIndex].GetState());
                break;
            case GatherData.State.Full:
                mObjectCollider.enabled = true;
                mRenderer.sprite = mSprites[1];
                UIManager.instance.GetMainHud().UpdateGatheringActionButtonSprite(UserData.instance.mGatherDataList[mIndex].GetState());
                break;
            case GatherData.State.Half:
                mObjectCollider.enabled = true;
                mRenderer.sprite = mSprites[2];
                UIManager.instance.GetMainHud().UpdateGatheringActionButtonSprite(UserData.instance.mGatherDataList[mIndex].GetState());
                break;

        }
    }

    // 이건 유저가 인터렉션했을때 바뀌는 상태 
    public void AddStep()
    {
        switch (UserData.instance.mGatherDataList[mIndex].GetState())
        {
            case GatherData.State.Full:
                UserData.instance.mGatherDataList[mIndex].SetState(GatherData.State.Half);
                break;
            case GatherData.State.Half:
                UserData.instance.mGatherDataList[mIndex].SetState(GatherData.State.None);
                break;
            case GatherData.State.None:
                UserData.instance.mGatherDataList[mIndex].SetState(GatherData.State.Full);
                break;

        }
    }


    public void Update()
    {
        if (UserData.instance.mGatherDataList[mIndex].GetState() == GatherData.State.None)
        {

            if (UserData.instance.mGatherDataList[mIndex].IsGrowComplete(mSpawnTurm) == true)
            {
                UserData.instance.mGatherDataList[mIndex].SetState(GatherData.State.Half);
                UpdateSprite();
            }
       
        }

        if(UserData.instance.mGatherDataList[mIndex].GetState() == GatherData.State.Half)
        {

            if (UserData.instance.mGatherDataList[mIndex].IsBerryGrowComplete(mSpawnTurm) == true)
            {
                UserData.instance.mGatherDataList[mIndex].SetState(GatherData.State.Full);
                UpdateSprite();

            }
        }


    }


}
