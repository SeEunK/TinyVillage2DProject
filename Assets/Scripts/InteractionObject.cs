using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class InteractionObject : MonoBehaviour
{
    public enum ObjectType { None, Fishing, Mining, Gathering, Logging, Farming ,Doorway, Npc}

    public ObjectType mType = ObjectType.None;
  

    protected virtual void Start()
    {
        
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
   
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
    
    }

   

 

   
}
