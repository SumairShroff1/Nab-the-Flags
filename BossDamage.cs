using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    public Boss boss;
    public float cooldownTime = 1.5f;
    //public Retorn retorn;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            boss.AttackPlayer();
            StartCoroutine(Cooldown());
        }
    }
    IEnumerator Cooldown(){
        Vector3 newPosition = transform.position;
        newPosition.y += 5f;
        transform.position = newPosition;

        //retorn.enabled = false;
        yield return new WaitForSeconds(1f);
        //retorn.enabled = true;

        yield return new WaitForSeconds(cooldownTime);
        
        Vector3 OldPosition = transform.position;
        OldPosition.y -= 5f;
        transform.position = OldPosition;
    }
}
