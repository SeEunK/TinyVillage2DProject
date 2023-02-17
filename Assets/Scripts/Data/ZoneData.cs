using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ZoneData 
{
    public enum Name
    {
        Field,      // �ʵ�
        House       // ����
      
    }

    [SerializeField]
    private Name mName = Name.Field;


    public Name GetZoneName()
    {
        return mName;
    }

    public void SetZoneName(Name zoneName)
    {
        mName = zoneName;
    }
}
