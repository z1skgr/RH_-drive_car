using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    //Countdown when game starts
    public GameObject CountDown;

    public GameObject LapTimer;
    public GameObject Agents;
    public GameObject Panels;
    public GameObject writeMan;

    void Start()
    {
        StartCoroutine(CountStart());
    }

    IEnumerator CountStart()
    {
        yield return new WaitForSeconds(0.5f);
        CountDown.GetComponent<Text>().text = "3";
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        CountDown.GetComponent<Text>().text = "2";
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        CountDown.GetComponent<Text>().text = "1";
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1);
        CountDown.GetComponent<Text>().text = "GO!";
        CountDown.SetActive(true);
        yield return new WaitForSeconds(2);
        LapTimer.SetActive(true);

        CountDown.SetActive(false);

        Panels.SetActive(true);
        writeMan.SetActive(true);
        Agents.SetActive(true);
    }
}
