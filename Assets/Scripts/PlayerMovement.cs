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
    public FarmData.State mFarmObjectState = FarmData.State.None;
    public FishingData.State mFishingObjectState = FishingData.State.None;

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

    private IEnumerator WaitAttackAction()
    {
        Debug.Log("WaitAttackAction");

        Time.timeScale = 1;

        yield return new WaitForSeconds(0.75f);

        Debug.Log("Wait_end");
        mAnimator.SetBool("IsAttack", false);
        mState = State.Active;
    }

    public void SetInteractionObject(InteractionObject interactionObj)
    {
        mInteractionObj = interactionObj;

        if (interactionObj == null)
        {
            mFarmObjectState = FarmData.State.None;
            mFishingObjectState = FishingData.State.None;
            mZoneName = ZoneData.Name.Field;
            return;
        }

        //mFarmObjectState = state;
        switch (interactionObj.mType)
        {
            case InteractionObject.ObjectType.Farming:
                FarmObject farm = (FarmObject)mInteractionObj;
                mFarmObjectState = UserData.instance.mFarmDataList[farm.mIndex].GetState();

                break;
            case InteractionObject.ObjectType.Fishing:
                FishingObject fishing = (FishingObject)mInteractionObj;
                mFishingObjectState = UserData.instance.mFishingDataList[fishing.mIndex].GetState();
                break;

            case InteractionObject.ObjectType.Doorway:
                DoorwayObject door = (DoorwayObject)mInteractionObj;
                mGoToZoneName = door.GetGoToZone();
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

            case InteractionObject.ObjectType.Mining:

            case InteractionObject.ObjectType.Gathering:

            case InteractionObject.ObjectType.Logging:

            case InteractionObject.ObjectType.Npc:
                ActionCheck();

                break;

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
            case InteractionObject.ObjectType.Fishing:

                if (mFishingObjectState == FishingData.State.None)
                {
                    mAnimator.Play("FishingTree_0"); // 낚시 던지기 

                    ActionCheck();

                }
                else if (mFishingObjectState >= FishingData.State.Start)
                {
                    mAnimator.Play("FishingTree_1"); // 낚시대 땅기기 

                    ActionCheck();

                }
                else
                {
                    /* nothing */
                }

                return;


            case InteractionObject.ObjectType.Farming:

                if (mFarmObjectState == FarmData.State.None)
                {
                    mAnimator.Play("FarmTree");

                    ActionCheck();

                }
                else if (mFarmObjectState == FarmData.State.Base)
                {
                    mAnimator.Play("FarmTree_2");

                    ActionCheck();

                }
                else if (mFarmObjectState == FarmData.State.Done)
                {
                    mAnimator.Play("FarmTree");

                    ActionCheck();
                }
                else
                {
                    /* nothing */
                }

                return;

        }
    }
    public void GameSceneLoad()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void HouseSceneLoad()
    {
        SceneManager.LoadScene("HouseScene");
    }
    public void ActionCheck()
    {

        if (mInteractionObj.mType == InteractionObject.ObjectType.Farming)
        {
            FarmObject farm = (FarmObject)mInteractionObj;
            farm.UpdateState();

        }

        if (mInteractionObj.mType == InteractionObject.ObjectType.Fishing)
        {
            FishingObject fishing = (FishingObject)mInteractionObj;

            // FishingData.State state = UserData.instance.mFishingDataList[fishing.mIndex].GetState();

            // start --> Bait 변경은 fish update 에서 시간 체크 후 자동으롷 addStep 해주는데, 액션 누른경우 낚시 캔슬 
            //if (state == FishingData.State.Start)
            //{
            //    UserData.instance.mFishingDataList[fishing.mIndex].SetState(FishingData.State.None);
            //    return;
            //}

            fishing.UpdateState();

        }

        if (mInteractionObj.mType == InteractionObject.ObjectType.Npc)
        {
            NpcObject npc = (NpcObject)mInteractionObj;

            npc.UpdateState();
        }
    }

}
