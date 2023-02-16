using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class FishingObject : InteractionObject
{
    public List<Sprite> mSprites = new List<Sprite>();
    public SpriteRenderer mRenderer;
    
    public int mIndex = -1;

    public Image mImgBaitGauge = null;
    public double mTimer = 0.0d;
    public float mHookableTime =  5.0f; // ����Ⱑ �̳��� ���� ������� ��ȿ �ð�

    public void Init()
    {
        mImgBaitGauge.fillAmount = 1.0f;
        mTimer = 0.0d;
        mImgBaitGauge.enabled = false;
    }
    protected override void Start()
    {
        base.Start();
        base.mType = ObjectType.Fishing;
        
        mRenderer = GetComponent<SpriteRenderer>(); // ���� ����Ʈ ���� ������ ��ġ�� ������ �����;���.

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

            UIManager.instance.SetFishingActionButton(true);
            UIManager.instance.UpdateFishingActionButtonSprite(UserData.instance.mFishingDataList[mIndex].GetState());

        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.CompareTag("Player"))
        {
            GameObject playerObject = other.gameObject;
            PlayerMovement player = playerObject.GetComponent<PlayerMovement>();

            UIManager.instance.SetFishingActionButton(false);
            player.SetInteractionObject(null);

        }
    }

    public void UpdateState()
    {
        FishingData.State state = UserData.instance.mFishingDataList[mIndex].GetState();


        if( state == FishingData.State.None)
        {
            // ���� �� ������ --> Start
            Debug.Log("fishing start!!!");
            AddStep();
        }
        if (state == FishingData.State.Start)
        {
            // ���ô� ���� ���¿��� ���� ���� �õ� (���ô� ȸ��) -->  None
            Debug.Log("fishing cancle!!!");
            UserData.instance.mFishingDataList[mIndex].SetState(FishingData.State.None);
            
        }
        if ( state == FishingData.State.Bait)
        {
            Debug.Log("fishing Success!!!");
            UserData.instance.mFishingDataList[mIndex].SetState(FishingData.State.None);
           
        }

    
        UpdateSprite();
    }


    public void AddStep()
    {

        switch (UserData.instance.mFishingDataList[mIndex].GetState())
        {
            case FishingData.State.None:
                UserData.instance.mFishingDataList[mIndex].SetState(FishingData.State.Start);
                break;
            case FishingData.State.Start:
                UserData.instance.mFishingDataList[mIndex].SetState(FishingData.State.Bait);
                break;
            case FishingData.State.Bait:
                UserData.instance.mFishingDataList[mIndex].SetState(FishingData.State.None);
                break;
         
        }

        UpdateSprite();
    }


    public void UpdateSprite()
    {
        switch (UserData.instance.mFishingDataList[mIndex].GetState())
        {
            case FishingData.State.None:
                Init();
                mRenderer.sprite = mSprites[0];
                UIManager.instance.UpdateFishingActionButtonSprite(UserData.instance.mFishingDataList[mIndex].GetState());
                break;
            case FishingData.State.Start:
                mRenderer.sprite = mSprites[1];
                UIManager.instance.UpdateFishingActionButtonSprite(UserData.instance.mFishingDataList[mIndex].GetState());
                break;
            case FishingData.State.Bait:
                mRenderer.sprite = mSprites[2];
                UIManager.instance.UpdateFishingActionButtonSprite(UserData.instance.mFishingDataList[mIndex].GetState());
                break;
    
        }
    }




    private void Update()
    {
        if (UserData.instance.mFishingDataList[mIndex].GetState() == FishingData.State.Start)
        {
            float randomTime = Random.Range(3.0f, 10.0f);

            if (UserData.instance.mFishingDataList[mIndex].IsBait(randomTime) == true)
            {
                mImgBaitGauge.enabled = true; 
                AddStep(); //-->  Bait ���� ����
                Debug.Log("fish bait!!!");
            }

        }

        if (UserData.instance.mFishingDataList[mIndex].GetState() == FishingData.State.Bait)
        {
            if (mImgBaitGauge.fillAmount > 0.0f) // ������ �ִ� Ÿ�̹��� ���� ���, ������ ����
            {
                mTimer += Time.deltaTime;
                mImgBaitGauge.fillAmount = 1.0f - (float)mTimer/ mHookableTime; // �̰͵� ���߿� �ð� ���̽� ������� �ٲ����
               
                Debug.Log($"{mImgBaitGauge.fillAmount}");
            }
            else // ��ħ
            {
                
                Debug.Log("fish run");
                Init();

                UserData.instance.mFishingDataList[mIndex].SetState(FishingData.State.Start); // ���ļ� �ٽ� ���� ���·� ����
                UpdateSprite();
            }
        }

    }
}
