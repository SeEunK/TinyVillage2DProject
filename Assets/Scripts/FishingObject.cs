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
    public float mHookableTime =  5.0f; // 물고기가 미끼를 물고 낚기까지 유효 시간

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
        
        mRenderer = GetComponent<SpriteRenderer>(); // 낚시 포인트 말고 보여줄 위치의 랜더러 가져와야함.

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
            // 낚시 대 던지기 --> Start
            Debug.Log("fishing start!!!");
            AddStep();
        }
        if (state == FishingData.State.Start)
        {
            // 낚시대 던진 상태에서 상태 변경 시도 (낚시대 회수) -->  None
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
                AddStep(); //-->  Bait 상태 변경
                Debug.Log("fish bait!!!");
            }

        }

        if (UserData.instance.mFishingDataList[mIndex].GetState() == FishingData.State.Bait)
        {
            if (mImgBaitGauge.fillAmount > 0.0f) // 낚을수 있는 타이밍이 남은 경우, 게이지 차감
            {
                mTimer += Time.deltaTime;
                mImgBaitGauge.fillAmount = 1.0f - (float)mTimer/ mHookableTime; // 이것도 나중에 시간 베이스 계산으로 바꿔야함
               
                Debug.Log($"{mImgBaitGauge.fillAmount}");
            }
            else // 놓침
            {
                
                Debug.Log("fish run");
                Init();

                UserData.instance.mFishingDataList[mIndex].SetState(FishingData.State.Start); // 놓쳐서 다시 시작 상태로 변경
                UpdateSprite();
            }
        }

    }
}
