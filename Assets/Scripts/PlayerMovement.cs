using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    public enum State { None, Active, Attack }
    public enum Direction { Left, Right, Up, Down }
    public State mState = State.None;
    public Direction mDirection = Direction.Down;


    [SerializeField] 
    private float mSpeed = 5.0f;

    private Vector2 mMovement;
    private Rigidbody2D mRigid;
    private Animator mAnimator;

    public InteractionObject mInteractionObj = null;

    public ZoneData.Name mZoneName = ZoneData.Name.Field;
    public ZoneData.Name mGoToZoneName = ZoneData.Name.Field;


    public int mMaxHP = 0;
    public int mHP = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            mRigid = GetComponent<Rigidbody2D>();
            mAnimator = GetComponent<Animator>();
            mMaxHP = 100;
            mHP = mMaxHP;
            
        }
        else
        {
            Destroy(this.gameObject);
        }

    }


    public void SetState(State state)
    {
        mState = state;
    }
    private void OnMovement(InputValue value)
    {
        mMovement = value.Get<Vector2>();
        if (mMovement.x != 0 || mMovement.y != 0)
        {
            if (mMovement.x < 0.0f)
            {
                SetDirection(Direction.Left);
            }
            else if (mMovement.x > 0.0f)
            {
                SetDirection(Direction.Right);
            }
            if (mMovement.y < 0.0f)
            {
                SetDirection(Direction.Down);
            }
            else if (mMovement.y > 0.0f)
            {
                SetDirection(Direction.Up);
            }
            //mAnimator.SetFloat("X", mMovement.x);
            //mAnimator.SetFloat("Y", mMovement.y);

            mAnimator.SetBool("IsWalking", true);

        }
        else
        {
            mAnimator.SetBool("IsWalking", false);

        }

    }
    private void OnAttack()
    {
        Debug.Log("OnAttack");
        mState = State.Attack;
        //mAnimator.SetBool("IsAttack", true);
        
        mAnimator.Play("AttackTree");

        mState = State.Active;

    }
    
    public void OnDamege(int damege)
    {
        mHP -= damege;
        UIManager.instance.GetMainHud().UpdatePlayerHpBar(mHP, mMaxHP);

        if(mHP<=0)
        {
            Debug.Log("player die");
        }
    }

    public void SetDirection(Direction value)
    {
        switch (value)
        {
            case Direction.Down:
                mAnimator.SetFloat("Y", -1.0f);
                mAnimator.SetFloat("X", 0.0f);
                break;
            case Direction.Up:
                mAnimator.SetFloat("Y", 1.0f);
                mAnimator.SetFloat("X", 0.0f);
                break;
            case Direction.Left:
                mAnimator.SetFloat("X", -1.0f);
                mAnimator.SetFloat("Y", 0.0f);
                break;
            case Direction.Right:
                mAnimator.SetFloat("X", 1.0f);
                mAnimator.SetFloat("Y", 0.0f);
                break;
        }
    }

    private void FixedUpdate()
    {
        if (mState == State.None)
        {
            return;
        }
        if (mState == State.Active)
        {
            //variant 1
            mRigid.MovePosition(mRigid.position + mMovement * mSpeed * Time.fixedDeltaTime);
        }
    }

    public int GetHpCount ()
    {
        return mHP;
    }

    public int GetMaxHp()
    {
        return mMaxHP;
    }



    public void SetInteractionObject(InteractionObject interactionObj)
    {
        mInteractionObj = interactionObj;

        if (interactionObj == null)
        {
            mZoneName = ZoneData.Name.Field;
            return;
        }

      
        switch (interactionObj.mType)
        {
            case InteractionObject.ObjectType.Farming:
                FarmObject farm = (FarmObject)mInteractionObj;
                

                break;
            case InteractionObject.ObjectType.Fishing:
                FishingObject fishing = (FishingObject)mInteractionObj;
              
                break;

            case InteractionObject.ObjectType.Doorway:
                DoorwayObject door = (DoorwayObject)mInteractionObj;
                mGoToZoneName = door.GetGoToZone();
                break;

            case InteractionObject.ObjectType.Mining:
                MiningObject miningObj = (MiningObject)mInteractionObj;
                break;


            case InteractionObject.ObjectType.Logging:
                LoggingObject loggingObj = (LoggingObject)mInteractionObj;
                break;


            case InteractionObject.ObjectType.Gathering:
                GatheringObject gatheringObj = (GatheringObject)mInteractionObj;
                break;


        }

    }

    public void Interaction()
    {
        //ObjectType { None, Fishing, Mining, Gathering, Logging, Farming }
        switch (mInteractionObj.mType)
        {
            case InteractionObject.ObjectType.None:
                return;

            case InteractionObject.ObjectType.Gathering:
                {
                    GatheringObject gatheringObj = (GatheringObject)mInteractionObj;
                    int index = gatheringObj.mIndex;
                    GatherData.State state = UserData.instance.mGatherDataList[index].GetState();

                    
                    if(state == GatherData.State.Full)
                    {
                        mAnimator.Play("AxingTree"); // 채칩 애니메이션 추가해서 변경해주고
                        ActionCheck();
                    }
                    else if (state == GatherData.State.Half)
                    {
                        mAnimator.Play("AxingTree"); // 열매없어도 벌목 가능하면 벌목하고.
                        ActionCheck();
                    }
                    break;
                }

            case InteractionObject.ObjectType.Npc:
                ActionCheck();

                break;
            case InteractionObject.ObjectType.Logging:
                {
                    LoggingObject loggingObj = (LoggingObject)mInteractionObj;
                    int index = loggingObj.mIndex;
                    LoggingData.State state = UserData.instance.mLoggingDataList[index].GetState();

                    if(state != LoggingData.State.Empty)
                    {
                        mAnimator.Play("AxingTree");
                        ActionCheck();
                    }
                    break;
                }
            case InteractionObject.ObjectType.Doorway:
                if (mGoToZoneName == ZoneData.Name.House)
                {
                    GameManager.Instance.HouseSceneLoad();
                }
                if (mGoToZoneName == ZoneData.Name.Field)
                {

                    GameManager.Instance.GameSceneLoad();
                }
                break;

            case InteractionObject.ObjectType.Mining:
                {
                    MiningObject miningObj = (MiningObject)mInteractionObj;
                    int index = miningObj.mIndex;
                    MiningData.State miningState = UserData.instance.mMiningDataList[index].GetState();

                    if(miningState != MiningData.State.Empty)
                    {
                        mAnimator.Play("HammeringTree");
                        ActionCheck();
                    }
                    else
                    {
                        Debug.Log("비어있음");
                    }
                    break;
                }

            case InteractionObject.ObjectType.Fishing:
                {
                    FishingObject fishing = (FishingObject)mInteractionObj;
                    int fishingIndex = fishing.mIndex;
                    FishingData.State fishingState = UserData.instance.mFishingDataList[fishingIndex].GetState();
                    
                    if (fishingState == FishingData.State.None)
                    {
                        // 필요 아이템 체크 및 있으면 1개 소모
                        if (HasNeedItem(11))
                        {
                            mAnimator.Play("FishingTree_0"); // 낚시 던지기 

                            ActionCheck();
                        }
                        else
                        {
                            UIManager.instance.SetSystemMessage("미끼 아이템이 부족합니다.");
                        }
                    }
                    else if (fishingState >= FishingData.State.Start)
                    {
                        mAnimator.SetBool("fishingFinish", true);
                        mAnimator.Play("FishingTree_1"); // 낚시대 당기기 

                        ActionCheck();
                    }
                    else
                    {
                        /* nothing */
                    }
                    break;
                }

            case InteractionObject.ObjectType.Farming:
                {
                    FarmObject farm = (FarmObject)mInteractionObj;
                    int farmIndex = farm.mIndex;
                    FarmData.State farmState = UserData.instance.mFarmDataList[farmIndex].GetState();
                    if (farmState == FarmData.State.None)
                    {
                        mAnimator.Play("FarmTree");

                        ActionCheck();

                    }
                    else if (farmState == FarmData.State.Base)
                    {
                        // 필요 아이템 체크 및 있으면 1개 소모
                        if (HasNeedItem(10))
                        {
                            mAnimator.Play("FarmTree_2");

                            ActionCheck();
                        }
                        else
                        {
                            UIManager.instance.SetSystemMessage("씨앗 아이템이 부족합니다.");
                        }
                    }
                    else if (farmState == FarmData.State.Done)
                    {
                        mAnimator.Play("FarmTree");

                        ActionCheck();
                    }
                    else
                    {
                        /* nothing */
                    }
                    break;
                }
        }
    }

    public bool HasNeedItem(int id)
    {
       if(UserData.instance.GetItemByID(id) != null)
        {
            UserData.instance.RemoveItemByItemIndex(id); // 아이템 차감
            UIManager.instance.GetInventory().UpdateInventoryList(); //인벤 갱신

            return true;
        }
        return false;
    }

    public void ActionCheck()
    {
        if (mInteractionObj.mType == InteractionObject.ObjectType.Gathering)
        {
            GatheringObject gatheringObject = (GatheringObject)mInteractionObj;
            gatheringObject.UpdateState();
        }

        if (mInteractionObj.mType == InteractionObject.ObjectType.Logging)
        {
            LoggingObject logging = (LoggingObject)mInteractionObj;
            logging.UpdateState();
        }

            if (mInteractionObj.mType == InteractionObject.ObjectType.Mining)
        {
           MiningObject mining = (MiningObject)mInteractionObj;
            mining.UpdateState();

        }

        if (mInteractionObj.mType == InteractionObject.ObjectType.Farming)
        {
            FarmObject farm = (FarmObject)mInteractionObj;
            farm.UpdateState();

        }

        if (mInteractionObj.mType == InteractionObject.ObjectType.Fishing)
        {
            FishingObject fishing = (FishingObject)mInteractionObj;
            fishing.UpdateState();

        }

        if (mInteractionObj.mType == InteractionObject.ObjectType.Npc)
        {
            NpcObject npc = (NpcObject)mInteractionObj;
            npc.UpdateState();
        }
    }

    public Animator GetAnimator()
    {
        return mAnimator;
    }


    public void GameSceneLoad()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void HouseSceneLoad()
    {
        SceneManager.LoadScene("HouseScene");
    }
}
