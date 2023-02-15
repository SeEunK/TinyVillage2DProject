using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingData : MonoBehaviour
{
    public enum State
    {
        None,   // 시작 전
        Start,  // 낚시 시작
        Wait,   // 찌 물기 대기 
        Hit     // 물고기 낚음
    }

    public State mState = State.None;



}
