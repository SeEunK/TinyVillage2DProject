using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{


    private float mThrust = 5.0f;
    private float mKnockTime = 0.2f;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();

            if(enemy != null)
            {
                enemy.isKinematic= false;
                Vector2 difference = enemy.transform.position - this.transform.position;

                difference = difference.normalized * mThrust;
                enemy.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(WaitKnockTime(enemy));
            }
        }
    }

    private IEnumerator WaitKnockTime(Rigidbody2D enemy)
    {
        if(enemy != null)
        {
            yield return new WaitForSeconds(mKnockTime);
            enemy.velocity = Vector2.zero;
            enemy.isKinematic = true; 
        }
    }
}