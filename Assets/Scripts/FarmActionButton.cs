using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmActionButton : MonoBehaviour
{
    public PlayerMovement player;

    private void Start()
    {
        
    }
    public void FarmAction()
    {
        player.Interaction();
    }

    public void FishingAction()
    {
        player.Interaction();
    }

    public void DoorAction()
    {
        player.Interaction();
    }

}
