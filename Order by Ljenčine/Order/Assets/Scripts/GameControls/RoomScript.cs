using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{

    public List<GameObject> planes;
    public GameObject GameControl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnterZeRoom()
    {
        List<Material> materials = GameControl.GetComponent<GameController>().getSelectedMaterials();

        int counter = 0;
        planes.ForEach(plane => { 
            plane.GetComponent<Renderer>().material = materials[counter]; 
            counter++;
            DoorScript skripta = plane.GetComponent<DoorScript>();
            if (skripta != null) Destroy(skripta);
            plane.AddComponent<DoorScript>();
        });

        ShuffleMaterials();
    }

    public void ShuffleMaterials()
    {
        for (int i = 0; i < planes.Count; i++)
        {
            int randomIndex = Random.Range(i, planes.Count);

            Material temp = planes[i].GetComponent<Renderer>().material;
            planes[i].GetComponent<Renderer>().material = planes[randomIndex].GetComponent<Renderer>().material;
            planes[randomIndex].GetComponent<Renderer>().material = temp;
        }

        RemoveInstance();
    }

    private void RemoveInstance()
    {
        planes.ForEach(plane => { 
            plane.GetComponent<Renderer>().material.name = plane.GetComponent<Renderer>().material.name.Split(' ')[0]; 
        });
    }

}
