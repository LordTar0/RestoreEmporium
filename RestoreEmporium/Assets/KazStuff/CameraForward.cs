using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraForward : MonoBehaviour
{
    public GameObject Camera_1;
    public GameObject Workstation;
    public GameObject Camera_3;
    public GameObject BuildMode;
    public int Manager;


    public void ChangeCamera()
    {
        GetComponent<Animator>().SetTrigger("Change");
    }



    public void ManageCamera()
    {
        if (Manager == 0)
        {
            Cam_2();
            Manager = 1;
        }
        else if (Manager == 1)
        {
            Cam_3();
            Manager = 2;
        }
        else
        {
            Cam_1();
            Manager = 0;
        }
    }



    void Cam_1()
    {
        BuildMode.SetActive(true);
        Camera_1.SetActive(true);
        Workstation.SetActive(false);
        Camera_3.SetActive(false);
    }

    void Cam_2()
    {
        BuildMode.SetActive(false);
        Camera_1.SetActive(false);
        Workstation.SetActive(true);
        Camera_3.SetActive(false);
    }
    void Cam_3()
    {
        BuildMode.SetActive(false);
        Camera_1.SetActive(false);
        Workstation.SetActive(false);
        Camera_3.SetActive(true);
    }



}
