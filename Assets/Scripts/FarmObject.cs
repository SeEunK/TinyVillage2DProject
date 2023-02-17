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
            //player.SetInteractionObject(this, UserData.instance.mFarmDataList[mIndex].GetState());

            UIManager.instance.SetFarmActionButton(true);
            UIManager.instance.UpdateFarmActionButtonSprite(UserData.instance.mFarmDataList[mIndex].GetState());

        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.CompareTag("Player"))
        {
            GameObject playerObject = other.gameObject;
            PlayerMovement player = playerObject.GetComponent<PlayerMovement>();

            UIManager.instance.SetFarmActionButton(false);

            player.SetInteractionObject(null);
           // player.SetInteractionObject(null, FarmData.State.None);

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
                UIManager.instance.UpdateFarmActionButtonSprite(UserData.instance.mFarmDataList[mIndex].GetState());
                break;
            case FarmData.State.Base:
                mRenderer.sprite = mSprites[1];
                UIManager.instance.UpdateFarmActionButtonSprite(UserData.instance.mFarmDataList[mIndex].GetState());
                break;
            case FarmData.State.Wait:
                mRenderer.sprite = mSprites[2];
                UIManager.instance.UpdateFarmActionButtonSprite(UserData.instance.mFarmDataList[mIndex].GetState());
                break;
            case FarmData.State.Done:
                mRenderer.sprite = mSprites[3];
                UIManager.instance.UpdateFarmActionButtonSprite(UserData.instance.mFarmDataList[mIndex].GetState());
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
            mImgWait.fillAmount = (float)(mTimer / mTurm); // �̰͵� ���߿� �ð� ���̽� ������� �ٲ����

        }
    }
}