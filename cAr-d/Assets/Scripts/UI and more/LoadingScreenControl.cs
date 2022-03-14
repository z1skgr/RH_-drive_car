using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenControl : MonoBehaviour
{
    public GameObject loadingScreenObj;
    public Slider slider;
    public Text loadingText;
    public Text Loading;
    public GameObject loadButton;

    [SerializeField]
    [Range(0, 1)]
    private float progressAnimationMultiplier = 0.25f;

    private float currentValue;
    private float targetValue;

    public static AsyncOperation async;

    public void LoadScreenExample()
    {

        StartCoroutine(LoadingScreen());
    }

    //Load one scene that is the scene with Agents- Its cool

    //Async loading screen
    IEnumerator LoadingScreen()
    {

        async = SceneManager.LoadSceneAsync("MyTrack");
        async.allowSceneActivation = false;

        while (async.isDone == false )
        {
            targetValue = async.progress / 0.9f ;
            currentValue = Mathf.MoveTowards(currentValue, targetValue, progressAnimationMultiplier * Time.deltaTime);
            slider.value = currentValue;
            //Debug.Log(op.progress);
            loadingText.text = currentValue * 100f + "%";
            LoadButtonOnLoading();
            yield return null;
            
        }
    } 

    //Delay in showing Button
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);
        Loading.GetComponent<Text>().text = "Ready";
        loadButton.SetActive(true);
        loadingText.gameObject.SetActive(true);
    }

    public void LoadButtonOnLoading()
    {
        //Showing labels in loading screen
        if (Mathf.Approximately(currentValue, 1))
        {
            StartCoroutine(Delay());
        }
        else
        {
            Loading.GetComponent<Text>().text = "Loading";
            loadingText.gameObject.SetActive(true);
            loadButton.SetActive(false);
        }
    }

    
    public void LoadButtonOnClick()
    {
        async.allowSceneActivation = true;

    }

}