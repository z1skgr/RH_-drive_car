using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Start is called before the first frame update
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
        //Debug.Log("Checkpoint "+id);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Agent")
        {
            Destroy(gameObject);

        }

    }

    void Update()
    {

        if (RH.startt[id] && (!RH.checkReached))
        {
            RH.timeLapsed[id, 0] += Time.deltaTime;

        }
        


    }
}
