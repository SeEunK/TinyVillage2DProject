using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Slime : Enemy
{
   
    public Transform mTarget;
    
    public enum State { Idle, AttackIn, AttackOut, Move , GoToHome, Hit, Die }
    public State mState = State.Idle;

    [SerializeField]
    private float mAttackRadius;
    [SerializeField]
    private float mMoveRadius;

    [SerializeField]
    private float mAreaRadius;


    private float mThrust = 2.0f;
    private float mKnockTime = 0.5f;

    public Animator mAnimator;

    public enum Direction { Up, Down, Left, Right }
    public Direction mDirection;

    public float mAttackSpeed;
    public float mAttackActionTime;
    public float mHitActionTime;
    public float mDieActionTime;

    public Vector3 mAttackTargetPos;
    public Vector3 mAttackStartPos;
    public Vector3 mHitOppositePos;

    public Image mImgHp = null;
    
    public int mMaxHP = 0;
    public TMP_Text mHpCount = null;

    public MonsterSpawner mSpawner = null;


    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mTarget = GameObject.FindWithTag("Player").transform;
        
    }

    public void Init(Vector3 startPos)
    {
        mHP = 10;
        mName = "그린슬라임";
        mMoveSpeed = 1.0f;
        mBaseAttack = 1;
        mAttackSpeed = 3.0f;

        mMaxHP = mHP;
        mImgHp.enabled = true;
        mImgHp.fillAmount = (float)mHP / (float)mMaxHP;
        mHpCount.text = string.Format("{0}/{1}", mHP, mMaxHP);

        mAreaRadius = 10.0f;
        mMoveRadius = 4.0f;
        mAttackRadius = 1.0f;
        mAttackActionTime = 1.0f;
        mHitActionTime = 1.0f;
        mDieActionTime  = 1.0f;

        mDirection = Direction.Down;
        mState = State.Idle;
        mHitOppositePos = Vector3.zero;
        mStartPos = startPos;
        Reposition();
    }

    private void Update()
    {
        UpdateState();
        UpdateAction();
      
    }

    public void UpdateState()
    {
        bool isDead = (mState == State.Die);
        bool isAttackAtion = (mState == State.AttackIn || mState == State.AttackOut);
        bool isHitAction = (mState == State.Hit);

        // 공격중일때는 상태 변경을 막기 위해 return;
        if(isAttackAtion || isHitAction || isDead)
        {
            return;
        }
        //타겟이 공격범위안에 있다면 공격상태로 바꾼다.
        if (Vector3.Distance(mTarget.position, transform.position) < mAttackRadius)
        {
        
            mState = State.AttackIn;
            mAttackTargetPos = mTarget.position;
            mAttackStartPos = this.transform.position;
      

        } 
        // 스폰 지점 으로 부터 내 위치가 허용 범위일때,
        else if(Vector3.Distance(mStartPos, transform.position) < mAreaRadius)
        {
            // 타겟이 이동가능범위안에왔을때 이동(쫒아가기) 상태로 바꾼다
            if (Vector3.Distance(mTarget.position, transform.position) < mMoveRadius)
            {

                mState = State.Move;
            }
            else
            {   // player die 일때도 gotohome 추가해야함.
                // 시작 포지션과 현재 위치가 1 이상 벌어진 경우까지만 gotohome (1 정도는 허용 하기위함)
                if (Vector3.Distance(transform.position, mStartPos) > 1.0f)
                {
                    mState = State.GoToHome;
                }
                else
                {
                    mState = State.Idle;
                }
            }
        }
        else 
        {
            mState = State.GoToHome;
        }
        
       
    }


    public void UpdateAction()
    {
        switch (mState)
        {
            case State.Idle:
                {
                    mAnimator.SetBool("IsMove", false);
                    break;
                }
            case State.AttackIn:
                {
                    if(!IsAttackAtionEnd(Time.deltaTime))
                    {
                        CheckDirection(transform.position, mTarget.position);
                        SetDirection(mDirection);
                        mAnimator.SetBool("IsAttack", true);
                    }
                    else
                    {
                        mAnimator.SetBool("IsAttack", false);
                        mState = State.Idle;
                    }

                   
                    break;
                }
            case State.AttackOut:
                {
                    // attackout -> idle
                    if (transform.position == mAttackStartPos)
                    {
                        mState = State.Idle;
                    }
                    else // 공격 시작 지점으로 빽
                    {
                        CheckDirection(transform.position, mAttackStartPos);
                        SetDirection(mDirection);
                        mAnimator.SetBool("IsAttack", false);
                        //transform.position = Vector3.MoveTowards(transform.position, mAttackStartPos, mAttackSpeed * Time.deltaTime);
                    }
                    break;
                }
            case State.Move:
                {
                    CheckDirection(transform.position, mTarget.position);
                    SetDirection(mDirection);
                    mAnimator.SetBool("IsMove", true);
                    transform.position = Vector3.MoveTowards(transform.position, mTarget.position, mMoveSpeed * Time.deltaTime);
                    break;
                }
            case State.GoToHome:
                {
                    CheckDirection(transform.position, mStartPos);
                    SetDirection(mDirection);
                    mAnimator.SetBool("IsMove", true);
                    transform.position = Vector3.MoveTowards(transform.position, mStartPos, mMoveSpeed * Time.deltaTime);
                    break;
                }
            case State.Hit:
                {
                    transform.position = Vector3.MoveTowards(transform.position, mHitOppositePos, mThrust * Time.deltaTime);
                    // knockback 종료 대기 후 상태 변경.
                    if (IsHitAtionEnd(Time.deltaTime))
                    { 
                        SetState(State.Idle);
                    }
                    break;
                }
            case State.Die:
                {
                    if (IsDieAtionEnd(Time.deltaTime))
                    {
                        mSpawner.PushSlime(this.gameObject);
                        Init(mStartPos);
                    }
                    break;
                }
        }
    }

    public void Attack()
    {
        if (mTarget == null)
        {
            return;
        }
        if (Vector3.Distance(mTarget.position, transform.position) < 1.0f)
        {
            mTarget.GetComponent<PlayerMovement>().OnDamege(mBaseAttack);
        }
    }


    public override void Attacked()
    {
        base.Attacked();
        if (mState != State.Die)
        {
            // 공격중인 모션 종료하고,
            mAnimator.SetBool("IsAttack", false);

            // knockback 으로 밀려날 좌표를 저장해두고
            Vector2 opposite = (mTarget.position - this.transform.position);
            Debug.LogFormat("1. {0}", opposite);
            opposite = opposite.normalized * mThrust;
            Debug.LogFormat("2. {0}", opposite);
            mHitOppositePos = transform.position - (Vector3)opposite;

            // 데미지 차감
            mHP -= 1;
            mImgHp.fillAmount = (float)mHP / (float)mMaxHP;
            mHpCount.text = string.Format("{0}/{1}", mHP, mMaxHP);



            if (mHP <= 0)
            {
                SetState(State.Die);
                Die();
            }
            else
            {
                // 상태 hit 전환
                SetState(State.Hit);
            }
        }

    }

    public void Die()
    {
        
        mImgHp.enabled = false;
        mAnimator.SetTrigger("IsDead");
        QuestManager.instance.AddAccCount(QuestData.QuestConditionType.MonsterKill, 1);

        Sprite[] itemImages = Resources.LoadAll<Sprite>("Sprites/Icon");
        Sprite itemIcon = itemImages[4];
        ItemData getItem = new ItemData(4, "초록색 이파리", itemIcon, 99, 30);
        UserData.instance.AddItem(getItem);

    }

    public bool IsDieAtionEnd(float time)
    {
        mDieActionTime -= time;
        if (mDieActionTime < 0)
        {
            mDieActionTime = 1.0f;
            return true;
        }
        else
        {
            return false;   
        }
    }

    public bool IsHitAtionEnd(float time)
    {
        mHitActionTime -= time;

        if (mHitActionTime < 0)
        {
            SetState(State.Idle);
            mHitOppositePos = Vector3.zero;
            mHitActionTime = 1.0f;
            return true;
        }
        else
        {
            return false;
        }
    }


    private IEnumerator WaitKnockTime()
    {
        yield return new WaitForSeconds(mKnockTime);
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        SetState(Slime.State.Idle);
        Debug.Log(mState);
    }

    public void SetState(State state)
    {
        mState = state;
    }
 
    public bool IsAttackAtionEnd(float time)
    {
        mAttackActionTime -= time;

        if (mAttackActionTime < 0)
        {
            mAnimator.SetBool("IsAttack", false);
            mState = State.AttackOut;
            mAttackActionTime = 2.0f;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckDirection (Vector3 pos, Vector3 targetPos)
    {

        Vector3 direction = (targetPos - pos).normalized;
        
        if(direction.y <= 0)
        {
            if(Mathf.Abs(direction.x) <= 0.5f)
            {
                mDirection = Direction.Down;
            }
            else
            {
                if(direction.x < 0)
                {
                    mDirection = Direction.Left;
                }
                else if(direction.x > 0)
                {
                    mDirection= Direction.Right;
                }
                else
                {
                    mDirection = Direction.Down;
                }
            }
        }
        else 
        {
            if (Mathf.Abs(direction.x) <= 0.5f)
            {
                mDirection = Direction.Up;
            }
            else
            {
                if (direction.x < 0)
                {
                    mDirection = Direction.Left;
                }
                else if (direction.x > 0)
                {
                    mDirection = Direction.Right;
                }
                else
                {
                    mDirection = Direction.Up;
                }
            }
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

    public void Reposition()
    {
        transform.position = mStartPos;

    }
}
