using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZeGameNormal : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter()
    {
        Teleport();
    }

    public void Teleport()
    {
        GameController controller = GameObject.Find("GameController").GetComponent<GameController>();
        controller.setNormalActive();
        int count = controller.countRooms("Normal");

        GameObject.Find("Player").transform.position = GameObject.Find("SpawnPointNormal" + Random.Range(0, count)).transform.position;
        controller.GetComponent<GameController>().Initialize("normal");
        controller.getRooms("Normal").ForEach(room => { room.GetComponent<RoomScript>().EnterZeRoom(); });
    }
}
