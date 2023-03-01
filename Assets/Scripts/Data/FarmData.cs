using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

[Serializable]
public class FarmData
{

    public enum State
    {
        None, // 빈상태
        Base, // 밭 간후
        Wait,  // 성장 진행
        Grow,  // 성장중
        Done // 성장 완료
    }

    [SerializeField]    
    private State mState = State.None;

   
    private double mStartTime = 0; // 성장 진행 시작 시간

    public State GetState()
    {
        return mState;
    }

    public void SetState(State state)
    {
        mState = state;

        switch(state)
        {
            case State.None:
                mStartTime = 0;
                break;
            case State.Base:
                mStartTime = 0;
                break;
            case State.Wait:
            case State.Grow:
                mStartTime = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                break;
        }
    }

    public double GetStartTime()
    {
        return mStartTime;
    }

    public bool IsGrowComplete(double growTime)
    {
        if(mState != State.Grow)
        {
            return false;
        }

        double currTime = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        if (mStartTime + growTime <= currTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsHalfGrow(double growTime)
    {
        if (mState != State.Wait)
        {
            return false;
        }

        double currTime = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        double halfTime = growTime / 2;
        if (mStartTime + halfTime <= currTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
