using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider player)
    {
        GameObject.Find("GameController").GetComponent<GameController>().CheckSelectedSymbol(this.GetComponent<Renderer>().material.name, this.transform.eulerAngles.y);
    }
}
