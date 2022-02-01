using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoundaryEnd : MonoBehaviour
{
    private int id;

    // Start is called before the first frame update
    void Awake()
    {

        float f = float.Parse(((int)transform.position.x).ToString("f0")); ;
        float multiple = float.Parse((500).ToString("f0"));

        f /= multiple;

        f = Mathf.Ceil(f) * multiple;
        //Debug.Log("f" + f);
        f/=500;
        id = (int)f;
        //Debug.Log("Boundary endv " + id);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BAgent"))
        {
            RH.evolveendt[id] = true;
            Destroy(other.gameObject);
            Destroy(gameObject);

        }
        RH.boundaryLapsed[id] = (float)(Mathf.Round(RH.boundaryLapsed[id] * 1000) / 1000.0f);
        //Debug.Log("BLapsed" + id + ":" + RH.boundaryLapsed[id]);

    }

    // Update is called once per frame
    void Update()
    {
        if (RH.boundarystartt[id])
        {
            RH.boundaryLapsed[id] += Time.deltaTime;
        }
    }
}
