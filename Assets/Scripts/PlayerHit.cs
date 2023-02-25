using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other == null)
            {
                return;
            }
            
            if (Vector3.Distance(other.GetComponent<Slime>().transform.position, transform.position) < 1.5f)
            {
                other.GetComponent<Slime>().Attacked();
           
            }
           
        }
    }
}
