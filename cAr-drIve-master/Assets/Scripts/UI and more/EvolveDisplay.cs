using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EvolveDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject evolveDisplay;
    public GameObject lvlDisplay;

    // Update is called once per frame
    //Reset level and evolution when level ends
    void Update()
    {
        evolveDisplay.GetComponent<Text>().text = RH.evolutioncount.ToString();
        lvlDisplay.GetComponent<Text>().text = RH.level.ToString();
    }
}
