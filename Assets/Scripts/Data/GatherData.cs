using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GatherData
{
    public enum State
    {
        None,  // 빈상태 - 채집 / 벌목 불가.
        Full,  // 가득참 - 열매 채집 가능
        Half   // 나무만 남음 - 벌목 가능.

    }

    [SerializeField]
    private State mState = State.Full;


    private double mStartTime = 0; // 열매 자라기 시작 시간

    public State GetState()
    {
        return mState;
    }

    public void SetState(State state)
    {
        mState = state;

        switch (state)
        {
            case State.None:
            case State.Half:
                mStartTime = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                break;

        }
    }

    public double GetStartTime()
    {
        return mStartTime;
    }

    public bool IsGrowComplete(double respawnTime)
    {
        if (mState != State.None)
        {
            return false;
        }

        double currTime = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        if (mStartTime + respawnTime <= currTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsBerryGrowComplete(double respawnTime)
    {
        if (mState != State.Half)
        {
            return false;
        }
        double currTime = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        if (mStartTime + respawnTime <= currTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
