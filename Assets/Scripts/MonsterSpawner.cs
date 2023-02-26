using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner Instance = null;
    
    public List<GameObject> mSlimesPool = new List<GameObject>();
    public GameObject mSlimePrefab = null;
    public List<Vector3> mSlimeSpawnPosList = new List<Vector3>();

    public float mTimer = 0.0f;
    private float mSpawnerDelayTime = 3.0f;
    public bool mIsPuse = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            CreateSlimePool();

        }
        else
        {
            Destroy(this.gameObject);
        }
    }



    private void CreateSlimePool()
    {
        for (int i = 0; i < mSlimeSpawnPosList.Count; i++){
            GameObject newSlime = CreateSlime(mSlimeSpawnPosList[i]);
            newSlime.transform.parent = this.transform;
            mSlimesPool.Add(newSlime);

        }
    }


    private void Update()
    {
        if (mIsPuse == false)
        {
            if (mSlimesPool.Count > 0)
            {
                mTimer += Time.deltaTime;
                if (mTimer >= mSpawnerDelayTime)
                {
                    mTimer = 0.0f;
                    PopSlime();
                }
            }
        }
    }

    public void SetPuse(bool value)
    {
        mIsPuse= value;
    }

    public GameObject PopSlime()
    {

        if (mSlimesPool.Count > 0)
        {
            GameObject temp = mSlimesPool[0];
            GameManager.Instance.mSlimeList.Add(temp);
            mSlimesPool.Remove(temp);
            Slime slime = temp.GetComponent<Slime>();
            
            temp.SetActive(true);
            return temp;
        }
        return null;

    }


    public void PushSlime(GameObject slime)
    {
     
        GameManager.Instance.mSlimeList.Remove(slime);
        slime.SetActive(false);
        mSlimesPool.Add(slime);

    }

    public GameObject CreateSlime(Vector3 spawnPos)
    {
        GameObject slimeObject = Instantiate(mSlimePrefab);
        slimeObject.SetActive(false);

        Slime slime = slimeObject.GetComponent<Slime>();
        slime.mSpawner = this;
        slime.Init(spawnPos);
        
        return slimeObject;
    }
}
