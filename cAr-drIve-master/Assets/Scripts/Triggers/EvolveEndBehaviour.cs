using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EvolveEndBehaviour : MonoBehaviour
{
    private int id;
    private readonly float roundTo = RH.popGap;
    private double closestTo;

    private int col;
    private int line;

    //id & col for multiple lines of tracks. Population size > 25 means more than 1 line
    //Try to find where id is. So identify its id

    // Start is called before the first frame update
    void Awake()
    {
        float pos = transform.position.x;
        if (pos < 0)
        {
            closestTo = ((pos - (0.5 * roundTo)) / roundTo) * roundTo;
        }
        else
        {
            closestTo = ((pos + (0.5 * roundTo)) / roundTo) * roundTo;
        }
        col = (int)(closestTo / RH.popGap);

        pos = transform.position.z - RH.firstevolvedGap;
        if (pos < 0)
        {
            closestTo = ((pos - (0.5 * roundTo)) / roundTo) * roundTo;
        }
        else
        {
            closestTo = ((pos + (0.5 * roundTo)) / roundTo) * roundTo;
        }

        line = (int)((closestTo) / RH.popGap);
        id = Math.Abs(line) * RH.trackGap + col;
        //Debug.Log("Evolve endv "+id);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Agent"))
        {
            RH.evolveendt[id] = true;
            Destroy(other.gameObject);
            Destroy(gameObject);

        }
        RH.evolveLapsed[id] = (float)(Mathf.Round(RH.evolveLapsed[id] * 1000) / 1000.0f);


    }

    // Update is called once per frame
    void Update()
    {
        if (RH.evolvestartt[id])
        {
            RH.evolveLapsed[id] += Time.deltaTime;
        }
    }
}
