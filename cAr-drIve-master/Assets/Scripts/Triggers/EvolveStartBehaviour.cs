using UnityEngine;
using System;

public class EvolveStartBehaviour : MonoBehaviour
{
    private int id;
    private int col;


    private readonly float roundTo = RH.popGap;
    private double closestTo;

    private int line;

    //id & col for multiple lines of tracks. Population size > 25 means more than 1 line
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
        //Debug.Log("Evolve start " + id);
    }



    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Agent"))
        {
            RH.evolvestartt[id] = true;
            Destroy(gameObject);
        }

    }

}
