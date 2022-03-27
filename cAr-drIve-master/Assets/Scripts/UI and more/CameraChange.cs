using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject NormalCam;
    public GameObject FarCam;
    public GameObject FPCam;
    public GameObject AgCam;
    public GameObject PCam;
    public GameObject ChCam;
    public GameObject OffCam;


    public static int CamMode;
    void Awake()
    {
        CamMode = 0;
        NormalCam.SetActive(true);
        FarCam.SetActive(false);
        FPCam.SetActive(false);
        AgCam.SetActive(false);
        PCam.SetActive(false);
        ChCam.SetActive(false);
        OffCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("CameraChange"))
        {
            if (CamMode == 6)
            {
                CamMode = 0;
            }
            else
            {
                CamMode++;
            }
            StartCoroutine(ModeChange());
        }
    }

    IEnumerator ModeChange()
    {
        yield return new WaitForSeconds(0.01f);
        if (CamMode == 0)
        {
            NormalCam.SetActive(true);
            FarCam.SetActive(false);
            FPCam.SetActive(false);
            AgCam.SetActive(false);
            PCam.SetActive(false);
            ChCam.SetActive(false);
            OffCam.SetActive(false);
        }
        if (CamMode == 1)
        {
            NormalCam.SetActive(false);
            FarCam.SetActive(true);
            FPCam.SetActive(false);
            AgCam.SetActive(false);
            PCam.SetActive(false);
            ChCam.SetActive(false);
            OffCam.SetActive(false);
        }
        if (CamMode == 2)
        {
            NormalCam.SetActive(false);
            FarCam.SetActive(false);
            FPCam.SetActive(true);
            AgCam.SetActive(false);
            PCam.SetActive(false);
            ChCam.SetActive(false);
            OffCam.SetActive(false);
        }
        if (CamMode == 3)
        {
            NormalCam.SetActive(false);
            FarCam.SetActive(false);
            FPCam.SetActive(false);
            AgCam.SetActive(true);
            PCam.SetActive(false);
            ChCam.SetActive(false);
            OffCam.SetActive(false);
        }
        if (CamMode == 4)
        {
            NormalCam.SetActive(false);
            FarCam.SetActive(false);
            FPCam.SetActive(false);
            AgCam.SetActive(false);
            PCam.SetActive(true);
            ChCam.SetActive(false);
            OffCam.SetActive(false);
        }
        if (CamMode == 5)
        {
            NormalCam.SetActive(false);
            FarCam.SetActive(false);
            FPCam.SetActive(false);
            AgCam.SetActive(false);
            PCam.SetActive(false);
            ChCam.SetActive(true);
            OffCam.SetActive(false);
        }
        if (CamMode == 6)
        {
            NormalCam.SetActive(false);
            FarCam.SetActive(false);
            FPCam.SetActive(false);
            AgCam.SetActive(false);
            PCam.SetActive(false);
            ChCam.SetActive(false);
            OffCam.SetActive(true);
        }
    }
}
