using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float mSpeed = 5.0f;
    
    private Vector2 mMovement;
    private Rigidbody2D mRigid;
    private Animator mAnimator;
    
    public InteractionObject mInteractionObj = null;
    public FarmData.State mFarmObjectState = FarmData.State.None;

    private void Awake()
    {
        mRigid= GetComponent<Rigidbody2D>();
        mAnimator= GetComponent<Animator>();
    }
    private void OnMovement(InputValue value)
    {
        mMovement = value.Get<Vector2>();
        if (mMovement.x != 0 || mMovement.y != 0)
        {
            mAnimator.SetFloat("X", mMovement.x);
            mAnimator.SetFloat("Y", mMovement.y);

            mAnimator.SetBool("IsWalking", true);
        }
        else
        {
            mAnimator.SetBool("IsWalking", false);
        }

    }

    private void FixedUpdate()
    {
        //variant 1
         mRigid.MovePosition(mRigid.position + mMovement * mSpeed * Time.fixedDeltaTime);

        // varivant 2
        //if (movement.x != 0 || movement.y != 0)
        //{
        //    mRigid.velocity = movement * mSpeed;
        //}

        // varivant 3
        //mRigid.AddForce(movement * mSpeed);
    }

    public void SetInteractionObject(InteractionObject interactionObj, FarmData.State state)
    {
        mInteractionObj = interactionObj;
        mFarmObjectState = state;


    }

    public void Interaction()
    {
        //ObjectType { None, Fishing, Mining, Gathering, Logging, Farming }
       
        switch (mInteractionObj.mType)
        {
            case InteractionObject.ObjectType.None:
                return;
            
            case InteractionObject.ObjectType.Fishing: 

            case InteractionObject.ObjectType.Mining:

            case InteractionObject.ObjectType.Gathering:

            case InteractionObject.ObjectType.Logging:

            case InteractionObject.ObjectType.Farming:


                //mAnimator.SetBool("IsFarming", true);
                //mAnimator.SetInteger("FarmLevel", mInteractionStep);
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

                }

                return;

        }
    }


    public void ActionCheck()
    {

        if (mInteractionObj.mType == InteractionObject.ObjectType.Farming)
        {
            FarmObject farm = (FarmObject)mInteractionObj;
            farm.UpdateState();
               
        }
    }
 
}
