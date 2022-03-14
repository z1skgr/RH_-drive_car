using UnityEngine;

public class SegmentStartBehaviour : MonoBehaviour
{
    private int id;
    private int col;
    private int row;
    
    //id & col for multiple lines of tracks. Population size > 25 means more than 1 line
    void Awake()
    {
            col = ((int)(transform.position.x / RH.popGap));
            row = ((int)(transform.position.z / RH.popGap));
            id = row * RH.trackGap + col;
           // Debug.Log("Segment Start" + id);
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Agent"))
        {
            RH.startt[id] = true;
            Destroy(gameObject);
        } 

    }
}

