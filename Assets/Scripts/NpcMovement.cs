using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NpcMovement : MonoBehaviour
{
    [SerializeField] 
    private float mMoveSpeed;
    private Rigidbody2D mRigidbody;
    public bool isMove = false;

    private Vector3 mPrevPos;
    [SerializeField]
    private float mMoveTime = 3.0f;
    [SerializeField]
    private float mWaitTime = 2.0f;
    public float mMoveTimer = 0.0f;
    public float mWaitTimer = 0.0f;

    private Animator mAnimator;
    public enum Direction { Up,Down,Left,Right}
    public Direction mDirection = Direction.Down;

    public bool mIsPause = false;

    private void Start()
    {
        mRigidbody= GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        
        mMoveSpeed = 3.0f;
        mMoveTimer = mMoveTime;
        mWaitTimer = mWaitTime;
        mPrevPos = transform.position;
        mIsPause = false;
    }

    private void Update()
    {
        if (!mIsPause)
        {
            if (isMove)
            {
                mMoveTimer -= Time.deltaTime;
                SetDirection(mDirection);


                switch (mDirection)
                {
                    case Direction.Up:
                        mRigidbody.velocity = new Vector2(0, mMoveSpeed);

                        SetDirection(mDirection);
                        break;
                    case Direction.Down:

                        mRigidbody.velocity = new Vector2(0, -mMoveSpeed);

                        SetDirection(mDirection);
                        break;
                    case Direction.Left:

                        mRigidbody.velocity = new Vector2(-mMoveSpeed, 0);
                        SetDirection(mDirection);
                        break;
                    case Direction.Right:

                        mRigidbody.velocity = new Vector2(mMoveSpeed, 0);
                        SetDirection(mDirection);
                        break;

                }



                if (mMoveTimer < 0)
                {
                    mAnimator.SetBool("IsMove", false);
                    isMove = false;
                    mWaitTimer = mWaitTime;

                }
            }
            else
            {
                mWaitTimer -= Time.deltaTime;

                mRigidbody.velocity = Vector2.zero;
                if (mWaitTimer < 0)
                {
                    mAnimator.SetBool("IsMove", true);
                    ChooseDirection();
                    isMove = true;
                    mMoveTimer = mMoveTime;
                }
            }
        }
    }

    public void Pause()
    {
        mIsPause = true;
        mRigidbody.velocity = Vector2.zero;
        mMoveTimer = mMoveTime;
        mWaitTimer = mWaitTime;
        isMove = false;

    }
    public void Resume()
    {
        mIsPause = false;
        isMove = true;

    }

    public void ChooseDirection()
    {
        mDirection = (Direction)Random.Range(0, 4);
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(isMove == false)
        {
            return;
        }
        if (collision.CompareTag("Wall"))
        {
            mAnimator.SetBool("IsMove", false);
            isMove = false;
            mWaitTimer = mWaitTime;
        }
    }
}
