using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    private Vector2 movement;
    private Rigidbody2D mRigid;
    float mSpeed = 5.0f;
    private void Awake()
    {
        mRigid= GetComponent<Rigidbody2D>();
    }
    private void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        // variant 1
        // mRigid.MovePosition(mRigid.position + movement * mSpeed * Time.fixedDeltaTime);

        // varivant 2
        //if (movement.x != 0 || movement.y != 0)
        //{
        //    mRigid.velocity = movement * mSpeed;
        //}

        // varivant 3
        mRigid.AddForce(movement * mSpeed);
    }
}
