using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoundaryStart : MonoBehaviour
{

    private int id;
    private int col;
    
    // Start is called before the first frame update
    void Awake()
    {
        col = ((int)(transform.position.x / 500f));
        id = col;
       //Debug.Log("Boundary Start" + id);
    }




    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BAgent"))
        {
            RH.boundarystartt[id] = true;
            Destroy(gameObject);
        }

    }
}
