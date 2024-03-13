using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Material> materials;
    public List<Texture> symbols;
    public GameObject lichQuotes;

    public GameObject roomsNormal;
    public GameObject roomsHard;
    public GameObject letThereBeLight;
    public GameObject outside;

    public GameObject uiImage;
    public Text scoreUI;
    public Text newSymbol;

    public int maxScoreNormal = 7;
    public int maxScoreHard = 11;

    public Text beginText;
    public Text endText;
    public float timeBegin = 5f;
    public float timeEnd = 5f;
    private bool showBeginning = false;
    private bool showEnding = false;

    private int maxScore;
    private bool showNewSymbolBool = false;
    private float showNewSymbolTime;
    private bool showScore = false;
    private int iterator;

    private List<Material> sekvencija;
    private List<Material> selectedMaterials;
    private Material rngMat;


    void Start()
    {
        showBeginning = true;
        showEnding = false;

        uiImage.SetActive(false);
        newSymbol.text = "";
        reset();

        letThereBeLight.SetActive(false);
        outside.SetActive(false);
        unset();

        foreach (Transform soundObject in lichQuotes.transform)
        {
            VoiceScript skripta = soundObject.gameObject.GetComponent<VoiceScript>();
            if (skripta != null) Destroy(skripta);
            soundObject.gameObject.AddComponent<VoiceScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (showNewSymbolBool)
        {
            if (showNewSymbolTime > 0)
            {
                showNewSymbolTime -= Time.deltaTime;
            }
            else
            {
                showNewSymbolBool = false;
                uiImage.SetActive(false);
                newSymbol.text = "";
            }
        }

        if (showScore)
        {
            if (sekvencija.Count <= maxScore)
            {
                scoreUI.text = sekvencija.Count + " / " + maxScore;
            }
        }
        else scoreUI.text = "";

        //----- pocetni text
        if (showBeginning)
        {
            beginText.enabled = true;
            if (timeBegin > 0)
            {
                timeBegin -= Time.deltaTime;
            }
            else showBeginning = false;
        }
        else beginText.enabled = false;

        //----- zavrsni text
        if (showEnding)
        {
            endText.enabled = true;
            if (timeEnd > 0)
            {
                timeEnd -= Time.deltaTime;
            }
            else showEnding = false;
        }
        else endText.enabled = false;

    }

    //---------------------------------------------------------------------
    //---------------------------------------------------------------------
    //---------------------------------------------------------------------
    public List<Material> getSelectedMaterials()
    {
        return selectedMaterials;
    }



    public void addNewSymbol()
    {
        Material selected = selectedMaterials[Random.Range(0, selectedMaterials.Count)];
        sekvencija.Add(selected);

        if (sekvencija.Count > maxScore)
        {
            showScore = false;
            letThereBeLight.SetActive(true);
            outside.SetActive(true);
            reset();
            unset();

            GameObject changeLocation = GameObject.Find("SpawnOutside");

            GameObject.Find("Player Camera").GetComponent<PlayerCameraController>().RotateOnY(
                changeLocation.transform.eulerAngles.y, 0, 0);
            GameObject.Find("Player").transform.position = changeLocation.transform.position;

            showEnding = true;

        }

        else
        {
            showNewSymbolBool = true;
            showNewSymbolTime = 1.0f;

            symbols.ForEach(symbol =>
            {
                if (symbol.name == selected.name)
                {
                    uiImage.GetComponent<RawImage>().texture = symbol;
                    newSymbol.text = "New symbol:\r\n" + symbol.name;
                }
            });
            uiImage.SetActive(true);
        }
        
    }

    //---------------------------------------------------------------------
    //---------------------------------------------------------------------
    //---------------------------------------------------------------------
    public void Initialize(string difficulty)
    {

        reset();
        if (difficulty == "normal")
        {
            maxScore = maxScoreNormal;

            for(int i=0; i<4; i++)
            {
                bool postavi = true;
                rngMat = materials[Random.Range(0,8)];

                selectedMaterials.ForEach(mat => { if (mat.name == rngMat.name) postavi = false; });

                if (postavi)
                    selectedMaterials.Add(rngMat);
                else i--;
            }

            /*
            for (int i=0; i<selectedMaterials.Count; i++)
            {
                Debug.Log(selectedMaterials[i].name);
            }
            */
        }
        else if(difficulty == "hard")
        {
            maxScore = maxScoreHard;
            selectedMaterials = materials;
        }

        showScore = true;
        addNewSymbol();
    }

    //---------------------------------------------------------------------
    //---------------------------------------------------------------------
    //---------------------------------------------------------------------
    
    public void CheckSelectedSymbol(string material, float planeY)
    {
        //material = material.Split(' ')[0];
        //Debug.Log(sekvencija[iterator].name + " -|- " + material);


        GameObject player = GameObject.Find("Player");
        GameObject playerCam = GameObject.Find("Player Camera");

        if (sekvencija[iterator].name == material)
        {
            iterator++;
            if (iterator == sekvencija.Count)
            {
                addNewSymbol();
                iterator = 0;
            }

            List<RoomScript> rooms = new List<RoomScript>(GameObject.FindObjectsOfType<RoomScript>());
            rooms.ForEach(room => room.ShuffleMaterials());

            if (selectedMaterials.Count == 4) //Normal difficulty
            {
                int count = countRooms("Normal");

                GameObject changeLocation = GameObject.Find("SpawnPointNormal" + Random.Range(0, count));
                playerCam.GetComponent<PlayerCameraController>().RotateOnY(
                    changeLocation.transform.eulerAngles.y, 
                    player.transform.eulerAngles.x,
                    player.transform.eulerAngles.y + 180 - planeY);
                player.transform.position = changeLocation.transform.position;
            }
            else if(selectedMaterials.Count == 8) //Hard difficulty
            {
                int count = countRooms("Hard");

                GameObject changeLocation = GameObject.Find("SpawnPointHard" + Random.Range(0, count));
                playerCam.GetComponent<PlayerCameraController>().RotateOnY(
                    changeLocation.transform.eulerAngles.y,
                    player.transform.eulerAngles.x,
                    player.transform.eulerAngles.y + 180 - planeY);
                player.transform.position = changeLocation.transform.position;
            }
        }
        else
        {
            GameObject.Find("RIP_FPS_False").GetComponent<RIPFPSFalse>().enableRestartLocation();
            playerCam.GetComponent<PlayerCameraController>().RotateOnY(
                    GameObject.Find("RespawnPoint1").transform.eulerAngles.y,
                    player.transform.eulerAngles.x, 0);
            player.transform.position = GameObject.Find("RespawnPoint1").transform.position;
            audioPlay();

            reset();
            unset();
        }
    }

    //---------------------------------------------------------------------
    //---------------------------------------------------------------------
    //---------------------------------------------------------------------

    private void reset()
    {
        sekvencija = new List<Material>();
        selectedMaterials = new List<Material>();
        iterator = 0;
        showScore = false;
    }


    public int countRooms(string difficulty)
    {
        Transform rooms = GameObject.Find("Rooms" + difficulty).transform;
        int counter = 0;

        foreach (Transform room in rooms)
        {
            counter++;
        }

        return counter;
    }

    public List<GameObject> getRooms(string difficulty)
    {
        List<GameObject> rooms = new List<GameObject>();

        foreach(Transform room in GameObject.Find("Rooms" + difficulty).transform)
        {
            rooms.Add(room.gameObject);
        }

        return rooms;
    }

    //---------------------------------------------------------------------
    //---------------------------------------------------------------------
    //---------------------------------------------------------------------

    public void setNormalActive()
    {
        roomsNormal.SetActive(true);
    }

    public void setHardActive()
    {
        roomsHard.SetActive(true);
    }

    public void unset()
    {
        roomsHard.SetActive(false);
        roomsNormal.SetActive(false);
    }

    private void audioPlay()
    {
        lichQuotes.transform.Find("Voice" + Random.Range(0, lichQuotes.transform.childCount)).GetComponent<VoiceScript>().PlayVoice();
    }
    
    //---------------------------------------------------------------------
    //---------------------------------------------------------------------
    //---------------------------------------------------------------------

}
