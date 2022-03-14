using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoice : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject TextBox1;
    public GameObject TextBox2;
    public GameObject playButton;
    public GameObject backButton;

    //5 Buttons in playing screen
    public GameObject c11;
    public GameObject c12;

    public GameObject c21;
    public GameObject c22;
    public GameObject c23;

    //2 choices - 1 for sequence length , 1 for population size
    public int cmade1=0;
    public int cmade2=0;

    public string theName1;
    public string theName2;


    /*Button Option for Sequence*/
    public void Choice1Option1()
    {
        TextBox1.GetComponent<Text>().text = "Sequence Length: 20";
        cmade1 = 1;
        theName1 = c11.GetComponentInChildren<Text>().text;
        int.TryParse(theName1, out Indi.SEQUENCE_LENGTH);
        Debug.Log("Sequence Length:" +  Indi.SEQUENCE_LENGTH);
    }

    public void Choice1Option2()
    {
        TextBox1.GetComponent<Text>().text = "Sequence Length: 40";
        cmade1 = 2;
        theName1 = c12.GetComponentInChildren<Text>().text;
        int.TryParse(theName1, out Indi.SEQUENCE_LENGTH);
        Debug.Log("Sequence Length:" +  Indi.SEQUENCE_LENGTH);
    }


    /*Button Option for Population*/
    public void Choice2Option1()
    {
        TextBox2.GetComponent<Text>().text = "Population Size: 10";
        cmade2 = 1;
        theName2 = c21.GetComponentInChildren<Text>().text;
        int.TryParse(theName2, out RH.POPULATION_SIZE);
        Debug.Log("Population size:" + RH.POPULATION_SIZE);
    }

    public void Choice2Option2()
    {
        TextBox2.GetComponent<Text>().text = "Population Size: 25";
        cmade2 = 2;
        theName2 = c22.GetComponentInChildren<Text>().text;
        int.TryParse(theName2, out RH.POPULATION_SIZE);
        Debug.Log("Population size:" + RH.POPULATION_SIZE);
    }

    public void Choice2Option3()
    {
        TextBox2.GetComponent<Text>().text = "Population Size: 50";
        cmade2 = 3;
        theName2 = c23.GetComponentInChildren<Text>().text;
        int.TryParse(theName2, out RH.POPULATION_SIZE);
        Debug.Log("Population size:" + RH.POPULATION_SIZE);
    }

    /*After choosing buttons go to show bottun for loading screen*/
    public void CheckOptions()
    {
        if (cmade1 != 0 && cmade2 != 0)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }
    }

    public void ResetOptions()
    {
        cmade1 = 0;
        cmade2 = 0;
        RH.POPULATION_SIZE = 0;
        Indi.SEQUENCE_LENGTH = 0;
        theName1 = "";
        theName2 = "";
        c11.SetActive(true);
        c12.SetActive(true);
        c21.SetActive(true);
        c22.SetActive(true);
        c23.SetActive(true);
        TextBox1.GetComponent<Text>().text = "_______";
        TextBox2.GetComponent<Text>().text = "_______";
    }

    void Update()
    {
        CheckOptions();
        if (cmade1 >= 1)
        {
            
            c11.SetActive(false);
            c12.SetActive(false);
        }

        if (cmade2 >= 1)
        {
            c21.SetActive(false);
            c22.SetActive(false);
            c23.SetActive(false);
        }
    }

}
