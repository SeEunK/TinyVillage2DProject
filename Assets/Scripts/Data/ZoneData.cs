using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ZoneData 
{
    public enum Name
    {
        Field,      // 필드
        House       // 집안
      
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
