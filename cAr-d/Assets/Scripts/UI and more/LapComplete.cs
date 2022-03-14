using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapComplete : MonoBehaviour
{
    public GameObject LapCompleteTrig;
    public GameObject MinuteDisplay;
    public GameObject SecondDisplay;
    public GameObject MilliDisplay;

    public static float prevLap;
    public static float ttimer;

    //Player Track Time
    void OnTriggerEnter(Collider other)
    {
        if(TimeManager.SecondCount <= 9)
        {
            SecondDisplay.GetComponent<Text>().text = "0" + TimeManager.SecondCount + ".";
        }
        else
        {
            SecondDisplay.GetComponent<Text>().text = "" + TimeManager.SecondCount + ".";
        }

        if(TimeManager.MinuteCount <= 9) {
            MinuteDisplay.GetComponent<Text>().text = "0" + TimeManager.MinuteCount + ".";
        }
        else
        {
            MinuteDisplay.GetComponent<Text>().text = "" + TimeManager.MinuteCount + ".";
        }

        MilliDisplay.GetComponent<Text>().text = "" + TimeManager.MilliDisplay + ".";
        TimeManager.timer = ((float)(((float)(TimeManager.MinuteCount * 60)) + (float)TimeManager.SecondCount + (float)TimeManager.MilliCount * 0.1));
        TimeManager.timer =  (float)(Mathf.Round(TimeManager.timer * 1000) / 1000.0f);
        Debug.Log("timer"+TimeManager.timer);
        prevLap = TimeManager.timer;
        //Debug.Log("timer apo prev lap apo to trigger" + prevLap);
        PlayerWriteScript.tt = TimeManager.timer;
        ttimer = TimeManager.timer;
        //Debug.Log("timer apo ttimer apo to trigger" + ttimer);

        PlayerWriteScript.CreateFlow();
        TimeManager.MinuteCount = 0;
        TimeManager.SecondCount = 0;
        TimeManager.MilliCount = 0;
        TimeManager.timer = 0;

        RH.ChoosePool();
        //Reset on level end
    }

}
