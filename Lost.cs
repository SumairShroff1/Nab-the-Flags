using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lost : MonoBehaviour
{
    public Sceleton sceleton;

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
        {
            sceleton.lost = true;
            if (!sceleton.slipStatus)
            {
                sceleton.LostPlayer();
            }
        }
    }
}
