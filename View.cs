using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public Sceleton sceleton;

    private void OnTriggerEnter(Collider other) {
        if (!sceleton.lost){
            return;
        }
        if (other.CompareTag("Player"))
        {
            Vector3 directionToPlayer = (sceleton.player.transform.position - transform.parent.position).normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.parent.position, directionToPlayer, out hit, 5f))
            {
                if (hit.collider.tag == "Player")
                {
                    //Debug.Log("Player11111");
                    sceleton.TargetPlayer(true);
                }else{
                    // Debug.Log(hit.collider.name);
                    // Debug.Log(hit.collider.transform.parent.parent.name);
                    // Debug.Log("tag " + hit.collider.tag);
                }
            }
            StartCoroutine(Cooldown());
        }
    }
    IEnumerator Cooldown(){
        Vector3 newPosition = transform.position;
        newPosition.y += 5f;
        transform.position = newPosition;

        yield return new WaitForSeconds(2f);
        
        Vector3 OldPosition = transform.position;
        OldPosition.y -= 5f;
        transform.position = OldPosition;
    }
}
