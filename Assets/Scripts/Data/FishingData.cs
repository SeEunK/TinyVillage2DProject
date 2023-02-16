using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FishingData
{
    public enum State
    {
        None,   // �����. (���� ���� ��)
        Start,  // ���� ���� --> ���� �ð� �� ����
        Bait    // ����Ⱑ ������
      
    }

    [SerializeField]
    private State mState = State.None;

    private double mStartTime = 0; // ���� ���� �ð�
    private double mBaitTime = 0;  // ����Ⱑ �̳��� �� �ð� 

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


    // ����Ⱑ ���� ���� �ð� �� �������� ���⼺�� ó��.
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