using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class LoggingData 
{
    public enum State
    {
        Empty, // 빈상태 - 벌목 불가.
        Full,  // 가득참 - 벌목 가능
        Half   // 반남음(밑동) - 벌목 가능

    }

    [SerializeField]
    private State mState = State.Full;


    private double mStartTime = 0; // 리 스폰 시작 시간

    public State GetState()
    {
        return mState;
    }

    public void SetState(State state)
    {
        mState = state;

        switch (state)
        {
            case State.Empty:
                mStartTime = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                break;

        }
    }

    public double GetStartTime()
    {
        return mStartTime;
    }

    public bool IsRespawnComplete(double respawnTime)
    {
        if (mState != State.Empty)
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
