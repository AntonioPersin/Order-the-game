using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZeGameHard : MonoBehaviour
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
        controller.setHardActive();
        int count = controller.countRooms("Hard");

        GameObject location = GameObject.Find("SpawnPointHard" + Random.Range(0, count));
        GameObject.Find("Player").transform.position = location.transform.position;
        controller.GetComponent<GameController>().Initialize("hard");

        controller.getRooms("Hard").ForEach(room => { room.GetComponent<RoomScript>().EnterZeRoom(); });
    }
}
