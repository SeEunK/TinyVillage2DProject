using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class Slime : Enemy
{
    public Transform mStartPos;
    public Transform mTarget;
    
    public enum State { Idle, AttackIn,AttackOut, Move , GoToHome}
    public State mState = State.Idle;
    [SerializeField]
    private float mAttackRadius;
    [SerializeField]
    private float mMoveRadius;

    [SerializeField]
    private float mAreaRadius;

    public Animator mAnimator;

    public enum Direction { Up, Down, Left, Right }
    public Direction mDirection;

    void Start()
    {
        mHP = 10;
        mName = "그린슬라임";
        mMoveSpeed = 1.0f;
        mBaseAttack = 1;
        mAnimator = GetComponent<Animator>();

        mTarget = GameObject.FindWithTag("Player").transform;
        mAreaRadius = 10.0f;
        mMoveRadius = 4.0f;
        mAttackRadius = 2.0f;

        mDirection = Direction.Down;
        mState = State.Idle;
    }

    private void Update()
    {
        UpdateState();
        UpdateAction();
      
    }

    public void UpdateState()
    {
        bool isAttackAtion = (mState == State.AttackIn || mState == State.AttackOut);

        // 공격중일때는 상태 변경을 막기 위해 return;
        if(isAttackAtion)
        {
            return;
        }
        //타겟이 공격범위안에 있다면 공격상태로 바꾼다.
        if (Vector3.Distance(mTarget.position, transform.position) < mAttackRadius)
        {
         
                mState = State.AttackIn;
      

        } 
        // 스폰 지점 으로 부터 내 위치가 허용 범위일때,
        else if(Vector3.Distance(mStartPos.position, transform.position) < mAreaRadius)
        {
            // 타겟이 이동가능범위안에왔을때 이동(쫒아가기) 상태로 바꾼다
            if (Vector3.Distance(mTarget.position, transform.position) < mMoveRadius)
            {

                mState = State.Move;
            }
            else
            {   // player die 일때도 gotohome 추가해야함.
                if (transform.position != mStartPos.position)
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
                    break;
                }
            case State.AttackIn:
                {
                    // attackin -> out
                    break;
                }
            case State.AttackOut:
                {
                    // attackout -> idle
                    break;
                }
            case State.Move:
                {
                    break;
                }
            case State.GoToHome:
                {
                    break;
                }
        }
    }

    public override void Attacked()
    {
        base.Attacked();
        mHP -= 1;

    }

 
    public void CheckDistance()
    {

        if(Vector3.Distance (mTarget.position, transform.position) <= mMoveRadius &&
            Vector3.Distance(mTarget.position, transform.position) > mAttackRadius)
        {
            CheckDirection(transform.position, mTarget.position);
            SetDirection(mDirection);
            mAnimator.SetBool("IsMove", true);
            transform.position = Vector3.MoveTowards(transform.position, mTarget.position, mMoveSpeed * Time.deltaTime);
        }
        else
        {
            mAnimator.SetBool("IsMove", false);
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
}
