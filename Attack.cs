using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Sceleton sceleton;
    public float cooldownTime = 1.5f;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            sceleton.AttackPlayer();
            StartCoroutine(Cooldown());
        }
    }
    IEnumerator Cooldown(){
        Vector3 newPosition = transform.position;
        newPosition.y += 10f;
        transform.position = newPosition;

        yield return new WaitForSeconds(cooldownTime);
        
        Vector3 OldPosition = transform.position;
        OldPosition.y -= 10f;
        transform.position = OldPosition;
    }
}
