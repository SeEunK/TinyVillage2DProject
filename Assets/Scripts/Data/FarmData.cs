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
        None, // �����
        Base, // �� ����
        Wait,  // ���� ����
        Done // ���� �Ϸ�
    }

    [SerializeField]    
    private State mState = State.None;

   
    private double mStartTime = 0; // ���� ���� ���� �ð�

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
        if(mState != State.Wait)
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

}
