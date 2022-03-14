using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrackTrigger : MonoBehaviour
{
    //Trigger when player ends its level
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RH.playerfinish = true;
            Destroy(gameObject);
            RH.level++;
            RH.evolutioncount = 0;
        }


    }
}
