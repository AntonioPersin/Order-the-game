using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RIPFPSFalse : MonoBehaviour
{
    // Start is called before the first frame update
    private bool delete;
    private float time;
    private GameObject restartLocation;
    
    void Start()
    {
        delete = false;
        time = 1f;
        restartLocation = GameObject.Find("RestartLocation");
        restartLocation.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (delete)
        {

            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                restartLocation.SetActive(false);
                delete = false;
            }
        }

    }

    void OnTriggerEnter()
    {
        delete = true;
        time = 1f;
    }

    public void enableRestartLocation()
    {
        restartLocation.SetActive(true);
    }
}
