using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData instance = null;

    
    public List<FarmData> mFarmDataList = new List<FarmData>();


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }

        }
        
    }

    private void Start()
    {
         for(int i= 0; i<6; i++)
        {
            mFarmDataList.Add(new FarmData());

        }
    }


}
