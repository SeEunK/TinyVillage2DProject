using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FishingData
{
    public enum State
    {
        None,   // 빈상태. (낚시 시작 전)
        Start,  // 낚시 시작 --> 랜덤 시간 후 물기
        Bait    // 물고기가 물었음
      
    }

    [SerializeField]
    private State mState = State.None;

    private double mStartTime = 0; // 낚시 던진 시간
    private double mBaitTime = 0;  // 물고기가 미끼를 문 시간 

    public State GetState()
    {
        return mState;
    }

    public void SetState(State state)
    {
        mState = state;

        switch (mState)
        {
                case State.Start:
                mStartTime = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                mBaitTime = 0;
                break;

                case State.Bait:
                mStartTime = 0;
                mBaitTime = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                break;

                case State.None:
                mStartTime = 0;
                mBaitTime = 0;
                break;

               
        }
    }

    public bool IsBait(float waitTime)
    {
        if (mState != State.Start)
        {
            return false;
        }

        double currTime = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        if (mStartTime + waitTime <= currTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    // 물고기가 물고 일정 시간 내 낚았을때 낚기성공 처리.
    public bool IsHookSuccess(float waitTime)
    {
        if (mState != State.Bait)
        {
            return false;
        }

        double currTime = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        if (mBaitTime + (double)waitTime >= currTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}