using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float mMoveSpeed;
    public string mName;
    public int mHP;
    
    public int mBaseAttack;
    public Vector3 mStartPos;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void Attacked()
    {

    }
}
