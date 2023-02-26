using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class FarmObject : InteractionObject
{
    public List<Sprite> mSprites = new List<Sprite>();
    public SpriteRenderer mRenderer;

    public double mTimer = 0.0f;
    public double mTurm = 3.0d;

    public Image mImgWait = null;
    public int mIndex = -1;

    public void Init()
    {
        mImgWait.fillAmount = 0.0f;
        mTimer = 0.0d;
        mImgWait.enabled = false;
    }
    protected override void Start()
    {
        base.Start();
        base.mType = ObjectType.Farming;
        mRenderer = GetComponent<SpriteRenderer>();
        Init();

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
            UIManager.instance.GetMainHud().UpdateFarmActionButtonSprite(UserData.instance.mFarmDataList[mIndex].GetState());

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

        if (UserData.instance.mFarmDataList[mIndex].GetState() < FarmData.State.Wait)
        {
            AddStep();
            if (UserData.instance.mFarmDataList[mIndex].GetState() == FarmData.State.Wait)
            {
                mImgWait.enabled = true;
            }
        }
        if (UserData.instance.mFarmDataList[mIndex].GetState() == FarmData.State.Done)
        {
            Sprite[] itemImages = Resources.LoadAll<Sprite>("Sprites/Icon");
            Sprite itemIcon = itemImages[13];
            ItemData getItem = new ItemData(1,"무", itemIcon,  99);
            UserData.instance.AddItem(getItem);

            QuestManager.instance.AddAccCount(QuestData.QuestConditionType.Farming, 1);
            AddStep();
            Init();

        }

        UpdateSprite();


    }


    public void UpdateSprite()
    {
        switch (UserData.instance.mFarmDataList[mIndex].GetState())
        {
            
            case FarmData.State.None:
                mRenderer.sprite = mSprites[0];
                UIManager.instance.GetMainHud().UpdateFarmActionButtonSprite(UserData.instance.mFarmDataList[mIndex].GetState());
                break;
            case FarmData.State.Base:
                mRenderer.sprite = mSprites[1];
                UIManager.instance.GetMainHud().UpdateFarmActionButtonSprite(UserData.instance.mFarmDataList[mIndex].GetState());
                break;
            case FarmData.State.Wait:
                mRenderer.sprite = mSprites[2];
                UIManager.instance.GetMainHud().UpdateFarmActionButtonSprite(UserData.instance.mFarmDataList[mIndex].GetState());
                break;
            case FarmData.State.Done:
                mRenderer.sprite = mSprites[3];
                UIManager.instance.GetMainHud().UpdateFarmActionButtonSprite(UserData.instance.mFarmDataList[mIndex].GetState());
                break;
        }
    }


    public void AddStep()
    {

        switch (UserData.instance.mFarmDataList[mIndex].GetState())
        {
            case FarmData.State.None:
                UserData.instance.mFarmDataList[mIndex].SetState(FarmData.State.Base);
                break;
            case FarmData.State.Base:
                UserData.instance.mFarmDataList[mIndex].SetState(FarmData.State.Wait);
                break;
            case FarmData.State.Wait:
                UserData.instance.mFarmDataList[mIndex].SetState(FarmData.State.Done);
                break;
            case FarmData.State.Done:
                UserData.instance.mFarmDataList[mIndex].SetState(FarmData.State.None);
                break;
        }

    }


    private void Update()
    {
        if (UserData.instance.mFarmDataList[mIndex].GetState() == FarmData.State.Wait)
        {
            if (UserData.instance.mFarmDataList[mIndex].IsGrowComplete(mTurm) == true)
            {
                AddStep();
                UpdateSprite();
            }

            mTimer += Time.deltaTime;
            mImgWait.fillAmount = (float)(mTimer / mTurm); // 이것도 나중에 시간 베이스 계산으로 바꿔야함

        }
    }
}
