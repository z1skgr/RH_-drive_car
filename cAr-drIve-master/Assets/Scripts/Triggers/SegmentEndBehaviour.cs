using UnityEngine;

public class SegmentEndBehaviour : MonoBehaviour
{
    private int id;
    private readonly float roundTo = RH.popGap;
    private double closestTo;

    private int col;
    private int line;

    //id & col for multiple lines of tracks. Population size > 25 means more than 1 line
    //Try to find where id is. So identify its id
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

        pos = transform.position.z;
        if (pos < 0)
        {
            closestTo = ((pos - (0.5 * roundTo)) / roundTo) * roundTo;
        }
        else
        {
            closestTo = ((pos + (0.5 * roundTo)) / roundTo) * roundTo;
        }

        line = (int)(closestTo / RH.popGap);
        id = line * RH.trackGap + col;
       // Debug.Log("Segment endv "+id);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Agent"))
        {
            RH.endt[id] = true;
            Destroy(other.gameObject);
            Destroy(gameObject);

        }

        RH.timeLapsed[id, 1] = (float)(Mathf.Round(RH.timeLapsed[id, 1] * 1000) / 1000.0f);
        RH.avgLapsed += RH.timeLapsed[id, 1];
    }


    void Update()
    {
        if (RH.startt[id])
        {
            RH.timeLapsed[id, 1] += Time.deltaTime;
        }
    }
}
