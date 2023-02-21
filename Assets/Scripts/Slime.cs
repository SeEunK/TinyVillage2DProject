using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    public Transform mStartPos;
    public Transform mTarget;

    [SerializeField]
    private float mAttackRadius;
    [SerializeField]
    private float mMoveRadius;

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
        mMoveRadius = 4.0f;
        mAttackRadius = 2.0f;

        mDirection = Direction.Down;
    }

    private void Update()
    {
        CheckDistance();
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
        if(pos.x == targetPos.x && pos.y < targetPos.y)
        {
            mDirection = Direction.Up;
        }
        else if (pos.x == targetPos.x && pos.y > targetPos.y)
        {
            mDirection = Direction.Down;
        }
        else if (pos.y == targetPos.y && pos.x > targetPos.x)
        {
            mDirection = Direction.Left;
        }
        else if (pos.y == targetPos.y && pos.x > targetPos.x)
        {
            mDirection = Direction.Right;
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
