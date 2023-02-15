using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class InteractionObject : MonoBehaviour
{
    public enum ObjectType { None, Fishing, Mining, Gathering, Logging, Farming }

    public ObjectType mType = ObjectType.None;
    public int mStep = -1;
    public int mMaxStep = 4;
    public int mWaitStep = 2;

    public List<Sprite> mSprites = new List<Sprite>();
    public SpriteRenderer mRenderer;

    public float mTimer = 0.0f;
    public float mTurm = 0.0f;

    public Image mImgWait = null;

    private void Start()
    {
        mRenderer = GetComponent<SpriteRenderer>();
        mImgWait.fillAmount = mTimer;
        mStep = 0;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject playerObject = collision.gameObject;
            PlayerMovement player = playerObject.GetComponent<PlayerMovement>();
            
            player.SetInteractionObject(this, mStep);
            
            
            UIManager.instance.FarmActionButtonEnable(true, mStep);
            


        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject playerObject = other.gameObject;
            PlayerMovement player = playerObject.GetComponent<PlayerMovement>();

            UIManager.instance.SetFarmActionButton(false);
            player.SetInteractionObject(null, -1);

        }
    }

    public void UpdateState()
    {
      
        if (mStep < mWaitStep)
        {
            AddStep();
            if(mStep == mWaitStep)
            {
                mImgWait.enabled = true;
            }
        }
        if (mStep == mMaxStep)
        {
            SetStep(0);
            mImgWait.fillAmount = 0.0f;
            mImgWait.enabled = false;
        }
      
        mRenderer.sprite = mSprites[mStep];
    }
    public void AddStep()
    {
        if (mStep < mMaxStep)
        {
           
            mStep += 1;
        }
    }

    public void SetStep(int step)
    {
        mStep= step;
    }

    private void Update()
    {
        if(mStep >=  mWaitStep && mStep < mMaxStep)
        {
            mTimer += Time.deltaTime;
            mImgWait.fillAmount = mTimer/mTurm;
            if (mTimer >= mTurm)
            {
                mTimer = 0;
                if (mStep < mMaxStep)
                {
                    AddStep();
                    mRenderer.sprite = mSprites[mStep];
                }
               
            }
        }
    }
}
