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
        mName = "�׸�������";
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

        // �������϶��� ���� ������ ���� ���� return;
        if(isAttackAtion)
        {
            return;
        }
        //Ÿ���� ���ݹ����ȿ� �ִٸ� ���ݻ��·� �ٲ۴�.
        if (Vector3.Distance(mTarget.position, transform.position) < mAttackRadius)
        {
         
                mState = State.AttackIn;
      

        } 
        // ���� ���� ���� ���� �� ��ġ�� ��� �����϶�,
        else if(Vector3.Distance(mStartPos.position, transform.position) < mAreaRadius)
        {
            // Ÿ���� �̵����ɹ����ȿ������� �̵�(�i�ư���) ���·� �ٲ۴�
            if (Vector3.Distance(mTarget.position, transform.position) < mMoveRadius)
            {

                mState = State.Move;
            }
            else
            {   // player die �϶��� gotohome �߰��ؾ���.
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
