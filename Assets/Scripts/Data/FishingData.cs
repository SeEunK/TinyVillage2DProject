using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingData : MonoBehaviour
{
    public enum State
    {
        None,   // ���� ��
        Start,  // ���� ����
        Wait,   // �� ���� ��� 
        Hit     // ����� ����
    }

    public State mState = State.None;



}
